/**
 * @file
 * @brief ErrorReport.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "ErrorReport.hpp"
 
//------------------------------------------------------------
#include <cstdio>

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
void ErrorReport::argumentNotEnoughError()
{
    std::fprintf( stderr, "Error: Not enough arguments provided; try 'textga help' for more info\n" );
}

//------------------------------------------------------------
void ErrorReport::argumentTooMuchError()
{
    std::fprintf( stderr, "Error: Num of argument is too much; try 'textga help' for more info\n" );
}

//------------------------------------------------------------
void ErrorReport::argumentUnknownOptionError( const char* aOptionName )
{
    std::fprintf( stderr, "Error: Unknown option '%s'.\n" , aOptionName );
}

//------------------------------------------------------------
void ErrorReport::fileOpenError( const char* aFilename )
{
    std::fprintf( stderr, "Error: Can't open '%s'\n" , aFilename);
}

//------------------------------------------------------------
void ErrorReport::notSupportedTGAError( const char* aFilename )
{
    std::fprintf( stderr, "Error: Not supported targa file '%s'\n" , aFilename );
}

//------------------------------------------------------------
void ErrorReport::notTexTargaError( const char* aFilename )
{
    std::fprintf( stderr, "Error: Not tex-targa file '%s'\n" , aFilename );
}

//------------------------------------------------------------
void ErrorReport::fileWriteError()
{
    std::fprintf( stderr, "Error: Writing failed.\n" );
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
