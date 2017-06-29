/**
 * @file
 * @brief ConvertRecipe.hpp�̎������L�q����B
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
    {// �����͍Œ�ł�2����B
        ErrorReport::argumentNotEnoughError();
        return false;
    }    
    
    // ������
    ConvertRecipe& aRecipe = *this;
    aRecipe.appendSrcData = true;
    aRecipe.forceMode = false;
    aRecipe.isPixelFormatValid = false;
    aRecipe.outputFilePath = "";
    
    // �ŏ��̃I�v�V�����̓C���v�b�g�t�@�C���p�X
    aRecipe.inputFilePath = itr.next();
    if ( !itr.hasNext() )
    {// �����͍Œ�ł�2����B
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    aRecipe.outputFilePath = itr.next();
    
    // �I�v�V�������
    while( itr.hasNext() )
    {
        const char* option = itr.next();
        if ( StringUtil::equals( option , "-p" ) )
        {// PixelFormat
            if ( aRecipe.isPixelFormatValid )
            {// ���Ɏw�肳��Ă���
                std::fprintf( stderr, "Error: Found two pixel-format setting.\n" );
                return false;
            }
            if ( !itr.hasNext() )
            {// �s�N�Z���t�H�[�}�b�g���w�肳��Ă��Ȃ�
                std::fprintf( stderr, "Error: Not found pixel-format after '-f'.\n" );
                return false;
            }
            const char* pixFmt = itr.next();
            for ( int i = PixelFormat_Begin; i < PixelFormat_End; ++i )
            {// �s�N�Z���t�H�[�}�b�g�̑I���B
                const PixelFormat fmt = static_cast< PixelFormat >(i);
                if ( StringUtil::caseEquals( pixFmt ,  PixelFormatUtil::toName( fmt ) ) )
                {
                    aRecipe.pixelFormat = fmt;
                    aRecipe.isPixelFormatValid = true;
                    break;
                }
            }
            if ( !aRecipe.isPixelFormatValid )
            {// �L���ȃI�v�V�����ł͂Ȃ�
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
    
    // �G���[�Ȃ��I��
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
