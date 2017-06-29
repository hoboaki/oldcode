/** 
 * @file 
 * @brief �]�艉�Z�q�C���^�[�t�F�[�X���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief �]�艉�Z�q�C���^�[�t�F�[�X�B
   * <pre>
   * �����Ɠ����^or�ʂ̌^�ɑ΂��Ă̗]������鉉�Z�q�C���^�[�t�F�[�X��񋟂���B
   * public�Ōp������B
   * OWNER�͌p�������N���X���g�CTARGET�͔���Ώۂ̌^�ł���K�v������B
   * OWNER��TARGET�������^�ł���ꍇ�CTARGET���w�肷��K�v�͂Ȃ��B
   * �p����N���X�́C�R�s�[�R���X�g���N�^���Ăׂ�K�v������B
   * void mod( const TARGET& )���p����Ŏ������邱�ƁB
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Modable
  {
  public:  
    //=================================================
    /// @name �]�艉�Z�q�B
    //@{
    OWNER& operator %=( const TARGET& aVal )
    {
      OWNER& myOwner = owner();
      myOwner.mod( aVal );
      return myOwner;
    }
    OWNER operator %( const TARGET& aVal )const
    {      
      OWNER tmpOwner( owner() );
      tmpOwner.mod( aVal );
      return tmpOwner;
    }
    //@}

  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    Modable()
    {
    }
    virtual ~Modable()
    {
    }
    //@}

  private:
    /// �I�[�i�[���擾����B
    OWNER& owner()
    {
      OWNER* ptr = dynamic_cast< OWNER* >( this );
      apclAssert( ptr );
      return *ptr;
    }
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
