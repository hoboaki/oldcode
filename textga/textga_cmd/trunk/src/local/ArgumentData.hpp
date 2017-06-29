/**
 * @file
 * @brief �����I�u�W�F�N�g���L�q����B
 */
#pragma once
 
//------------------------------------------------------------
namespace local {

    /// �����I�u�W�F�N�g�B
    struct ArgumentData
    {
        int                 count; ///< �����B
        const char*const*   texts; ///< ������̔z��ւ̃|�C���^�B
        
        //------------------------------------------------------------
        void dump()const; ///< �R���\�[���ɒ��g���_���v����B
        const char* getTextAtIndex( int index )const; ///< ������̎擾�B
        
        //------------------------------------------------------------
        /// �쐬�֐��B
        static ArgumentData create( int count , const char*const* texts );
    };
    
}
//------------------------------------------------------------
// EOF
