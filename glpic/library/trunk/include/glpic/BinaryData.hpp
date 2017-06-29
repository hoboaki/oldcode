/**
 * @file
 * @brief �o�C�i���f�[�^�������\���̂��L�q����B
 */
#if defined(GLPIC_INCLUDED_BINARYDATA_HPP)
#else
#define GLPIC_INCLUDED_BINARYDATA_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /// �o�C�i���f�[�^�������\���́B
    struct BinaryData
    {
        void*         address;  ///< �f�[�^�̐擪�A�h���X�B
        unsigned long size;     ///< �f�[�^���B

        //----------------------------------------------------------------
        /// �쐬�֐��B
        static BinaryData create( void* address , unsigned long size );
    };

}
//----------------------------------------------------------------
#endif
// EOF
