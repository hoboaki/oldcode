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
        
        const char* get()const; ///< ���A�����Ă���v�f���擾�B
        bool isEnd()const;      ///< �I��肩�B
        void next();            ///< ���̗v�f�ֈړ��B
        
    private:
        const ArgumentData& data_;
        int index_;
    };

}
//------------------------------------------------------------
// EOF
