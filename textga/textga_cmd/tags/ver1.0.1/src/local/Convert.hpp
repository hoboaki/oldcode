/**
 * @file
 * @brief �ϊ�����֐����L�q����B
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

    /// �ϊ�����֐��B
    class Convert
    {
    public:
        /// �ϊ����s�֐��B
        static bool execute( const ArgumentDataIterator& );
        
    private:
        static bool impl( const ConvertRecipe& , const textga::TGADataAccessor& );
        static bool convertPixels( const ConvertRecipe& , const textga::TGADataAccessor& , textga::File& );
    };

}
//------------------------------------------------------------
// EOF
