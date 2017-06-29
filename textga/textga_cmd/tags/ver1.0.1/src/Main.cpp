/**
 * @file
 * @brief mainŠÖ”‚ğ‹Lq‚·‚éB
 */

//------------------------------------------------------------
#include <local/ArgumentData.hpp>
#include <local/EntryPoint.hpp>

//------------------------------------------------------------
int main (int argc, char * const argv[])
{
    return ::local::EntryPoint::run(
        ::local::ArgumentData::create( argc, argv )
        );
}

//------------------------------------------------------------
// EOF

