using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �����ς݂̌^���B
    /// </summary>
    class TypeInfo
    {
        //------------------------------------------------------------
        // �^�̃V���{���B
        public class TypeSymbol
        {
            //------------------------------------------------------------
            // ��ށB
            public enum Kind
            {
                BuiltIn,
                Symbol,
            }

            //------------------------------------------------------------
            // �R���X�g���N�^�B�@
            public TypeSymbol(TypeSymbolNode aNode)
            {
                mKind = Kind.Symbol;
                mNode = aNode;
                mToken = aNode.GetIdentifier().Token;
            }

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public TypeSymbol(Token aToken, BuiltInType aBuiltInType)
            {
                mKind = Kind.BuiltIn;
                mBuildInType = aBuiltInType;
                mToken = aToken;
            }

            //------------------------------------------------------------
            // ��ނ��擾�B
            public Kind GetKind()
            {
                return mKind;
            }

            //------------------------------------------------------------
            // �g�ݍ��݌^�̎�ނ��擾����B
            public BuiltInType GetBuiltInType()
            {
                return mBuildInType;
            }

            //------------------------------------------------------------
            // �V���{���m�[�h���擾����B
            public TypeSymbolNode GetTypeSymbolNode()
            {
                return mNode;
            }

            //------------------------------------------------------------
            // �g�[�N�����擾����B
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
        public readonly TypeAttribute Attribute;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public TypeInfo(TypeSymbol aSymbol,TypeAttribute aAttr)
        {
            Symbol = aSymbol;
            Attribute = aAttr;
        }
    }
}
