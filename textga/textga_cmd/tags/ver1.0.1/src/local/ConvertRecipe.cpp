/**
 * @file
 * @brief ConvertRecipe.hppの実装を記述する。
 */
#include "ConvertRecipe.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <textga/PixelFormatUtil.hpp>
#include <textga/StringUtil.hpp>
#include "ArgumentData.hpp"
#include "ArgumentDataIterator.hpp"
#include "ErrorReport.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool ConvertRecipe::initialize(
    const ArgumentDataIterator& aDataItr
    )
{
    ArgumentDataIterator itr( aDataItr );
    
    if ( !itr.hasNext() )
    {// 引数は最低でも2ついる。
        ErrorReport::argumentNotEnoughError();
        return false;
    }    
    
    // 初期化
    ConvertRecipe& aRecipe = *this;
    aRecipe.appendSrcData = true;
    aRecipe.forceMode = false;
    aRecipe.isPixelFormatValid = false;
    aRecipe.outputFilePath = "";
    
    // 最初のオプションはインプットファイルパス
    aRecipe.inputFilePath = itr.next();
    if ( !itr.hasNext() )
    {// 引数は最低でも2ついる。
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    aRecipe.outputFilePath = itr.next();
    
    // オプション解析
    while( itr.hasNext() )
    {
        const char* option = itr.next();
        if ( StringUtil::equals( option , "-p" ) )
        {// PixelFormat
            if ( aRecipe.isPixelFormatValid )
            {// 既に指定されている
                std::fprintf( stderr, "Error: Found two pixel-format setting.\n" );
                return false;
            }
            if ( !itr.hasNext() )
            {// ピクセルフォーマットが指定されていない
                std::fprintf( stderr, "Error: Not found pixel-format after '-f'.\n" );
                return false;
            }
            const char* pixFmt = itr.next();
            for ( int i = PixelFormat_Begin; i < PixelFormat_End; ++i )
            {// ピクセルフォーマットの選択。
                const PixelFormat fmt = static_cast< PixelFormat >(i);
                if ( StringUtil::caseEquals( pixFmt ,  PixelFormatUtil::toName( fmt ) ) )
                {
                    aRecipe.pixelFormat = fmt;
                    aRecipe.isPixelFormatValid = true;
                    break;
                }
            }
            if ( !aRecipe.isPixelFormatValid )
            {// 有効なオプションではない
                std::fprintf( stderr, "Error: Invalid pixel-format name'%s'.\n" , pixFmt );
                return false;
            }
        }
        else if ( StringUtil::equals( option, "-rs" ) )
        {
            aRecipe.appendSrcData = false;
        }
        else if ( StringUtil::equals( option, "-f" ) )
        {
            aRecipe.forceMode = false;
        }
        else
        {
            ErrorReport::argumentUnknownOptionError( option );
            return false;
        }
    }
    
    // エラーなく終了
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
