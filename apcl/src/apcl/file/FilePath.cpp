/** 
 * @file
 * @brief FilePath.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB 
 */
#include <apcl/file/FilePath.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
using namespace ::apcl::file;

//-----------------------------------------------------------
void FilePath::unitTest()
{
  FilePath mypath( "C:/Temp" );
  apclAssert( mypath == FilePath( "C:/Temp" ) );
  mypath.setPath( "" );
  apclAssert( mypath.path() == "" );
}

//-----------------------------------------------------------
FilePath::FilePath()
{
}

//-----------------------------------------------------------
FilePath::FilePath( const string& aPath ):
  path_( aPath )
{
}

//-----------------------------------------------------------
bool FilePath::equals( const FilePath& aPath )const
{
  return path_ == aPath.path();
}

//-----------------------------------------------------------
FilePath::~FilePath()
{
}

//-----------------------------------------------------------
void FilePath::setPath( const string& aPath )
{
	path_ = aPath;
}

//-----------------------------------------------------------
const string& FilePath::path()const
{
	return path_;
}

//-----------------------------------------------------------
// EOF
