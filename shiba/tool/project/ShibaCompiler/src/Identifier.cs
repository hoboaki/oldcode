using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            Assert.Check(aToken.Value == Token.Kind.Identifier);
            Token = aToken;
        }
        
        //------------------------------------------------------------
        // 同じかどうか判定。
        public bool IsSame(Identifier aIdent)
        {
            return Token.Identifier == aIdent.Token.Identifier;
        }

        //------------------------------------------------------------
        // 名前を取得する。
        public string String()
        {
            return Token.Identifier;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteValue(aName, String());
        }
    }
}
