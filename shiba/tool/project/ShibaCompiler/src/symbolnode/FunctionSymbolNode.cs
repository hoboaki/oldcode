using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            ,BCObjectType aBCObjectType
            ,MemberFunctionDecl aFunctionDecl
            )
        {
            // �����o������
            mParent = aParent;
            mBCObjectType = aBCObjectType;
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
        // ���ʎq�̎擾�B
        public Identifier GetIdentifier()
        {
            return mFunctionDecl.Ident;
        }

        //------------------------------------------------------------
        // ���j�[�N�ȃt���p�X�B
        public string GetUniqueFullPath()
        {
            // ���ʂ̃p�X
            string path = SymbolNodeUtil.FullPath(this);

            // ����
            path += "(";
            foreach (var entry in mArgTypeInfos)
            {
                // todo:
            }
            path += ")";

            // const
            if (mFunctionDecl.IsConst())
            {
                path += "const";
            }

            // ���ʂ�Ԃ�
            return path;
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
        // �߂�l��TypeInfo���擾����B
        public TypeInfo ReturnTypeInfo()
        {
            return mReturnTypeInfo;
        }

        //------------------------------------------------------------
        // �V���{����W�J����B
        public void SymbolExpand(SymbolExpandCmdKind aCmdKind)
        {
            if (aCmdKind != SymbolExpandCmdKind.FunctionNodeImpl)
            {// �֐������ȊO�Ȃ牽�����Ȃ�
                return;
            }

            // BCFunction�쐬
            mBCFunction = mBCObjectType.GenerateFunction(this);

            // �R���|�[�l���g
            SemanticAnalyzeComponent comp = new SemanticAnalyzeComponent(
                mBCFunction
                , mParent
                , this
                );

            // �R�s�[���郌�W�X�^�̐�����������ϐ�
            byte copyRegCount = 0;

            {// �֐������̃X�R�[�v
                // �X�R�[�v�ǉ�
                comp.ScopeEnter();

                // �߂�l�Ή�
                if (mReturnTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                    || mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Void
                    )
                {// void�ȊO�Ȃ�B

                    // todo: �����Ȍ^�̑Ή�
                    // �g�ݍ��݌^��int,bool�����Ή����Ă��܂���B
                    if (mReturnTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                        || (mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                        )
                    {
                        comp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, mReturnTypeInfo.Symbol.GetToken());
                    }

                    // EI�쐬
                    var returnEI = EvaluateInfo.CreateAsValue(mReturnTypeInfo);

                    // SR���m��
                    returnEI.SR = comp.SRReserve();

                    // �߂�l�Ƃ��ēo�^�@
                    comp.ReturnEvaluateInfoSet(returnEI);

                    // �R�s�[���郌�W�X�^�J�E���g�A�b�v
                    ++copyRegCount;
                }

                // this
                if (!mFunctionDecl.IsStatic())
                {
                    // TypeInfo�쐬
                    var ti = comp.CreateTypeInfo(
                        new TypePath(new IdentPath(mParent))
                        , mFunctionDecl.IsConst()
                        , true
                        );

                    // �_�~�[�g�[�N�����쐬
                    var token = new Token();
                    token.Value = Token.Kind.KeyThis;
                    token.pos = GetIdentifier().Token.pos;
                    token.posColumn = GetIdentifier().Token.posColumn;
                    token.posLine = GetIdentifier().Token.posLine;

                    // �V���{���m�[�h���쐬
                    VariableSymbolNode symbolNode = new VariableSymbolNode(
                        comp.PrevSymbolNode()
                        , new Identifier(token)
                        , ti
                        );

                    // �m�[�h��ǉ�
                    comp.AddSymbolNode(symbolNode);

                    // �]���m�[�h���쐬
                    var evaluateNode = new EvaluateNode(symbolNode);

                    // �]���C�x���g���M
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Analyze);
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Evaluate);

                    // �ǉ�
                    comp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

                    // �R�s�[���郌�W�X�^�J�E���g�A�b�v
                    ++copyRegCount;
                }

                // ����
                foreach (var arg in mArgTypeInfos)
                {
                    // �V���{���m�[�h���쐬
                    VariableSymbolNode symbolNode = new VariableSymbolNode(
                        comp.PrevSymbolNode()
                        , arg.Ident
                        , arg.TypeInfo
                        );

                    // �m�[�h��ǉ�
                    comp.AddSymbolNode(symbolNode);

                    // �]���m�[�h���쐬
                    var evaluateNode = new EvaluateNode(symbolNode);

                    // �]���C�x���g���M
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Analyze);
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Evaluate);

                    // �ǉ�
                    comp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

                    // �R�s�[���郌�W�X�^�J�E���g�A�b�v
                    ++copyRegCount;
                }

                {// Statement
                    // �X�R�[�v�ɓ���
                    comp.ScopeEnter();

                    // Return���x���m�ہE�o�^
                    BCLabel labelReturn = comp.BCFunction.LabelCreate();
                    comp.RegisterLabelReturn(labelReturn);

                    // ���
                    mFunctionDecl.Statement().SemanticAnalyze(comp);

                    // �X�R�[�v����o��
                    comp.ScopeLeave();

                    // Return���x���}��
                    comp.BCFunction.LabelInsert(labelReturn);
                }

                // �X�R�[�v�I��
                comp.ScopeLeave();
            }

            // �֐����߂�ǉ�
            // todo: ���W�X�^�g�������`�F�b�N
            mBCFunction.PushFrontOPCode_CU1_CU1(BCOpCode.OpType.FENTER, (byte)comp.SRPeakCount(), copyRegCount);
            mBCFunction.AddOPCode_CU1(BCOpCode.OpType.FLEAVE, (byte)comp.SRPeakCount());

            // ���x������
            mBCFunction.LabelResolve();
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(),"FunctionSymbolNode");
        }

        //============================================================

        //------------------------------------------------------------
        // �߂�l�Athis�A�����p�]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(VariableSymbolNode aSymbol)
            {
                mSymbol = aSymbol;
            }

            //------------------------------------------------------------
            // TypeInfo���擾����B
            public EvaluateInfo GetEvaluateInfo()
            {
                return mEvaluateInfo;
            }

            //------------------------------------------------------------
            // �]���C�x���g�𑗐M����B
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;
                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            VariableSymbolNode mSymbol;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // todo:���낢��Ή����Ă܂��񥥥�B

                // �^���擾
                TypeInfo typeInfo = mSymbol.GetTypeInfo();

                // �]�����쐬
                mEvaluateInfo = EvaluateInfo.CreateAsValue(typeInfo);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^�m�ہ@
                mEvaluateInfo.SR = aComp.SRReserve();

                // �ė��p�֎~
                mEvaluateInfo.DisableReuseSR();
            }


            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^���
                aComp.SRRelease(mEvaluateInfo.SR);
            }
        };

        //------------------------------------------------------------
        TypeSymbolNode mParent;
        BCObjectType mBCObjectType;
        MemberFunctionDecl mFunctionDecl;
        TypeInfo mReturnTypeInfo;
        List<ArgTypeInfo> mArgTypeInfos;
        BCFunction mBCFunction;

        //------------------------------------------------------------
        // TypeInfo����
        static TypeInfo createTypeInfo(
            TypePath aTP
            , bool aIsConst
            , bool aIsIn
            , bool aIsRef
            )
        {
            // �r���g�C���^�C�v
            BuiltInType builtInType = aTP.BuiltInType;
            Assert.Check(builtInType != BuiltInType.Unknown); // todo: �g�ݍ��݌^�ȊO�̑Ή�

            // in�L�[���[�h�΍�
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
                new TypeInfo.TypeSymbol(aTP.BuiltInToken, builtInType)
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }
    }
}
