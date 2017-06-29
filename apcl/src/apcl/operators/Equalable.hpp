/** 
 * @file 
 * @brief �������艉�Z�q�C���^�[�t�F�[�X���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief �������艉�Z�q�C���^�[�t�F�[�X�B
   * <pre>
   * �����Ɠ����^or�ʂ̌^�ɑ΂��Ă̓������肷�鉉�Z�q�C���^�[�t�F�[�X��񋟂���B
   * public�Ōp������B
   * OWNER�͌p�������N���X���g�CTARGET�͔���Ώۂ̌^�ł���K�v������B
   * OWNER��TARGET�������^�ł���ꍇ�CTARGET���w�肷��K�v�͂Ȃ��B
   * bool equals( const TARGET& )const���p����Ŏ������邱�ƁB
   * equals()�̖߂�l�� OWNER == TARGET �̂Ƃ�true�B
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Equalable
  {
  public:  
    //=================================================
    /// @name �������艉�Z�q�B
    //@{
    bool operator ==( const TARGET& aVal )const
    {
      return owner().equals( aVal );
    }
    bool operator !=( const TARGET& aVal )const
    {      
      return !owner().equals( aVal );
    }
    //@}

  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    Equalable()
    {
    }
    virtual ~Equalable()
    {
    }
    //@}

  private:
    /// �I�[�i�[���擾����B
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
