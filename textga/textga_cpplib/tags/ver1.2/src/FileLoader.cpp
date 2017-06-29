/**
 * @file
 * @brief FileLoader.hppの実装を記述する。
 */
#include <textga/FileLoader.hpp>

//------------------------------------------------------------
#include <textga/Assert.hpp>
#include <textga/File.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
FileLoader::FileLoader( const char* aFilepath )
: isLoaded_( false )
, bytes_()
{    
    File file( aFilepath , "rb" );
    if ( file.fp() == 0 )
    {
        return;
    }
    
    // ファイルサイズを調べる
    if ( std::fseek( file.fp() , 0 , SEEK_END ) != 0 )
    {
        return;
    }
    const long tellSize = std::ftell( file.fp() );
    if ( tellSize <= 0 )
    {
        return ;
    }
    if ( std::fseek( file.fp() , 0 , SEEK_SET ) != 0 )
    {
        return;
    }
    
    // ロード
    const size_t fileSize = static_cast< size_t >( tellSize );
    bytes_.resize( fileSize );
    if ( std::fread( &bytes_.at(0) , fileSize , 1 , file.fp()  ) < 1 )
    {
        return;
    }
    
    // 成功
    isLoaded_ = true;
}

//------------------------------------------------------------
bool FileLoader::isLoaded()const
{
    return isLoaded_;
}

//------------------------------------------------------------
const byte* FileLoader::data()const
{
    TEXTGA_ASSERT( isLoaded() );
    if ( dataSize() == 0 )
    {
        return 0;
    }
    else
    {
        return &bytes_.at(0);
    }
}

//------------------------------------------------------------
size_t FileLoader::dataSize()const
{
    return bytes_.size();
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
