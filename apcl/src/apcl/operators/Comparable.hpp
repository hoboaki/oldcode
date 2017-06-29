/** 
 * @file 
 * @brief ��r���艉�Z�q�C���^�[�t�F�[�X���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief ��r���艉�Z�q�C���^�[�t�F�[�X�B
   * <pre>
   * �����Ɠ����^or�ʂ̌^�ɑ΂��Ă̔�r���肷�鉉�Z�q�C���^�[�t�F�[�X��񋟂���B
   * public�Ōp������B
   * OWNER�͌p�������N���X���g�CTARGET�͔���Ώۂ̌^�ł���K�v������B
   * OWNER��TARGET�������^�ł���ꍇ�CTARGET���w�肷��K�v�͂Ȃ��B
   * int compare( const TARGET& )const���p����Ŏ������邱�ƁB
   * compare()�̖߂�l�� 
   *  OWNER < TARGET : -1�ȉ�
   *  OWNER > TARGET : 1�ȏ�
   *  OWNER <= TARGET : 0�ȉ�
   *  OWNER >= TARGET : 0�ȏ�
   * �𖞂����Ă�K�v������B
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Comparable
  {
  public:  
    //=================================================
    /// @name ��r���艉�Z�q�B
    //@{
    bool operator <( const TARGET& aVal )const
    {
      return owner().compare( aVal ) <= -1;
    }
    bool operator >( const TARGET& aVal )const
    {      
      return owner().compare( aVal ) >= 1;
    }
    bool operator <=( const TARGET& aVal )const
    {
      return owner().compare( aVal ) <= 0;
    }
    bool operator >=( const TARGET& aVal )const
    {      
      return owner().compare( aVal ) >= 0;
    }
    //@}

  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    Comparable()
    {
    }
    virtual ~Comparable()
    {
    }
    //@}

  private:
    /// �I�[�i�[�擾
    const OWNER& owner()const
    {
      const OWNER* ptr = dynamic_cast< const OWNER* >( this );
      apclAssert( ptr );
      return *ptr;
    }

  };

}} // end of namespace ::apcl::operators

//-----------------------------------------------------------
// EOF
