/**
 * @file
 * @brief ArgumentData�̃C�e���[�^�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace local {
    struct ArgumentData;
}

//------------------------------------------------------------
namespace local {

    /// ArgumentData�̃C�e���[�^�N���X�B
    class ArgumentDataIterator
    {
    public:
        explicit ArgumentDataIterator( const ArgumentData& );
        
        bool hasNext()const;    ///< �������邩�B
        const char* next();     ///< ���̗v�f�ֈړ��B
        
    private:
        const ArgumentData& data_;
        int index_;
    };

}
//------------------------------------------------------------
// EOF
