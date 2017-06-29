/**
 * @file
 * @brief Elementの抽象クラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/IElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /**
     * @brief Elementの抽象クラス。
     * 名前のポインタと要素の種類を保持する。
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
