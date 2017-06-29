/** 
 * @file 
 * @brief ����Z���Z�q�C���^�[�t�F�[�X���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief ����Z���Z�q�C���^�[�t�F�[�X�B
   * <pre>
   * �����Ɠ����^or�ʂ̌^�ɑ΂��Ă̊���Z�����鉉�Z�q�C���^�[�t�F�[�X��񋟂���B
   * public�Ōp������B
   * OWNER�͌p�������N���X���g�CTARGET�͔���Ώۂ̌^�ł���K�v������B
   * OWNER��TARGET�������^�ł���ꍇ�CTARGET���w�肷��K�v�͂Ȃ��B
   * �p����N���X�́C�R�s�[�R���X�g���N�^���Ăׂ�K�v������B
   * void div( const TARGET& )���p����Ŏ������邱�ƁB
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Dividable
  {
  public:  
    //=================================================
    /// @name ����Z���Z�q�B
    //@{
    OWNER& operator /=( const TARGET& aVal )
    {
      OWNER& myOwner = owner();
      myOwner.div( aVal );
      return myOwner;
    }
    OWNER operator /( const TARGET& aVal )const
    {      
      OWNER tmpOwner( owner() );
      tmpOwner.div( aVal );
      return tmpOwner;
    }
    //@}

  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    Dividable()
    {
    }
    virtual ~Dividable()
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
