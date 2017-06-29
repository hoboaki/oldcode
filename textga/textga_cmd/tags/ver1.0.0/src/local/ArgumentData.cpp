/**
 * @file
 * @brief ArgumentData.hpp‚ÌÀ‘•‚ğ‹Lq‚·‚éB
 */
#include "ArgumentData.hpp"

//------------------------------------------------------------
#include <cstdio>
#include "Assert.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
void ArgumentData::dump()const
{
    std::printf("[ArgumentData]%p (Count:%d)\n" , this , count );
    for ( int i = 0; i < count; ++i )
    {
        std::printf( "[%d]%s\n" , i , texts[i] );
    }
}

//------------------------------------------------------------
const char* ArgumentData::getTextAtIndex( const int aIndex )const 
{
    PJ_ASSERT( aIndex < count );
    return texts[ aIndex ];
}

//------------------------------------------------------------
ArgumentData ArgumentData::create( 
    const int aCount
    , const char* const* aTexts
    )
{
    ArgumentData data;
    data.count = aCount;
    data.texts = aTexts;
    return data;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
