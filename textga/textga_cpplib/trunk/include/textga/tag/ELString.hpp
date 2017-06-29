/**
 * @file
 * @brief ������̗v�f�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// ������̗v�f�N���X�B
    class ELString : public AbstractElement
    {
    public:
        explicit ELString( const char* name , const char* text );
        virtual ~ELString();

        /// ��������擾����B
        const char* text()const;

    private:
        const char* text_;
    };

}}
//------------------------------------------------------------
// EOF
