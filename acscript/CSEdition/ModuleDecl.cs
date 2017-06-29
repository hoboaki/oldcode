using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���W���[���̐錾�B
    /// </summary>
    class ModuleDecl
    {
        public readonly IdentPath IdentifierPath;
        public readonly bool IsProtoType;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ModuleDecl(IdentPath aPath, bool aIsProtoType)
        {
            IdentifierPath = aPath;
            IsProtoType = aIsProtoType;
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                IdentifierPath.Trace(aTracer, "IdentifierPath");
                aTracer.WriteValue("IsProtoType", IsProtoType.ToString());
            }
        }
    }
}
