/**
 * @file
 * @brief �v�f�̃C�e���[�^�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <memory>
#include <textga/Types.hpp>
#include <textga/tag/IElement.hpp>
#include <textga/tag/TagKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// �v�f�̃C�e���[�^�N���X�B
    class ElementIterator
    {
    public:
        ElementIterator( const byte* , size_t size );
        ~ElementIterator();

        /**
         * @brief �������Ă���v�f���쐬���Ď��̗v�f�Ɉړ�����B
         * @return �쐬�Ɏ��s������NULL��Ԃ��B
         */
        std::auto_ptr< IElement > next();
        /// ���̗v�f�����邩�B
        bool hasNext()const;

    private:
        const byte* const data_;
        const size_t size_;
        size_t offset_;
        TagKind lastTagKind_;

        //------------------------------------------------------------
        struct NextTagResult
        {
            TagKind     tagKind;
            const char* tagName;
            const byte* data;
        };

        size_t calculateRestSize()const;
        NextTagResult nextTag();
       
    };

}}
//------------------------------------------------------------
// EOF
