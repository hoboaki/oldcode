using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ���W���[���̒�`�N���X�B
    /// </summary>
    class ModuleDef
    {
        public readonly StaticTypeDef StaticTypeDef;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ModuleDef(StaticTypeDef aStaticTypeDef)
        {
            StaticTypeDef = aStaticTypeDef;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using(new Tracer.IndentScope(aTracer))
            {
                StaticTypeDef.Trace(aTracer, aName);
            }
        }
    }
}
