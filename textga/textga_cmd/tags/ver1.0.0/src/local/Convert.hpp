/**
 * @file
 * @brief 変換する関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace textga {
    class File;
    class TGADataAccessor;
}

//------------------------------------------------------------
namespace local {

    class ConvertRecipe;
    class ArgumentDataIterator;

    /// 変換する関数。
    class Convert
    {
    public:
        /// 変換実行関数。
        static bool execute( const ArgumentDataIterator& );
        
    private:
        static bool impl( const ConvertRecipe& , const textga::TGADataAccessor& );
        static bool convertPixels( const ConvertRecipe& , const textga::TGADataAccessor& , textga::File& );
    };

}
//------------------------------------------------------------
// EOF
