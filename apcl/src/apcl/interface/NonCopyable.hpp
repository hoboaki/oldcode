/**
 * @file
 * @brief �R�s�[�ł��Ȃ��C���^�[�t�F�[�X�B
 */
#pragma once

//-----------------------------------------------------------
namespace apcl { namespace interface
{
  /// �R�s�[�ł��Ȃ�����C���^�[�t�F�[�X�Bprivate�Ōp������B
  class NonCopyable
  {
  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    NonCopyable();
    virtual ~NonCopyable();
    //@}

  private:
    //=================================================
    /// @name �������Ȃ��֐�
    //@{
    NonCopyable( const NonCopyable& aObject ); ///< �R�s�[�R���X�g���N�^
    const NonCopyable& operator = ( const NonCopyable& aObject ); ///< ������Z�q
    //@}
  };

}} // end of namespace ::apcl::interface

//-----------------------------------------------------------
// EOF
