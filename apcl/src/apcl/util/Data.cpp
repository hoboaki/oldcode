/**
 * @file
 * @brief Data.hppの実装を記述する。 
 */
#include <apcl/util/Data.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <cstring>

//-----------------------------------------------------------
using namespace ::apcl::util;
using namespace ::std;

//-----------------------------------------------------------
void Data::unitTest()
{
  Data data;
  apclAssert( data.bytes() == 0 );
  apclAssert( data.length() == 0 );

  char* str = "Test";
  Data pushData( str , 4 );
  data += pushData;
  apclAssert( data.length() == 4 );
  apclAssert( data == pushData );
  data.remove( 4 );
  apclAssert( data.length() == 0 );
}

//-----------------------------------------------------------
Data::Data(void):
bytes_(0),
length_(0)
{
}

//-----------------------------------------------------------
Data::Data( const Data& aData ):
bytes_(0),
length_(0)
{
	init( aData.bytes() , aData.length() );
}

//-----------------------------------------------------------
Data::Data( const void* const aBytes , const u32 aLength ):
bytes_(0),
length_(0)
{
  init( aBytes , aLength );
}

//-----------------------------------------------------------
Data::~Data()
{
	_releaseBytes();
}

//-----------------------------------------------------------
void Data::init( const void* const aBytes , const u32 aLength )
{
	_releaseBytes();
	length_ = aLength;
  if ( aLength > 0 )
  {
    apclAssert( bytes_ == 0 );
    bytes_ = new byte[aLength];
	  memcpy( bytes_ , aBytes , aLength );
  }
}

//-----------------------------------------------------------
void Data::copy( const Data& aData )
{
  init( aData.bytes() , aData.length() );
}

//-----------------------------------------------------------
u32 Data::length()const
{
	return length_;
}

//-----------------------------------------------------------
void Data::resize( const u32 aSize )
{
	// メモ
	byte* preBytes = bytes_;
	u32 preLength = length_;

	// 新領域確保
	if ( aSize > 0 )
		bytes_ = new byte[aSize];
	else
		bytes_ = 0;
	length_ = aSize;

	// コピー or 0セット
	if ( length_ > 0 ) 
	{
		if ( preLength > 0 )
		{// 前の内容をコピー
			const u32 size = preLength < length_ ? preLength : length_;
			memcpy( bytes_ , preBytes, size );
		}
		else 
		{// 0をセット
			memset( bytes_ , 0 , length_ );
		}
	}

	if ( length_ > preLength )
	{// 前の長さより長ければ，追加分を0で埋める。
		memset( &bytes_[preLength] , 0 , length_ - preLength );
	}

	// 前領域解放
  if ( preBytes )
  	delete[] preBytes;

}

//-----------------------------------------------------------
void Data::add( const Data& aData )
{
  if ( aData.length() > 0 )
  {
    const u32 preLength = length_;
	  resize( length_ + aData.length() );
	  memcpy( &bytes_[preLength] , aData.bytes() , aData.length() );
  }
}

//-----------------------------------------------------------
void Data::remove( const u32 aLength )
{
	if ( aLength > 0 )
  {
	  assert( aLength <= length_ );
	  resize( length_ - aLength );
  }
}

//-----------------------------------------------------------
bool Data::equals( const Data& aData )const
{
	if ( length_ == aData.length() )
		return ( memcmp( bytes_ , aData.bytes() , length_ ) == 0 );
	return false;
}

//-----------------------------------------------------------
byte& Data::at( const u32 aIndex )
{
	assert( aIndex >= 0 && aIndex < length_ );
	return bytes_[aIndex];
}

//-----------------------------------------------------------
byte Data::at( const u32 aIndex )const
{
	assert( aIndex >= 0 && aIndex < length_ );
	return bytes_[aIndex];
}

//-----------------------------------------------------------
byte* Data::bytes()
{
	return bytes_;
}

//-----------------------------------------------------------
const byte* Data::bytes()const
{
	return bytes_;
}

//-----------------------------------------------------------
byte& Data::operator []( const u32 aIndex )
{
	return at( aIndex );
}

//-----------------------------------------------------------
byte Data::operator []( const u32 aIndex )const
{
	return at( aIndex );
}

//-----------------------------------------------------------
Data& Data::operator =( const Data& aData )
{
  copy( aData );
  return *this;
}

//-----------------------------------------------------------
void Data::_releaseBytes()
{
	if ( bytes_ )
	{
		delete[] bytes_;
		bytes_ = 0;
		length_ = 0;
	}
}

//-----------------------------------------------------------
// EOF
