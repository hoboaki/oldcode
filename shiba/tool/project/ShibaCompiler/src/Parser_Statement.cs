using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Statementの実装。
    /// </summary>
    partial class Parser
    {
        //------------------------------------------------------------
        /// <summary>
        /// BlockStatement:
        ///   "{" (Statement)* "}"
        /// </summary>
        /// <returns></returns>
        BlockStatement parseBlockStatement()
        {
            // "{"
            if (currentToken().Value != Token.Kind.OpLCurly)
            {
                setErrorKind(ErrorKind.BLOCK_STATEMENT_LCURLY_EXPECTED);
                return null;
            }
            nextToken();

            BlockStatement blockStatement = new BlockStatement();
            while (currentToken().Value != Token.Kind.OpRCurly)
            {
                IStatement statement = parseStatement();
                if (statement == null)
                {
                    return null;
                }
                blockStatement.Add(statement);
            }
            nextToken();

            return blockStatement;
        }

        //------------------------------------------------------------
        // Statementのパース。
        IStatement parseStatement()
        {
            if (false) { }
            else if (currentToken().Value == Token.Kind.OpLCurly)
            {// BlockStatement
                return parseBlockStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyWhile)
            {// WhileStatement
                return parseWhileStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyBreak)
            {// BreakStatement
                return parseBreakStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyContinue)
            {// ContinueStatement
                return parseContinueStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyIf)
            {// IfStatement
                return parseIfStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyDo)
            {// DoWhileStatement
                return parseDoWhileStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyFor)
            {// ForStatement
                return parseForStatement();
            }
            else if (currentToken().Value == Token.Kind.KeyReturn)
            {// ReturnStatement
                return parseReturnStatement();
            }
            else
            {// DeclarationStatement or ExpressionStatement
                // resetするためにTokenをバックアップ
                Token backupToken = currentToken();

                IStatement statement = parseDeclarationStatement();
                if (statement == null)
                {// ExpressionStatementに再挑戦
                    resetCurrentToken(backupToken);
                    statement = parseExpressionStatement();
                }
                return statement;
            }

        }

        //------------------------------------------------------------　
        // DeclarationStatement。
        DeclarationStatement parseDeclarationStatement()
        {
            // 属性のパース
            Token isConst = null;
            while (true)
            {
                if (currentToken().Value == Token.Kind.KeyConst)
                {// const
                    if (isConst == null)
                    {
                        setErrorKind(ErrorKind.DECLARATION_STATEMENT_ALREADY_ASSIGNED_ATTRIBUTE_CONST);
                        return null;
                    }
                    isConst = currentToken();
                }
                else
                {
                    break;
                }
                nextToken();
            }

            // TypePath
            TypePath typePath = parseTypePath();
            if (typePath == null)
            {
                return null;
            }

            // Identifier
            if (currentToken().Value != Token.Kind.Identifier)
            {
                return null;
            }
            Identifier ident = new Identifier(currentToken());
            nextToken();

            // '='
            IExpression expr = null;
            if (currentToken().Value == Token.Kind.OpAssign)
            {
                nextToken();

                // Expression
                expr = parseExpression();
                if (expr == null)
                {
                    return null;
                }
            }

            // ';'
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.DECLARATION_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            nextToken();

            return new DeclarationStatement(
                new VariableDecl(typePath, ident, expr)
                , isConst != null
                );
        }

        //------------------------------------------------------------
        /// <summary>
        /// ExpressionStatement:
        ///   Expression ";"
        /// </summary>
        /// <returns></returns>
        ExpressionStatement parseExpressionStatement()
        {
            // Expression
            IExpression expr = parseExpression();
            if (expr == null)
            {
                return null;
            }

            // ';'
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.EXPRESSION_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            nextToken();

            return new ExpressionStatement(expr);
        }

        //------------------------------------------------------------
        /// <summary>
        /// WhileStatement:
        ///   "while" "(" Expression ")" Statement
        /// </summary>
        /// <returns></returns>
        WhileStatement parseWhileStatement()
        {
            // whileをスキップ
            nextToken();

            // "("
            if (currentToken().Value != Token.Kind.OpLParen)
            {
                setErrorKind(ErrorKind.WHILE_STATEMENT_LPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Expression
            IExpression expr = parseExpression();
            if (expr == null)
            {
                return null;
            }

            // ")"
            if (currentToken().Value != Token.Kind.OpRParen)
            {
                setErrorKind(ErrorKind.WHILE_STATEMENT_RPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Statement
            IStatement statement = parseStatement();
            if (statement == null)
            {
                return null;
            }

            return new WhileStatement(expr,statement);
        }

        //------------------------------------------------------------
        /// <summary>
        /// BreakStatement:
        ///   "break" ";"
        /// </summary>
        /// <returns></returns>
        BreakStatement parseBreakStatement()
        {
            // breakをスキップ
            nextToken();

            // ";"
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.BREAK_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            Token opToken = currentToken();
            nextToken();

            return new BreakStatement(opToken);
        }

        //------------------------------------------------------------
        /// <summary>
        /// ContinueStatement:
        ///   "continue" ";"
        /// </summary>
        /// <returns></returns>
        ContinueStatement parseContinueStatement()
        {
            // continueをスキップ
            nextToken();

            // ";"
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.CONTINUE_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            Token opToken = currentToken();
            nextToken();

            return new ContinueStatement(opToken);
        }

        //------------------------------------------------------------
        /// <summary>
        /// IfStatement:
        ///   "if" "(" Expression ")" Statement
        ///   "if" "(" Expression ")" Statement "else" Statement
        /// </summary>
        /// <returns></returns>
        IfStatement parseIfStatement()
        {
            // ifをスキップ
            nextToken();

            // "("
            if (currentToken().Value != Token.Kind.OpLParen)
            {
                setErrorKind(ErrorKind.IF_STATEMENT_LPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Expression
            IExpression expr = parseExpression();
            if (expr == null)
            {
                return null;
            }

            // ")"
            if (currentToken().Value != Token.Kind.OpRParen)
            {
                setErrorKind(ErrorKind.IF_STATEMENT_RPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Statement
            IStatement thenStatement = parseStatement();
            if (thenStatement == null)
            {
                return null;
            }

            // "else"があるか
            IStatement elseStatement = null;
            if (currentToken().Value == Token.Kind.KeyElse)
            {
                // elseがある。まずトークンを進める
                nextToken();

                // 文の解析
                elseStatement = parseStatement();
                if (elseStatement == null)
                {
                    return null;
                }
            }
            
            // 作成
            return new IfStatement(expr, thenStatement, elseStatement);
        }

        //------------------------------------------------------------
        /// <summary>
        /// DoWhileStatement:
        ///   "do" Statement "while" (" Expression ")" ";"
        /// </summary>
        /// <returns></returns>
        DoWhileStatement parseDoWhileStatement()
        {
            // doをスキップ
            nextToken();

            // Statement
            IStatement statement = parseStatement();
            if (statement == null)
            {
                return null;
            }

            // while
            if (currentToken().Value != Token.Kind.KeyWhile)
            {
                setErrorKind(ErrorKind.DOWHILE_STATEMENT_WHILE_EXPECTED);
                return null;
            }
            nextToken();

            // "("
            if (currentToken().Value != Token.Kind.OpLParen)
            {
                setErrorKind(ErrorKind.DOWHILE_STATEMENT_LPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Expression
            IExpression expr = parseExpression();
            if (expr == null)
            {
                return null;
            }

            // ")"
            if (currentToken().Value != Token.Kind.OpRParen)
            {
                setErrorKind(ErrorKind.DOWHILE_STATEMENT_RPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // ";"
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.DOWHILE_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            nextToken();

            return new DoWhileStatement(expr, statement);
        }

        //------------------------------------------------------------
        /// <summary>
        /// ForStatement:
        ///   "for" "(" ForInitialize ForCondition ForIncrement ")" Statement
        ///   
        /// ForInitialize:
        ///   Statement
        ///   ";"
        ///   
        /// ForCondition:
        ///   Expression
        ///   ";"
        /// 
        /// ForIncrement:
        ///   Expression
        ///   empty
        /// </summary>
        /// <returns></returns>
        ForStatement parseForStatement()
        {
            // forをスキップ
            nextToken();

            // "("
            if (currentToken().Value != Token.Kind.OpLParen)
            {
                setErrorKind(ErrorKind.FOR_STATEMENT_LPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // ForInitialize
            IStatement initStatement = null;
            if (currentToken().Value == Token.Kind.OpSemicolon)
            {
                nextToken();
            }
            else
            {
                initStatement = parseStatement();
                if (initStatement == null)
                {
                    return null;
                }
            }

            // ForCondition
            IExpression condExpr = null;
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                condExpr = parseExpression();
                if (condExpr == null)
                {
                    return null;
                }
            }
            // ';'
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.FOR_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            nextToken();

            // ForIncrement
            IExpression incExpr = null;
            if (currentToken().Value != Token.Kind.OpRParen)
            {
                incExpr = parseExpression();
                if (incExpr == null)
                {
                    return null;
                }
            }


            // ")"
            if (currentToken().Value != Token.Kind.OpRParen)
            {
                setErrorKind(ErrorKind.FOR_STATEMENT_RPAREN_EXPECTED);
                return null;
            }
            nextToken();

            // Statement
            IStatement statement = parseStatement();
            if (statement == null)
            {
                return null;
            }

            return new ForStatement(initStatement, condExpr, incExpr, statement);
        }

        //------------------------------------------------------------
        /// <summary>
        /// ReturnStatement:
        ///   "return" ";"
        ///   "return" Expression ";"
        /// </summary>
        /// <returns></returns>
        ReturnStatement parseReturnStatement()
        {
            // returnをスキップ
            nextToken();

            // Expression
            IExpression expr = null;
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                expr = parseExpression();
                if (expr == null)
                {
                    return null;
                }
            }

            // ";"
            if (currentToken().Value != Token.Kind.OpSemicolon)
            {
                setErrorKind(ErrorKind.RETURN_STATEMENT_SEMICOLON_EXPECTED);
                return null;
            }
            Token opToken = currentToken();
            nextToken();

            return new ReturnStatement(opToken,expr);
        }
    }
}
