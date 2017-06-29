using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// アサート。
    /// </summary>
    class Assert
    {
        //------------------------------------------------------------
        // チェックする。
        static public void Check(bool aCondition)
        {
            if (!aCondition)
            {
                throw new Exception("Assert Failed.");
            }
        }

        //------------------------------------------------------------
        // 到達しない場所。
        static public void NotReachHere()
        {
            throw new Exception("Not reach here.");
        }
    }
}
