/**
 * @file
 * @brief TGA�̃w�b�_�B
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// TGA�̃w�b�_�B
    struct TGAHeader
    {   
        char  idLength;
        char  colorMapType;
        char  dataTypeCode;
        //short colorMapOrigin;
        //short colorMapLength;
        //char  colorMapDepth;
        // �p�f�B���O�΍�B
        char  colorMapElement1;
        int   colorMapElement2;
        // �����܂�
        short x_origin;
        short y_origin;
        short width;
        short height;
        char  bitsPerPixel;
        char  imageDescriptor;
        
        static const size_t SIZE = 18; ///< sizeof�̑���Ɏg���āB
    };

}
//------------------------------------------------------------
// EOF
