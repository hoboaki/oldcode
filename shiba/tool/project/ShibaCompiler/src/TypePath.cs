using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 型のパス。
    /// </summary>
    class TypePath
    {
        public readonly BuiltInType BuiltInType = BuiltInType.Unknown;
        public readonly Token BuiltInToken = null;
        public readonly IdentPath IdentPath = null;

        //------------------------------------------------------------
        // 組み込み型として初期化。
        public TypePath(Token aToken, BuiltInType aBT)
        {
            BuiltInToken = aToken;
            BuiltInType = aBT;
        }

        //------------------------------------------------------------
        // ユーザー定義型として初期化。
        public TypePath(IdentPath aIP)
        {
            IdentPath = aIP;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTrace, string aName)
        {
            if (BuiltInType != BuiltInType.Unknown)
            {
                aTrace.WriteValue(aName, this.BuiltInType.ToString());
            }
            if (IdentPath != null)
            {
                IdentPath.Trace(aTrace, aName);
            }
        }
    }
}
