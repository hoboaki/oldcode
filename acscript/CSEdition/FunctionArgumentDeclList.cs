using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �֐������錾�̃��X�g�B
    /// </summary>
    class FunctionArgumentDeclList : IEnumerable<FunctionArgumentDecl>
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionArgumentDeclList()
        {
            mList = new List<FunctionArgumentDecl>();
        }

        //------------------------------------------------------------
        // �����̐��B
        public int Count()
        {
            return mList.Count;
        }

        //------------------------------------------------------------
        // ������ǉ�����B
        public void Add(FunctionArgumentDecl decl)
        {
            mList.Add(decl);
        }

        //------------------------------------------------------------
        // IEnumerable�̎����B
        public IEnumerator<FunctionArgumentDecl> GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        //------------------------------------------------------------
        // IEnumerable�̎����B
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteNameWithCount(aName, mList.Count);
            using (new Tracer.IndentScope(aTracer))
            {
                uint idx = 0;
                foreach (var entry in mList)
                {
                    entry.Trace(aTracer, "[" + idx + "]");
                    ++idx;
                }
            }
        }

        //============================================================
        List<FunctionArgumentDecl> mList;
    }
}
