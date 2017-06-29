/**
 * @file
 * @brief Endian.hpp‚ÌÀ‘•‚ğ‹Lq‚·‚éB
 */
#include "Endian.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Endian::isLittleEndian()
{
#if defined(__LITTLE_ENDIAN__)
        return true;
#elif defined(__BIG_ENDIAN__)
        return false;
#else
        const int i = 1;
        return (*reinterpret_cast< const char* >( &i ) != 0);
#endif
}

//------------------------------------------------------------
bool Endian::isBigEndian()
{
    return !isLittleEndian();
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
