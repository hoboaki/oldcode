/** 
 * @file
 * @brief FileLoader.hpp �̎������L�q����B 
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
	//---�I�[�v�����[�h�̐ݒ�
	std::ios_base::open_mode	mode = std::ios::in;
	if ( type_ == FileType_Binary )
		mode |= std::ios::binary;

	//---�C���v�b�g�X�g���[���̍쐬
	std::ifstream	in( path_.path().c_str() , mode  );
	if ( !in.is_open() )
	{//---�t�@�C���I�[�v���Ɏ��s
		return false;
	}
	in.setf( std::ios::skipws );

	//---�t�@�C����ǂݍ���
	Data	tmpData;
	byte* buffer = new byte[aBufferSize];
	while( !in.eof() )
	{
		in.read( reinterpret_cast<char*>(buffer) , aBufferSize );
		tmpData += Data( buffer , in.gcount() );
	}
	delete[] buffer;

	//---�X�g���[�������
	in.close();	

	//---���ʂ̃R�s�[
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
