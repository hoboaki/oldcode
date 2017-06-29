using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �V���{���̒�`�B
    /// </summary>
    class SymbolDef
    {
        //------------------------------------------------------------
        // �V���{���̎�ށB
        public enum Kind
        {
            StaticTypeDef,
            MemberVariableDecl,
            MemberFunctionDecl,
        };

        //------------------------------------------------------------
        // ���J�����o�ϐ��B
        public readonly Kind SymbolKind;
        public readonly StaticTypeDef StaticTypeDef;
        public readonly MemberVariableDecl MemberVariableDecl;
        public readonly MemberFunctionDecl MemberFunctionDecl;

        //------------------------------------------------------------
        // �^�̒�`�Ƃ��ăR���X�g���N�g����B
        public SymbolDef(StaticTypeDef aST)
        {
            SymbolKind = Kind.StaticTypeDef;
            StaticTypeDef = aST;
        }

        //------------------------------------------------------------
        // �����o�ϐ��̐錾�Ƃ��ăR���X�g���N�g����B
        public SymbolDef(MemberVariableDecl aMV)
        {
            SymbolKind = Kind.MemberVariableDecl;
            MemberVariableDecl = aMV;
        }

        //------------------------------------------------------------
        // �����o�֐��̐錾�Ƃ��ăR���X�g���N�g����B
        public SymbolDef(MemberFunctionDecl aMF)
        {
            SymbolKind = Kind.MemberFunctionDecl;
            MemberFunctionDecl = aMF;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("SymbolKind", SymbolKind.ToString());
                if (StaticTypeDef != null) { StaticTypeDef.Trace(aTracer, "StaticTypeDef"); }
                if (MemberVariableDecl != null) { MemberVariableDecl.Trace(aTracer, "MemberVariableDecl"); }
                if (MemberFunctionDecl != null) { MemberFunctionDecl.Trace(aTracer, "MemberFunctionDecl"); }
            }
        }
    }
}
