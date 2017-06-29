/**
 * @file
 * @brief TGAのヘッダ。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// TGAのヘッダ。
    struct TGAHeader
    {   
        char  idLength;
        char  colorMapType;
        char  dataTypeCode;
        //short colorMapOrigin;
        //short colorMapLength;
        //char  colorMapDepth;
        // パディング対策。
        char  colorMapElement1;
        int   colorMapElement2;
        // ここまで
        short x_origin;
        short y_origin;
        short width;
        short height;
        char  bitsPerPixel;
        char  imageDescriptor;
        
        static const size_t SIZE = 18; ///< sizeofの代わりに使って。
    };

}
//------------------------------------------------------------
// EOF
