/**
 * @file
 * @brief �G���f�B�A���Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// �G���f�B�A���Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class EndianUtil
    {
    public:
        // BigEndian���ł̂݃X���b�v����B
        static short swapS16BE( short value );
        static int   swapS32BE( int value );
    };
}
//------------------------------------------------------------
// EOF
