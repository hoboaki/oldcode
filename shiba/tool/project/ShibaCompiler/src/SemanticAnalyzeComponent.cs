using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 意味解析用コンポーネント。
    /// </summary>
    class SemanticAnalyzeComponent
    {
        //------------------------------------------------------------
        // 公開メンバ変数。
        public readonly BCFunction BCFunction;
        public readonly TypeSymbolNode TypeSymbolNode;
        public readonly ISymbolNode BeginSymbolNode;

        //------------------------------------------------------------
        // コンストラクタ。
        public SemanticAnalyzeComponent(
            BCFunction aBCFunction
            , TypeSymbolNode aTypeSymbolNode
            , ISymbolNode aBeginSymbolNode
            )
        {
            BCFunction = aBCFunction;
            TypeSymbolNode = aTypeSymbolNode;
            BeginSymbolNode = aBeginSymbolNode;
            mPrevSymbolNode = aBeginSymbolNode;
            mFreeStackRegister = new Stack<uint>();
            mSymbolNodeStack = new Stack<ISymbolNode>();
            mEvaluateScopeStack = new Stack<EvaluateScope>();
        }

        //------------------------------------------------------------
        // エラー例外をthrowする関数。
        public void ThrowErrorException(SymbolTree.ErrorKind aErrorKind, Token aToken)
        {
            throw new SymbolTree.ErrorException(
                new SymbolTree.ErrorInfo(aErrorKind, TypeSymbolNode.ModuleContext(), aToken)
                );
        }

        //------------------------------------------------------------
        // TypeInfoを生成する。
        public TypeInfo CreateTypeInfo(TypePath aTypePath,bool aIsConst, bool aIsRef)
        {
            // todo:
            // 今は組み込み型しか対応していません
            Assert.Check(aTypePath.BuiltInType != BuiltInType.Unknown);

            // 組み込み型として作成する
            BuiltInType builtInType = aTypePath.BuiltInType;
            return new TypeInfo(
                new TypeInfo.TypeSymbol(aTypePath.BuiltInToken, builtInType)
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }

        //------------------------------------------------------------
        // 直前のSymbolNodeを取得する。
        public ISymbolNode PrevSymbolNode()
        {
            return mPrevSymbolNode;
        }

        //------------------------------------------------------------
        // SymbolNodeを現在のスコープに追加する。
        public void AddSymbolNode(ISymbolNode aNode)
        {
            // 直前Nodeの更新
            mPrevSymbolNode = aNode;
        }

        //------------------------------------------------------------
        // EvaluateNodeを現在のスコープに追加する。
        public void AddEvaluateNode(IEvaluateNode aNode)
        {
            currentScope().AddEvaluateNode(aNode);
        }

        //------------------------------------------------------------
        // 評価済みノードとしてシンボルノードを登録する。
        public void AddEvaluatedSymbolNode(EvaluatedSymbolNode aEvaluatedSymbolNode)
        {
            currentScope().AddEvaluatedSymbolNode(aEvaluatedSymbolNode);
        }

        //------------------------------------------------------------
        // 戻り値用の評価情報を登録する。
        public void ReturnEvaluateInfoSet(EvaluateInfo aEvaluateInfo)
        {
            Assert.Check(mReturnEvaluateInfo == null);
            mReturnEvaluateInfo = aEvaluateInfo;
        }

        //------------------------------------------------------------
        // 戻り値用の評価情報を取得する。戻り値がなければnullを返す。
        public EvaluateInfo ReturnEvaluateInfoGet()
        {
            return mReturnEvaluateInfo;
        }

        //------------------------------------------------------------
        // 現在のスコープにcontinue用のラベルを登録する。
        public void RegisterLabelContinue(BCLabel aLabel)
        {
            currentScope().RegisterLabelContinue(aLabel);
        }

        //------------------------------------------------------------
        // 現在のスコープにbreak用のラベルを登録する。
        public void RegisterLabelBreak(BCLabel aLabel)
        {
            currentScope().RegisterLabelBreak(aLabel);
        }

        //------------------------------------------------------------
        // 現在のスコープにreturn用のラベルを登録する。
        public void RegisterLabelReturn(BCLabel aLabel)
        {
            currentScope().RegisterLabelReturn(aLabel);
        }

        //------------------------------------------------------------
        // 指定の名前のシンボルノードを探す。
        public ISymbolNode FindSymbolNode(Identifier aIdent)
        {
            for(var node = mPrevSymbolNode;
                node != null && node.GetNodeKind() != SymbolNodeKind.Root;
                node = node.ParentNode()
                )
            {
                if (node.GetIdentifier().IsSame(aIdent))
                {
                    return node;
                }

                var childNode = node.FindChildNode(aIdent);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // 指定のシンボルノードに対応する評価済みノードを探す。
        public EvaluatedSymbolNode FindEvaluatedSymbolNode(ISymbolNode aNode)
        {
            foreach (var scope in mEvaluateScopeStack)
            {
                var node = scope.FindEvaluatedSymbolNode(aNode);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // スタックレジスタを借りる。
        public StackRegister SRReserve()
        {
            // todo:
            // 256を超えたときの処理。

            // 空きがあるならそれをもらう
            if (mFreeStackRegister.Count != 0)
            {
                return new StackRegister(mFreeStackRegister.Pop());
            }

            // 確保　
            StackRegister sr = new StackRegister(mStackRegisterIndexNext);
            ++mStackRegisterIndexNext;

            // ピーク値を記憶
            mStackRegisterIndexPeak = Math.Max(mStackRegisterIndexPeak, mStackRegisterIndexNext);

            // 結果を返す
            return sr;
        }

        //------------------------------------------------------------
        // スタックレジスタを返す。
        public void SRRelease(StackRegister aSR)
        {
            if (mFreeStackRegister.Count == 0
                && aSR.Index() + 1 == mStackRegisterIndexNext
                )
            {// 一番最後に渡したレジスタが返されたのでNext値を１下げる。
                --mStackRegisterIndexNext;
            }
            else
            {// 空きスタックに追加
                mFreeStackRegister.Push(aSR.Index());
            }
        }

        //------------------------------------------------------------
        // 確保レジスタのピーク数を取得する。
        public uint SRPeakCount()
        {
            return mStackRegisterIndexPeak;
        }

        //------------------------------------------------------------
        // 伝達するEvaluateInfoを設定する。
        public void TransferredEvaluateInfoSet(EvaluateInfo aEvaluateInfo)
        {
            Assert.Check(mTransferredEvaluateInfo == null);
            mTransferredEvaluateInfo = aEvaluateInfo;
        }

        //------------------------------------------------------------
        // 伝達されたEvaluateInfoを取得する。取得したら伝達されたEvaluateInfoは自動的にnullが設定される。
        public EvaluateInfo TransferredEvaluateInfoReceive()
        {
            EvaluateInfo result = mTransferredEvaluateInfo;
            mTransferredEvaluateInfo = null;
            return result;
        }

        //------------------------------------------------------------
        // 伝達されたEvaluateInfoをリセットする。
        public void TransferredEvaluateInfoReset()
        {
            mTransferredEvaluateInfo = null;
        }

        //------------------------------------------------------------
        // スコープに入るときの処理。
        public void ScopeEnter()
        {
            // PrevSymbolNodeをPush
            mSymbolNodeStack.Push(mPrevSymbolNode);

            // スコープをPush
            mEvaluateScopeStack.Push(new EvaluateScope());
        }

        //------------------------------------------------------------
        // スコープから出るときの処理。
        public void ScopeLeave()
        {
            // スコープ離脱命令を挿入
            currentScope().InsertOpCodeOnScopeLeave(this);

            // スコープ離脱イベントを送信
            currentScope().SendEventRelease(this);

            // スコープをPop
            mEvaluateScopeStack.Pop();

            // PrevSymbolNodeをPop
            mPrevSymbolNode = mSymbolNodeStack.Pop();
        }

        //------------------------------------------------------------
        // Break用のラベルを探しつつBreak文を実行する。
        public BCLabel ExecBreakStatement()
        {
            // まずLabelのあるスコープを探す
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelBreak();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// 見つかりませんでした。
                return null;
            }

            // 目標のスコープまでスコープ離脱命令を挿入
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ラベルを返す
            return targetScope.LabelBreak();
        }

        //------------------------------------------------------------
        // Continue用のラベルを探しつつContinue文を実行する。
        public BCLabel ExecContinueStatement()
        {
            // まずLabelのあるスコープを探す
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelContinue();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// 見つかりませんでした。
                return null;
            }

            // 目標のスコープまでスコープ離脱命令を挿入
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ラベルを返す
            return targetScope.LabelContinue();
        }

        //------------------------------------------------------------
        // Return用のラベルを探しつつReturn文を実行する。
        public BCLabel ExecReturnStatement()
        {
            // まずLabelのあるスコープを探す
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelReturn();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// 見つかりませんでした。
                return null;
            }

            // 目標のスコープまでスコープ離脱命令を挿入
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ラベルを返す
            return targetScope.LabelReturn();
        }

        //------------------------------------------------------------
        // 関数呼び出しのターゲットを設定します。
        public void FunctionCallTargetSet(FunctionSymbolNode aNode)
        {
            Assert.Check(mFunctionCallTarget == null);
            mFunctionCallTarget = aNode;
            mFunctionCallFRNextIndex = 0; // FRのインデックスもリセット
        }

        //------------------------------------------------------------
        // 関数呼び出しのターゲットをリセットします。
        public void FunctionCallTargetReset()
        {
            mFunctionCallTarget = null;
        }

        //------------------------------------------------------------
        // 関数呼び出しで使用する、次の引数レジスタのインデックスを取得する。
        public byte FunctionCallFRNextIndex()
        {
            Assert.Check(mFunctionCallTarget != null);
            byte frIndex = mFunctionCallFRNextIndex;
            ++mFunctionCallFRNextIndex;
            return frIndex;
        }

        //============================================================
        private uint mStackRegisterIndexNext;
        private uint mStackRegisterIndexPeak;
        private Stack<uint> mFreeStackRegister;
        private Stack<ISymbolNode> mSymbolNodeStack;
        private Stack<EvaluateScope> mEvaluateScopeStack;
        private ISymbolNode mPrevSymbolNode;
        private EvaluateInfo mTransferredEvaluateInfo;
        private EvaluateInfo mReturnEvaluateInfo;
        private FunctionSymbolNode mFunctionCallTarget;
        private byte mFunctionCallFRNextIndex;

        //------------------------------------------------------------
        // 一番最後にPushしたスコープを取得する。
        EvaluateScope currentScope()
        {
            return mEvaluateScopeStack.Peek();
        }
    }
}
