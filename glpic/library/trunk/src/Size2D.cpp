/**
 * @file
 * @brief Size2D.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
Size2D Size2D::create(
   const unsigned short aWidth
   , const unsigned short aHeight
   )
{
    Size2D size2D;
    size2D.width = aWidth;
    size2D.height = aHeight;
    return size2D;
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
