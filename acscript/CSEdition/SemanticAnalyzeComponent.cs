using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �Ӗ���͗p�R���|�[�l���g�B
    /// </summary>
    class SemanticAnalyzeComponent
    {
        //------------------------------------------------------------
        // �V���{���m�[�h�����ꂽ���p��delegate�H
        public delegate bool OnSymbolNodeCreate(VariableSymbolNode node);

        //------------------------------------------------------------
        // ���J�����o�ϐ��B
        public readonly SymbolTree.ErrorInfoHolder ErrorInfoHolder;
        public readonly ModuleContext ModuleContext;
        public readonly TypeSymbolNode TypeSymbolNode;
        public readonly OnSymbolNodeCreate OnSymbolNodeCreateMethod;
        public ISymbolNode PrevSymbolNode;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
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
        // TypeInfo�𐶐�����B
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
