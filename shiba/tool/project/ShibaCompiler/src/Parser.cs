using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �\����́B
    /// </summary>
    partial class Parser
    {
        //------------------------------------------------------------
        /// <summary>
        /// �G���[�̎�ށB
        /// </summary>
        public enum ErrorKind
        {
            NONE,
            MODULE_DECL_KEYWORD_EXPECTED, // [ModuleDecl] 'module' expected
            MODULE_DECL_SEMICOLON_TERM_EXPECTED, // [ModuleDecl] ';' expected
            MODULE_DEF_TYPE_PROTECTION_MUST_BE_PUBLIC, // [ModuleDef] protection must be 'public'
            IDENT_PATH_IDENTIFIER_EXPECTED, // [IdentPath] identifier expected
            IDENT_PATH_DOT_EXPECTED, // [IdentPath] '.' expected
            EXTERNAL_MODULE_DECL_SEMICOLON_TERM_EXPECTED, // [ExternalModuleDecl] ';' expected
            STATIC_TYPE_DEF_ALREADY_ASIGNED_TYPE_PROTECTION, // [StaticTypeDef] already assigned protection
            STATIC_TYPE_DEF_ALREADY_ASIGNED_TYPE_KIND, // [StaticTypeDef] already assigned type kind
            STATIC_TYPE_DEF_TYPE_KEYWORD_EXPECTED, // [StaticTypeDef] type keyword(class,struct,...) expected
            STATIC_TYPE_DEF_IDENTIFIER_EXPECTED, // [StaticTypeDef] identifier exected
            STATIC_TYPE_DEF_ONLY_CLASS_OR_INTERFACE_CAN_IMPLEMENT_INTERFACE, // [StaticTypeDef] only class or interface can implement interface 
            STATIC_TYPE_DEF_LCURLY_EXPECTED, // [StaticTypeDef] '{' expected
            STATIC_TYPE_DEF_RCURLY_EXPECTED, // [StaticTypeDef] '}' expected
            STATIC_TYPE_DEF_SEMICOLON_EXPECTED, // [StaticTypeDef] ';' expected
            STATIC_TYPE_DEF_COLON_UNEXPECTED, // [StaticTypeDef] ':' unexpected
            SYMBOL_DEF_ALREADY_ASIGNED_TYPE_PROTECTION, // [SymbolDef] already assigned protection
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_ABSTRACT, // [SymbolDef] already assigned 'abstract'
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_CONST, // [SymbolDef] already assigned 'const'
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_OVERRIDE, // [SymbolDef] already assigned 'override'
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_READONLY, // [SymbolDef] already assigned 'readonly'
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_REF, // [SymbolDef] already assigned 'ref'
            SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_STATIC, // [SymbolDef] already assigned 'static'
            SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_MEMBER_VARIABLE_DECL, // [SymbolDef] illegal attribute for member variable decl.
            SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_MEMBER_FUNCTION_DECL, // [SymbolDef] illegal attribute for member function decl.
            SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF, // [SymbolDef] illegal attribute for static type def.
            SYMBOL_DEF_ILLEGAL_MEMBER_SYNTAX, // [SymbolDef] illegal member syntax
            SYMBOL_DEF_SEMICOLON_EXPECTED, // [SymbolDef] ';' expected
            SYMBOL_DEF_ONLY_INTERFACE_TYPE_CAN_ABSTRACT_ATTRIBUTE_MEMBER, // [SymbolDef] Only 'interface' type can 'abstract' attribute member.
            SYMBOL_DEF_ONLY_CLASS_TYPE_CAN_OVERRIDE_ATTRIBUTE_MEMBER, // [SymbolDef] Only 'class' type can 'override' attribute member.
            CONDITIONAL_EXPRESSION_COLON_EXPECTED, // [ConditionalExpression] ':' expected
            CAST_EXPRESSION_LPAREN_EXPECTED, // [CastExpression] "(" expected
            CAST_EXPRESSION_RPAREN_EXPECTED, // [CastExpression] ")" expected
            FUNCTION_CALL_EXPRESSION_LPAREN_EXPECTED, // [FunctionCallExpression] "(" exptected
            FUNCTION_CALL_EXPRESSION_RPAREN_EXPECTED, // [FunctionCallExpression] ")" exptected
            POSTFIX_EXPRESSION_IDENTIFIER_EXPECTED, // [PostfixExpression] identifier expected
            INDEX_EXPRESSION_LBRACKET_EXPECTED, // [IndexExpression] "[" expected
            INDEX_EXPRESSION_RBRACKET_EXPECTED, // [IndexExpression] "]" expected
            PRIMARY_EXPRESSION_IDENTIFIER_EXPECTED, // [PrimaryExpression] identifier expected
            PRIMARY_EXPRESSION_LPAREN_EXPECTED, // [PrimaryExpression] "(" expected
            PRIMARY_EXPRESSION_RPAREN_EXPECTED, // [PrimaryExpression] ")" expected
            MEMBER_DEF_IDENTIFIER_EXPECTED, // [MemberDef] identifier expected
            MEMBER_FUNCTION_DECL_SEMICOLON_EXPECTED, // [MemberFuncionDecl] ';' expected
            MEMBER_FUNCTION_DECL_LCURLY_EXPECTED, // [MemberFuncionDecl] '{' expected
            FUNCTION_ARGUMENT_DECL_LIST_COMMA_EXPECTED, // [FunctionArgumentDeclList] ',' expected
            FUNCTION_ARGUMENT_DECL_LIST_LPAREN_EXPECTED, // [FunctionArgumentDeclList] '(' expected
            FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_CONST, // [FunctionArgumentDecl] already assigned 'const'
            FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_IN, // [FunctionArgumentDecl] already assigned 'in'
            FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_REF, // [FunctionArgumentDecl] already assigned 'ref'
            FUNCTION_ARGUMENT_DECL_IDENTIFIER_EXPECTED, // [FunctionArgumentDecl] identifier expected
            MEMBER_VARIABLE_DECL_INTERFACE_CANT_HAVE_NONSTATIC_MEMBER_VARIABLE, // [MemberVariableDecl] 'interface' can't have nonstatic member variable
            MEMBER_VARIABLE_DECL_UTILITY_CANT_HAVE_NONSTATIC_MEMBER_VARIABLE, // [MemberVariableDecl] 'utility' can't have nonstatic member variable
            BLOCK_STATEMENT_LCURLY_EXPECTED, // [BlockStatement] '{' expected
            EXPRESSION_STATEMENT_SEMICOLON_EXPECTED, // [ExpressionStatement] ';' expected
            DECLARATION_STATEMENT_ALREADY_ASSIGNED_ATTRIBUTE_CONST, // [DeclarationStatement] already assigned 'const'
            DECLARATION_STATEMENT_IDENTIFIER_EXPECTED, // [DeclarationStatement] identifier expected
            DECLARATION_STATEMENT_SEMICOLON_EXPECTED, // [DeclarationStatement] ';' expected
            PROTECTION_CANT_USE_PRIVATE_IN_PROTOTYPE_MODULE, // [Protection] Can't use 'private' in prototype module
            WHILE_STATEMENT_LPAREN_EXPECTED, // [WhileStatement] '(' expected
            WHILE_STATEMENT_RPAREN_EXPECTED, // [WhileStatement] ')' expected
            BREAK_STATEMENT_SEMICOLON_EXPECTED, // [BreakStatement] ';' expected
            CONTINUE_STATEMENT_SEMICOLON_EXPECTED, // [ContinueStatement] ';' expected
            IF_STATEMENT_LPAREN_EXPECTED, // [IfStatement] '(' expected
            IF_STATEMENT_RPAREN_EXPECTED, // [IfStatement] ')' expected
            DOWHILE_STATEMENT_LPAREN_EXPECTED, // [DoWhileStatement] '(' expected
            DOWHILE_STATEMENT_RPAREN_EXPECTED, // [DoWhileStatement] ')' expected
            DOWHILE_STATEMENT_WHILE_EXPECTED, // [DoWhileStatement] 'while' expected
            DOWHILE_STATEMENT_SEMICOLON_EXPECTED, // [DoWhileStatement] ';' expected
            FOR_STATEMENT_LPAREN_EXPECTED, // [ForStatement] '(' expected
            FOR_STATEMENT_RPAREN_EXPECTED, // [ForStatement] ')' expected
            FOR_STATEMENT_SEMICOLON_EXPECTED, // [ForStatement] ';' expected
            RETURN_STATEMENT_SEMICOLON_EXPECTED, // [ReturnStatement] ';' expected
        };

        //------------------------------------------------------------
        // BuildInType�B
        static public BuiltInType ParseBuiltInType(Token aT)
        {
            switch (aT.Value)
            {
                case Token.Kind.KeyVoid:
                    return BuiltInType.Void;
                case Token.Kind.KeyBool:
                    return BuiltInType.Bool;
                case Token.Kind.KeyUInt8:
                    return BuiltInType.UInt8;
                case Token.Kind.KeyUInt16:
                    return BuiltInType.UInt16;
                case Token.Kind.KeyUInt32:
                    return BuiltInType.UInt64;
                case Token.Kind.KeyUInt64:
                    return BuiltInType.UInt64;
                case Token.Kind.KeySInt8:
                    return BuiltInType.SInt8;
                case Token.Kind.KeySInt16:
                    return BuiltInType.UInt16;
                case Token.Kind.KeySInt32:
                    return BuiltInType.SInt32;
                case Token.Kind.KeySInt64:
                    return BuiltInType.SInt64;
                case Token.Kind.KeyFloat32:
                    return BuiltInType.Float32;
                case Token.Kind.KeyFloat64:
                    return BuiltInType.Float64;
                case Token.Kind.KeyChar:
                    return BuiltInType.Char;
                case Token.Kind.KeyWChar:
                    return BuiltInType.WChar;
                case Token.Kind.KeyDChar:
                    return BuiltInType.DChar;
                case Token.Kind.KeyObject:
                    return BuiltInType.Object;

                default:
                    return BuiltInType.Unknown;
            }
        }

        //------------------------------------------------------------
        // ���J����郁���o�ϐ��B
        public ModuleContext ModuleContext;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public Parser( Lexer aLexer )
        {
            // ����
            mLexer = aLexer;
            mTokenPtr = aLexer.FirstToken();

            // �p�[�X
            ModuleContext moduleContext = new ModuleContext();
            bool parseResult = parseModule(moduleContext);
            if (!parseResult)
            {
                Assert.Check(GetErrorKind() != ErrorKind.NONE);
            }
            this.ModuleContext = moduleContext;
        }

        //------------------------------------------------------------
        // �G���[���擾����B
        public ErrorKind GetErrorKind()
        {
            return mErrorKind;
        }

        //------------------------------------------------------------
        // �G���[�ƂȂ����g�[�N����Ԃ��B
        public Token GetErrorToken()
        {
            return mErrorToken;
        }

        //============================================================
        Lexer mLexer;
        Token mTokenPtr;
        ErrorKind mErrorKind = ErrorKind.NONE;
        Token mErrorToken = null;

        //------------------------------------------------------------
        // �~������Protection�̒l���擾����B
        static Protection getProtectionWithDefaultValue(Protection aValue, Protection aDefaultValue)
        {
            if (aValue == Protection.DEFAULT)
            {
                return aDefaultValue;
            }
            return aValue;
        }

        //------------------------------------------------------------
        // ConstantLiteral�B
        static bool isConstantLiteral(Token aT)
        {
            switch (aT.Value)
            {
                case Token.Kind.NumSInt32:
                case Token.Kind.NumUInt32:
                case Token.Kind.NumSInt64:
                case Token.Kind.NumUInt64:
                case Token.Kind.NumFloat32:
                case Token.Kind.NumFloat64:
                case Token.Kind.NumChar8:
                case Token.Kind.NumChar16:
                case Token.Kind.NumChar32:
                case Token.Kind.StringChar8:
                case Token.Kind.StringChar16:
                case Token.Kind.StringChar32:
                case Token.Kind.KeyTrue:
                case Token.Kind.KeyFalse:
                    return true;

                default:
                    return false;
            }
        }

        //------------------------------------------------------------
        // ���ݎ����Ă���Token�̎擾�B
        Token currentToken()
        {
            return mTokenPtr;
        }

        //------------------------------------------------------------
        // ���ݎ����Ă���Token���Đݒ肷��B
        void resetCurrentToken(Token aT)
        {
            mTokenPtr = aT;
            mErrorKind = ErrorKind.NONE;
            mErrorToken = null;
        }

        //------------------------------------------------------------
        // �ړ����A�V����Token���擾����B
        Token nextToken()
        {
            if (mTokenPtr.Next == null)
            {
                throw new Exception("Invalid operation");
            }
            mTokenPtr = mTokenPtr.Next;
            return mTokenPtr;
        }

        //------------------------------------------------------------
        // �G���[�̐ݒ�B
        void setErrorKind(ErrorKind aErrorKind)
        {
            setErrorKind(aErrorKind, currentToken());
        }
        void setErrorKind(ErrorKind aErrorKind,Token aErrorToken)
        {
            mErrorKind = aErrorKind;
            mErrorToken = aErrorToken;
        }

        //------------------------------------------------------------
        /// <summary>
        /// Module:
        ///   ModuleDecl ModuleDef
        ///   ModuleDecl ExternalModuleDecls ModuleDef
        /// </summary>
        bool parseModule(ModuleContext aModuleContext)
        {
            // �擪�͕K��ModuleDecl
            if (!parseModuleDecl(aModuleContext))
            {
                return false;
            }

            // �O�����W���[���g�p�錾
            if (!parseExternalModuleDecls(aModuleContext))
            {
                return false;
            }

            // Module�̒�`
            if (!parseModuleDef(aModuleContext))
            {
                return false;
            }

            return true;
        }

        //------------------------------------------------------------
        /// <summary>
        /// ModuleDecl:
        ///   "module" ModulePath ";"
        ///   "prototype" "module" ModulePath ";"
        /// </summary>
        bool parseModuleDecl(ModuleContext aModuleContext)
        {
            // 'prototype'
            bool isProtoType = false;
            if (currentToken().Value == Token.Kind.KeyPrototype)
            {
                isProtoType = true;

                nextToken();
            }

            // 'module'
            if (currentToken().Value != Token.Kind.KeyModule)
            {
                setErrorKind(ErrorKind.MODULE_DECL_KEYWORD_EXPECTED);
                return false;
            }
            nextToken();

            // ModulePath
            IdentPath modulePath = parseModulePath();
            if ( modulePath == null )
            {
                return false;
            }

            // ';'
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.MODULE_DECL_SEMICOLON_TERM_EXPECTED);
                return false;
            }
            nextToken();

            // ModuleDecl���쐬
            aModuleContext.ModuleDecl = new ModuleDecl(modulePath, isProtoType);

            return true;
        }

        //------------------------------------------------------------
        /// <summary>
        /// ModulePath:
        ///   Identifier "." IdentifierPath
        /// </summary>
        IdentPath parseModulePath()
        {
            return parseIdentPath(2);
        }

        //------------------------------------------------------------
        /// <summary>
        /// IdentPath:
        ///   (".") Identifier ("." Identifier)*
        /// </summary>
        IdentPath parseIdentPath()
        {
            return parseIdentPath(1);
        }
        IdentPath parseIdentPath(int aMinDepth)
        {
            TokenArray tokens = new TokenArray();
            bool fromRoot = false;
            if (currentToken().Value == Token.Kind.OpDot)
            {// root����
                fromRoot = true;
                nextToken();
            }
            while(true)
            {
                // Identifier
                if (currentToken().Value != Token.Kind.Identifier)
                {
                    setErrorKind(ErrorKind.IDENT_PATH_IDENTIFIER_EXPECTED);
                    return null;
                }
                tokens.Add(currentToken());
                nextToken();

                // dot
                if (currentToken().Value == Token.Kind.OpDot)
                {// ����Identifier�����ɍs��
                    nextToken();
                    continue;
                }

                if (tokens.Count < aMinDepth)
                {// �[��������Ȃ�
                    setErrorKind(ErrorKind.IDENT_PATH_DOT_EXPECTED);
                    return null;
                }
                break;
            }

            return new IdentPath(tokens, fromRoot);
        }

        //------------------------------------------------------------
        /// <summary>
        /// ExternalModuleDecls:
        ///   ExternalModuleDecl
        ///   ExternalModuleDecl ExternalModuleDecls
        /// 
        /// ExternalModuleDecl:
        ///   ImportExternalModule
        ///   UsingExternalModule
        /// 
        /// ImportExternalModule:
        ///   "import" ModulePath ";"
        ///    
        /// UsingExternalModule
        ///   "using" ModulePath ";"
        /// 
        /// </summary>
        bool parseExternalModuleDecls(ModuleContext aModuleContext)
        {
            List<ExternalModuleDecl> decls = new List<ExternalModuleDecl>();

            while(true)
            {
                // �L�[���[�h�`�F�b�N
                ExternalModuleDecl.Kind kind;
                if (currentToken().Value == Token.Kind.KeyImport)
                {// import
                    kind = ExternalModuleDecl.Kind.IMPORT;
                }
                else if (currentToken().Value == Token.Kind.KeyUsing)
                {// using
                    kind = ExternalModuleDecl.Kind.USING;
                }
                else
                {// exit
                    break;
                }
                nextToken();

                // �p�X���p�[�X
                IdentPath identPath = parseModulePath();
                if (identPath == null)
                {// �p�[�X���s
                    return false;
                }
                // �Z�~�R�����ŏI���͂�
                if (currentToken().Value != Token.Kind.OpSemicolon)
                {// ���s
                    setErrorKind(ErrorKind.EXTERNAL_MODULE_DECL_SEMICOLON_TERM_EXPECTED);
                    return false;
                }
                nextToken();

                // �ǉ�
                decls.Add(new ExternalModuleDecl(identPath, kind));
            }

            // �ݒ�
            aModuleContext.ExternalModuleDecls = new ExternalModuleDecls(decls);
            return true;
        }

        //------------------------------------------------------------
        /// <summary>
        /// ModuleDef:
        ///   StaticTypeDef:
        /// </summary>
        bool parseModuleDef(ModuleContext aModuleContext)
        {
            StaticTypeDef staticTypeDef = parseStaticTypeDef(Protection.Public,aModuleContext.ModuleDecl.IsProtoType);
            if (staticTypeDef == null)
            {
                return false;
            }
            if (staticTypeDef.TypeProtection != Protection.Public)
            {
                setErrorKind(ErrorKind.MODULE_DEF_TYPE_PROTECTION_MUST_BE_PUBLIC);
                return false;
            }

            // �ݒ�
            aModuleContext.ModuleDef = new ModuleDef(staticTypeDef);
            return true;
        }

        //------------------------------------------------------------
        /// <summary>
        /// StaticTypeDef:
        ///   ClassDef
        ///   EnumDef
        ///   InterfaceDef
        ///   PodDef
        ///   TypedefDef
        ///   UtilityDef
        /// </summary>
        StaticTypeDef parseStaticTypeDef(Protection aDefaultTypeProtection,bool aIsProtoType)
        {
            StaticTypeDef.Kind kind = StaticTypeDef.Kind.Unknown; 
            Protection typeProtection = Protection.DEFAULT;

            // �O�C�����
            while(true)
            {
                Token t = currentToken();
                {// typeProtection
                    Protection tokenTypeProtection = parseTypeProtection(t);
                    if (tokenTypeProtection != Protection.Unknown)
                    {
                        if (typeProtection != Protection.DEFAULT)
                        {
                            setErrorKind(ErrorKind.STATIC_TYPE_DEF_ALREADY_ASIGNED_TYPE_PROTECTION);
                            return null;
                        }
                        typeProtection = tokenTypeProtection;
                        nextToken();
                        continue;
                    }
                }
                {// kind
                    StaticTypeDef.Kind tokenStaticTypeKind = parseStaticTypeKind(t);
                    if (tokenStaticTypeKind != StaticTypeDef.Kind.Unknown)
                    {
                        if (kind != StaticTypeDef.Kind.Unknown)
                        {
                            setErrorKind(ErrorKind.STATIC_TYPE_DEF_ALREADY_ASIGNED_TYPE_KIND);
                            return null;
                        }
                        kind = tokenStaticTypeKind;
                        nextToken();
                        continue;
                    }
                }
                // exit
                break;
            }

            // �f�t�H���g�l��ݒ�
            typeProtection = getProtectionWithDefaultValue(typeProtection, aDefaultTypeProtection);

            // ��ނ̎w��m�F
            if (kind == StaticTypeDef.Kind.Unknown)
            {
                setErrorKind(ErrorKind.STATIC_TYPE_DEF_TYPE_KEYWORD_EXPECTED);
                return null;
            }

            // �^�̉��
            StaticTypeDef staticTypeDef = new StaticTypeDef(kind, typeProtection);
            if (kind == StaticTypeDef.Kind.Enum)
            {// enum
                if (!parseEnumDef(staticTypeDef))
                {
                    return null;
                }
            }
            else if (kind == StaticTypeDef.Kind.Typedef)
            {// typedef
                if (!parseTypedefDef(staticTypeDef))
                {
                    return null;
                }
            }
            else
            {// other
                if (!parseStaticTypeStandardDef(staticTypeDef,aIsProtoType))
                {
                    return null;
                }
            }
            return staticTypeDef;
        }

        //------------------------------------------------------------
        // TypeProtection
        Protection parseTypeProtection(Token aT)
        {
            switch (aT.Value)
            {
                case Token.Kind.KeyPublic:
                    return Protection.Public;

                case Token.Kind.KeyPrivate:
                    return Protection.Private;

                default:
                    return Protection.Unknown;
            }
        }

        //------------------------------------------------------------
        // StaticTypeKind
        StaticTypeDef.Kind parseStaticTypeKind(Token aT)
        {
            switch (aT.Value)
            {
                case Token.Kind.KeyClass:
                    return StaticTypeDef.Kind.Class;
                case Token.Kind.KeyEnum:
                    return StaticTypeDef.Kind.Enum;
                case Token.Kind.KeyInterface:
                    return StaticTypeDef.Kind.Interface;
                case Token.Kind.KeyPod:
                    return StaticTypeDef.Kind.Pod;
                case Token.Kind.KeyStruct:
                    return StaticTypeDef.Kind.Struct;
                case Token.Kind.KeyTypedef:
                    return StaticTypeDef.Kind.Typedef;
                case Token.Kind.KeyUtility:
                    return StaticTypeDef.Kind.Utility;

                default:
                    return StaticTypeDef.Kind.Unknown;
            }
        }

        //------------------------------------------------------------
        // EnumDef
        bool parseEnumDef(StaticTypeDef aTD)
        {
            // todo: impl
            return false;
        }
        
        //------------------------------------------------------------
        // TypedefDef
        bool parseTypedefDef(StaticTypeDef aTD)
        {
            // todo: impl
            return false;
        }

        //------------------------------------------------------------
        // StaticTypeDef
        bool parseStaticTypeStandardDef(StaticTypeDef aTD,bool aIsProtoType)
        {
            // �ŏ���Identifier�̂͂�
            if (currentToken().Value != Token.Kind.Identifier)
            {
                setErrorKind(ErrorKind.STATIC_TYPE_DEF_IDENTIFIER_EXPECTED);
                return false;
            }
            aTD.Ident = new Identifier(currentToken());
            nextToken();

            if (currentToken().Value == Token.Kind.OpColon)
            {// �p��
                if (aTD.TypeKind != StaticTypeDef.Kind.Class
                    || aTD.TypeKind != StaticTypeDef.Kind.Interface
                    )
                {
                    setErrorKind(ErrorKind.STATIC_TYPE_DEF_ONLY_CLASS_OR_INTERFACE_CAN_IMPLEMENT_INTERFACE);
                    return false;
                }
                nextToken();

                // IdentifierPath , �̃��[�v
                while(true)
                {
                    // �p������C���^�[�t�F�[�X�̃p�X
                    IdentPath typeIdentPath = parseIdentPath();
                    if (typeIdentPath == null)
                    {
                        setErrorKind(ErrorKind.STATIC_TYPE_DEF_IDENTIFIER_EXPECTED);
                        return false;
                    }

                    // �o�^
                    aTD.InheritTypeList.Add(new InheritType(typeIdentPath));

                    if (currentToken().Value == Token.Kind.OpColon)
                    {// ���̃C���^�[�t�F�[�X��
                        nextToken();
                        continue;
                    }

                    break;
                }
            }

            // �J�n����
            if (currentToken().Value != Token.Kind.OpLCurly)
            {
                setErrorKind(ErrorKind.STATIC_TYPE_DEF_LCURLY_EXPECTED);
                return false;
            }
            nextToken();

            // �����������[�v
            {
                Protection defaultProtection = Protection.Public;
                while (currentToken().Value != Token.Kind.OpRCurly
                    && currentToken().Value != Token.Kind.EOF
                    )
                {
                    // Protection
                    if ( parseTypeProtection(currentToken()) != Protection.Unknown
                        && currentToken().Next.Value == Token.Kind.OpColon
                        )
                    {// DefaultProtectionChange
                        defaultProtection = parseTypeProtection(currentToken());
                        if (!checkProtection(defaultProtection, aIsProtoType))
                        {
                            return false;
                        }
                        nextToken();
                        nextToken();
                        continue;
                    }

                    // Symbol
                    SymbolDef symbolDef = parseSymbolDef(aTD, defaultProtection, aIsProtoType);
                    if ( symbolDef == null )
                    {
                        return false;
                    }
                    aTD.SymbolDefList.Add(symbolDef);
                }
            }
            
            // �I�[����
            if (currentToken().Value != Token.Kind.OpRCurly)
            {
                setErrorKind(ErrorKind.STATIC_TYPE_DEF_RCURLY_EXPECTED);
                return false;
            }
            nextToken();

            // �Z�~�R����
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.STATIC_TYPE_DEF_SEMICOLON_EXPECTED);
                return false;
            }
            nextToken();

            return true;
        }

        //------------------------------------------------------------
        // �w��ł��Ȃ������̃`�F�b�N�Btrue���Ԃ��Ă�����G���[�����B
        bool checkErrorSymbolDefIllegalAttr(Token aToken, ErrorKind aErrorKind)
        {
            if (aToken != null)
            {
                setErrorKind(aErrorKind,aToken);
                return true;
            }
            return false;
        }

        //------------------------------------------------------------
        // SymbolDef�B
        SymbolDef parseSymbolDef(StaticTypeDef aTD, Protection aDefaultProtection,bool aIsProtoType)
        {
            Protection protect = Protection.DEFAULT;
            Token isAbstract = null;
            Token isConst = null;
            Token isOverride = null;
            Token isReadonly = null;
            Token isRef = null;
            Token isStatic = null;
            while(true)
            {// pre
                Token t = currentToken();
                {// attribute
                    {// ChangeProtection
                        Protection tmpProtection = parseTypeProtection(t);
                        if (tmpProtection != Protection.Unknown)
                        {// set protect
                            if (protect != Protection.DEFAULT)
                            {
                                setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASIGNED_TYPE_PROTECTION);
                                return null;
                            }
                            protect = tmpProtection;
                            if (!checkProtection(protect, aIsProtoType))
                            {
                                return null;
                            }
                            nextToken();
                            continue;
                        }
                    }
                    if (t.Value == Token.Kind.KeyAbstract)
                    {// SetAbstractAttribute
                        if (aTD.TypeKind != StaticTypeDef.Kind.Interface)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ONLY_INTERFACE_TYPE_CAN_ABSTRACT_ATTRIBUTE_MEMBER);
                            return null;
                        }
                        if (isAbstract != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_ABSTRACT);
                            return null;
                        }
                        isAbstract = currentToken();
                        nextToken();
                        continue;
                    }
                    if (t.Value == Token.Kind.KeyConst)
                    {// SetConstAttribute
                        if (isConst != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_CONST);
                            return null;
                        }
                        isConst = currentToken();
                        nextToken();
                        continue;
                    }
                    if (t.Value == Token.Kind.KeyOverride)
                    {// SetOverrideAttribute
                        if (aTD.TypeKind != StaticTypeDef.Kind.Class)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ONLY_CLASS_TYPE_CAN_OVERRIDE_ATTRIBUTE_MEMBER);
                            return null;
                        }
                        if (isConst != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_OVERRIDE);
                            return null;
                        }
                        isOverride = currentToken();
                        nextToken();
                        continue;
                    }
                    if (t.Value == Token.Kind.KeyRef)
                    {// SetRefAttribute
                        if (isRef != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_REF);
                            return null;
                        }
                        isRef = currentToken();
                        nextToken();
                        continue;
                    }
                    if (t.Value == Token.Kind.KeyReadonly)
                    {// SetReadonlyAttribute
                        if (isReadonly != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_READONLY);
                            return null;
                        }
                        isReadonly = currentToken();
                        nextToken();
                        continue;
                    }
                    if (t.Value == Token.Kind.KeyStatic)
                    {// SetStaticAttribute
                        if (isStatic != null)
                        {
                            setErrorKind(ErrorKind.SYMBOL_DEF_ALREADY_ASSIGNED_ATTRIBUTE_STATIC);
                            return null;
                        }
                        isStatic = currentToken();
                        nextToken();
                        continue;
                    }
                }

                // �V���{���̎����̑O����
                protect = getProtectionWithDefaultValue(protect, aDefaultProtection);

                {// StaticTypeDef
                    StaticTypeDef.Kind typeKind = parseStaticTypeKind(t);
                    if (typeKind != StaticTypeDef.Kind.Unknown)
                    {
                        // �����ȑ����̃`�F�b�N
                        if (checkErrorSymbolDefIllegalAttr(isAbstract, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            || checkErrorSymbolDefIllegalAttr(isConst, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            || checkErrorSymbolDefIllegalAttr(isOverride, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            || checkErrorSymbolDefIllegalAttr(isReadonly, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            || checkErrorSymbolDefIllegalAttr(isRef, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            || checkErrorSymbolDefIllegalAttr(isStatic, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_STATIC_TYPE_DEF)
                            )
                        {
                            return null;
                        }
                        // ���
                        StaticTypeDef staticTypeDef = parseStaticTypeDef(protect, aIsProtoType);
                        if (staticTypeDef == null)
                        {
                            return null;
                        }
                        return new SymbolDef(staticTypeDef);
                    }
                }
                {// StaticCtorDef,CtorDef
                    // todo: impl
                }
                {// StaticDtorDef,DtorDef
                    // todo: impl
                }
                {// MemberDef
                    // �߂�l�̌^�������͕ϐ��̌^
                    TypePath firstTypePath = parseTypePath();
                    if (firstTypePath != null)
                    {
                        // �����o��
                        if (currentToken().Value != Token.Kind.Identifier)
                        {
                            setErrorKind(ErrorKind.MEMBER_DEF_IDENTIFIER_EXPECTED);
                            return null;
                        }
                        Identifier nameIdent = new Identifier(currentToken());
                        nextToken();
                        
                        if (currentToken().Value == Token.Kind.OpAssign
                            || currentToken().Value == Token.Kind.OpSemicolon
                            )
                        {// '=' or ';' �Ȃ�MemberVariableDecl
                            // �G���[�`�F�b�N
                            if (checkErrorSymbolDefIllegalAttr(isAbstract, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_MEMBER_VARIABLE_DECL)
                                || checkErrorSymbolDefIllegalAttr(isOverride, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_MEMBER_VARIABLE_DECL)
                                || checkErrorSymbolDefIllegalAttr(isRef, ErrorKind.SYMBOL_DEF_ILLEGAL_ATTRIBUTE_FOR_MEMBER_VARIABLE_DECL)
                                )
                            {
                                return null;
                            }
                            if (isStatic == null)
                            {
                                if (aTD.TypeKind == StaticTypeDef.Kind.Interface)
                                {// interface�^�͔�static�����o�ϐ������ĂȂ�
                                    setErrorKind(ErrorKind.MEMBER_VARIABLE_DECL_INTERFACE_CANT_HAVE_NONSTATIC_MEMBER_VARIABLE);
                                    return null;
                                }
                                if (aTD.TypeKind == StaticTypeDef.Kind.Utility)
                                {// utility�^�͔�static�����o�ϐ������ĂȂ�
                                    setErrorKind(ErrorKind.MEMBER_VARIABLE_DECL_UTILITY_CANT_HAVE_NONSTATIC_MEMBER_VARIABLE);
                                    return null;
                                }
                            }

                            // �E��
                            IExpression expr = null;
                            if (currentToken().Value == Token.Kind.OpAssign)
                            {
                                nextToken();

                                // �E�Ӊ��
                                expr = parseConditionalExpression();
                            }

                            // ';'
                            if (currentToken().Value != Token.Kind.OpSemicolon)
                            {
                                setErrorKind(ErrorKind.SYMBOL_DEF_SEMICOLON_EXPECTED);
                                return null;
                            }
                            nextToken();

                            return new SymbolDef(
                                new MemberVariableDecl(
                                    new VariableDecl(firstTypePath, nameIdent, expr)
                                    , isStatic != null
                                    , isConst != null
                                    , isReadonly != null
                                    )
                                );
                        }
                        else if (currentToken().Value == Token.Kind.OpLParen)
                        {// '(' �Ȃ�MemberFunctionDecl

                            // �߂�l
                            FunctionReturnValueDecl retValueDecl = new FunctionReturnValueDecl(
                                firstTypePath
                                , isConst != null
                                , isRef != null
                                );

                            // �������X�g
                            FunctionArgumentDeclList argDeclList = parseFunctionArgumentDeclList();
                            if (argDeclList == null)
                            {
                                return null;
                            }

                            // ) �̌��const
                            Token isFunctionConst = null;
                            if (currentToken().Value == Token.Kind.KeyConst)
                            {
                                isFunctionConst = currentToken();
                                nextToken();
                            }

                            // '{' or ';'
                            BlockStatement blockStatement = null;
                            if (isAbstract != null || aIsProtoType)
                            {// �錾�̂݁B�Z�~�R����������͂��B
                                if (currentToken().Value != Token.Kind.OpSemicolon)
                                {
                                    setErrorKind(ErrorKind.MEMBER_FUNCTION_DECL_SEMICOLON_EXPECTED);
                                    return null;
                                }
                                nextToken();
                            }
                            else
                            {// non abstract function
                                if (currentToken().Value != Token.Kind.OpLCurly)
                                {
                                    setErrorKind(ErrorKind.MEMBER_FUNCTION_DECL_LCURLY_EXPECTED);
                                    return null;
                                }
                                // BlockStatement
                                blockStatement = parseBlockStatement();
                                if (blockStatement == null)
                                {
                                    return null;
                                }
                            }

                            // �쐬
                            return new SymbolDef(new MemberFunctionDecl(
                                nameIdent
                                , retValueDecl
                                , argDeclList
                                , blockStatement
                                , isAbstract != null
                                , isFunctionConst != null
                                , isOverride != null
                                , isStatic != null
                                ));
                        }
                        else
                        {// �G���[
                            setErrorKind(ErrorKind.SYMBOL_DEF_ILLEGAL_MEMBER_SYNTAX);
                            return null;
                        }
                    }
                }
                break;
            }
            return null;
        }


        //------------------------------------------------------------
        // �֐��̈������X�g�B
        // currentToken()��'('�̈ʒu�ɃZ�b�g���Ă������ƁB
        FunctionArgumentDeclList parseFunctionArgumentDeclList()
        {
            // '('
            if (currentToken().Value != Token.Kind.OpLParen)
            {
                setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_LIST_LPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // ArgDeclList
            FunctionArgumentDeclList argDeclList = new FunctionArgumentDeclList();
            while (currentToken().Value != Token.Kind.OpRParen)
            {
                // 2�߈ȍ~��',' ������͂�
                if (argDeclList.Count() != 0)
                {
                    if (currentToken().Value != Token.Kind.OpComma)
                    {
                        setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_LIST_COMMA_EXPECTED);
                        return null;
                    }
                }
                nextToken();

                // FunctionArgumentDecl
                FunctionArgumentDecl argDecl = parseFunctionArgumentDecl();
                if (argDecl == null)
                {
                    return null;
                }
                argDeclList.Add(argDecl);
            }
            nextToken();

            return argDeclList;
        }

        //------------------------------------------------------------
        // �֐��̂P�̈����̐錾�B
        FunctionArgumentDecl parseFunctionArgumentDecl()
        {
            Token isConst = null;
            Token isRef = null;
            Token isIn = null;
            while (true)
            {
                if (currentToken().Value == Token.Kind.KeyConst)
                {
                    if (isConst != null)
                    {
                        setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_CONST);
                        return null;
                    }
                    isConst = currentToken();
                }
                else if (currentToken().Value == Token.Kind.KeyRef)
                {
                    if (isRef != null)
                    {
                        setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_REF);
                        return null;
                    }
                    isRef = currentToken();
                }
                else if (currentToken().Value == Token.Kind.KeyIn)
                {
                    if (isIn != null)
                    {
                        setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_ALREADY_ASSIGNED_ATTRIBUTE_IN);
                        return null;
                    }
                    isIn = currentToken();
                }
                else
                {
                    break;
                }
                nextToken();
            }
             
            TypePath typePath = parseTypePath();
            if (typePath == null)
            {
                return null;
            }
            Identifier ident = null;
            if (currentToken().Value != Token.Kind.OpColon
                && currentToken().Value != Token.Kind.OpRParen
                )
            {
                if (currentToken().Value != Token.Kind.Identifier)
                {
                    setErrorKind(ErrorKind.FUNCTION_ARGUMENT_DECL_IDENTIFIER_EXPECTED);
                    return null;
                }
                ident = new Identifier(currentToken());
                nextToken();
            }
            return new FunctionArgumentDecl(
                typePath
                , ident
                , isConst != null
                , isRef != null
                , isIn != null
                );
        }

        //------------------------------------------------------------
        // TypePath�B
        TypePath parseTypePath()
        {
            // todo:
            // �z�񉉎Z�q�Ή�
            // �I�u�W�F�N�g�n���h���Ή�

            Token t = currentToken();
            TypePath typePath = null;
            if (t.Value == Token.Kind.Identifier)
            {// identPath
                IdentPath identPath = parseIdentPath(); ;
                if (identPath != null)
                {
                    typePath = new TypePath(identPath);
                }
            }
            else
            {// builtInType?
                BuiltInType builtinType = ParseBuiltInType(t);
                if (builtinType != BuiltInType.Unknown)
                {
                    typePath = new TypePath(t, builtinType);
                    nextToken();
                }
            }
            return typePath;
        }

        //------------------------------------------------------------
        /// <summary>
        /// �w��̃g�[�N���̎�ނł��邱�Ƃ����҂���B
        /// ���Ғʂ�ł���΃g�[�N�������ɐi��true��Ԃ��B
        /// ���Ғʂ�łȂ���Ύw��̃G���[��ݒ肵false��Ԃ��B
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="errorKind"></param>
        /// <returns></returns>
        bool expectToken(Token.Kind aKind, ErrorKind aErrorKind)
        {
            if (currentToken().Value != aKind)
            {
                setErrorKind(aErrorKind);
                return false;
            }
            nextToken();
            return true;
        }

        //------------------------------------------------------------
        // Protection�ɕs�����Ȃ����`�F�b�N����Btrue�Ȃ�G���[�Ȃ��B
        bool checkProtection(Protection aProtection, bool aIsProtoType)
        {
            if (aIsProtoType && aProtection == Protection.Private)
            {// ProtoType��Public��p
                setErrorKind(ErrorKind.PROTECTION_CANT_USE_PRIVATE_IN_PROTOTYPE_MODULE);
                return false;
            }
            return true;
        }
    }
}
