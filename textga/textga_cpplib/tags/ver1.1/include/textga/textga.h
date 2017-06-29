/**
 * @file
 * @brief textgaライブラリの全ヘッダをインクルードする。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Assert.hpp>
#include <textga/Constant.hpp>
#include <textga/Endian.hpp>
#include <textga/EndianUtil.hpp>
#include <textga/File.hpp>
#include <textga/FileLoader.hpp>
#include <textga/Pixel.hpp>
#include <textga/PixelFormat.hpp>
#include <textga/PixelFormatUtil.hpp>
#include <textga/StringUtil.hpp>
#include <textga/TGADataAccessor.hpp>
#include <textga/TGAFileLoader.hpp>
#include <textga/TGAHeader.hpp>
#include <textga/Types.hpp>
#include <textga/Version.hpp>

//------------------------------------------------------------
// Sample Code
/*
#include <textga/textga.h>

void main()
{
    // Load TGAFile
    ::textga::TGAFileLoader loader( "tgaFilePath.tga" );
    if ( !loader.isLoaded() )
    {
        // Load Failed
        return;
    }
    
    // Standard TGA Check
    const ::textga::TGADataAccessor tgaDataAccessor = loader.tgaDataAccessor();
    if ( !tgaDataAccessor.isSupportedTGA() )
    {
        // Not supported tga file
        return;
    }
    
    // Standard TGA Data Access
    // tgaDataAccessor.width();  // get width
    // tgaDataAccessor.height(); // get height
    // tgaDataAccessor.pixelAtIndex(0); // get first pixel data
    // and more. see TGADataAccessor.hpp
    
    // TexTarga Check
    if ( !tgaDataAccessor.isTexTarga() )
    {
        // Not tex targa
        return;
    }
    
    // TexTarga Data Access
    // tgaDataAccessor.pixelFormat() // get PixelFormat(see PixelFormat.hpp)
    // tgaDataAccessor.srcDataPixelAtIndex(0) // get before convert pixel data
    // and more. see TGADataAccessor.hpp

}

*/
//------------------------------------------------------------
// EOF
