using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Expression実装。
    /// </summary>
    partial class Parser
    {
        //------------------------------------------------------------
        /// <summary>
        /// AssignmentExpression:
        ///   (ConditionalExpression AssignmentOperator)* ConditionalExpression
        /// </summary>
        /// <returns></returns>
        IExpression parseAssignmentExpression()
        {
            // left
            IExpression leftExpr = parseConditionalExpression();
            if (leftExpr == null)
            {
                return null;
            }

            // op
            AssignmentExpression.OpKind opKind = parseAssignmentOperator();
            if (opKind == AssignmentExpression.OpKind.Unknown)
            {
                return leftExpr;
            }
            Token opToken = currentToken();
            nextToken();

            // right
            IExpression rightExpr = parseAssignmentExpression();
            if (rightExpr == null)
            {
                return null;
            }

            return new AssignmentExpression(opToken, opKind, leftExpr, rightExpr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// AssignmentOperator:
        ///   "=" | "+=" | "-=" | "*=" | "/=" | "%=" | >>=" | "<<=" | "&=" | "|=" | "^="
        /// </summary>
        /// <returns></returns>
        private AssignmentExpression.OpKind parseAssignmentOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpAssign:
                    return AssignmentExpression.OpKind.Assign;

                case Token.Kind.OpPlusAssign:
                    return AssignmentExpression.OpKind.AddAssign;

                case Token.Kind.OpMinusAssign:
                    return AssignmentExpression.OpKind.SubAssign;

                case Token.Kind.OpMulAssign:
                    return AssignmentExpression.OpKind.MulAssign;

                case Token.Kind.OpDivAssign:
                    return AssignmentExpression.OpKind.DivAssign;

                case Token.Kind.OpModAssign:
                    return AssignmentExpression.OpKind.ModAssign;

                case Token.Kind.OpRShiftAssign:
                    return AssignmentExpression.OpKind.RShiftAssign;

                case Token.Kind.OpLShiftAssign:
                    return AssignmentExpression.OpKind.LShiftAssign;

                case Token.Kind.OpAndAssign:
                    return AssignmentExpression.OpKind.AndAssign;

                case Token.Kind.OpOrAssign:
                    return AssignmentExpression.OpKind.OrAssign;

                case Token.Kind.OpXorAssign:
                    return AssignmentExpression.OpKind.XorAssign;

                default:
                    return AssignmentExpression.OpKind.Unknown;                    
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// Expression:
        ///   AssignmentExpression
        /// </summary>
        /// <returns></returns>
        IExpression parseExpression()
        {
            return parseAssignmentExpression();
        }

        //------------------------------------------------------------
        /// <summary>
        /// SequenceExpression:
        ///   Expression ("," Expression)*
        /// </summary>
        /// <returns></returns>
        SequenceExpression parseSequenceExpression()
        {
            // Expression
            IExpression expr = parseExpression();
            if (expr == null)
            {
                return null;
            }

            // ","
            if (currentToken().Value != Token.Kind.OpComma)
            {
                return new SequenceExpression(expr);
            }
            nextToken();

            // SequenceExpression
            SequenceExpression seqExpr = parseSequenceExpression();
            if (seqExpr == null)
            {
                return null;
            }
            return new SequenceExpression(expr, seqExpr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// FunctionCallExpression:
        ///   "(" SequenceExpression ")"
        /// </summary>
        /// <returns></returns>
        FunctionCallExpression parseFunctionCallExpression()
        {
            // "("
            Token opToken = currentToken();
            if (!expectToken(Token.Kind.OpLParen, ErrorKind.FUNCTION_CALL_EXPRESSION_LPAREN_EXPECTED))
            {
                return null;
            }
            if (currentToken().Value == Token.Kind.OpRParen)
            {// 引数無しの関数
                nextToken();
                return new FunctionCallExpression(opToken);
            }

            // SequenceExpression
            SequenceExpression sequenceExpr = parseSequenceExpression();
            if (sequenceExpr == null)
            {
                return null;
            }

            // ")"
            if (!expectToken(Token.Kind.OpRParen, ErrorKind.FUNCTION_CALL_EXPRESSION_RPAREN_EXPECTED))
            {
                return null;
            }

            return new FunctionCallExpression(opToken,sequenceExpr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// IndexExpression:
        ///   "[" SequenceExpression "]"
        /// </summary>
        /// <returns></returns>
        IndexExpression parseIndexExpression()
        {
            // "["
            if (!expectToken(Token.Kind.OpLBracket, ErrorKind.INDEX_EXPRESSION_LBRACKET_EXPECTED))
            {
                return null;
            }

            // SequenceExpression
            SequenceExpression sequenceExpr = parseSequenceExpression();
            if (sequenceExpr == null)
            {
                return null;
            }

            // "]"
            if (!expectToken(Token.Kind.OpRBracket, ErrorKind.INDEX_EXPRESSION_RBRACKET_EXPECTED))
            {
                return null;
            }

            return new IndexExpression(sequenceExpr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// NewExpression:
        ///   "new" TypePath FunctionCallExpression
        /// </summary>
        /// <returns></returns>
        IExpression parseNewExpression()
        {
            // "new"
            Assert.Check(currentToken().Value == Token.Kind.KeyNew);
            nextToken();

            // TypePath
            TypePath typePath = parseTypePath();
            if (typePath == null)
            {
                return null;
            }

            // FunctionCallExpression
            FunctionCallExpression funcCallExpr = parseFunctionCallExpression();
            if (funcCallExpr == null)
            {
                return null;
            }

            return new NewExpression(typePath, funcCallExpr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// DeleteExpression:
        ///   "delete" UnaryExpression
        /// </summary>
        /// <returns></returns>
        IExpression parseDeleteExpression()
        {
            // "delete"
            Assert.Check(currentToken().Value == Token.Kind.KeyDelete);
            nextToken();

            // UnaryExpression
            IExpression expr = parseUnaryExpression();
            if (expr == null)
            {
                return expr;
            }

            return new DeleteExpression(expr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// CastExpression:
        ///   "cast" "(" TypePath ")" UnaryExpression
        /// </summary>
        /// <returns></returns>
        IExpression parseCastExpression()
        {
            // "cast"
            Assert.Check(currentToken().Value == Token.Kind.KeyCast);
            nextToken();

            // "("
            if (!expectToken(Token.Kind.OpLParen,ErrorKind.CAST_EXPRESSION_LPAREN_EXPECTED))
            {
                return null;
            }

            // TypePath
            TypePath typePath = parseTypePath();
            if (typePath == null)
            {
                return null;
            }

            // ")"
            if (!expectToken(Token.Kind.OpRParen,ErrorKind.CAST_EXPRESSION_RPAREN_EXPECTED))
            {
                return null;
            }

            // UnaryExpression
            IExpression expr = parseUnaryExpression();
            if (expr == null)
            {
                return null;
            }

            return new CastExpression(typePath, expr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// PrimaryExpression:
        ///   Identifier # ScopeRootIdentExpression
        ///   | "." Identifier # NamespaceRootIdentExpression
        ///   | ConstantLiteral
        ///   | "(" Expression ")"
        /// </summary>
        /// <returns></returns>
        IExpression parsePrimaryExpression()
        {
            IExpression expr = null;
            if (currentToken().Value == Token.Kind.Identifier)
            {
                expr = new RootIdentExpression(new Identifier(currentToken()), false);
                nextToken();
            }
            else if (currentToken().Value == Token.Kind.OpDot)
            {
                if (currentToken().Value != Token.Kind.Identifier)
                {
                    setErrorKind(ErrorKind.PRIMARY_EXPRESSION_IDENTIFIER_EXPECTED);
                    return null;
                }
                expr = new RootIdentExpression(new Identifier(currentToken()), true);
                nextToken();
            }
            else if (isConstantLiteral(currentToken()))
            {
                expr = new ConstantLiteralExpression(currentToken());
                nextToken();
            }
            else
            {
                // "("
                if (!expectToken(Token.Kind.OpLParen, ErrorKind.PRIMARY_EXPRESSION_LPAREN_EXPECTED))
                {
                    return null;
                }

                // Expression
                expr = parseExpression();

                // ")"
                if (!expectToken(Token.Kind.OpRParen, ErrorKind.PRIMARY_EXPRESSION_RPAREN_EXPECTED))
                {
                    return null;
                }
            }

            return expr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// PostfixExpression:
        ///   PrimaryExpression (PostfixOperator)*
        ///   
        /// PostfixOperator:
        ///   "++"
        ///   | "--"
        ///   | IndexExpression
        ///   | FunctionCallExpression
        ///   | "." Identifier # ChildIdentExpression
        /// </summary>
        /// <returns></returns>
        IExpression parsePostfixExpression()
        {
            IExpression firstExpr = parsePrimaryExpression();

            while (firstExpr != null)
            {
                PostfixExpression.OpKind opKind = PostfixExpression.OpKind.Unknown;
                
                // OpTokenがある式
                Token opToken = currentToken();
                if (currentToken().Value == Token.Kind.OpPlusPlus)
                {// "++"
                    opKind = PostfixExpression.OpKind.Inc;
                    nextToken();
                }
                else if (currentToken().Value == Token.Kind.OpMinusMinus)
                {// "--"
                    opKind = PostfixExpression.OpKind.Dec;
                    nextToken();
                }
                if (opKind != PostfixExpression.OpKind.Unknown)
                {
                    firstExpr = new PostfixExpression(opToken, opKind, firstExpr);
                }
                
                // OpTokenがない式
                if (currentToken().Value == Token.Kind.OpLBracket)
                {// IndexExpression
                    IndexExpression expr = parseIndexExpression();
                    if (expr == null)
                    {
                        return null;
                    }
                    return new PostfixExpression(firstExpr, expr);
                }
                else if (currentToken().Value == Token.Kind.OpLParen)
                {// FunctionCallExpression
                    FunctionCallExpression expr = parseFunctionCallExpression();
                    if (expr == null)
                    {
                        return null;
                    }
                    return new PostfixExpression(firstExpr, expr);
                }
                else if (currentToken().Value == Token.Kind.OpDot)
                {// ChildIdentExpression
                    nextToken();
                    if (currentToken().Value != Token.Kind.Identifier)
                    {
                        setErrorKind(ErrorKind.POSTFIX_EXPRESSION_IDENTIFIER_EXPECTED);
                        return null;
                    }
                    ChildIdentExpression expr = new ChildIdentExpression(new Identifier(currentToken()));
                    nextToken();
                    return new PostfixExpression(firstExpr, expr);
                }
                else
                {
                    return firstExpr;
                }
            }

            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// UnaryExpression:
        ///   (UnaryOperator)* (CastExpression | NewExpression | DeleteExpression | PostfixExpression)
        /// 
        /// </summary>
        /// <returns></returns>
        IExpression parseUnaryExpression()
        {
            // 単項演算子解析
            {
                UnaryOpExpression.OpKind opKind = parseUnaryOperator();
                if (opKind != UnaryOpExpression.OpKind.Unknown)
                {
                    Token opToken = currentToken();
                    nextToken();
                    IExpression unaryExpr = parseUnaryExpression();
                    if (unaryExpr == null)
                    {
                        return null;
                    }
                    return new UnaryOpExpression(opToken, opKind, unaryExpr);
                }
            }

            // 右辺解析
            IExpression expr = null;
            if (currentToken().Value == Token.Kind.KeyNew)
            {
                expr = parseNewExpression();
            }
            else if (currentToken().Value == Token.Kind.KeyDelete)
            {
                expr = parseDeleteExpression();
            }
            else if (currentToken().Value == Token.Kind.KeyCast)
            {
                expr = parseCastExpression();
            }
            else
            {
                expr = parsePostfixExpression();
            }
            if (expr == null)
            {// エラー
                return null;
            }

            return expr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// UnaryOperator:
        ///   "++" | "--" | "+" | "-" | "!" | "~" | "@"
        /// </summary>
        /// <returns></returns>
        UnaryOpExpression.OpKind parseUnaryOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpPlusPlus:
                    return UnaryOpExpression.OpKind.Inc;

                case Token.Kind.OpMinusMinus:
                    return UnaryOpExpression.OpKind.Dec;

                case Token.Kind.OpPlus:
                    return UnaryOpExpression.OpKind.Positive;

                case Token.Kind.OpMinus:
                    return UnaryOpExpression.OpKind.Negative;

                case Token.Kind.OpNot:
                    return UnaryOpExpression.OpKind.LogicalNot;

                case Token.Kind.OpTilde:
                    return UnaryOpExpression.OpKind.BitwiseNot;

                default:
                    return UnaryOpExpression.OpKind.Unknown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// MultiplicativeExpression:
        ///   CastExpression (MultiplicativeOperator CastExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseMultiplicativeExpression()
        {
            // 第1式
            IExpression firstExpr = parseUnaryExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseMultiplicativeOperator();
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseMultiplicativeOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseUnaryExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// MultiplicativeOperator:
        ///   "*" | "/" | "%"
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseMultiplicativeOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpMul:
                    return BinaryOpExpression.OpKind.MultiplicativeMul;

                case Token.Kind.OpDiv:
                    return BinaryOpExpression.OpKind.MultiplicativeDiv;

                case Token.Kind.OpMod:
                    return BinaryOpExpression.OpKind.MultiplicativeMod;

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// AdditiveExpression:
        ///   MultiplicativeExpression (AdditiveOperator MultiplicativeExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseAdditiveExpression()
        {
            // 第1式
            IExpression firstExpr = parseMultiplicativeExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseAdditiveOperator();
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseAdditiveOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseMultiplicativeExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// AdditiveOperator:
        ///   "+" | "-"
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseAdditiveOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpPlus:
                    return BinaryOpExpression.OpKind.AdditiveAdd;

                case Token.Kind.OpMinus:
                    return BinaryOpExpression.OpKind.AdditiveSub;

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// ShiftExpression:
        ///   AdditiveExpression (ShiftOperator AdditiveExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseShiftExpression()
        {
            // 第1式
            IExpression firstExpr = parseAdditiveExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseShiftOperator();
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseShiftOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseAdditiveExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// ShiftOperator:
        ///   "<<" | ">>"
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseShiftOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpLShift:
                    return BinaryOpExpression.OpKind.ShiftLeft;

                case Token.Kind.OpRShift:
                    return BinaryOpExpression.OpKind.ShiftRight;

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// RelationalExpression:
        ///   ShiftExpression (RelationalOperator ShiftExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseRelationalExpression()
        {
            // 第1式
            IExpression firstExpr = parseShiftExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseRelationalOperator(); 
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseRelationalOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseShiftExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// RelationalOperator:
        ///   "<" | "<=" | ">" | ">="
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseRelationalOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpGreater:
                    return BinaryOpExpression.OpKind.RelationalGreater;

                case Token.Kind.OpGreaterEqual:
                    return BinaryOpExpression.OpKind.RelationalGreaterEqual;

                case Token.Kind.OpLess:
                    return BinaryOpExpression.OpKind.RelationalLess;

                case Token.Kind.OpLessEqual:
                    return BinaryOpExpression.OpKind.RelationalLessEqual;

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// EqualityExpression:
        ///   RelationalExpression (EqulityOperator RelationalExpression)* 
        /// </summary>
        /// <returns></returns>
        IExpression parseEqualityExpression()
        {
            // 第1式
            IExpression firstExpr = parseRelationalExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseEqualityOperator(); 
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseEqualityOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseRelationalExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// EqualityOperator:
        ///   "==" | "!="
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseEqualityOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.OpEqual:
                    return BinaryOpExpression.OpKind.EqualityEqual;

                case Token.Kind.OpNotEqual:
                    return BinaryOpExpression.OpKind.EqualityNotEqual;

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }

        //------------------------------------------------------------
        /// <summary>
        /// IdentityExpression:
        ///   EqualityExpression (IdentityOperator EqualityExpression )* 
        /// </summary>
        /// <returns></returns>
        IExpression parseIdentityExpression()
        {
            // 第1式
            IExpression firstExpr = parseEqualityExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            for (BinaryOpExpression.OpKind opKind = parseIdentityOperator();
                opKind != BinaryOpExpression.OpKind.Unknnown;
                opKind = parseEqualityOperator()
                )
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseEqualityExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, opKind, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// IdentityOperator:
        ///   "is" | "!" "is"
        /// </summary>
        /// <returns></returns>
        BinaryOpExpression.OpKind parseIdentityOperator()
        {
            switch (currentToken().Value)
            {
                case Token.Kind.KeyIs:
                    return BinaryOpExpression.OpKind.IdentityEqual;

                case Token.Kind.OpNot:
                    {
                        if (currentToken().Next.Value == Token.Kind.KeyIs)
                        {
                            return BinaryOpExpression.OpKind.IdentityNotEqual;
                        }
                        else
                        {
                            return BinaryOpExpression.OpKind.Unknnown;
                        }
                    }

                default:
                    return BinaryOpExpression.OpKind.Unknnown;
            }
        }
        //------------------------------------------------------------
        /// <summary>
        /// BitwiseAndExpression:
        ///   IdentityExpression (BitwiseAndOperator IdentityExpression )* 
        /// </summary>
        /// <returns></returns>
        IExpression parseBitwiseAndExpression()
        {
            // 第1式
            IExpression firstExpr = parseIdentityExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while (currentToken().Value == Token.Kind.OpAnd)
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseIdentityExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, BinaryOpExpression.OpKind.BitwiseAnd, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// BitwiseXorExpression:
        ///   BitwiseAndExpression (BitwiseXorOperator BitwiseAndExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseBitwiseXorExpression()
        {
            // 第1式
            IExpression firstExpr = parseBitwiseAndExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while (currentToken().Value == Token.Kind.OpXor)
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseBitwiseAndExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, BinaryOpExpression.OpKind.BitwiseXor, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// BitwiseOrExpression:
        ///   BitwiseXorExpression (BitwiseOrOperator BitwiseXorExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseBitwiseOrExpression()
        {
            // 第1式
            IExpression firstExpr = parseBitwiseXorExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while (currentToken().Value == Token.Kind.OpOr)
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseBitwiseXorExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, BinaryOpExpression.OpKind.BitwiseOr, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// LogicalAndExpression:
        ///   BitwiseOrExpression (LogicalAndOperator BitwiseOrExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseLogicalAndExpression()
        {
            // 第1式
            IExpression firstExpr = parseBitwiseOrExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while (currentToken().Value == Token.Kind.OpAndAnd)
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();

                // 第2式
                IExpression secondExpr = parseBitwiseOrExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, BinaryOpExpression.OpKind.LogicalAnd, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// LogicalOrExpression:
        ///   LogicalAndExpression (LogicalOrOperator LogicalAndExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseLogicalOrExpression()
        {
            // 第1式
            IExpression firstExpr = parseLogicalAndExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while(currentToken().Value == Token.Kind.OpOrOr)
            {
                // トークンをメモ
                Token opToken = currentToken();

                // 次のトークンへ
                nextToken();
                
                // 第2式
                IExpression secondExpr = parseLogicalAndExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }

                // 第1式を更新
                firstExpr = new BinaryOpExpression(opToken, BinaryOpExpression.OpKind.LogicalOr, firstExpr, secondExpr);
            }
            return firstExpr;
        }

        //------------------------------------------------------------
        /// <summary>
        /// ConditionalExpression:
        ///   LogicalOrExpression ("?" ConditionalExpression ":" ConditionalExpression)*
        /// </summary>
        /// <returns></returns>
        IExpression parseConditionalExpression()
        {
            // 第1式
            IExpression firstExpr = parseLogicalOrExpression();
            if (firstExpr == null)
            {// エラー
                return null;
            }
            while(currentToken().Value == Token.Kind.OpQuestion)
            {
                nextToken();

                // 第2式
                IExpression secondExpr = parseConditionalExpression();
                if (secondExpr == null)
                {// エラー
                    return null;
                }
                if (!expectToken(Token.Kind.OpColon,ErrorKind.CONDITIONAL_EXPRESSION_COLON_EXPECTED))
                {// ':' じゃない
                    return null;
                }

                // 第3式
                IExpression thirdExpr = parseConditionalExpression();
                if (thirdExpr == null)
                {// エラー
                    return null;
                }
                
                // 第1式を更新
                firstExpr = new ConditionalExpression(firstExpr, secondExpr, thirdExpr);
            }
            return firstExpr;
        }
    }
}
