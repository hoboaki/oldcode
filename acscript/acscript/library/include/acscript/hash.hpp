/**
 * @file
 * @brief Hash型を記述する。
 */
#ifndef ACS_INCLUDE_HASH
#define ACS_INCLUDE_HASH

//------------------------------------------------------------
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// ハッシュ値。MD5で求めた値が入るように128bitの領域を用意。
    struct Hash
    {
        U32 data[8];

        /// ２つのハッシュは等しいか。
        bool equals( const Hash& rhs );
    };
    /// Hash::equalsのエイリアス。
    bool operator ==(const Hash&,const Hash&);

}
//------------------------------------------------------------
#endif
// EOF
