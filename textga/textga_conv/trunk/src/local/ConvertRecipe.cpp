/**
 * @file
 * @brief ConvertRecipe.hppの実装を記述する。
 */
#include "ConvertRecipe.hpp"

//------------------------------------------------------------
#include <cstdio>
#include "ArgumentData.hpp"
#include "ArgumentDataIterator.hpp"
#include "PixelFormatUtil.hpp"
#include "StringUtil.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool ConvertRecipe::initialize(
    const ArgumentData& aData
    )
{
    ArgumentDataIterator itr( aData );
    
    itr.next();
    if ( itr.isEnd() )
    {// 引数は最低でも２いる。
        return false;
    }    
    
    // 初期化
    ConvertRecipe& aRecipe = *this;
    aRecipe.appendSrcData = true;
    aRecipe.isPixelFormatValid = false;
    aRecipe.outputFilePath = "";
    
    // 最初のオプションはインプットファイルパス
    aRecipe.inputFilePath = itr.get();
    itr.next();
    
    // オプション解析
    while( !itr.isEnd() )
    {
        
        if ( StringUtil::equals( itr.get() , "-f" ) )
        {// PixelFormat
            if ( aRecipe.isPixelFormatValid )
            {// 既に指定されている
                std::fprintf( stderr, "Error: Found Two Pixel Format Setting.\n" );
                return false;
            }
            itr.next();
            if ( itr.isEnd() )
            {// ピクセルフォーマットが指定されていない
                std::fprintf( stderr, "Error: Not Found Pixel Format after '-f'.\n" );
                return false;
            }
            for ( int i = PixelFormat_Begin; i < PixelFormat_End; ++i )
            {// ピクセルフォーマットの選択。
                const PixelFormat fmt = static_cast< PixelFormat >(i);
                if ( StringUtil::caseEquals( itr.get() ,  PixelFormatUtil::toName( fmt ) ) )
                {
                    aRecipe.pixelFormat = fmt;
                    aRecipe.isPixelFormatValid = true;
                    break;
                }
            }
            if ( !aRecipe.isPixelFormatValid )
            {// 有効なオプションではない
                std::fprintf( stderr, "Error: Invalid Pixel Format '%s'.\n" , itr.get() );
                return false;
            }
        }
        else if ( StringUtil::equals( itr.get() , "-o" ) )
        {// 出力パス
            if ( aRecipe.outputFilePath.length() != 0 )
            {// 既に設定されている。
                std::fprintf( stderr, "Error: Found Two Output File Path Setting.\n" );
                return false;
            }
            itr.next();
            if ( itr.isEnd() )
            {// 出力パスが指定されていない
                std::fprintf( stderr, "Error: Not Found Output File Path after '-o'.\n" );
                return false;
            }
            aRecipe.outputFilePath = itr.get();
        }
        else if ( StringUtil::equals( itr.get() , "-rs" ) )
        {
            aRecipe.appendSrcData = false;
        }
        else
        {
            std::fprintf( stderr, "Error: Unknown Option '%s'.\n" , itr.get() );
            return false;
        }
        
        itr.next();
    }
    
    // エラーなく終了
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
