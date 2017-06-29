/**
 * @file
 * @brief ELString.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/tag/ELString.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
ELString::ELString( 
    const char* aName
    , const char* aText
    )
: AbstractElement( aName , ElementKind_String )
, text_( aText )
{
}

//------------------------------------------------------------
ELString::~ELString()
{
}

//------------------------------------------------------------
const char* ELString::text()const
{
    return text_;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
