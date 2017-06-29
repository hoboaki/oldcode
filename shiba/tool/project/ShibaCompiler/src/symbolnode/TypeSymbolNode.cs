using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Typeノード。
    /// </summary>
    class TypeSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public TypeSymbolNode(ISymbolNode aParent, BCModule aBCModule, StaticTypeDef aStaticTypeDef)
        {
            mParent = aParent;
            mBCModule = aBCModule;
            mStaticTypeDef = aStaticTypeDef;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // 自分自身のIdent。
        public Identifier GetIdentifier()
        {
            return mStaticTypeDef.Ident;
        }

        //------------------------------------------------------------
        // ユニークなフルパス。
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
        }

        //------------------------------------------------------------
        // ノードの種類。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Type;
        }

        //------------------------------------------------------------
        // 親ノード。
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // 指定のIdentのノードを探す。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mNodeList.FindNode(aIdent);
        }

        //------------------------------------------------------------
        // 展開命令。
        public void SymbolExpand(SymbolExpandCmdKind aCmd)
        {
            switch (aCmd)
            {
                case SymbolExpandCmdKind.TypeNode:
                    expandTypeNode();
                    break;

                case SymbolExpandCmdKind.FunctionNodeDecl:
                    expandFunctionNodeDecl();
                    break;

                case SymbolExpandCmdKind.VariableNode:
                    expandVariableNode();
                    break;

                case SymbolExpandCmdKind.FunctionNodeImpl:
                    expandChilds(SymbolExpandCmdKind.FunctionNodeImpl);
                    break;

                default:
                    break;
            }
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(), "TypeSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var node in mNodeList)
                {
                    node.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // ModuleContextを取得する。
        public ModuleContext ModuleContext()
        {
            return mBCModule.ModuleContext();
        }

        //============================================================
        ISymbolNode mParent;
        BCModule mBCModule;
        StaticTypeDef mStaticTypeDef;
        SymbolNodeList mNodeList;
        BCObjectType mBCObjectType;
        
        //------------------------------------------------------------
        // TypeNode展開。
        private void expandTypeNode()
        {
            // メモ：この時点で子ノードは0なので子ノード展開要求は出さない
            Assert.Check(mNodeList.Count() == 0);

            // バイトコードオブジェクト作成
            mBCObjectType = mBCModule.GenerateObjectType(this);

            // 自分自身
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.StaticTypeDef != null)
                {
                    // 重複チェック
                    checkIdentDuplication(symbol.StaticTypeDef.Ident);

                    // ノード作成
                    var newNode = new TypeSymbolNode(this, mBCModule, symbol.StaticTypeDef);

                    // 更に展開
                    newNode.SymbolExpand(SymbolExpandCmdKind.TypeNode);

                    // 追加
                    addNode(newNode);
                }
            }
        }

        //------------------------------------------------------------
        // FunctionNode宣言展開。
        private void expandFunctionNodeDecl()
        {
            // まず子ノード
            expandChilds(SymbolExpandCmdKind.FunctionNodeDecl);

            // シンボル走査
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.MemberFunctionDecl != null)
                {
                    // 重複チェック
                    checkIdentDuplication(symbol.MemberFunctionDecl.Ident);

                    // ノード作成
                    var newNode = new FunctionSymbolNode(this, mBCObjectType, symbol.MemberFunctionDecl);

                    // 更に展開
                    newNode.SymbolExpand(SymbolExpandCmdKind.FunctionNodeDecl);

                    // 追加
                    addNode(newNode);
                }
            }
        }

        //------------------------------------------------------------
        // VariableNode展開。
        private void expandVariableNode()
        {
            // まず子ノード
            expandChilds(SymbolExpandCmdKind.VariableNode);

            // todo:
            // ↑このtodoなんだっけ･･･　（2010/08/07)
        }

        //------------------------------------------------------------
        // 子ノードの展開。
        private void expandChilds(SymbolExpandCmdKind aCmd)
        {
            foreach (var node in mNodeList)
            {
                // 展開
                node.SymbolExpand(aCmd);
            }
        }


        //------------------------------------------------------------
        // Ident重複チェック。trueなら重複している。
        private void checkIdentDuplication(Identifier aIdent)
        {
            if (FindChildNode(aIdent) != null)
            {
                throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(SymbolTree.ErrorKind.NODE_NAME_IS_ALREADY_EXIST, mBCModule.ModuleContext(), aIdent.Token));
            }
        }

        //------------------------------------------------------------
        // ノードの追加。
        private void addNode(ISymbolNode aNode)
        {
            Assert.Check(
                aNode.GetNodeKind() == SymbolNodeKind.Type
                || aNode.GetNodeKind() == SymbolNodeKind.Variable
                || aNode.GetNodeKind() == SymbolNodeKind.Function
                );
            mNodeList.Add(aNode);
        }       
    }
}
