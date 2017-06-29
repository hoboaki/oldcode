/**
 * @file
 * @brief 変換する関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ConvertRecipe;
    class File;
    class TGAFileLoader;
    struct ArgumentData;

    /// 変換する関数。
    class Convert
    {
    public:
        /// 変換実行関数。
        static bool execute( const ArgumentData& );
        
    private:
        static bool impl( const ConvertRecipe& , const TGAFileLoader& );
        static bool convertPixels( const ConvertRecipe& , const TGAFileLoader& , File& );
    };

}
//------------------------------------------------------------
// EOF
