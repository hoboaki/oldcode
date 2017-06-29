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
        ::textga::u8  idLength;
        ::textga::u8  colorMapType;
        ::textga::u8  dataTypeCode;
        //short colorMapOrigin;
        //short colorMapLength;
        //char  colorMapDepth;
        // �p�f�B���O�΍�B
        ::textga::u8  colorMapElement1;
        ::textga::u32 colorMapElement2;
        // �����܂�
        ::textga::u16 x_origin;
        ::textga::u16 y_origin;
        ::textga::u16 width;
        ::textga::u16 height;
        ::textga::u8  bitsPerPixel;
        ::textga::u8  imageDescriptor;
        
        static const size_t SIZE = 18; ///< sizeof�̑���Ɏg���āB
    };

}
//------------------------------------------------------------
// EOF
