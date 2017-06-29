using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 変数の宣言。
    /// </summary>
    class VariableDecl
    {
        public readonly TypePath TypePath;
        public readonly Identifier Ident;
        public readonly IExpression Expr;

        //------------------------------------------------------------
        // コンストラクタ。
        public VariableDecl(TypePath aTypePath, Identifier aIdent, IExpression aExpr)
        {
            TypePath = aTypePath;
            Ident = aIdent;
            Expr = aExpr;
        }

        //------------------------------------------------------------
        // 初期化式（nullの場合もある）。
        public IExpression Expression()
        {
            return Expr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, String aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypePath.Trace(aTracer, "TypePath");
                Ident.Trace(aTracer, "Ident");
                if (Expr == null)
                {
                    aTracer.WriteValue("Expr", "null");
                }
                else
                {
                    aTracer.WriteName("Expr");
                    using (new Tracer.IndentScope(aTracer))
                    {
                        Expr.Trace(aTracer);
                    }
                }
            }
        }
    }
}
