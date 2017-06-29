/**
 * @file
 * @brief �o�[�W���������L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <string>

//------------------------------------------------------------
namespace textga {

    /**
     * @brief �o�[�W�������B
     * TexTargaLib�̃o�[�W������*.*�̌`���ŕ\�����B
     * 1�߂�FORMAT_VERSION�BTexTarga�t�H�[�}�b�g���傫���ύX�����x�ɑ�����B
     * 2�߂�LIBRARY_VERSION�BTexTargaC++Lib���o�O�t�B�b�N�X�����x�ɑ�����B
     */
    class Version
    {
    public:
        static const ::textga::u8   FORMAT_VERSION;
        static const ::textga::uint LIBRARY_VERSION;
        
        static std::string asString();
    };
}
//------------------------------------------------------------
// EOF
