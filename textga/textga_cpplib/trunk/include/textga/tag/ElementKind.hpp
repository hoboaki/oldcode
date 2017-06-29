/**
 * @file
 * @brief �v�f�̎�ނ������񋓌^���L�q����B�@
 */
#pragma once

//------------------------------------------------------------
namespace textga { 
namespace tag {

    /// �v�f�������񋓌^�B
    enum ElementKind
    {
        ElementKind_Unknown  ///< �s���B
        ,ElementKind_Section ///< �Z�N�V�����B
        ,ElementKind_NumU8   ///< U8�B
        ,ElementKind_NumU16  ///< U16�B
        ,ElementKind_NumU32  ///< U32�B
        ,ElementKind_NumS8   ///< S8�B
        ,ElementKind_NumS16  ///< S16�B
        ,ElementKind_NumS32  ///< S32�B
        ,ElementKind_String  ///< ������B
        ,ElementKind_Binary  ///< �o�C�i���f�[�^�B
    };

}}
//------------------------------------------------------------
// EOF
