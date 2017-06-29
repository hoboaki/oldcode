/**
 * @file
 * @brief 引数オブジェクトを記述する。
 */
#pragma once
 
//------------------------------------------------------------
namespace local {

    /// 引数オブジェクト。
    struct ArgumentData
    {
        int                 count; ///< 総数。
        const char*const*   texts; ///< 文字列の配列へのポインタ。
        
        //------------------------------------------------------------
        void dump()const; ///< コンソールに中身をダンプする。
        const char* getTextAtIndex( int index )const; ///< 文字列の取得。
        
        //------------------------------------------------------------
        /// 作成関数。
        static ArgumentData create( int count , const char*const* texts );
    };
    
}
//------------------------------------------------------------
// EOF
