/**
 * @file
 * @brief Element�̒��ۃN���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/IElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /**
     * @brief Element�̒��ۃN���X�B
     * ���O�̃|�C���^�Ɨv�f�̎�ނ�ێ�����B
     */
    class AbstractElement : public IElement
    {
    public:
        AbstractElement( const char* name , ElementKind kind );
        virtual ~AbstractElement();

        // IElement
        virtual const char* name()const;
        virtual ElementKind elementKind()const;

    private:
        const char* const name_;
        const ElementKind elementKind_;
    };

}}
//------------------------------------------------------------
// EOF
