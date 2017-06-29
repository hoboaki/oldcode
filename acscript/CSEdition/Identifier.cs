using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 識別子。
    /// </summary>
    class Identifier
    {
        public readonly Token Token;

        //------------------------------------------------------------
        // コンストラクタ。
        public Identifier(Token aToken)
        {
            System.Diagnostics.Debug.Assert(aToken.Value == Token.Kind.Identifier);
            Token = aToken;
        }
        
        //------------------------------------------------------------
        // 同じかどうか判定。
        public bool IsSame(Identifier aIdent)
        {
            return Token.Identifier == aIdent.Token.Identifier;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTrace, string aName)
        {
            aTrace.WriteValue(aName, Token.Identifier);
        }
    }
}
