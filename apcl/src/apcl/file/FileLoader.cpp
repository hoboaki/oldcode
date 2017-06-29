/** 
 * @file
 * @brief FileLoader.hpp の実装を記述する。 
 */
#include <apcl/file/FileLoader.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <fstream>

//-----------------------------------------------------------
using namespace ::apcl::file;
using namespace ::apcl::util;

//-----------------------------------------------------------
void FileLoader::unitTest()
{
  FilePath mypath( "abc.txt" );
  FileType mytype( FileType_Ascii );
  FileLoader loader( mypath , mytype );
  apclAssert( mypath == loader.path() );
  apclAssert( mytype == loader.type() );

}

//-----------------------------------------------------------
FileLoader::FileLoader( const FilePath& aPath , const FileType aType ):
path_( aPath ),
type_( aType )
{
}

//-----------------------------------------------------------
FileLoader::~FileLoader()
{
}

//-----------------------------------------------------------
const FilePath& FileLoader::path()const
{
	return path_;
}

//-----------------------------------------------------------
FileType FileLoader::type()const
{
	return type_;
}

//-----------------------------------------------------------
bool FileLoader::load( const u32 aBufferSize )
{
	//---オープンモードの設定
	std::ios_base::open_mode	mode = std::ios::in;
	if ( type_ == FileType_Binary )
		mode |= std::ios::binary;

	//---インプットストリームの作成
	std::ifstream	in( path_.path().c_str() , mode  );
	if ( !in.is_open() )
	{//---ファイルオープンに失敗
		return false;
	}
	in.setf( std::ios::skipws );

	//---ファイルを読み込む
	Data	tmpData;
	byte* buffer = new byte[aBufferSize];
	while( !in.eof() )
	{
		in.read( reinterpret_cast<char*>(buffer) , aBufferSize );
		tmpData += Data( buffer , in.gcount() );
	}
	delete[] buffer;

	//---ストリームを閉じる
	in.close();	

	//---結果のコピー
	data_ = tmpData;

	return true;
}

//-----------------------------------------------------------
const Data& FileLoader::data()const
{
	return data_;
}

//-----------------------------------------------------------
// EOF
