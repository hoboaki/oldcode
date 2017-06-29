/**
 * @file
 * @brief ObjectCode�^���L�q����B
 */
#ifndef ACS_INCLUDE_OBJECTCODE
#define ACS_INCLUDE_OBJECTCODE

//------------------------------------------------------------
namespace acscript {

    /**
     * @brief �P�̃I�u�W�F�N�g�R�[�h�B
     * �R���p�C�����ꂽ�o�C�g�R�[�h�A�R���p�C�����ɐ������ꂽ�������B
     */
    class ObjectCode
    {
    private:
        /// �R�[�h�̈ˑ����������^�B
        struct DependCodeInfo
        {
            ByteCodeId ByteCodeId; ///< �Q�Ƃ����I�u�W�F�N�g�R�[�h��ID�B
            Hash hash; ///< �R���p�C�����̃R�[�h�̃n�b�V���l�B
        };

        /// �R���p�C�����ꂽ�o�C�g�R�[�h�B
        ByteCode* mByteCode;

        /// �R���p�C�����̃R�[�h�̃n�b�V���l�B 
        Hash mHash;

        /// �V���{�����e�[�u���B�R�[���X�^�b�N�̕\����A�V���A���C�Y�Ɏg�p����B
        Vector< SymbolName >::BuildTimeType mLabeledSymbolNameTable;

        /// �R�[�h�̈ˑ����B
        Vector< DependCodeInfo >::BuildTimeType mDependCodeInfoTable;
    };

}
//------------------------------------------------------------
#endif
// EOF
