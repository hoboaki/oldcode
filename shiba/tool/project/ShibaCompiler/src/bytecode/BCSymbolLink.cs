using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：シンボルリンク。
    /// </summary>
    class BCSymbolLink
    {
        public readonly ISymbolNode TargetNode; // 指しているシンボルノード。
        public readonly ushort Index; // インデックス値。

        //------------------------------------------------------------
        // コンストラクタ。
        public BCSymbolLink(ISymbolNode aSymbol, ushort aIndex)
        {
            TargetNode = aSymbol;
            Index = aIndex;
        }
        
        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            aWriter.WriteStringLine(TargetNode.GetUniqueFullPath());
        }
    }
}
