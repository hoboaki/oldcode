/**
 * @file
 * @brief �G���f�B�A�����擾����֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// �G���f�B�A�����擾����֐����L�q����B
    class Endian
    {
    public:
        static bool isLittleEndian(); ///< LE���B
        static bool isBigEndian();    ///< BE���B
    };
    
}
//------------------------------------------------------------
// EOF
