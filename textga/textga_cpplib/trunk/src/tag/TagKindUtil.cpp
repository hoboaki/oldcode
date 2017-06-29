/**
 * @file
 * @brief TagKindUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/tag/TagKindUtil.hpp>

//------------------------------------------------------------
#include <textga/StringUtil.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
TagKind TagKindUtil::fromName( const char* aName )
{
    if ( StringUtil::equals( aName , "BEGIN" ) )
    {
        return TagKind_SectionBegin;
    }
    else if ( StringUtil::equals( aName , "END" ) )
    {
        return TagKind_SectionEnd;
    }
    else if ( StringUtil::equals( aName , "U8" ) )
    {
        return TagKind_NumU8; 
    }
    else if ( StringUtil::equals( aName , "U16" ) )
    {
        return TagKind_NumU16; 
    }
    else if ( StringUtil::equals( aName , "U32" ) )
    {
        return TagKind_NumU32;
    }
    else if ( StringUtil::equals( aName , "S8" ) )
    {
        return TagKind_NumS8; 
    }
    else if ( StringUtil::equals( aName , "S16" ) )
    {
        return TagKind_NumS16;
    }
    else if ( StringUtil::equals( aName , "S32" ) )
    {
        return TagKind_NumS32;
    }
    else if ( StringUtil::equals( aName , "STRING" ) )
    {
        return TagKind_String;
    }
    else if ( StringUtil::equals( aName , "BINARY" ) )
    {
        return TagKind_Binary;
    }
    else
    {
        return TagKind_Unknown;
    }
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
