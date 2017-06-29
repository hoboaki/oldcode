using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Variableノード。
    /// </summary>
    class VariableSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public VariableSymbolNode(
            ISymbolNode aParent
            , Identifier aIdent
            , TypeInfo aTypeInfo
            )
        {
            mParent = aParent;
            mIdent = aIdent;
            mTypeInfo = aTypeInfo;
        }

        //------------------------------------------------------------
        // TypeInfoの取得。
        public TypeInfo GetTypeInfo()
        {
            return mTypeInfo;
        }

        //------------------------------------------------------------
        // 識別子の取得。
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // ユニークなフルパス。
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
        }

        //------------------------------------------------------------
        // ノードの種類の取得。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Variable;
        }

        //------------------------------------------------------------
        // 親ノードの取得。
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // 子ノードを探す。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null; // 子ノードはない。
        }

        //------------------------------------------------------------
        // シンボルを展開する。
        public void SymbolExpand(SymbolExpandCmdKind aCmdKind)
        {
            // todo: impl
        }

        //------------------------------------------------------------
        // トレースする。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(), "VariableSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {                
                // todo: typeinfo
            }
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        TypeInfo mTypeInfo;
    }
}
