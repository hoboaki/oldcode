/**
 * @file
 * @brief Hash�^���L�q����B
 */
#ifndef ACS_INCLUDE_HASH
#define ACS_INCLUDE_HASH

//------------------------------------------------------------
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// �n�b�V���l�BMD5�ŋ��߂��l������悤��128bit�̗̈��p�ӁB
    struct Hash
    {
        U32 data[8];

        /// �Q�̃n�b�V���͓��������B
        bool equals( const Hash& rhs );
    };
    /// Hash::equals�̃G�C���A�X�B
    bool operator ==(const Hash&,const Hash&);

}
//------------------------------------------------------------
#endif
// EOF
