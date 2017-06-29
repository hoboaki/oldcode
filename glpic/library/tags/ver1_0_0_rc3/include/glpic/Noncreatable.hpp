/**
 * @file
 * @brief �C���X�^���X���s�ȃC���^�[�t�F�[�X�N���X���L�q����B
 */
#if defined(GLPIC_INCLUDED_NONCREATABLE_HPP)
#else
#define GLPIC_INCLUDED_NONCREATABLE_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /**
     * @brief �C���X�^���X���s�ȃC���^�[�t�F�[�X�N���X�B
     *
     * ���̃N���X���p�����Ă���N���X�̓R���X�g���N�g�ł��Ȃ��B
     * static�Ȋ֐��݂̂�񋟂���N���X���p������B
     */
    class Noncreatable
    {
    private:
        virtual void privateAbstractFunction()=0;
    };

}
//----------------------------------------------------------------
#endif
// EOF
