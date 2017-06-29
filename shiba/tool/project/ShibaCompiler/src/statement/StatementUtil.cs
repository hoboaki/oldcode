using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Statementで使う便利関数。
    /// </summary>
    class StatementUtil
    {
        //------------------------------------------------------------
        // 式の結果がboolであることをチェックする。
        static public void CheckBoolExpression(
            SemanticAnalyzeComponent aComp
            , IEvaluateNode aExprEN
            , IExpression aExpr
            )
        {
            if (aExprEN.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                || aExprEN.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
            {
                throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                    SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                    , aComp.TypeSymbolNode.ModuleContext()
                    , aExpr.GetToken()
                    , aExprEN.GetEvaluateInfo().TypeInfo
                    , new TypeInfo(new TypeInfo.TypeSymbol(null, BuiltInType.Bool), new TypeInfo.TypeAttribute(true, false))
                    ));
            }
        }
    }
}
