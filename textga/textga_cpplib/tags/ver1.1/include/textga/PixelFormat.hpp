/**
 * @file
 * @brief ピクセルフォーマットを記述する。
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// ピクセルフォーマットの識別子。
    enum PixelFormat
    {
        PixelFormat_Unknown
        // Alpha(1,1,1,A)
        ,PixelFormat_A1    ///< A1bit。1bit/pixel。
        ,PixelFormat_A2    ///< A2bit。2bit/pixel。
        ,PixelFormat_A4    ///< A4bit。4bit/pixel。
        ,PixelFormat_A8    ///< A8bit。8bit/pixel。
        // Luminance(L,L,L,1)
        ,PixelFormat_L1    ///< L1bit。1bit/pixel。
        ,PixelFormat_L2    ///< L2bit。2bit/pixel。
        ,PixelFormat_L4    ///< L4bit。4bit/pixel。
        ,PixelFormat_L8    ///< L8bit。8bit/pixel。
        // LuminanceWithAlpha(L,L,L,A)
        ,PixelFormat_LA1   ///< LA1bit。2bit/pixel。
        ,PixelFormat_LA2   ///< LA2bit。4bit/pixel。
        ,PixelFormat_LA4   ///< LA4bit。8bit/pixel。
        ,PixelFormat_LA8   ///< LA8bit。16bit/pixel。
        // Intensity(I,I,I,I)
        ,PixelFormat_I1    ///< I1bit。1bit/pixel。
        ,PixelFormat_I2    ///< I2bit。2bit/pixel。
        ,PixelFormat_I4    ///< I4bit。4bit/pixel。
        ,PixelFormat_I8    ///< I8bit。8bit/pixel。
        // RGB(R,G,B,1)
        ,PixelFormat_RGB232  ///< R2G3B2bit。8bit/pixel。
        ,PixelFormat_RGB332  ///< R3G3B2bit。8bit/pixel。
        ,PixelFormat_RGB565  ///< R5G6B5bit。16bit/pixel。
        ,PixelFormat_RGB8    ///< RGB8bit。24bit/pixel。
        // RGBA(R,G,B,A)
        ,PixelFormat_RGB5A1  ///< RGB5A1bit。16bit/pixel。
        ,PixelFormat_RGBA1   ///< RGBA1bit。4bit/pixel。
        ,PixelFormat_RGBA2   ///< RGBA2bit。8bit/pixel。
        ,PixelFormat_RGBA4   ///< RGBA4bit。16bit/pixel。
        ,PixelFormat_RGBA6   ///< RGBA8bit。24bit/pixel。
        ,PixelFormat_RGBA8   ///< RGBA8bit。32bit/pixel。
        
        // DXTC(S3TC)
        ,PixelFormat_RGBA_S3TC_DXT1 ///< DXTC,DXT1フォーマット。4bit/pixel。
        ,PixelFormat_RGBA_S3TC_DXT3 ///< DXTC,DXT3フォーマット。8bit/pixel。
        ,PixelFormat_RGBA_S3TC_DXT5 ///< DXTC,DXT5フォーマット。8bit/pixel。
        // Other
        // ...
        
        ,PixelFormat_Terminate
        ,PixelFormat_Begin = PixelFormat_Unknown+1
        ,PixelFormat_End = PixelFormat_Terminate
    };
    
}
//------------------------------------------------------------
// EOF
