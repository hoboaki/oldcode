using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            NOT_DECLARATION_IDENT,
            NOT_SUPPORTED_EXPRESSION,
            NOT_SUPPORTED_TYPENAME,
            CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB,
            INVALID_BREAK,
            INVALID_CONTINUE,
            SEMICOLON_EXPECTED,
            EXPRESSION_EXPECTED,
            FUNCTION_SYMBOL_EXPECTED,
        }

        //------------------------------------------------------------
        // エラー情報。
        public struct  ErrorInfo
        {
            public readonly ErrorKind Kind;
            public readonly ModuleContext ModuleContext;
            public readonly Token Token;
            public readonly TypeInfo TypeInfoA;
            public readonly TypeInfo TypeInfoB;

            //------------------------------------------------------------
            // コンストラクタ。
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken)
                : this(aErrorKind, aModuleContext, aErrorToken, null, null)
            {
            }

            //------------------------------------------------------------
            //  型が２つあるコンストラクタ。
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken, TypeInfo aTypeInfoA, TypeInfo aTypeInfoB)
            {
                Kind = aErrorKind;
                ModuleContext = aModuleContext;
                Token = aErrorToken;
                TypeInfoA = aTypeInfoA;
                TypeInfoB = aTypeInfoB;
            }
        };
        
        //------------------------------------------------------------
        // エラーが設定されたときに作られる例外。
        public class ErrorException : Exception
        {
            public ErrorInfo Info;

            //------------------------------------------------------------
            // コンストラクタ。
            public ErrorException(ErrorInfo aErrorInfo)
                : base()
            {
                Info = aErrorInfo;
            }
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public SymbolTree()
        {
            mRoot = new RootSymbolNode();
            mModuleSymbolNodeList = new List<ModuleSymbolNode>();
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            mRoot.Trace(aTracer);
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

                if (nextNode == null)
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
            Assert.Check(node.GetNodeKind() == SymbolNodeKind.NameSpace);

            // Moduleノードを追加
            var moduleSymbolNode = new ModuleSymbolNode(node, aModuleContext);
            ((NamespaceSymbolNode)node).AddNode(moduleSymbolNode);
            mModuleSymbolNodeList.Add(moduleSymbolNode);

            return true;
        }

        //------------------------------------------------------------
        // 全Moduleを展開する。
        public bool Expand()
        {
            try
            {
                // 型
                mRoot.SymbolExpand(SymbolExpandCmdKind.TypeNode);

                // 変数
                mRoot.SymbolExpand(SymbolExpandCmdKind.VariableNode);

                // 関数の宣言
                mRoot.SymbolExpand(SymbolExpandCmdKind.FunctionNodeDecl);

                // 関数の実装
                mRoot.SymbolExpand(SymbolExpandCmdKind.FunctionNodeImpl);

                // 出力準備
                foreach (var entry in mModuleSymbolNodeList)
                {
                    entry.ReadyToOutput();
                }
            }
            catch (ErrorException aException)
            {// エラー発生
                mErrorInfo = aException.Info;
                return false;
            }

            // 展開成功
            return true;
        }

        //------------------------------------------------------------
        // 全Moduleをファイルに出力する。
        public void WriteToXML(string aOutputDirPath)
        {
            foreach (var entry in mModuleSymbolNodeList)
            {
                entry.WriteToXML(aOutputDirPath);
            }
        }

        //------------------------------------------------------------
        // 全ModuleをXData化してダンプする。
        public void XDataDump()
        {
            foreach (var entry in mModuleSymbolNodeList)
            {
                entry.XDataDump();
            }
        }

        //------------------------------------------------------------
        // エラー情報を取得する。
        public ErrorInfo GetErrorInfo()
        {
            return mErrorInfo;
        }
        
        //============================================================
        RootSymbolNode mRoot;
        ErrorInfo mErrorInfo;
        List<ModuleSymbolNode> mModuleSymbolNodeList;

        //------------------------------------------------------------
        // エラーを情報を設定する。
        void setErrorInfo(ModuleContext aModuleContext, ErrorKind aErrorKind, Token aErrorToken)
        {
            mErrorInfo = new ErrorInfo(aErrorKind, aModuleContext, aErrorToken);
        }
    }
}
