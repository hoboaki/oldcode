/**
 * @file
 * @brief CommandKind�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include "CommandKind.hpp"

//------------------------------------------------------------
namespace local {

    struct ArgumentData;

    /// CommandKind�Ɋւ��郆�[�e�B���e�B�֐��B
    class CommandKindUtil
    {
    public:
        /// ��������R�}���h���擾����B
        static CommandKind getCommandWithArgumentData( const ArgumentData& );
    };

}
//------------------------------------------------------------
// EOF
