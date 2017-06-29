using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Function�m�[�h�B
    /// </summary>
    class FunctionSymbolNode : ISymbolNode
    {
        /// <summary>
        /// �����̌^���B
        /// </summary>
        class ArgTypeInfo
        {
            public readonly Identifier Ident;
            public readonly TypeInfo TypeInfo;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public ArgTypeInfo(Identifier aIdent, TypeInfo aTypeInfo)
            {
                Ident = aIdent;
                TypeInfo = aTypeInfo;
            }
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionSymbolNode(
            TypeSymbolNode aParent
            ,ModuleContext aModuleContext
            ,MemberFunctionDecl aFunctionDecl
            )
        {
            mParent = aParent;
            mModuleContext = aModuleContext;
            mFunctionDecl = aFunctionDecl;

            // TypeInfo����
            {// �߂�l
                FunctionReturnValueDecl retValDecl = aFunctionDecl.ReturnValueDecl;
                mReturnTypeInfo = createTypeInfo(
                    retValDecl.TypePath
                    , retValDecl.IsConst
                    , false // isIn
                    , retValDecl.IsRef
                    );
            }
            {// ����
                mArgTypeInfos = new List<ArgTypeInfo>();
                foreach (FunctionArgumentDecl argDecl in aFunctionDecl.ArgDeclList)
                {
                    mArgTypeInfos.Add(new ArgTypeInfo(argDecl.Ident, createTypeInfo(
                        argDecl.TypePath
                        , argDecl.IsConst
                        , argDecl.IsIn
                        , argDecl.IsRef
                        )));
                }
            }
        }

        //------------------------------------------------------------
        // TypeInfo����
        static TypeInfo createTypeInfo(
            TypePath aTP
            , bool aIsConst
            , bool aIsIn
            , bool aIsRef
            )
        {
            BuiltInType builtInType = aTP.BuiltInType;
            TypeInfo.TypeKind kind = TypeInfo.TypeKind.VALUE;
            if (builtInType == BuiltInType.Void)
            {
               kind = TypeInfo.TypeKind.UNKNOWN;
            }
            if (aIsIn)
            {
                if (aTP.BuiltInType == BuiltInType.Unknown)
                {
                    aIsConst = true;
                    aIsRef = true;
                }
                else
                {
                    aIsConst = true;
                }
            }
            return new TypeInfo(
                new TypeInfo.TypeSymbol(builtInType)
                , kind
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }

        //------------------------------------------------------------
        // ���ʎq�̎擾�B
        public Identifier GetIdentifier()
        {
            return mFunctionDecl.Ident;
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ނɎ擾�B
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Function;
        }

        //------------------------------------------------------------
        // �e�m�[�h���擾�B
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // �q�m�[�h����������B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null;
        }

        //------------------------------------------------------------
        // �V���{����W�J����B
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aCmdKind)
        {
            if (aCmdKind != SymbolExpandCmdKind.FunctionNodeImpl)
            {// �֐������ȊO�Ȃ牽�����Ȃ�
                return true;
            }

            mLocalVariableSymbolNodes = new List<VariableSymbolNode>();
            ISymbolNode prevNode = this;

            {// ����
                foreach (ArgTypeInfo argTypeInfo in mArgTypeInfos)
                {
                    VariableSymbolNode node = new VariableSymbolNode(
                        prevNode
                        , argTypeInfo.Ident
                        , argTypeInfo.TypeInfo
                        );
                    prevNode = node;
                    mLocalVariableSymbolNodes.Add(node);
                }
            }

            {// Statement���
                SemanticAnalyzeComponent comp = new SemanticAnalyzeComponent(
                    aHolder
                    , mModuleContext
                    , mParent
                    , new SemanticAnalyzeComponent.OnSymbolNodeCreate(onSymbolNodeCreate)
                    , prevNode
                    );
                if (!mFunctionDecl.Statement().SemanticAnalyze(comp))
                {
                    return false;
                }
            }

            return true;
        }

        //============================================================
        TypeSymbolNode mParent;
        ModuleContext mModuleContext;
        MemberFunctionDecl mFunctionDecl;
        TypeInfo mReturnTypeInfo;
        List<ArgTypeInfo> mArgTypeInfos;
        List<VariableSymbolNode> mLocalVariableSymbolNodes;

        //------------------------------------------------------------
        // �V���{���m�[�h���쐬����B
        private bool onSymbolNodeCreate(VariableSymbolNode aNode)
        {
            // �d���`�F�b�N
            // ...

            // �ǉ�
            mLocalVariableSymbolNodes.Add(aNode);

            return true;
        }


    }
}
