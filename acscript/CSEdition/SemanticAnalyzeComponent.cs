using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 意味解析用コンポーネント。
    /// </summary>
    class SemanticAnalyzeComponent
    {
        //------------------------------------------------------------
        // シンボルノードが作られた時用のdelegate？
        public delegate bool OnSymbolNodeCreate(VariableSymbolNode node);

        //------------------------------------------------------------
        // 公開メンバ変数。
        public readonly SymbolTree.ErrorInfoHolder ErrorInfoHolder;
        public readonly ModuleContext ModuleContext;
        public readonly TypeSymbolNode TypeSymbolNode;
        public readonly OnSymbolNodeCreate OnSymbolNodeCreateMethod;
        public ISymbolNode PrevSymbolNode;

        //------------------------------------------------------------
        // コンストラクタ。
        public SemanticAnalyzeComponent(
            SymbolTree.ErrorInfoHolder aErrorInfoHolder
            , ModuleContext aModuleContext
            , TypeSymbolNode aTypeSymbolNode
            , OnSymbolNodeCreate aOnSymbolNodeCreate
            , ISymbolNode aPrevSymbolNode
            )
        {
            ErrorInfoHolder = aErrorInfoHolder;
            ModuleContext = aModuleContext;
            TypeSymbolNode = aTypeSymbolNode;
            OnSymbolNodeCreateMethod = aOnSymbolNodeCreate;
            PrevSymbolNode = aPrevSymbolNode;
        }

        //------------------------------------------------------------
        // TypeInfoを生成する。
        public TypeInfo CreateTypeInfo(TypePath typePath)
        {
            BuiltInType builtInType = typePath.BuiltInType;
            TypeInfo.TypeKind kind = TypeInfo.TypeKind.VALUE;
            if (builtInType == BuiltInType.Void)
            {
                kind = TypeInfo.TypeKind.UNKNOWN;
            }
            return new TypeInfo(
                new TypeInfo.TypeSymbol(builtInType)
                , kind
                , new TypeInfo.TypeAttribute(false, false)
                );
        }
    }
}
