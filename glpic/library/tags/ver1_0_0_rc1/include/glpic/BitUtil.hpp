/**
 * @file
 * @brief �r�b�g�v�Z�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#if defined(GLPIC_INCLUDED_BITUTIL_HPP)
#else
#define GLPIC_INCLUDED_BITUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// �r�b�g�v�Z�Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class BitUtil : public ::glpic::Noncreatable
    {
    public:
        /// �r�b�g�����݂��Ȃ����Ƃ������l�B
        static const signed char NO_BIT;

        /**
        * @brief �w�肵���l��1�ł���ő�̃r�b�g�ʒu���擾����B
        * @return �w�肵���l��0�̂Ƃ�GLPicBitUtil_NoBit��Ԃ��B
        * ��F0b01001000 = 6
        */
        static signed char getMaxBitU16( unsigned short ); 

        /**
        * @brief �w�肵���l��1�ł���ŏ��̃r�b�g�ʒu���擾����B
        * @return �w�肵���l��0�̂Ƃ�GLPicBitUtil_NoBit��Ԃ��B
        * ��F0b01110100 = 2
        */
        static signed char getMinBitU16( unsigned short );

        /**
        * @brief 2�̗v�f��4:4�Ńp�b�N����B
        * @param component1 ���4�r�b�g������l�B
        * @param component2 ���4�r�b�g������l�B
        */
        static unsigned char pack4b4bTo8b(
            unsigned char component1
            , unsigned char component2
            );

        /**
        * @brief 2�̗v�f��6:2�Ńp�b�N����B
        * @param component1 ���6�r�b�g������l�B
        * @param component2 ���2�r�b�g������l�B
        */
        static unsigned char pack6b2bTo8b(
            unsigned char component1
            , unsigned char component2
            );

        /**
        * @brief �R�̗v�f��3:3:2�Ńp�b�N����B
        * @param component1 ���3�r�b�g������l�B
        * @param component2 ���3�r�b�g������l�B
        * @param component3 ���2�r�b�g������l�B
        */
        static unsigned char pack3b3b2bTo8b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            );

        /**
        * @brief �R�̗v�f��5:5:5(N1)�Ńp�b�N����B
        * @param component1 ���5�r�b�g������l�B
        * @param component2 ���5�r�b�g������l�B
        * @param component3 ���5�r�b�g������l�B
        */
        static unsigned short pack5b5b5bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            );

        /**
        * @brief �R�̗v�f��10:10:10(N2)�Ńp�b�N����B
        * @param component1 ���10�r�b�g������l�B
        * @param component2 ���10�r�b�g������l�B
        * @param component3 ���10�r�b�g������l�B
        */
        static unsigned long pack10b10b10bTo32b(
            unsigned short component1
            , unsigned short component2
            , unsigned short component3
            );

        /**
        * @brief �S�̗v�f��2:2:2:2�Ńp�b�N����B
        * @param component1 ���2�r�b�g������l�B
        * @param component2 ���2�r�b�g������l�B
        * @param component3 ���2�r�b�g������l�B
        * @param component4 ���2�r�b�g������l�B
        */
        static unsigned char pack2b2b2b2bTo8b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief �S�̗v�f��4:4:4:4�Ńp�b�N����B
        * @param component1 ���4�r�b�g������l�B
        * @param component2 ���4�r�b�g������l�B
        * @param component3 ���4�r�b�g������l�B
        * @param component4 ���4�r�b�g������l�B
        */
        static unsigned short pack4b4b4b4bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief �S�̗v�f��5:5:5:1�Ńp�b�N����B
        * @param component1 ���5�r�b�g������l�B
        * @param component2 ���5�r�b�g������l�B
        * @param component3 ���5�r�b�g������l�B
        * @param component4 ���1�r�b�g������l�B
        */
        static unsigned short pack5b5b5b1bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief �S�̗v�f��10:10:10:2�Ńp�b�N����B
        * @param component1 ���10�r�b�g������l�B
        * @param component2 ���10�r�b�g������l�B
        * @param component3 ���10�r�b�g������l�B
        * @param component4 ���2�r�b�g������l�B
        */
        static unsigned long pack10b10b10b2bTo32b(
            unsigned short component1
            , unsigned short component2
            , unsigned short component3
            , unsigned char component4
            );

        /// BitUtil�̃��j�b�g�e�X�g�B
        static void unitTest();
    };

}
//----------------------------------------------------------------
#endif 
//EOF
