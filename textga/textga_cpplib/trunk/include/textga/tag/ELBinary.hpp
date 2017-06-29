/**
 * @file
 * @brief �o�C�i���f�[�^�̗v�f�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// �o�C�i���f�[�^�̗v�f�N���X�B
    class ELBinary : public AbstractElement
    {
    public:
        explicit ELBinary( const char* name , size_t size , const byte* data );
        virtual ~ELBinary();

        /// �o�C�i���f�[�^�̒������擾����B
        size_t size()const;
        /// �o�C�i���f�[�^�̃A�h���X���擾����B
        const byte* data()const;

    private:
        const size_t size_;
        const byte* const data_;
    };

}}
//------------------------------------------------------------
// EOF
