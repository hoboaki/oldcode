/**
 * @file
 * @brief �o�[�W���������L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace local {

    /// �o�[�W�������B
    class Version
    {
    public:
        /// �o�O�t�B�b�N�X�o�[�W�����B�o�O�t�B�b�N�X�����x�ɂ�����B
        static const ::textga::uint BUGFIX_VERSION;
        
        /// ������Ŏ擾�B�i*.*.*�j
        static std::string asString();
    };

}
//------------------------------------------------------------
// EOF
