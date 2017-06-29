/**
 * @file
 * @brief �v�f�̃C���^�[�t�F�[�X�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/ElementKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
    
    /// �v�f�̃C���^�[�t�F�[�X�N���X�B
    class IElement
    {
    public:
        virtual ~IElement();

        /// �v�f�̖��O�B
        virtual const char* name()const = 0;
        /// �v�f�̎�ށB
        virtual ElementKind elementKind()const = 0;
    };

}}
//------------------------------------------------------------
// EOF
