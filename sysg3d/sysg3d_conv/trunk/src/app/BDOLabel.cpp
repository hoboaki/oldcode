/** 
 * @file
 * @brief BDOLabelå^ÇãLèqÇ∑ÇÈÅB
 */
#include "app/BDOLabel.hpp"

//------------------------------------------------------------
namespace app {
//------------------------------------------------------------
BDOLabel::BDOLabel()
: mainNo_( INVALID_NO )
, subNo_( INVALID_NO )
{
}

//------------------------------------------------------------
BDOLabel::BDOLabel( 
    const U32 aMainNo
    , const U32 aSubNo
    )
: mainNo_( aMainNo )
, subNo_( aSubNo )
{
    PJ_ASSERT( mainNo_ != INVALID_NO );
    PJ_ASSERT( subNo_ != INVALID_NO );
}

//------------------------------------------------------------
bool BDOLabel::operator <( const BDOLabel& aRHS )const
{
    return mainNo_ < aRHS.mainNo_
        || subNo_ < aRHS.subNo_;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
