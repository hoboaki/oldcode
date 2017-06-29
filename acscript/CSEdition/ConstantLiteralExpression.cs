using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 定数式。
    /// </summary>
    class ConstantLiteralExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ConstantLiteralExpression(Token aT)
        {
            mToken = aT;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ConstantLiteralExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mToken.Value", mToken.Value.ToString());
                switch (mToken.Value)
                {
                    case Token.Kind.StringChar8:
                    case Token.Kind.StringChar16:
                    case Token.Kind.StringChar32:
                        aTracer.WriteValue("value", mToken.UString);
                        break;

                    case Token.Kind.NumSInt32:
                    case Token.Kind.NumSInt64:
                        aTracer.WriteValue("value", mToken.Int64Value.ToString());
                        break;

                    case Token.Kind.NumUInt32:
                    case Token.Kind.NumUInt64:
                        aTracer.WriteValue("value", mToken.UInt64Value.ToString());
                        break;

                    case Token.Kind.NumFloat32:
                        aTracer.WriteValue("value", mToken.Float32Value.ToString());
                        break;

                    case Token.Kind.NumFloat64:
                        aTracer.WriteValue("value", mToken.Float64Value.ToString());
                        break;

                    default:
                        break;
                }
            }
        }

        //============================================================
        Token mToken;
    }
}
