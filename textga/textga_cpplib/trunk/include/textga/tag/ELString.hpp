/**
 * @file
 * @brief 文字列の要素クラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// 文字列の要素クラス。
    class ELString : public AbstractElement
    {
    public:
        explicit ELString( const char* name , const char* text );
        virtual ~ELString();

        /// 文字列を取得する。
        const char* text()const;

    private:
        const char* text_;
    };

}}
//------------------------------------------------------------
// EOF
