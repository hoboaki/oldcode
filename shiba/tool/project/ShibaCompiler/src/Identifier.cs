using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            Assert.Check(aToken.Value == Token.Kind.Identifier);
            Token = aToken;
        }
        
        //------------------------------------------------------------
        // �������ǂ�������B
        public bool IsSame(Identifier aIdent)
        {
            return Token.Identifier == aIdent.Token.Identifier;
        }

        //------------------------------------------------------------
        // ���O���擾����B
        public string String()
        {
            return Token.Identifier;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteValue(aName, String());
        }
    }
}
