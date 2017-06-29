using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �^���B
    /// </summary>
    class TypeInfo
    {
        //------------------------------------------------------------
        // �^�̎�ށB
        public enum TypeKind
        {
            UNKNOWN,
            VALUE, // �l�^�B
            HANDLE, // �I�u�W�F�N�g�n���h���^�B�I�u�W�F�N�g�n���h������������ƎQ�Ƃ�������B
        }

        //------------------------------------------------------------
        // �^�̃V���{���B
        public class TypeSymbol
        {
            //------------------------------------------------------------
            // �^�C�v�B
            enum Type
            {
                BuiltIn,
                Symbol,
            }

            //------------------------------------------------------------
            // �R���X�g���N�^�B�@
            public TypeSymbol(TypeSymbolNode aNode)
            {
                mType = Type.Symbol;
                mNode = aNode;
            }

            //------------------------------------------------------------
            // �R���X�g���N�^�B
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
        // �^�̑����B
        public struct TypeAttribute
        {
            public readonly bool IsConst;
            public readonly bool IsRef;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public TypeAttribute(bool aIsConst,bool aIsRef)
            {
                IsConst = aIsConst;
                IsRef = aIsRef;
            }
        }

        //------------------------------------------------------------
        // ���J�����o�ϐ��B
        public readonly TypeSymbol Symbol;
        public readonly TypeKind Kind;
        public readonly TypeAttribute Attribute;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public TypeInfo(TypeSymbol aSymbol,TypeKind aKind, TypeAttribute aAttr)
        {
            Symbol = aSymbol;
            Kind = aKind;
            Attribute = aAttr;
        }
    }
}
