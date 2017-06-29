/**
 * @file
 * @brief TGAのヘッダ。
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// TGAのヘッダ。
    struct TGAHeader
    {   
        u8  idLength;
        u8  colorMapType;
        u8  dataTypeCode;
        //short colorMapOrigin;
        //short colorMapLength;
        //char  colorMapDepth;
        // パディング対策。
        u8  colorMapElement1;
        u32 colorMapElement2;
        // ここまで
        u16 x_origin;
        u16 y_origin;
        u16 width;
        u16 height;
        u8  bitsPerPixel;
        u8  imageDescriptor;
        
        static const size_t SIZE = 18; ///< sizeofの代わりに使って。
    };

}
//------------------------------------------------------------
// EOF
