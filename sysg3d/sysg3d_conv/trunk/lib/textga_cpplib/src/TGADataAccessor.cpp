/**
 * @file
 * @brief TGADataAccessor.hpp�̎������L�q����B
 */
#include <textga/TGADataAccessor.hpp>

//------------------------------------------------------------
#include <cstdio>
#include <map>
#include <textga/Assert.hpp>
#include <textga/Constant.hpp>
#include <textga/EndianUtil.hpp>
#include <textga/File.hpp>
#include <textga/Pixel.hpp>
#include <textga/PixelFormatUtil.hpp>
#include <textga/StringUtil.hpp>
#include <textga/TGAHeader.hpp>
#include <textga/Version.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
TGADataAccessor::TGADataAccessor( const byte* aData , const size_t aDataSize )
: data_( aData )
, dataSize_( aDataSize )
, isSupportedTGA_(false)
, bitsPerPixel_()
, pixelCount_()
, isTexTarga_(false)
, ttPixelFormat_()
, ttSrcData_(0)
{
    // Targa�̃f�[�^����
    {
        if ( dataSize_ <= TGAHeader::SIZE )
        {
            return;
        }
        
        const TGAHeader& tgaHeader = header();
        if ( tgaHeader.idLength != 0 )
        {
            return;
        }
        if ( tgaHeader.colorMapType != 0 )
        {
            return;
        }
        if ( tgaHeader.dataTypeCode != 2 )
        {
            return;
        }
        if ( tgaHeader.colorMapElement1 != 0 )
        {
            return;
        }
        if ( tgaHeader.colorMapElement2 != 0 )
        {
            return;
        }
        if ( tgaHeader.x_origin != 0 )
        {
            return;
        }
        if ( tgaHeader.y_origin != 0 )
        {
            return;
        }
        if ( tgaHeader.width == 0 )
        {
            return;
        }
        if ( tgaHeader.height == 0 )
        {
            return;
        }
        switch( tgaHeader.bitsPerPixel )
        {
        case 24:
            if ( tgaHeader.imageDescriptor != 0 )
            {
                return;
            }
            break;
                
        case 32:
            if ( tgaHeader.imageDescriptor != 8 )
            {
                return;
            }
            break;
                
        default:
            return;
        }
        
        // �t�@�C���̒������\���ɂ��邩
        {
            const size_t needSize = TGAHeader::SIZE
            + (tgaHeader.bitsPerPixel/8) 
            * EndianUtil::swapU16BE( tgaHeader.width )
            * EndianUtil::swapU16BE( tgaHeader.height );
            
            if ( dataSize_ < needSize )
            {
                return;
            }
        }
    
        // �T�|�[�g���Ă���Targa�ł�
        isSupportedTGA_ = true;
        bitsPerPixel_ = header().bitsPerPixel;
        pixelCount_ = width() * height();
    }
    
    // TexTarga�̃f�[�^����
    {
        size_t offset = TGAHeader::SIZE + pixelDataSize();
        if ( dataSize_ <= offset )
        {// �f�[�^������Ȃ��B
            return;
        }
        
        std::map< std::string , int > findTagMap;
        while ( offset < dataSize_ )
        {
            const char* tagName = StringUtil::findString( &data_[offset] , dataSize_ - offset );
            if ( tagName == 0 )
            {// �^�O���Ȃ��B
                return;
            }
            if ( findTagMap.find( tagName ) != findTagMap.end() )
            {// ���ɑ��݂���^�O
                return;
            }
            // �^�O����
            findTagMap[ tagName ] = 1;
            // �I�t�Z�b�g���Z
            offset += std::strlen( tagName )+1;
            
            if ( StringUtil::equals( tagName , Constant::TAG_LOGO ) )
            {// Logo
                if ( findTagMap.size() != 1 )
                {// �擪�ɂȂ�
                    return;
                }
                if ( data_[offset] != Version::FORMAT_VERSION )
                {// �o�[�W�������Ⴄ�B
                    return;
                }
                offset += 1;
            }
            else if ( StringUtil::equals( tagName , Constant::TAG_PIXEL_FORMAT ) )
            {// PixelFormat
                const char* pixFmtName = StringUtil::findString( &data_[offset] , dataSize_ - offset );
                if ( pixFmtName == 0 )
                {// ���O��������Ȃ�
                    return;
                }
                ttPixelFormat_ = PixelFormatUtil::fromName( pixFmtName );
                if ( ttPixelFormat_ == PixelFormat_Unknown )
                {// �����ȃs�N�Z���t�H�[�}�b�g
                    return;
                }
                offset += std::strlen( pixFmtName ) + 1;
            }
            else if ( StringUtil::equals( tagName , Constant::TAG_SRC_DATA ) )
            {// SrcData
                const size_t restDataSize = dataSize_ - offset;
                const size_t pixelDataSize = pixelCount() * ( bitsPerPixel()/8 );
                if ( restDataSize < pixelDataSize )
                {// �f�[�^�T�C�Y������Ȃ��B
                    return;
                }
                ttSrcData_ = &data_[ offset ];
                offset += pixelDataSize;
            }
            else if ( StringUtil::equals( tagName , Constant::TAG_TERMINATE ) )
            {// Terminate
                break;
            }
            else
            {// �s���ȃ^�O
                return;
            }
        }
        
        // �K�v�ȃ^�O�����邩�B
        const char* needTagArray[]=
        {
            Constant::TAG_LOGO
            ,Constant::TAG_PIXEL_FORMAT
            ,Constant::TAG_TERMINATE
            ,0
        };
        for ( const char*const* ptr = needTagArray; *ptr != 0; ++ptr )
        {
            if ( findTagMap.find( *ptr ) == findTagMap.end() )
            {
                return;
            }
        }
        
        // TexTarga�ł�
        isTexTarga_ = true;
    }
}

//------------------------------------------------------------
bool TGADataAccessor::isSupportedTGA()const
{
    return isSupportedTGA_;
}

//------------------------------------------------------------
u16 TGADataAccessor::width()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return EndianUtil::swapU16BE( header().width );
}

//------------------------------------------------------------
u16 TGADataAccessor::height()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return EndianUtil::swapU16BE( header().height );
}

//------------------------------------------------------------
u8 TGADataAccessor::bitsPerPixel()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return bitsPerPixel_;
}

//------------------------------------------------------------
size_t TGADataAccessor::pixelCount()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return pixelCount_;
}

//------------------------------------------------------------
size_t TGADataAccessor::pixelDataSize()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return pixelCount() * ( bitsPerPixel() / 8 );
}

//------------------------------------------------------------
Pixel TGADataAccessor::pixelAtIndex( const size_t aIndex )const
{
    const size_t offset = TGAHeader::SIZE;
    return getPixel( &data_[ offset ] , aIndex );
}

//------------------------------------------------------------
bool TGADataAccessor::isTexTarga()const
{
    TEXTGA_ASSERT( isSupportedTGA() );
    return isTexTarga_;
}

//------------------------------------------------------------
PixelFormat TGADataAccessor::pixelFormat()const
{
    TEXTGA_ASSERT( isTexTarga() );
    return ttPixelFormat_;
}

//------------------------------------------------------------
bool TGADataAccessor::isExistSrcData()const
{
    TEXTGA_ASSERT( isTexTarga() );
    return ttSrcData_ != 0;
}

//------------------------------------------------------------
Pixel TGADataAccessor::srcDataPixelAtIndex( const size_t aIndex )const
{
    TEXTGA_ASSERT( isExistSrcData() );
    return getPixel( ttSrcData_ , aIndex );
}

//------------------------------------------------------------
const byte* TGADataAccessor::srcData()const
{
    TEXTGA_ASSERT( isExistSrcData() );
    return ttSrcData_;
}

//------------------------------------------------------------
const TGAHeader& TGADataAccessor::header()const
{
    return reinterpret_cast< const TGAHeader& >(
        *data_
        );
}

//------------------------------------------------------------
Pixel TGADataAccessor::getPixel( const byte* aData , const size_t aIndex )const
{
    TEXTGA_ASSERT( aIndex < pixelCount() );
    const size_t offset = (bitsPerPixel()/8) * aIndex;
    
    Pixel pix;
    pix.b = aData[offset];
    pix.g = aData[offset+1];
    pix.r = aData[offset+2];
    if ( bitsPerPixel() == 32 )
    {
        pix.a = aData[offset+3];
    }
    else
    {
        pix.a = 255;
    }
    return pix;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
