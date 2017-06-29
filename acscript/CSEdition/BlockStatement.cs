using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// BlockStatement�B
    /// </summary>
    class BlockStatement : IStatement
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public BlockStatement()
        {
            mStatements = new List<IStatement>();
        }

        //------------------------------------------------------------
        // �Ӗ���́B
        public bool SemanticAnalyze(SemanticAnalyzeComponent comp)
        {
            // �o�b�N�A�b�v
            ISymbolNode backupPrevSymbolNode = comp.PrevSymbolNode;

            // �Ӗ����
            foreach (IStatement st in mStatements)
            {
                if (!st.SemanticAnalyze(comp))
                {
                    return false;
                }
            }

            // �o�b�N�A�b�v�����ɖ߂�
            comp.PrevSymbolNode = backupPrevSymbolNode;

            return true;
        }

        //------------------------------------------------------------
        // Statement�̒ǉ��B
        public void Add(IStatement statement)
        {
            mStatements.Add(statement);
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("BlockStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var entry in mStatements)
                {
                    entry.Trace(aTracer);
                }
            }
        }

        //============================================================
        List<IStatement> mStatements;
    }
}
