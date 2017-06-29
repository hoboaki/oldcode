/**
 * @file
 * @brief 数の要素クラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// 数字の要素のテンプレートクラス。
    template < typename NumType , ElementKind ElementKindValue >
    class ELNumTmpl : public AbstractElement
    {
    public:
        ELNumTmpl( const char* name , const NumType value )
            : AbstractElement( name , ElementKindValue )
            , value_( value )
        {
        }
        virtual ~ELNumTmpl(){};

        /// 値を取得する。
        NumType value()const
        {
            return value_;
        };

    private:
        const NumType value_;
    };

    typedef ELNumTmpl< u8 , ElementKind_NumU8 > ELNumU8;
    typedef ELNumTmpl< u16, ElementKind_NumU16> ELNumU16;
    typedef ELNumTmpl< u32, ElementKind_NumU32> ELNumU32;
    typedef ELNumTmpl< s8 , ElementKind_NumS8 > ELNumS8;
    typedef ELNumTmpl< s16, ElementKind_NumS16> ELNumS16;
    typedef ELNumTmpl< s32, ElementKind_NumS32> ELNumS32;

}}
//------------------------------------------------------------
// EOF
