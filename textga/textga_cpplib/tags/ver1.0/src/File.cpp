/**
 * @file
 * @brief File.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/File.hpp>

//------------------------------------------------------------
#include <textga/Assert.hpp>
#include <textga/Compiler.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
File::File( const char* aFilepath , const char* aOption )
: fp_( 0 )
{
#if defined(TEXTGA_COMPILER_MSC)
    fopen_s( &fp_ , aFilepath , aOption );
#else
    fp_ = std::fopen( aFilepath , aOption );
#endif
}

//------------------------------------------------------------
File::~File()
{
    if ( fp_ != NULL )
    {
        const int result = std::fclose( fp_ );
        (void)result;
        TEXTGA_ASSERT( result == 0 ); 
    }
}

//------------------------------------------------------------
FILE* File::fp()
{
    return fp_;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
