/**
 * @file
 * @brief ELSection.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/tag/ELSection.hpp>

//------------------------------------------------------------
#include <limits>
#include <textga/Assert.hpp>
#include <textga/StringUtil.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
const uint ELSection::INVALID_INDEX = std::numeric_limits< uint >::max();

//------------------------------------------------------------
ELSection::ELSection( 
    const char* aName
    )
: AbstractElement( aName , ElementKind_Section )
, elements_()
{
}

//------------------------------------------------------------
ELSection::~ELSection()
{
    for ( size_t i = 0; i < elements_.size(); ++i )
    {
        delete elements_[i];
        elements_[i] = 0;
    }
}

//------------------------------------------------------------
void ELSection::add( std::auto_ptr< IElement > aElement )
{
    TEXTGA_ASSERT( aElement.get() != 0 );
    elements_.push_back( aElement.release() );
}

//------------------------------------------------------------
uint ELSection::count()const
{
    return static_cast< uint >( elements_.size() );
}

//------------------------------------------------------------
const IElement& ELSection::getElementAtIndex( const uint aIndex )const
{
    TEXTGA_ASSERT( aIndex < count() );
    return *elements_.at( aIndex );
}

//------------------------------------------------------------
const IElement* ELSection::getElementPtrWithName( const char* aName )const
{
    return getElementPtrWithName( aName , 0 );
}

//------------------------------------------------------------
const IElement* ELSection::getElementPtrWithName( 
    const char* aName 
    , const uint aFromIndex
    )const
{
    const uint index = getIndexWithName( aName , aFromIndex );
    if ( index == INVALID_INDEX )
    {
        return 0;
    }
    return &getElementAtIndex( index );
}

//------------------------------------------------------------
uint ELSection::getIndexWithName( const char* aName )const
{
    return getIndexWithName( aName , 0 );
}

//------------------------------------------------------------
uint ELSection::getIndexWithName( 
    const char* aName
    , const uint aFromIndex
    )const
{
    for ( uint i = aFromIndex; i < count(); ++i )
    {
        if ( StringUtil::equals( aName , getElementAtIndex(i).name() ) )
        {
            return i;
        }
    }
    return INVALID_INDEX;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
