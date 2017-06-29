using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���ʎq�̃p�X�BIdentifierPath�B
    /// </summary>
    class IdentPath
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public IdentPath( TokenArray aTokens , bool aFromRoot )
        {
            idents = new List<Identifier>();
            fromRoot = aFromRoot;

            // �S��Identifier���`�F�b�N
            foreach (Token token in aTokens)
            {
                idents.Add(new Identifier(token));
            }
        }

        //------------------------------------------------------------
        // Ident�̑����B
        public int Count
        {
            get { return idents.Count; }
        }

        //------------------------------------------------------------
        // �w��Ԗڂ�Ident���擾����B
        public Identifier At(int aIndex)
        {
            return idents[aIndex];
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace(Tracer aTracer, string aName)
        {
            // �����񏀔�
            string str = "";
            foreach (Identifier ident in idents)
            {
                // '.'
                str += (str.Length != 0 || fromRoot ? "." : "") + ident.Token.Identifier;
            }

            // ��������
            aTracer.WriteValue(aName,str);
        }

        //============================================================
        private List< Identifier > idents;
        private bool fromRoot;
    }
}
