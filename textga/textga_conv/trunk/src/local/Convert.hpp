/**
 * @file
 * @brief �ϊ�����֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ConvertRecipe;
    class File;
    class TGAFileLoader;
    struct ArgumentData;

    /// �ϊ�����֐��B
    class Convert
    {
    public:
        /// �ϊ����s�֐��B
        static bool execute( const ArgumentData& );
        
    private:
        static bool impl( const ConvertRecipe& , const TGAFileLoader& );
        static bool convertPixels( const ConvertRecipe& , const TGAFileLoader& , File& );
    };

}
//------------------------------------------------------------
// EOF
