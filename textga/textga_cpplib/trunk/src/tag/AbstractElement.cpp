/**
 * @file
 * @brief AbstractElement.hpp�̎������L�q����B
 */
#include <textga/tag/AbstractElement.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
AbstractElement::AbstractElement(
    const char *name 
    , const ElementKind aKind 
    )
: name_( name )
, elementKind_( aKind )
{
}

//------------------------------------------------------------
AbstractElement::~AbstractElement()
{
}

//------------------------------------------------------------
const char* AbstractElement::name()const
{
    return name_;
}

//------------------------------------------------------------
ElementKind AbstractElement::elementKind()const
{
    return elementKind_;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
