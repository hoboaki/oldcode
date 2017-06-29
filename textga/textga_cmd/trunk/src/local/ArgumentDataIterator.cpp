/**
 * @file
 * @brief ArgumentDataIterator.hpp�̎������L�q����B
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
bool ArgumentDataIterator::hasNext()const
{
    return data_.count != index_;
}

//------------------------------------------------------------
const char* ArgumentDataIterator::next()
{
    PJ_ASSERT( hasNext() );
    const char* val = data_.getTextAtIndex( index_ );
    if ( hasNext() ) // ���S�̂��߁B
    {
        ++index_;
    }
    return val;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
