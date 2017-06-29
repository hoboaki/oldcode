/** 
 * @file 
 * @brief FileSaver.hpp の実装を記述する。 
 */
#include <apcl/file/FileSaver.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <apcl/file/File.hpp>
#include <apcl/file/FilePath.hpp>
#include <apcl/util/Data.hpp>
#include <fstream>

//-----------------------------------------------------------
using namespace ::apcl::file;
using namespace ::apcl::util;

//-----------------------------------------------------------
void FileSaver::unitTest()
{
  File myfile;
  myfile.setPath( FilePath( "Out.txt" ) );
  
  FileSaver saver( myfile );
  apclAssert( myfile == saver.file() );
}

//-----------------------------------------------------------
FileSaver::FileSaver( const File& aFile ):
file_( aFile )
{
}

//-----------------------------------------------------------
FileSaver::~FileSaver()
{
}

//-----------------------------------------------------------
const File& FileSaver::file()const
{
	return file_;
}

//-----------------------------------------------------------
bool FileSaver::save()const
{
	//---オープンモードの設定
	std::ios_base::open_mode mode = std::ios_base::out;
	if ( file_.type() == FileType_Binary )
    mode |= std::ios::binary;

	//---インプットストリームの作成
	std::ofstream	out( file_.path().path().c_str() , mode  );
	if ( !out.is_open() )
	{//---ファイルオープンに失敗
		return false;
	}

	//---ファイルを書き出す
	out.write( reinterpret_cast<const char*>( file_.data().bytes() ) , file_.data().length() );

	//---ストリームを閉じる
	out.close();	

	return true;
}

//-----------------------------------------------------------
// EOF
