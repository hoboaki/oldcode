/**
 * @file
 * @brief TagKind�Ɋւ���֐��Q���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/TagKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// TagKind�Ɋւ���֐��Q�B
    class TagKindUtil
    {
    public:
        /// �����񂩂�擾����B
        static TagKind fromName( const char* name );
    };
}}
//------------------------------------------------------------
// EOF
