using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �g���[�X���T�|�[�g����N���X�B
    /// </summary>
    class Tracer
    {
        //------------------------------------------------------------
        // �C���f���g���x�����R���g���[������T�|�[�g�N���X�B
        public class IndentScope : IDisposable
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public IndentScope(Tracer aTracer)
            {
                // ����
                tracer = aTracer;

                // ���x���A�b�v
                tracer.indentInc();
            }

            //------------------------------------------------------------
            // IDisposable�̎����B
            public void Dispose()
            {
                // ���x���_�E��
                tracer.indentDec();
            }

            //============================================================
            private readonly Tracer tracer;
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public Tracer()
        {
        }

        //------------------------------------------------------------
        // �C���f���g���x�����グ��B
        public void indentInc()
        {
            ++indentLevel;
        }

        //------------------------------------------------------------
        // �C���f���g���x����������B
        public void indentDec()
        {
            --indentLevel;
        }

        //------------------------------------------------------------
        // �������ށB
        public void Write(string aMessage)
        {
            // �C���f���g
            for (uint i = 0; i < indentLevel; ++i)
            {
                System.Console.Write("    ");
            }

            // ���b�Z�[�W
            System.Console.Write(aMessage);

            // ���s
            System.Console.Write(System.Environment.NewLine);
        }

        //------------------------------------------------------------
        // ���O���������ށB
        public void WriteName(string aName)
        {
            Write(aName + ":");
        }

        //------------------------------------------------------------
        // ���O+�z�񐔂��������ށB
        public void WriteNameWithCount(string aName, int aCount)
        {
            WriteValue(aName, "{" + aCount + "}");
        }

        // �l���������ށB
        public void WriteValue(string aName, string aValue)
        {
            Write(aName + " => " + aValue);
        }

        //============================================================
        private uint indentLevel = 0;
    }
}
