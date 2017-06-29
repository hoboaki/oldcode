using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：モジュール。
    /// </summary>
    class BCModule
    {
        //------------------------------------------------------------
        // 定数。
        const byte BC_VERSION_PUBLISH_MAJOR = 0; // バイトコードのメジャーバージョン。
        const byte BC_VERSION_PUBLISH_MINOR = 1; // バイトコードのマイナーバージョン。
        const byte BC_VERSION_PRIVATE_MAJOR = 0; // バイトコードのプライベートメジャーバージョン。
        const byte BC_VERSION_PRIVATE_MINOR = 0; // バイトコードのプライベートマイナーバージョン。

        //------------------------------------------------------------
        // コンストラクタ。
        public BCModule(ModuleSymbolNode aModuleSymbolNode)
        {
            mModuleSymbolNode = aModuleSymbolNode;
            mConstantValueTable = new BCConstantValueTable();
            mSymbolLinkTable = new BCSymbolLinkTable();
            mObjectTypeList = new BCObjectTypeList();
        }

        //------------------------------------------------------------
        // コンテキストを取得する。
        public ModuleContext ModuleContext()
        {
            return mModuleSymbolNode.GetModuleContext();
        }

        //------------------------------------------------------------
        // ユーザー定義型を追加する。
        public BCObjectType GenerateObjectType(TypeSymbolNode aTypeSymbolNode)
        {
            // 生成
            var objectType = new BCObjectType(this, aTypeSymbolNode);

            // 追加
            mObjectTypeList.Add(objectType);

            // 返す
            return objectType;
        }

        //------------------------------------------------------------
        // 適切なConstantValueを取得する。
        public BCConstantValue GetConstantValue(BCConstantValue aValue)
        {
            return mConstantValueTable.CheckAndGet(aValue);
        }

        //------------------------------------------------------------
        // 適切なSymbolLinkを取得する。
        public BCSymbolLink GetSymbolLink(ISymbolNode aSymbol)
        {
            return mSymbolLinkTable.CheckAndGet(aSymbol);
        }

        //------------------------------------------------------------
        // 出力する準備をする。
        public void ReadyToOutput()
        {
            // オフセット位置の計算
            mConstantValueTable.CalcOffsetPos();
        }

        //------------------------------------------------------------
        // XDataに変換する。
        public string ToXDataXml()
        {
            // 作成
            var xdata = new XDataWriter();

            // BCModule
            xdata.WriteCommentLine("BCModule");
            using (new XDataWriter.IndentScope(xdata))
            {
                {// BCVersion
                    xdata.WriteIndent();
                    xdata.WriteComment("version (" + BC_VERSION_PUBLISH_MAJOR + "." + BC_VERSION_PUBLISH_MINOR + "." + BC_VERSION_PRIVATE_MAJOR + "." + BC_VERSION_PRIVATE_MINOR + ")");
                    xdata.WriteUInt8(BC_VERSION_PUBLISH_MAJOR);
                    xdata.WriteUInt8(BC_VERSION_PUBLISH_MINOR);
                    xdata.WriteUInt8(BC_VERSION_PRIVATE_MAJOR);
                    xdata.WriteUInt8(BC_VERSION_PRIVATE_MINOR);
                    xdata.WriteLine();
                }
                xdata.WriteStringLine("path", SymbolNodeUtil.FullPath(mModuleSymbolNode));
                mConstantValueTable.XDataWriteReference(xdata);
                mSymbolLinkTable.XDataWriteReference(xdata);
                mObjectTypeList.XDataWriteReference(xdata);
            }
                        
            // 各実体
            mConstantValueTable.XDataWriteEntity(xdata);
            mSymbolLinkTable.XDataWriteEntity(xdata);
            mObjectTypeList.XDataWriteEntity(xdata);

            // 結果を返す
            return xdata.ToXMLText();
        }

        //============================================================
        ModuleSymbolNode mModuleSymbolNode;
        BCConstantValueTable mConstantValueTable;
        BCSymbolLinkTable mSymbolLinkTable;
        BCObjectTypeList mObjectTypeList;
    }
}
