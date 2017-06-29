/**
 * @file
 * @brief 要素のインターフェースクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/ElementKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
    
    /// 要素のインターフェースクラス。
    class IElement
    {
    public:
        virtual ~IElement();

        /// 要素の名前。
        virtual const char* name()const = 0;
        /// 要素の種類。
        virtual ElementKind elementKind()const = 0;
    };

}}
//------------------------------------------------------------
// EOF
