/**
 * @file
 * @brief ObjectCode型を記述する。
 */
#ifndef ACS_INCLUDE_OBJECTCODE
#define ACS_INCLUDE_OBJECTCODE

//------------------------------------------------------------
namespace acscript {

    /**
     * @brief １つのオブジェクトコード。
     * コンパイルされたバイトコード、コンパイル時に生成された情報を持つ。
     */
    class ObjectCode
    {
    private:
        /// コードの依存情報を示す型。
        struct DependCodeInfo
        {
            ByteCodeId ByteCodeId; ///< 参照したオブジェクトコードのID。
            Hash hash; ///< コンパイル時のコードのハッシュ値。
        };

        /// コンパイルされたバイトコード。
        ByteCode* mByteCode;

        /// コンパイル時のコードのハッシュ値。 
        Hash mHash;

        /// シンボル名テーブル。コールスタックの表示や、シリアライズに使用する。
        Vector< SymbolName >::BuildTimeType mLabeledSymbolNameTable;

        /// コードの依存情報。
        Vector< DependCodeInfo >::BuildTimeType mDependCodeInfoTable;
    };

}
//------------------------------------------------------------
#endif
// EOF
