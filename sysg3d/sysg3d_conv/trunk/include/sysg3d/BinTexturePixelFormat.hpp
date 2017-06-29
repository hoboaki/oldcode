/**
 * @file
 * @brief BinTextureBinTexturePixelFormat型を記述する。
 */
#pragma once

//----------------------------------------------------------------
namespace sysg3d {

    /// ピクセルフォーマットの識別子。
    enum BinTexturePixelFormat
    {
        BinTexturePixelFormat_Unknown
        // Alpha(1,1,1,A)
        ,BinTexturePixelFormat_A8    ///< A8bit。8bit/pixel。
        // Luminance(L,L,L,1)
        ,BinTexturePixelFormat_L8    ///< L8bit。8bit/pixel。
        // LuminanceWithAlpha(L,L,L,A)
        ,BinTexturePixelFormat_LA8   ///< LA8bit。16bit/pixel。
        // Intensity(I,I,I,I)
        ,BinTexturePixelFormat_I8    ///< I8bit。8bit/pixel。
        // RGB(R,G,B,1)
        ,BinTexturePixelFormat_RGB332  ///< R3G3B2bit。8bit/pixel。
        ,BinTexturePixelFormat_RGB565  ///< R5G6B5bit。16bit/pixel。
        ,BinTexturePixelFormat_RGB8    ///< RGB8bit。24bit/pixel。
        // RGBA(R,G,B,A)
        ,BinTexturePixelFormat_RGB5A1  ///< RGB5A1bit。16bit/pixel。
        ,BinTexturePixelFormat_RGBA4   ///< RGBA4bit。16bit/pixel。
        ,BinTexturePixelFormat_RGBA8   ///< RGBA8bit。32bit/pixel。
        // DXTC(S3TC)
        ,BinTexturePixelFormat_RGBA_S3TC_DXT1 ///< DXTC,DXT1フォーマット。4bit/pixel。
        ,BinTexturePixelFormat_RGBA_S3TC_DXT3 ///< DXTC,DXT3フォーマット。8bit/pixel。
        ,BinTexturePixelFormat_RGBA_S3TC_DXT5 ///< DXTC,DXT5フォーマット。8bit/pixel。
        // Other
        // ...
        ,BinTexturePixelFormat_Terminate
    };

}
//----------------------------------------------------------------
// EOF
