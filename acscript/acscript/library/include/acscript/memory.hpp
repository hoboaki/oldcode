/**
 * @file
 * @brief Memory�^���L�q����B
 */
#ifndef ACS_INCLUDE_MEMORY
#define ACS_INCLUDE_MEMORY

//------------------------------------------------------------
#include <acscript/allocator.hpp>

//------------------------------------------------------------
namespace acscript {

    class Memory
    {
    public:
        /**
         * @brief �v���O�����\�z���p�A���P�[�^�B
         * �I�u�W�F�N�g�R�[�h�A�Ǘ��p�R�[�h�̃������͂�������Ƃ���B
         */
        static Allocator& buildTimeAllocator();
        
        /**
         * @brief �X�^�b�N�p�A���P�[�^�B
         * �R���e�L�X�g�̃X�^�b�N�������͂�������Ƃ���B
         */
        static Allocator& contextStackAllocator();

        /**
         * @brief ���s���p�A���P�[�^�B
         * �R���e�L�X�g�̎��s���Ɋm�ۂ����I�u�W�F�N�g�̃������͂�������Ƃ���B
         */
        static Allocator& runTimeAllocator();
    };

}
//------------------------------------------------------------
#endif
// EOF
