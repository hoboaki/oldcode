/**
 * @file 
 * @brief BinaryData.hpp�̎������L�q����B
 */
#include <glpic/BinaryData.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
BinaryData BinaryData::create( 
    void* aAddress
    , const unsigned long aSize
    )
{
    BinaryData data;
    data.address = aAddress;
    data.size = aSize;
    return data;
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
