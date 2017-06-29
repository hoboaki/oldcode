/**
 * @file
 * @brief �^�O�̎�ނ������񋓌^���L�q����B�@
 */
#pragma once

//------------------------------------------------------------
namespace textga { 
namespace tag {

    /// �^�O�������񋓌^�B
    enum TagKind
    {
        TagKind_Unknown  ///< �s���B
        ,TagKind_SectionBegin ///< �Z�N�V�����J�n�B
        ,TagKind_SectionEnd   ///< �Z�N�V�����I���B
        ,TagKind_NumU8      ///< U8�B
        ,TagKind_NumU16     ///< U16�B
        ,TagKind_NumU32     ///< U32�B
        ,TagKind_NumS8      ///< S8�B
        ,TagKind_NumS16     ///< S16�B
        ,TagKind_NumS32     ///< S32�B
        ,TagKind_String  ///< ������B
        ,TagKind_Binary  ///< �o�C�i���f�[�^�B
    };

}}
//------------------------------------------------------------
// EOF
