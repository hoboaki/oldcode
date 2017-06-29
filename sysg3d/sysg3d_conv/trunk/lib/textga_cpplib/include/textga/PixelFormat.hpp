/**
 * @file
 * @brief �s�N�Z���t�H�[�}�b�g���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// �s�N�Z���t�H�[�}�b�g�̎��ʎq�B
    enum PixelFormat
    {
        PixelFormat_Unknown
        // Alpha(1,1,1,A)
        ,PixelFormat_A1    ///< A1bit�B1bit/pixel�B
        ,PixelFormat_A2    ///< A2bit�B2bit/pixel�B
        ,PixelFormat_A4    ///< A4bit�B4bit/pixel�B
        ,PixelFormat_A8    ///< A8bit�B8bit/pixel�B
        // Luminance(L,L,L,1)
        ,PixelFormat_L1    ///< L1bit�B1bit/pixel�B
        ,PixelFormat_L2    ///< L2bit�B2bit/pixel�B
        ,PixelFormat_L4    ///< L4bit�B4bit/pixel�B
        ,PixelFormat_L8    ///< L8bit�B8bit/pixel�B
        // LuminanceWithAlpha(L,L,L,A)
        ,PixelFormat_LA1   ///< LA1bit�B2bit/pixel�B
        ,PixelFormat_LA2   ///< LA2bit�B4bit/pixel�B
        ,PixelFormat_LA4   ///< LA4bit�B8bit/pixel�B
        ,PixelFormat_LA8   ///< LA8bit�B16bit/pixel�B
        // Intensity(I,I,I,I)
        ,PixelFormat_I1    ///< I1bit�B1bit/pixel�B
        ,PixelFormat_I2    ///< I2bit�B2bit/pixel�B
        ,PixelFormat_I4    ///< I4bit�B4bit/pixel�B
        ,PixelFormat_I8    ///< I8bit�B8bit/pixel�B
        // RGB(R,G,B,1)
        ,PixelFormat_RGB232  ///< R2G3B2bit�B8bit/pixel�B
        ,PixelFormat_RGB332  ///< R3G3B2bit�B8bit/pixel�B
        ,PixelFormat_RGB565  ///< R5G6B5bit�B16bit/pixel�B
        ,PixelFormat_RGB8    ///< RGB8bit�B24bit/pixel�B
        // RGBA(R,G,B,A)
        ,PixelFormat_RGB5A1  ///< RGB5A1bit�B16bit/pixel�B
        ,PixelFormat_RGBA1   ///< RGBA1bit�B4bit/pixel�B
        ,PixelFormat_RGBA2   ///< RGBA2bit�B8bit/pixel�B
        ,PixelFormat_RGBA4   ///< RGBA4bit�B16bit/pixel�B
        ,PixelFormat_RGBA6   ///< RGBA8bit�B24bit/pixel�B
        ,PixelFormat_RGBA8   ///< RGBA8bit�B32bit/pixel�B
        
        // DXTC(S3TC)
        ,PixelFormat_RGBA_S3TC_DXT1 ///< DXTC,DXT1�t�H�[�}�b�g�B4bit/pixel�B
        ,PixelFormat_RGBA_S3TC_DXT3 ///< DXTC,DXT3�t�H�[�}�b�g�B8bit/pixel�B
        ,PixelFormat_RGBA_S3TC_DXT5 ///< DXTC,DXT5�t�H�[�}�b�g�B8bit/pixel�B
        // Other
        // ...
        
        ,PixelFormat_Terminate
        ,PixelFormat_Begin = PixelFormat_Unknown+1
        ,PixelFormat_End = PixelFormat_Terminate
    };
    
}
//------------------------------------------------------------
// EOF
