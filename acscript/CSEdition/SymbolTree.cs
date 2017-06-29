using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// シンボルのツリー。
    /// </summary>
    class SymbolTree
    {
        //------------------------------------------------------------
        // エラーの種類。
        public enum ErrorKind
        {
            NONE,
            NODE_NAME_IS_ALREADY_EXIST_AS_NOT_NAMESPACE,
            NODE_NAME_IS_ALREADY_EXIST,
        }

        //------------------------------------------------------------
        // エラー情報。
        public class  ErrorInfo
        {
            public readonly ErrorKind Kind;
            public readonly ModuleContext ModuleContext;
            public readonly Token Token;

            //------------------------------------------------------------
            // コンストラクタ。
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken)
            {
                Kind = aErrorKind;
                ModuleContext = aModuleContext;
                Token = aErrorToken;
            }
        };

        //------------------------------------------------------------
        // エラー情報を格納するクラス。
        public class ErrorInfoHolder
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public ErrorInfoHolder()
            {
            }

            //------------------------------------------------------------
            // 設定関数。
            public void Set( ErrorInfo aInfo )
            {
                mInfo = aInfo;
            }
            
            //------------------------------------------------------------
            // 取得関数。
            public ErrorInfo Get()
            {
                return mInfo;
            }

            //============================================================
            private ErrorInfo mInfo;
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public SymbolTree()
        {
            mRoot = new RootSymbolNode();
        }
        
        //============================================================
        RootSymbolNode mRoot;
        ErrorInfo mErrorInfo;

        //------------------------------------------------------------
        // エラーを情報を設定する。
        void setErrorInfo(ModuleContext aModuleContext, ErrorKind aErrorKind, Token aErrorToken)
        {
            mErrorInfo = new ErrorInfo(aErrorKind, aModuleContext, aErrorToken);
        }

        //------------------------------------------------------------
        // モジュールを追加する。
        public bool Add(ModuleContext aModuleContext)
        {
            // 同じパスのものが既に追加されていないかチェックしつつ追加する            
            ISymbolNode node = mRoot;
            IdentPath identPath = aModuleContext.ModuleDecl.IdentifierPath;
            for (int i = 0; i < identPath.Count; ++i)
            {
                ISymbolNode nextNode = node.FindChildNode(identPath.At(i));
                if (i + 1 == identPath.Count)
                {// 最後
                    if (nextNode != null)
                    {// 最後なのに存在している
                        setErrorInfo(aModuleContext, ErrorKind.NODE_NAME_IS_ALREADY_EXIST, node.GetIdentifier().Token);
                        return false;
                    }
                    break;
                }

                if ( nextNode == null)
                {// 存在しないようなので追加
                    NamespaceSymbolNode addNode = new NamespaceSymbolNode(node, identPath.At(i));
                    if (node.GetNodeKind() == SymbolNodeKind.Root)
                    {
                        ((RootSymbolNode)node).AddNode(addNode);
                    }
                    else
                    {
                        ((NamespaceSymbolNode)node).AddNode(addNode);
                    }
                    node = addNode;
                }                
                else
                {// 既に存在している
                    node = nextNode;
                    if (node.GetNodeKind() != SymbolNodeKind.NameSpace)
                    {// 名前空間じゃないものとして存在する
                        setErrorInfo(aModuleContext, ErrorKind.NODE_NAME_IS_ALREADY_EXIST_AS_NOT_NAMESPACE, node.GetIdentifier().Token);
                        return false;
                    }
                }
            }
            System.Diagnostics.Debug.Assert(node.GetNodeKind() == SymbolNodeKind.NameSpace);

            // Moduleノードを追加
            ((NamespaceSymbolNode)node).AddNode(new ModuleSymbolNode(node, aModuleContext));

            return true;
        }

        //------------------------------------------------------------
        // 全Moduleを展開する。
        bool Expand()
        {
            ErrorInfoHolder holder = new ErrorInfoHolder();

            // 型
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.TypeNode))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // 変数
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.VariableNode))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // 関数の宣言
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.FunctionNodeDecl))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // 関数の実装
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.FunctionNodeImpl))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // 展開成功
            return true;
        }
    }
}
