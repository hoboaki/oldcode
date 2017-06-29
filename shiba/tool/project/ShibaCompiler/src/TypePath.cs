using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �^�̃p�X�B
    /// </summary>
    class TypePath
    {
        public readonly BuiltInType BuiltInType = BuiltInType.Unknown;
        public readonly Token BuiltInToken = null;
        public readonly IdentPath IdentPath = null;

        //------------------------------------------------------------
        // �g�ݍ��݌^�Ƃ��ď������B
        public TypePath(Token aToken, BuiltInType aBT)
        {
            BuiltInToken = aToken;
            BuiltInType = aBT;
        }

        //------------------------------------------------------------
        // ���[�U�[��`�^�Ƃ��ď������B
        public TypePath(IdentPath aIP)
        {
            IdentPath = aIP;
        }

        //------------------------------------------------------------
        // �g���[�X�B
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