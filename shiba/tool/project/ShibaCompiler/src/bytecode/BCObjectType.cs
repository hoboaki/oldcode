using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：ユーザー定義型。
    /// </summary>
    class BCObjectType
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCObjectType(BCModule aBCModule, TypeSymbolNode aTypeSymbolNode)
        {
            mBCModule = aBCModule;
            mTypeSymbolNode = aTypeSymbolNode;
            mFunctionList = new BCFunctionList();
        }

        //------------------------------------------------------------
        // 関数を生成する。
        public BCFunction GenerateFunction(FunctionSymbolNode aFunctionSymbolNode)
        {
            // 作成
            var func = new BCFunction(mBCModule, aFunctionSymbolNode);

            // 追加
            mFunctionList.Add(func);
            
            // 返す
            return func;
        }

        //------------------------------------------------------------
        // 所属するモジュールを取得。
        public BCModule GetBCModule()
        {
            return mBCModule;
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter)
        {
            aWriter.WriteReferenceLine(XDATA_LABEL + ":" + SymbolNodeUtil.FullPath(mTypeSymbolNode));
        }

        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntry(XDataWriter aWriter)
        {
            // フルパスのメモ
            string fullPath = mTypeSymbolNode.GetUniqueFullPath();

            // 実体
            aWriter.WriteCommentLine("BCObjectType(" + fullPath + ")");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル。
                aWriter.WriteAlignLine(4);
                aWriter.WriteLabelLine(XDATA_LABEL + ":" + fullPath);

                // パス
                aWriter.WriteStringLine("path", fullPath);

                // 関数リスト
                mFunctionList.XDataWriteReference(aWriter, fullPath);
            }

            // その他
            mFunctionList.XDataWriteEntity(aWriter, fullPath);
        }

        //============================================================
        const string XDATA_LABEL = "LabelObjectType";
        BCModule mBCModule;
        TypeSymbolNode mTypeSymbolNode;
        BCFunctionList mFunctionList;
    }
}
