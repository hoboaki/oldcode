/**
 * @file
 * @brief テンプレートを使ったユーティリティを記述する。
 */
#ifndef ACS_INCLUDE_TEMPLATEUTIL
#define ACS_INCLUDE_TEMPLATEUTIL

//------------------------------------------------------------
namespace acscript {
namespace template_util {

    template< int B0 , int B1 , int B2, int B3 , int B4 , int B5 , int B6 , int B7 >
    struct BitToValue
    {
        enum { Value = 
            (B0&1) 
            + ((B1&1)<<1) 
            + ((B2&1)<<2) 
            + ((B3&1)<<3) 
            + ((B4&1)<<4) 
            + ((B5&1)<<5) 
            + ((B6&1)<<6) 
            + ((B7&1)<<7) 
        };
    };

}}
//------------------------------------------------------------
#endif
// EOF
