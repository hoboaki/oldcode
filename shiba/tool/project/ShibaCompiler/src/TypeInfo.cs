using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 解決済みの型情報。
    /// </summary>
    class TypeInfo
    {
        //------------------------------------------------------------
        // 型のシンボル。
        public class TypeSymbol
        {
            //------------------------------------------------------------
            // 種類。
            public enum Kind
            {
                BuiltIn,
                Symbol,
            }

            //------------------------------------------------------------
            // コンストラクタ。　
            public TypeSymbol(TypeSymbolNode aNode)
            {
                mKind = Kind.Symbol;
                mNode = aNode;
                mToken = aNode.GetIdentifier().Token;
            }

            //------------------------------------------------------------
            // コンストラクタ。
            public TypeSymbol(Token aToken, BuiltInType aBuiltInType)
            {
                mKind = Kind.BuiltIn;
                mBuildInType = aBuiltInType;
                mToken = aToken;
            }

            //------------------------------------------------------------
            // 種類を取得。
            public Kind GetKind()
            {
                return mKind;
            }

            //------------------------------------------------------------
            // 組み込み型の種類を取得する。
            public BuiltInType GetBuiltInType()
            {
                return mBuildInType;
            }

            //------------------------------------------------------------
            // シンボルノードを取得する。
            public TypeSymbolNode GetTypeSymbolNode()
            {
                return mNode;
            }

            //------------------------------------------------------------
            // トークンを取得する。
            public Token GetToken()
            {
                return mToken;
            }

            //============================================================
            Kind mKind;
            Token mToken;
            BuiltInType mBuildInType = BuiltInType.Unknown;
            TypeSymbolNode mNode = null;
        }

        //------------------------------------------------------------
        // 型の属性。
        public struct TypeAttribute
        {
            public readonly bool IsConst;
            public readonly bool IsRef;

            //------------------------------------------------------------
            // コンストラクタ。
            public TypeAttribute(bool aIsConst,bool aIsRef)
            {
                IsConst = aIsConst;
                IsRef = aIsRef;
            }
        }

        //------------------------------------------------------------
        // 公開メンバ変数。
        public readonly TypeSymbol Symbol;
        public readonly TypeAttribute Attribute;

        //------------------------------------------------------------
        // コンストラクタ。
        public TypeInfo(TypeSymbol aSymbol,TypeAttribute aAttr)
        {
            Symbol = aSymbol;
            Attribute = aAttr;
        }
    }
}
