using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 型情報。
    /// </summary>
    class TypeInfo
    {
        //------------------------------------------------------------
        // 型の種類。
        public enum TypeKind
        {
            UNKNOWN,
            VALUE, // 値型。
            HANDLE, // オブジェクトハンドル型。オブジェクトハンドルを解決すると参照が得られる。
        }

        //------------------------------------------------------------
        // 型のシンボル。
        public class TypeSymbol
        {
            //------------------------------------------------------------
            // タイプ。
            enum Type
            {
                BuiltIn,
                Symbol,
            }

            //------------------------------------------------------------
            // コンストラクタ。　
            public TypeSymbol(TypeSymbolNode aNode)
            {
                mType = Type.Symbol;
                mNode = aNode;
            }

            //------------------------------------------------------------
            // コンストラクタ。
            public TypeSymbol(BuiltInType aBuiltInType)
            {
                mType = Type.BuiltIn;
                mBuildInType = aBuiltInType;
            }

            //============================================================
            Type mType;
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
        public readonly TypeKind Kind;
        public readonly TypeAttribute Attribute;

        //------------------------------------------------------------
        // コンストラクタ。
        public TypeInfo(TypeSymbol aSymbol,TypeKind aKind, TypeAttribute aAttr)
        {
            Symbol = aSymbol;
            Kind = aKind;
            Attribute = aAttr;
        }
    }
}
