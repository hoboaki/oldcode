/**
 * @file
 * @brief エントリーポイント関数を記述する。
 */
#pragma once
 
//------------------------------------------------------------
namespace local {
    struct ArgumentData;
}
//------------------------------------------------------------
namespace local {

    /// エントリーポイント関数を提供するクラス。
    class EntryPoint
    {
    public:
        /// 実行する。
        static int run( const ArgumentData& );
        
    };
    
}
//------------------------------------------------------------
// EOF
