/**
 * @file
 * @brief �G���f�B�A���Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

    /// �G���f�B�A���Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class EndianUtil
    {
    public:
        // BigEndian���ł̂݃X���b�v����B
        static u16 swapU16BE( u16 value );
        static u32 swapU32BE( u32 value );
        static s16 swapS16BE( s16 value );
        static s32 swapS32BE( s32 value );
    };
    
}
//------------------------------------------------------------
// EOF
