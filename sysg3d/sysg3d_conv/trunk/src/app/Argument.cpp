/** 
 * @file
 * @brief Argument.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "app/Argument.hpp"

//------------------------------------------------------------
namespace app {
//------------------------------------------------------------
Argument::Argument( const U32 aArgc , char* aArgv[] )
: argc_( aArgc )
, argv_( aArgv )
{
}

//------------------------------------------------------------
U32 Argument::count()const
{
    return argc_;
}

//------------------------------------------------------------
ConstStr Argument::argAtIndex( const U32 aIndex )const
{
    PJ_ASSERT( aIndex < count() );
    return argv_[ aIndex ];
}

//------------------------------------------------------------
void Argument::dump()
{
    PJ_COUT( "Argument::count -> %lu\n" , argc_ );
    for ( U32 i = 0; i < argc_; ++i )
    {
        PJ_COUT( "Argument::arg[%lu] -> %s\n" , i , argAtIndex( i ) );
    }
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
