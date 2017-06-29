/**
 * @file
 * @brief ArgumentDataIterator.hppの実装を記述する。
 */
#include "ArgumentDataIterator.hpp"

//------------------------------------------------------------
#include <local/ArgumentData.hpp>
#include "Assert.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
ArgumentDataIterator::ArgumentDataIterator( const ArgumentData& aData )
: data_( aData )
, index_( 0 )
{
}

//------------------------------------------------------------
const char* ArgumentDataIterator::get()const
{
    PJ_ASSERT( !isEnd() );
    return data_.texts[ index_ ];
}

//------------------------------------------------------------
bool ArgumentDataIterator::isEnd()const
{
    return data_.count == index_;
}

//------------------------------------------------------------
void ArgumentDataIterator::next()
{
    PJ_ASSERT( !isEnd() );
    if ( !isEnd() ) // 安全のため。
    {
        ++index_;
    }
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
