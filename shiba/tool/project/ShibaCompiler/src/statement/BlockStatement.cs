using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // �X�R�[�v�ɓ���
            aComp.ScopeEnter();

            // �Ӗ����
            foreach (IStatement st in mStatements)
            {
                st.SemanticAnalyze(aComp);
            }

            // �X�R�[�v����o��
            aComp.ScopeLeave();
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
