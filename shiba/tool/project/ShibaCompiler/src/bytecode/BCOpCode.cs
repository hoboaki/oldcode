using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：命令コード。
    /// </summary>
    class BCOpCode
    {
        //------------------------------------------------------------
        // 命令の種類。
        public enum OpType
        {
            NOP,
            // ロード
            LDSRZR, // SR1 = 0
            LDSRBT, // SR1 = true
            LDSRC4, // SR1 = ConstantTable[ ValU16 ](as 32bit)
            LDSRSR, // SR1 = SR2
            LDSRFZ, // SR1 = FR00
            LDFRSR, // FR1 = SR1
            // 算術演算
            ADDI32, // SR1 = SR2 + SR3
            SUBI32, // SR1 = SR2 - SR3
            MULS32, // SR1 = SR2 * SR3
            DIVS32, // SR1 = SR2 / SR3
            MODS32, // SR1 = SR2 % SR3
            INCI32, // SR1++
            DECI32, // SR1--
            NEGS32, // SR1 = -SR2
            // 比較演算
            LTS32,  // SR1 = SR2 < SR3
            LES32,  // SR1 = SR2 <= SR3
            EQI32,  // SR1 = SR2 == SR3
            NEI32,  // SR1 = SR2 != SR3
            EQBOOL, // SR1 = SR2 == SR3
            NEBOOL, // SR1 = SR2 != SR3
            // ビット演算
            ANDI32, // SR1 = SR2 & SR3
            ORI32,  // SR1 = SR2 | SR3
            XORI32, // SR1 = SR2 ^ SR3
            NTI32,  // SR1 = ~SR2
            NTBOOL, // SR1 = !SR2
            // シフト演算
            SLLI32, // SR1 = SR2 << SR3
            SLRI32, // SR1 = SR2 >> SR3
            // 分岐命令
            JMP,    // Jump to CS2
            JMPPOS, // if ( SR1 == true ) { Jump to CS2 }
            JMPNEG, // if ( SR1 == false ) { Jump to CS2 }
            // 関数
            FENTER, // Alloc SReg Count , Copy RegCount
            FLEAVE, // Free SReg Count
            CALL,   // Call SymbolTable[ ValU16 ]
        };

        //------------------------------------------------------------
        // 命令形式。
        public enum OpFormat
        {
            UNKNOWN,
            CS2,         // OP CS2(Constant Signed 2byte value)
            CU1,         // OP CU1(Constant Unsigned 1byte value)
            CU1_CU1,     // OP CU1 CU1
            FR1_SR1,     // OP FR1 SR1
            SR1,         // OP SR1
            SR1_CS2,     // OP SR1 CS2(Constant Signed 2byte value)
            SR1_CTI,     // OP SR1 CTI(Constant Table Index)
            SR1_SR2,     // OP SR1 SR2
            SR1_SR2_SR3, // OP SR1 SR2 SR3            
            STI,         // OP STI(Symbol TableIndex)
        };

        //------------------------------------------------------------
        // OpFormat.CS2形式のコンストラクタ。
        public BCOpCode(OpType aOp, short aCS2)
        {
            // 設定
            mFormat = OpFormat.CS2;
            mOp = aOp;
            mCS2 = aCS2;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.CU1形式のコンストラクタ。
        public BCOpCode(OpType aOp, byte aCU1A)
        {
            // 設定
            mFormat = OpFormat.CU1;
            mOp = aOp;
            mCU1A = aCU1A;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.CU1_CU1形式のコンストラクタ。
        public BCOpCode(OpType aOp, byte aCU1A, byte aCU1B)
        {
            // 設定
            mFormat = OpFormat.CU1_CU1;
            mOp = aOp;
            mCU1A = aCU1A;
            mCU1B = aCU1B;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.FR_SR形式のコンストラクタ。
        public BCOpCode(OpType aOp, byte aFR, StackRegister aSR)
        {
            // 設定
            mFormat = OpFormat.FR1_SR1;
            mOp = aOp;
            mFR1 = aFR;
            mSR1 = aSR;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.SR1形式のコンストラクタ。
        public BCOpCode(OpType aOp, StackRegister aSR1)
        {
            // 設定
            mFormat = OpFormat.SR1;
            mOp = aOp;
            mSR1 = aSR1;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.SR1_CS2形式のコンストラクタ。
        public BCOpCode(OpType aOp, StackRegister aSR1, short aCS2)
        {
            // 設定
            mFormat = OpFormat.SR1_CS2;
            mOp = aOp;
            mSR1 = aSR1;
            mCS2 = aCS2;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.SR1_CTI形式のコンストラクタ。
        public BCOpCode(OpType aOp, StackRegister aSR1, BCConstantValue aCV)
        {
            // 設定
            mFormat = OpFormat.SR1_CTI;
            mOp = aOp;
            mSR1 = aSR1;
            mConstantValue = aCV;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.SR1_SR2形式のコンストラクタ。
        public BCOpCode(OpType aOp, StackRegister aSR1, StackRegister aSR2)
        {
            // 設定
            mFormat = OpFormat.SR1_SR2;
            mOp = aOp;
            mSR1 = aSR1;
            mSR2 = aSR2;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.SR1_SR2_SR3形式のコンストラクタ。
        public BCOpCode(OpType aOp, StackRegister aSR1, StackRegister aSR2, StackRegister aSR3)
        {
            // 設定
            mFormat = OpFormat.SR1_SR2_SR3;
            mOp = aOp;
            mSR1 = aSR1;
            mSR2 = aSR2;
            mSR3 = aSR3;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // OpFormat.STI形式のコンストラクタ。
        public BCOpCode(OpType aOp, BCSymbolLink aSymbolLink)
        {
            // 設定
            mFormat = OpFormat.STI;
            mOp = aOp;
            mSymbolLink = aSymbolLink;

            // チェック
            checkOpAndFormat();
        }

        //------------------------------------------------------------
        // CS2の値を変更する。
        public void ChangeCS2(short aValue)
        {
            mCS2 = aValue;
        }

        //------------------------------------------------------------
        // XDataに書き込む。
        public void XDataWrite(XDataWriter aWriter,uint aIndex)
        {
            aWriter.WriteIndent();
            aWriter.WriteComment("[" + aIndex.ToString("X4") + "]" + ToASMText());
            aWriter.WriteUInt8((byte)mOp);
            switch (mFormat)
            {
                case OpFormat.CS2:
                    {
                        aWriter.WriteUInt8(0xFF); // dummy
                        aWriter.WriteSInt16(mCS2);
                    }
                    break;

                case OpFormat.CU1:
                    {
                        aWriter.WriteUInt8(mCU1A);
                        aWriter.WriteUInt16(0xFFFF); // dummy
                    }
                    break;

                case OpFormat.CU1_CU1:
                    {
                        aWriter.WriteUInt8(mCU1A);
                        aWriter.WriteUInt8(mCU1B);
                        aWriter.WriteUInt8(0xFF); // dummy
                    }
                    break;

                case OpFormat.FR1_SR1:
                    {
                        aWriter.WriteUInt8(mFR1);
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteUInt8(0xFF); // dummy
                    }
                    break;

                case OpFormat.SR1:
                    {
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteUInt16(0xFFFF); // dummy
                    }
                    break;

                case OpFormat.SR1_CS2:
                    {
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteSInt16(mCS2);
                    }
                    break;

                case OpFormat.SR1_CTI:
                    {
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteUInt16((ushort)mConstantValue.Offset());
                    }
                    break;

                case OpFormat.SR1_SR2:
                    {
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteUInt8(mSR2.Index());
                        aWriter.WriteUInt8(0xFF); // dummy
                    }
                    break;

                case OpFormat.SR1_SR2_SR3:
                    {
                        aWriter.WriteUInt8(mSR1.Index());
                        aWriter.WriteUInt8(mSR2.Index());
                        aWriter.WriteUInt8(mSR3.Index());
                    }
                    break;

                case OpFormat.STI:
                    {
                        aWriter.WriteUInt8(0xFF); // dummy
                        aWriter.WriteUInt16(mSymbolLink.Index);
                    }
                    break;

                default:
                    Assert.NotReachHere();
                    break;
            }
            aWriter.WriteLine();
        }

        //------------------------------------------------------------
        // アセンブラテキストの変換。
        public string ToASMText()
        {
            string str = mOp.ToString().PadRight(6) + " ";
            switch (mFormat)
            {
                case OpFormat.CS2:
                    str += ( 0 <= mCS2 ? "+" : "" ) + mCS2.ToString("D5");
                    break;

                case OpFormat.CU1:
                    str += "0x" + mCU1A.ToString("X2");
                    break;

                case OpFormat.CU1_CU1:
                    str += "0x" + mCU1A.ToString("X2") + " 0x" + mCU1B.ToString("X2");
                    break;

                case OpFormat.FR1_SR1:
                    str += "FR" + mFR1.ToString("X2") + " " + mSR1.ToASMText();
                    break;

                case OpFormat.SR1:
                    str += mSR1.ToASMText();
                    break;

                case OpFormat.SR1_CS2:
                    str += mSR1.ToASMText() + " " + (0 <= mCS2 ? "+" : "") + mCS2.ToString("D5");
                    break;

                case OpFormat.SR1_CTI:
                    str += mSR1.ToASMText() + " 0x" + mConstantValue.Offset().ToString("X4");
                    break;

                case OpFormat.SR1_SR2:
                    str += mSR1.ToASMText() + " " + mSR2.ToASMText();
                    break;

                case OpFormat.SR1_SR2_SR3:
                    str += mSR1.ToASMText() + " " + mSR2.ToASMText() + " " + mSR3.ToASMText();
                    break;

                case OpFormat.STI:
                    str += mSymbolLink.TargetNode.GetUniqueFullPath();
                    break;

                default:
                    Assert.NotReachHere();
                    break;
            }
            return str;
        }

        //============================================================

        //------------------------------------------------------------
        OpFormat mFormat;
        OpType mOp;
        short mCS2;
        byte mCU1A;
        byte mCU1B;
        byte mFR1;
        StackRegister mSR1;
        StackRegister mSR2;
        StackRegister mSR3;
        BCConstantValue mConstantValue;
        BCSymbolLink mSymbolLink;

        //------------------------------------------------------------
        // 指定のOpTypeにあうOpFormatを取得する。
        OpFormat opTypeToOpFormat(OpType aOP)
        {
            switch (mOp)
            {
                case OpType.LDSRZR:
                    return OpFormat.SR1;

                case OpType.LDSRBT:
                    return OpFormat.SR1;

                case OpType.LDSRC4:
                    return OpFormat.SR1_CTI;

                case OpType.LDSRSR:
                    return OpFormat.SR1_SR2;

                case OpType.LDSRFZ:
                    return OpFormat.SR1;

                case OpType.LDFRSR:
                    return OpFormat.FR1_SR1;

                case OpType.ADDI32:
                case OpType.SUBI32:
                case OpType.MULS32:
                case OpType.DIVS32:
                case OpType.MODS32:
                    return OpFormat.SR1_SR2_SR3;

                case OpType.INCI32:
                case OpType.DECI32:
                    return OpFormat.SR1;

                case OpType.NEGS32:
                    return OpFormat.SR1_SR2;

                case OpType.LTS32:
                case OpType.LES32:
                case OpType.EQI32:
                case OpType.NEI32:
                    return OpFormat.SR1_SR2_SR3;

                case OpType.EQBOOL:
                case OpType.NEBOOL:
                    return OpFormat.SR1_SR2_SR3;

                case OpType.ANDI32:
                case OpType.ORI32:
                case OpType.XORI32:
                    return OpFormat.SR1_SR2_SR3;

                case OpType.NTI32:
                case OpType.NTBOOL:
                    return OpFormat.SR1_SR2;
                    
                case OpType.SLLI32:
                case OpType.SLRI32:
                    return OpFormat.SR1_SR2_SR3;

                case OpType.JMP:
                    return OpFormat.CS2;

                case OpType.JMPPOS:
                case OpType.JMPNEG:
                    return OpFormat.SR1_CS2;

                case OpType.FENTER:
                    return OpFormat.CU1_CU1;

                case OpType.FLEAVE:
                    return OpFormat.CU1;

                case OpType.CALL:
                    return OpFormat.STI;

                default: 
                    return OpFormat.UNKNOWN;
            }
        }

        //------------------------------------------------------------
        // OpとFormatが適切かどうか調べる。
        void checkOpAndFormat()
        {
            Assert.Check(opTypeToOpFormat(mOp) == mFormat);
        }
    }
}
