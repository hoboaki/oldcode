/**
 * @file
 * @brief ELBinary.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/tag/ELBinary.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
ELBinary::ELBinary( 
    const char* aName
    , const size_t aSize
    , const byte* aData
    )
: AbstractElement( aName , ElementKind_Binary )
, size_( aSize )
, data_( aData )
{
}

//------------------------------------------------------------
ELBinary::~ELBinary()
{
}

//------------------------------------------------------------
size_t ELBinary::size()const
{
    return size_;
}

//------------------------------------------------------------
const byte* ELBinary::data()const
{
    return data_;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
