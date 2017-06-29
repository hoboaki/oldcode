/**
 * @file
 * @brief TGA�̃w�b�_�B
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// TGA�̃w�b�_�B
    struct TGAHeader
    {   
        u8  idLength;
        u8  colorMapType;
        u8  dataTypeCode;
        //short colorMapOrigin;
        //short colorMapLength;
        //char  colorMapDepth;
        // �p�f�B���O�΍�B
        u8  colorMapElement1;
        u32 colorMapElement2;
        // �����܂�
        u16 x_origin;
        u16 y_origin;
        u16 width;
        u16 height;
        u8  bitsPerPixel;
        u8  imageDescriptor;
        
        static const size_t SIZE = 18; ///< sizeof�̑���Ɏg���āB
    };

}
//------------------------------------------------------------
// EOF
