/**
 * @file
 * @brief バイナリデータの要素クラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// バイナリデータの要素クラス。
    class ELBinary : public AbstractElement
    {
    public:
        explicit ELBinary( const char* name , size_t size , const byte* data );
        virtual ~ELBinary();

        /// バイナリデータの長さを取得する。
        size_t size()const;
        /// バイナリデータのアドレスを取得する。
        const byte* data()const;

    private:
        const size_t size_;
        const byte* const data_;
    };

}}
//------------------------------------------------------------
// EOF
