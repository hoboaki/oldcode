/** 
 * @file 
 * @brief File.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB 
 */
#include <apcl/file/File.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
using namespace ::apcl::file;

//-----------------------------------------------------------
void File::unitTest()
{
  File file;

  FilePath path;
  path.setPath( "MyPath" );
  file.setPath( path );
  apclAssert( path == file.path() );

  FileType type;
  type = FileType_Ascii;
  file.setType( type );
  apclAssert( type == file.type() );

  Data data;
  data.resize( 10 );
  file.setData( data );
  apclAssert( data == file.data() );

}

//-----------------------------------------------------------
File::File():
type_( FileType_Binary )
{
}

//-----------------------------------------------------------
File::~File()
{
}

//-----------------------------------------------------------
bool File::equals( const File& aFile )const
{
  return ( path_ == aFile.path() && 
    type_ == aFile.type() && 
    data_ == aFile.data() );
}

//-----------------------------------------------------------
const FilePath& File::path()const
{
	return path_;
}

//-----------------------------------------------------------
void File::setPath( const FilePath& aPath )
{
	path_ = aPath;
}

//-----------------------------------------------------------
const Data& File::data()const
{
	return data_;
}	

//-----------------------------------------------------------
void File::setData( const Data& aData )
{
	data_ = aData;
}

//-----------------------------------------------------------
FileType File::type()const
{
	return type_;
}

//-----------------------------------------------------------
void File::setType( const FileType aType )
{
	type_ = aType;
}

//-----------------------------------------------------------
// EOF
