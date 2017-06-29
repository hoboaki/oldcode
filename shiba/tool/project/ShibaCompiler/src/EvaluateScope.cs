using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 評価スコープ。
    /// </summary>
    class EvaluateScope
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public EvaluateScope()
        {
            mEvaluateNodeList = new List<IEvaluateNode>();
            mEvaluatedSymbolNodeList = new List<EvaluatedSymbolNode>();
        }

        //------------------------------------------------------------
        // 評価ノードを追加する。
        public void AddEvaluateNode(IEvaluateNode aNode)
        {
            mEvaluateNodeList.Add(aNode);
        }

        //------------------------------------------------------------
        // 評価済みシンボルノードを追加する。
        public void AddEvaluatedSymbolNode(EvaluatedSymbolNode aNode)
        {
            mEvaluatedSymbolNodeList.Add(aNode);
        }

        //------------------------------------------------------------
        // 指定のSymbolNodeに対応した評価済みシンボルノードを探す。
        public EvaluatedSymbolNode FindEvaluatedSymbolNode(ISymbolNode aNode)
        {
            foreach (var node in mEvaluatedSymbolNodeList)
            {
                if (node.SymbolNode == aNode)
                {
                    return node;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // break文用のラベルを登録する
        public void RegisterLabelBreak(BCLabel aLabel)
        {
            Assert.Check(mLabelBreak == null);
            mLabelBreak = aLabel;
        }

        //------------------------------------------------------------
        // continue文用のラベルを登録する
        public void RegisterLabelContinue(BCLabel aLabel)
        {
            Assert.Check(mLabelContinue == null);
            mLabelContinue = aLabel;
        }

        //------------------------------------------------------------
        // return文用のラベルを登録する
        public void RegisterLabelReturn(BCLabel aLabel)
        {
            Assert.Check(mLabelReturn == null);
            mLabelReturn = aLabel;
        }

        //------------------------------------------------------------
        // スコープ離脱命令を挿入。
        public void InsertOpCodeOnScopeLeave(SemanticAnalyzeComponent aComp)
        {
            // 逆順で。
            foreach (var entry in mEvaluateNodeList.Reverse<IEvaluateNode>())
            {
                entry.SendEvent(aComp, EvaluateNodeEventKind.InsertOpCodeOnScopeLeave);
            }
        }

        //------------------------------------------------------------
        // Releaseイベントを送信。
        public void SendEventRelease(SemanticAnalyzeComponent aComp)
        {
            // 逆順で。
            foreach (var entry in mEvaluateNodeList.Reverse<IEvaluateNode>())
            {
                entry.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        }

        //------------------------------------------------------------
        // break用ラベルを取得する。
        public BCLabel LabelBreak()
        {
            return mLabelBreak;
        }

        //------------------------------------------------------------
        // continue用ラベルを取得する。
        public BCLabel LabelContinue()
        {
            return mLabelContinue;
        }
        
        //------------------------------------------------------------
        // return用ラベルを取得する。
        public BCLabel LabelReturn()
        {
            return mLabelReturn;
        }

        //============================================================
        private List<IEvaluateNode> mEvaluateNodeList;
        private List<EvaluatedSymbolNode> mEvaluatedSymbolNodeList;
        private BCLabel mLabelBreak;
        private BCLabel mLabelContinue;
        private BCLabel mLabelReturn;
    }
}
