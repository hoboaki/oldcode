using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ISymbolNodeの便利関数群。
    /// </summary>
    class SymbolNodeUtil
    {
        //------------------------------------------------------------
        // フルパス文字列を取得する。
        static public string FullPath(ISymbolNode aNode)
        {
            string result = "";
            for (ISymbolNode node = aNode; 
                node.GetNodeKind() != SymbolNodeKind.Root; 
                node = node.ParentNode()
                )
            {
                if (result.Length == 0)
                {
                    result = node.GetIdentifier().String();
                }
                else
                {
                    result = node.GetIdentifier().String() + "." + result;
                }
            }
            return result;
        }
    }
}
