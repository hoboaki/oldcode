/**
 * @file
 * @brief 2D�̃T�C�Y�������\���̂��L�q����B
 */
#if defined(GLPIC_INCLUDED_SIZE2D_HPP)
#else
#define GLPIC_INCLUDED_SIZE2D_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /// 2D�̃T�C�Y�������\���́B
    struct Size2D
    {
        unsigned short width;  ///< �����B
        unsigned short height; ///< �c���B

        //----------------------------------------------------------------
        /// �쐬�֐��B
        static Size2D create( unsigned short , unsigned short height );
    };

}
//----------------------------------------------------------------
#endif 
// EOF
