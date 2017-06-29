/**
 * @file
 * @brief PixelFormatUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <local/PixelFormatUtil.hpp>

//------------------------------------------------------------
#include <squish.h>
#include "Assert.hpp"
#include "Pixel.hpp"

//------------------------------------------------------------
using ::local::Pixel;

//------------------------------------------------------------
namespace
{
    unsigned char t_mask( const int aBit )
    {
        PJ_ASSERT( 1 <= aBit && aBit <= 8 );
        const unsigned char maskArray[]=
        {
            0x80
            ,0xC0
            ,0xE0
            ,0xF0
            ,0xF8
            ,0xFC
            ,0xFE
            ,0xFF
        };
        return maskArray[ aBit -1 ];
    }
    
    unsigned char t_convert( const unsigned char aValue , const int aBit )
    {
        PJ_ASSERT( 1 <= aBit && aBit <= 8 );
        unsigned char mask = t_mask( aBit );
        unsigned char src = aValue;
        unsigned char result = 0;
        for ( int i = 0; i < 8; i += aBit )
        {
            result |= (src&mask);
            src >>= aBit;
            mask >>= aBit;
        }
        return result;
    }
    
    Pixel t_alphaConvert( const Pixel& aPixel , const int aBit )
    {
        Pixel pix;
        pix.r = 0xFF;
        pix.g = 0xFF;
        pix.b = 0xFF;
        pix.a = t_convert( aPixel.a , aBit );
        return pix;
    }
    Pixel t_luminanceConvert( const Pixel& aPixel , const int aBit )
    {
        const int tmpElement = aPixel.r + aPixel.g + aPixel.b;
        Pixel pix;
        pix.r = t_convert( static_cast<char>( tmpElement/3 ) , aBit );
        pix.g = pix.r;
        pix.b = pix.r;
        pix.a = 0xFF;
        return pix;
    }
    Pixel t_luminanceAlphaConvert( const Pixel& aPixel , const int aBit )
    {
        Pixel pix = t_luminanceConvert( aPixel , aBit );
        pix.a = t_alphaConvert( aPixel , aBit ).a;
        return pix;
    }
    Pixel t_intensityConvert( const Pixel& aPixel , const int aBit )
    {
        Pixel pix;
        pix.r = t_convert( static_cast<char>( (aPixel.r + aPixel.g + aPixel.b + aPixel.a)/4 ) , aBit );
        pix.g = pix.r;
        pix.b = pix.r;
        pix.a = pix.r;
        return pix;
    }
    Pixel t_rgbConvert( const Pixel& aPixel , const int aRBit , const int aGBit , const int aBBit )
    {
        Pixel pix;
        pix.r = t_convert( aPixel.r , aRBit );
        pix.g = t_convert( aPixel.g , aGBit );
        pix.b = t_convert( aPixel.b , aBBit );
        pix.a = 0xFF;
        return pix;
    }
    Pixel t_rgbaConvert( const Pixel& aPixel , const int aRBit , const int aGBit , const int aBBit , const int aABit )
    {
        Pixel pix;
        pix.r = t_convert( aPixel.r , aRBit );
        pix.g = t_convert( aPixel.g , aGBit );
        pix.b = t_convert( aPixel.b , aBBit );
        pix.a = t_convert( aPixel.a , aABit );
        return pix;
    }
}

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
const char* PixelFormatUtil::toName( const PixelFormat aPixelFormat )
{
    PJ_ASSERT( isValid( aPixelFormat ) );
    
    switch( aPixelFormat )
    {
    case PixelFormat_A4     : return "A4";
    case PixelFormat_A8     : return "A8";
    case PixelFormat_L4     : return "L4";
    case PixelFormat_L8     : return "L8";
    case PixelFormat_LA4    : return "LA4";
    case PixelFormat_LA8    : return "LA8";
    case PixelFormat_I4     : return "I4";
    case PixelFormat_I8     : return "I8";
    case PixelFormat_RGB232 : return "RGB232";
    case PixelFormat_RGB565 : return "RGB565";
    case PixelFormat_RGB8   : return "RGB8";
    case PixelFormat_RGB5A1 : return "RGB5A1";
    case PixelFormat_RGBA2  : return "RGBA2";
    case PixelFormat_RGBA4  : return "RGBA4";
    case PixelFormat_RGBA6  : return "RGBA6";
    case PixelFormat_RGBA8  : return "RGBA8";
    
    case PixelFormat_RGBA_S3TC_DXT1 : return "RGBA_S3TC_DXT1";
    case PixelFormat_RGBA_S3TC_DXT3 : return "RGBA_S3TC_DXT3";
    case PixelFormat_RGBA_S3TC_DXT5 : return "RGBA_S3TC_DXT5";
    
    default:
        PJ_ASSERT(0);
        return "";
    }
}

//------------------------------------------------------------
bool PixelFormatUtil::isValid( const PixelFormat aPixelFormat )
{
    return PixelFormat_Begin <= aPixelFormat
        && aPixelFormat < PixelFormat_End;
}

//------------------------------------------------------------
Pixel PixelFormatUtil::convert(
    const Pixel& aPixel
    , const PixelFormat aPixelFormat
    )
{
    PJ_ASSERT( isValid( aPixelFormat ) );
    
    switch( aPixelFormat )
    {
    case PixelFormat_A4     :
        return t_alphaConvert( aPixel , 4 );
        
    case PixelFormat_A8     :
        return t_alphaConvert( aPixel , 8 );
        
    case PixelFormat_L4     :
        return t_luminanceConvert( aPixel , 4 );
        
    case PixelFormat_L8     :
        return t_luminanceConvert( aPixel , 8 );
        
    case PixelFormat_LA4    :
        return t_luminanceAlphaConvert( aPixel , 4 );
        
    case PixelFormat_LA8    :
        return t_luminanceAlphaConvert( aPixel , 8 );
        
    case PixelFormat_I4     :
        return t_intensityConvert( aPixel , 4 );
        
    case PixelFormat_I8     :
        return t_intensityConvert( aPixel , 8 );
        
    case PixelFormat_RGB232 :
        return t_rgbConvert( aPixel , 2,3,2 );
        
    case PixelFormat_RGB565 :
        return t_rgbConvert( aPixel , 5,6,5 );
        
    case PixelFormat_RGB8   :
        return t_rgbConvert( aPixel , 8,8,8 );
        
    case PixelFormat_RGB5A1 :
        return t_rgbaConvert( aPixel , 5,5,5,1 );
        
    case PixelFormat_RGBA2  :
        return t_rgbaConvert( aPixel , 2,2,2,2 );
        
    case PixelFormat_RGBA4  :
        return t_rgbaConvert( aPixel , 4,4,4,4 );
        
    case PixelFormat_RGBA6  :
        return t_rgbaConvert( aPixel , 6,6,6,6 );
        
    case PixelFormat_RGBA8  :
        return t_rgbaConvert( aPixel , 8,8,8,8 );
        
    default:
        PJ_ASSERT(0);
        return Pixel();
    }
}

//------------------------------------------------------------
bool PixelFormatUtil::hasAlpha( const PixelFormat aFormat )
{
    switch( aFormat )
    {
    case PixelFormat_A4     :
    case PixelFormat_A8     :
        return true;
        
    case PixelFormat_L4     :
    case PixelFormat_L8     :
        return false;
    case PixelFormat_LA4    :
    case PixelFormat_LA8    :
    
    case PixelFormat_I4     :
    case PixelFormat_I8     :
        return true;
        
    case PixelFormat_RGB232 :
    case PixelFormat_RGB565 :
    case PixelFormat_RGB8   :
        return false;
        
    case PixelFormat_RGB5A1 :
    case PixelFormat_RGBA2  :
    case PixelFormat_RGBA4  :
    case PixelFormat_RGBA6  :
    case PixelFormat_RGBA8  :
        return true;
        
    case PixelFormat_RGBA_S3TC_DXT1 :
    case PixelFormat_RGBA_S3TC_DXT3 :
    case PixelFormat_RGBA_S3TC_DXT5 :
        return true;
        
    default:
        PJ_ASSERT(0);
        return false;
    }
}

//------------------------------------------------------------
bool PixelFormatUtil::isDXTC( const PixelFormat aPixelFormat )
{
    PJ_ASSERT( PixelFormatUtil::isValid( aPixelFormat ) );
    switch( aPixelFormat )
    {
    case PixelFormat_RGBA_S3TC_DXT1 :
    case PixelFormat_RGBA_S3TC_DXT3 :
    case PixelFormat_RGBA_S3TC_DXT5 :
        return true;
        
    default:
        return false;
    }
}

//------------------------------------------------------------
int PixelFormatUtil::dxtcCompressFlag( const PixelFormat aPixelFormat )
{
    PJ_ASSERT( isDXTC( aPixelFormat ) );
    switch( aPixelFormat )
    {
    case PixelFormat_RGBA_S3TC_DXT1:
        return squish::kDxt1;
            
    case PixelFormat_RGBA_S3TC_DXT3:
        return squish::kDxt3;
            
    case PixelFormat_RGBA_S3TC_DXT5:
        return squish::kDxt5;
        
    default:
        PJ_ASSERT(0);
        return 0;
    }
}

//------------------------------------------------------------
void PixelFormatUtil::codeTest()
{
    for ( unsigned char i = 0; i < 128; ++i )
    {
        PJ_ASSERT( t_convert( i , 1 ) == 0 );
    }
    for ( unsigned char i = 128; i < 255; ++i )
    {
        PJ_ASSERT( t_convert( i , 1 ) == 255 );
    }
    
    for ( unsigned char i = 0; i < 64; ++i )
    {
        PJ_ASSERT( t_convert( i , 2 ) == 0 );
    }
    for ( unsigned char i = 64; i < 128; ++i )
    {
        PJ_ASSERT( t_convert( i , 2 ) == 0x55 );
    }
    for ( unsigned char i = 128; i < 192; ++i )
    {
        PJ_ASSERT( t_convert( i , 2 ) == 0xAA );
    }
    for ( unsigned char i = 192; i < 255; ++i )
    {
        PJ_ASSERT( t_convert( i , 2 ) == 0xFF );
    }
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
