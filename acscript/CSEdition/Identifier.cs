using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���ʎq�B
    /// </summary>
    class Identifier
    {
        public readonly Token Token;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public Identifier(Token aToken)
        {
            System.Diagnostics.Debug.Assert(aToken.Value == Token.Kind.Identifier);
            Token = aToken;
        }
        
        //------------------------------------------------------------
        // �������ǂ�������B
        public bool IsSame(Identifier aIdent)
        {
            return Token.Identifier == aIdent.Token.Identifier;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTrace, string aName)
        {
            aTrace.WriteValue(aName, Token.Identifier);
        }
    }
}
