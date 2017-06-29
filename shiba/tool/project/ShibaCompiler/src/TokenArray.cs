using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// トークンの配列。
    /// </summary>
    class TokenArray : IEnumerable<Token>
    {
        public TokenArray()
        {
            tokens = new List<Token>();
        }

        public void Add(Token token)
        {
            tokens.Add(token);
        }

        public int Count
        {
            get { return tokens.Count; }
        }

        public Token At(int index)
        {
            return tokens[index];
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        private List<Token> tokens;
    }
}
