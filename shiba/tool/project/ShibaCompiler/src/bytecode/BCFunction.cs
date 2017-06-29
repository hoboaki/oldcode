using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：関数。
    /// </summary>
    class BCFunction
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCFunction(BCModule aBCModule, FunctionSymbolNode aFunctionSymbolNode)
        {
            mBCModule = aBCModule;
            mFunctionSymbolNode = aFunctionSymbolNode;
            mBCOpCodeList = new List<BCOpCode>();
            mBCLabelReferenceList = new List<BCLabelReference>();
        }

        //------------------------------------------------------------
        // OP CU1形式の命令コードを先頭に追加する。
        public void PushFrontOPCode_CU1_CU1(BCOpCode.OpType aOP, byte aValue1, byte aValue2)
        {
            mBCOpCodeList.Insert(0, new BCOpCode(
                aOP
                , aValue1
                , aValue2
                ));
        }

        //------------------------------------------------------------
        // OP Label形式の命令コードを追加する。
        public void AddOPCode_Label(BCOpCode.OpType aOP, BCLabel aLabel)
        {
            // OpCode作成
            var opCode = new BCOpCode(
                aOP
                , (short)0
                );

            // 追加されるindexをメモ
            uint index = (uint)mBCOpCodeList.Count;

            // OpCode追加
            mBCOpCodeList.Add(opCode);

            // LabelReference追加
            mBCLabelReferenceList.Add(new BCLabelReference(aLabel, opCode, index));
        }

        //------------------------------------------------------------
        // OP SReg Label形式の命令コードを追加する。
        public void AddOPCode_SReg_Label(BCOpCode.OpType aOP, StackRegister aSR, BCLabel aLabel)
        {
            // OpCode作成
            var opCode = new BCOpCode(
                aOP
                , aSR
                , (short)0
                );

            // 追加されるindexをメモ
            uint index = (uint)mBCOpCodeList.Count;

            // OpCode追加
            mBCOpCodeList.Add(opCode);

            // LabelReference追加
            mBCLabelReferenceList.Add(new BCLabelReference(aLabel, opCode, index));
        }

        //------------------------------------------------------------
        // OP CU1形式の命令コードを追加する。
        public void AddOPCode_CU1(BCOpCode.OpType aOP, byte aValue)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aValue
                ));
        }

        //------------------------------------------------------------
        // OP CU1 SReg形式の命令コードを追加する。
        public void AddOPCode_CU1_SR(BCOpCode.OpType aOP, byte aValue, StackRegister aSR)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aValue
                , aSR
                ));
        }

        //------------------------------------------------------------
        // OP SReg形式の命令コードを追加する。
        public void AddOPCode_SReg(BCOpCode.OpType aOP, StackRegister aSR)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aSR
                ));
        }

        //------------------------------------------------------------
        // OP SReg ConstantTableIndex形式の命令コードを追加する。
        public void AddOPCode_SReg_ConstantTableIndex(BCOpCode.OpType aOP,StackRegister aSR, int aValue)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aSR
                , mBCModule.GetConstantValue(new BCConstantValue(aValue))
                ));
        }

        //------------------------------------------------------------
        // OP SReg1 SReg2形式の命令コードを追加する。
        public void AddOPCode_SReg1_SReg2(BCOpCode.OpType aOP, StackRegister aSR1, StackRegister aSR2)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aSR1
                , aSR2
                ));
        }

        //------------------------------------------------------------
        // OP SReg1 SReg2 SReg3形式の命令コードを追加する。
        public void AddOPCode_SReg1_SReg2_SReg3(BCOpCode.OpType aOP, StackRegister aSR1, StackRegister aSR2, StackRegister aSR3)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , aSR1
                , aSR2
                , aSR3
                ));
        }

        //------------------------------------------------------------
        // OP SymbolTableIndex形式の命令コードを追加する。
        public void AddOPCode_SymbolTableIndex(BCOpCode.OpType aOP, ISymbolNode aSymbol)
        {
            mBCOpCodeList.Add(new BCOpCode(
                aOP
                , mBCModule.GetSymbolLink(aSymbol)
                ));
        }

        //------------------------------------------------------------
        // BCLabelを作成する。
        public BCLabel LabelCreate()
        {
            return new BCLabel();
        }

        //------------------------------------------------------------
        // BCLabelを現在の位置に挿入する。
        public void LabelInsert(BCLabel aLabel)
        {
            aLabel.SetOpCodeIndex((uint)mBCOpCodeList.Count());
        }

        //------------------------------------------------------------
        // ラベルを解決する。
        public void LabelResolve()
        {
            foreach (var entry in mBCLabelReferenceList)
            {
                int target = (int)entry.Label.OpCodeIndex;
                int own = (int)entry.OpCodeIndex;
                entry.OpCode.ChangeCS2((short)(target - own));
            }
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter)
        {
            aWriter.WriteReferenceLine("Function", XDATA_LABEL + ":" + SymbolNodeUtil.FullPath(mFunctionSymbolNode));
        }

        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            // フルパスのメモ
            string fullPath = mFunctionSymbolNode.GetUniqueFullPath();

            // 実体
            aWriter.WriteCommentLine("BCFunction(" + fullPath + ")");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル
                aWriter.WriteAlignLine(4);
                aWriter.WriteLabelLine(XDATA_LABEL + ":" + fullPath);

                // シンボル名
                aWriter.WriteStringLine("name", mFunctionSymbolNode.GetIdentifier().String());

                // 命令コード
                aWriter.WriteReferenceLine(XDATA_LABEL_OP_CODE + ":" + fullPath);
            }

            // その他
            {// 命令コード
                aWriter.WriteCommentLine("BCOpCode(" + fullPath + ")-" + mBCOpCodeList.Count * 4 + "bytes");
                using (new XDataWriter.IndentScope(aWriter))
                {
                    // アライメントとラベル
                    aWriter.WriteAlignLine(4);
                    aWriter.WriteLabelLine(XDATA_LABEL_OP_CODE + ":" + fullPath);
                    uint index = 0;
                    foreach(var entry in mBCOpCodeList)
                    {
                        entry.XDataWrite(aWriter, index);
                        ++index;
                    }
                }
            }
        }

        //============================================================
        const string XDATA_LABEL = "LabelFunction";
        const string XDATA_LABEL_OP_CODE = "LabelOpCode";
        BCModule mBCModule;
        FunctionSymbolNode mFunctionSymbolNode;
        List<BCOpCode> mBCOpCodeList;
        List<BCLabelReference> mBCLabelReferenceList;
    }
}
