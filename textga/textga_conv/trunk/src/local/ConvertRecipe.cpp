/**
 * @file
 * @brief ConvertRecipe.hpp�̎������L�q����B
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
    {// �����͍Œ�ł��Q����B
        return false;
    }    
    
    // ������
    ConvertRecipe& aRecipe = *this;
    aRecipe.appendSrcData = true;
    aRecipe.isPixelFormatValid = false;
    aRecipe.outputFilePath = "";
    
    // �ŏ��̃I�v�V�����̓C���v�b�g�t�@�C���p�X
    aRecipe.inputFilePath = itr.get();
    itr.next();
    
    // �I�v�V�������
    while( !itr.isEnd() )
    {
        
        if ( StringUtil::equals( itr.get() , "-f" ) )
        {// PixelFormat
            if ( aRecipe.isPixelFormatValid )
            {// ���Ɏw�肳��Ă���
                std::fprintf( stderr, "Error: Found Two Pixel Format Setting.\n" );
                return false;
            }
            itr.next();
            if ( itr.isEnd() )
            {// �s�N�Z���t�H�[�}�b�g���w�肳��Ă��Ȃ�
                std::fprintf( stderr, "Error: Not Found Pixel Format after '-f'.\n" );
                return false;
            }
            for ( int i = PixelFormat_Begin; i < PixelFormat_End; ++i )
            {// �s�N�Z���t�H�[�}�b�g�̑I���B
                const PixelFormat fmt = static_cast< PixelFormat >(i);
                if ( StringUtil::caseEquals( itr.get() ,  PixelFormatUtil::toName( fmt ) ) )
                {
                    aRecipe.pixelFormat = fmt;
                    aRecipe.isPixelFormatValid = true;
                    break;
                }
            }
            if ( !aRecipe.isPixelFormatValid )
            {// �L���ȃI�v�V�����ł͂Ȃ�
                std::fprintf( stderr, "Error: Invalid Pixel Format '%s'.\n" , itr.get() );
                return false;
            }
        }
        else if ( StringUtil::equals( itr.get() , "-o" ) )
        {// �o�̓p�X
            if ( aRecipe.outputFilePath.length() != 0 )
            {// ���ɐݒ肳��Ă���B
                std::fprintf( stderr, "Error: Found Two Output File Path Setting.\n" );
                return false;
            }
            itr.next();
            if ( itr.isEnd() )
            {// �o�̓p�X���w�肳��Ă��Ȃ�
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
    
    // �G���[�Ȃ��I��
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
