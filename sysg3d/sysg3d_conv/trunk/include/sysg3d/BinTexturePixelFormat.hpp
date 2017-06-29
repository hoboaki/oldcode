/**
 * @file
 * @brief BinTextureBinTexturePixelFormat�^���L�q����B
 */
#pragma once

//----------------------------------------------------------------
namespace sysg3d {

    /// �s�N�Z���t�H�[�}�b�g�̎��ʎq�B
    enum BinTexturePixelFormat
    {
        BinTexturePixelFormat_Unknown
        // Alpha(1,1,1,A)
        ,BinTexturePixelFormat_A8    ///< A8bit�B8bit/pixel�B
        // Luminance(L,L,L,1)
        ,BinTexturePixelFormat_L8    ///< L8bit�B8bit/pixel�B
        // LuminanceWithAlpha(L,L,L,A)
        ,BinTexturePixelFormat_LA8   ///< LA8bit�B16bit/pixel�B
        // Intensity(I,I,I,I)
        ,BinTexturePixelFormat_I8    ///< I8bit�B8bit/pixel�B
        // RGB(R,G,B,1)
        ,BinTexturePixelFormat_RGB332  ///< R3G3B2bit�B8bit/pixel�B
        ,BinTexturePixelFormat_RGB565  ///< R5G6B5bit�B16bit/pixel�B
        ,BinTexturePixelFormat_RGB8    ///< RGB8bit�B24bit/pixel�B
        // RGBA(R,G,B,A)
        ,BinTexturePixelFormat_RGB5A1  ///< RGB5A1bit�B16bit/pixel�B
        ,BinTexturePixelFormat_RGBA4   ///< RGBA4bit�B16bit/pixel�B
        ,BinTexturePixelFormat_RGBA8   ///< RGBA8bit�B32bit/pixel�B
        // DXTC(S3TC)
        ,BinTexturePixelFormat_RGBA_S3TC_DXT1 ///< DXTC,DXT1�t�H�[�}�b�g�B4bit/pixel�B
        ,BinTexturePixelFormat_RGBA_S3TC_DXT3 ///< DXTC,DXT3�t�H�[�}�b�g�B8bit/pixel�B
        ,BinTexturePixelFormat_RGBA_S3TC_DXT5 ///< DXTC,DXT5�t�H�[�}�b�g�B8bit/pixel�B
        // Other
        // ...
        ,BinTexturePixelFormat_Terminate
    };

}
//----------------------------------------------------------------
// EOF
