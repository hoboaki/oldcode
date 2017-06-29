using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 評価イベントの種類。
    /// </summary>
    enum EvaluateNodeEventKind
    {
        // 以下、通常イベント。
        Analyze,                    // 解析。EvaluateInfoを作成したり、コンパイル時計算ができるならここでやる。OpCodeの発行やレジスタの割り当てはここでは行わない。
        Evaluate,                   // 評価実行。レジスタ割り当てやOpCodeの発行はここで行う。
        Release,                    // 割り当てられているレジスタの解放をする。OpCodeの発行が必要なら発行する。Evaluateと対になるイベント。
        // 以下、特別なイベント。
        SetupFR,                    // 関数引数レジスタのセットアップをするOpCodeを発行する。
        InsertOpCodeOnScopeLeave,   // スコープから出るときに必要なOpCodeを発行する。breakやcontinueなどで途中離脱したときに呼ばれる。Releaseと違ってレジスタ割り当ての解放はしてはいけない。
    }
}
