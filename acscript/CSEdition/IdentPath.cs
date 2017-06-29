using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 識別子のパス。IdentifierPath。
    /// </summary>
    class IdentPath
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public IdentPath( TokenArray aTokens , bool aFromRoot )
        {
            idents = new List<Identifier>();
            fromRoot = aFromRoot;

            // 全てIdentifierかチェック
            foreach (Token token in aTokens)
            {
                idents.Add(new Identifier(token));
            }
        }

        //------------------------------------------------------------
        // Identの総数。
        public int Count
        {
            get { return idents.Count; }
        }

        //------------------------------------------------------------
        // 指定番目のIdentを取得する。
        public Identifier At(int aIndex)
        {
            return idents[aIndex];
        }

        //------------------------------------------------------------
        // トレースする。
        public void Trace(Tracer aTracer, string aName)
        {
            // 文字列準備
            string str = "";
            foreach (Identifier ident in idents)
            {
                // '.'
                str += (str.Length != 0 || fromRoot ? "." : "") + ident.Token.Identifier;
            }

            // 書き込む
            aTracer.WriteValue(aName,str);
        }

        //============================================================
        private List< Identifier > idents;
        private bool fromRoot;
    }
}
