using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 関数引数宣言のリスト。
    /// </summary>
    class FunctionArgumentDeclList : IEnumerable<FunctionArgumentDecl>
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionArgumentDeclList()
        {
            mList = new List<FunctionArgumentDecl>();
        }

        //------------------------------------------------------------
        // 引数の数。
        public int Count()
        {
            return mList.Count;
        }

        //------------------------------------------------------------
        // 引数を追加する。
        public void Add(FunctionArgumentDecl decl)
        {
            mList.Add(decl);
        }

        //------------------------------------------------------------
        // IEnumerableの実装。
        public IEnumerator<FunctionArgumentDecl> GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        //------------------------------------------------------------
        // IEnumerableの実装。
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        //------------------------------------------------------------
        // トレース。
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
