/**
 * @file
 * @brief �f�U�C���p�^�[��Singleton�C���^�[�t�F�[�X���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace interface
{
  /**
   * @brief �f�U�C���p�^�[��Singleton�C���^�[�t�F�[�X�B
   * <pre>
   * public�p�����Ďg���B
   * �p������N���X�̓f�t�H���g�R���X�g���N�^�����K�v������B
   * �C���X�^���X�ɃA�N�Z�X����ꍇ�A�N���X��::instance()�ŃA�N�Z�X����B
   * Singleton���p�������N���X���p������̂͐������Ȃ��B
   *
   * ��F���m��Singleton�i�R���X�g���N�^�����J���Ȃ��j
   * @code
   * class A : public ::apcl::interface::Singleton<A>
   * {
   *   friend ::apcl::interface::Singleton<A>;
   * public:
   *   void print();
   * private:
   *   A();
   * };
   *
   * void func()
   * {
   *  A::instance().print();
   * }
   * @endcode
   *
   * ��F���m�łȂ�Singleton�iA�̃C���X�^���X�����Ȃ��ۏ؂��K�v�j
   * @code
   * class A : public ::apcl::interface::Singleton<A>
   * {
   * public:
   *   void print();
   * };
   *
   * void func()
   * {
   *  A::instance().print();
   * }
   * @endcode
   *
   * </pre>
   */
  template< typename T >
  class Singleton
  {
  public:
    /// �e�X�g�R�[�h
    static void unitTest()
    {
      int objA = Singleton<int>::instance();
      int objB = Singleton<int>::instance();
      apclAssert( objA == objB );
      int objC = Singleton<char>::instance();
    }

    //=================================================
    /// @name �C���X�^���X�擾
    /// �C���X�^���X���擾����B
    static T& instance()
    {
#ifdef _DEBUG
      callInstanceFlg_ = true;
#endif
      static T instance_;
      return instance_;
    }

  protected:
    //=================================================
    /// @name �����E�j��
    //@{
    Singleton()
    {
#ifdef _DEBUG
      static bool instanceFlg = false;
      apclAssertMsg( callInstanceFlg_ , "Did Not Call Singleton::instance() Exception" );
      apclAssertMsg( !instanceFlg , "Recreate Instance Exception" );
      instanceFlg = true;
#endif
    }
    virtual ~Singleton()
    {
    }
    //@}

  private:
#ifdef _DEBUG
    static bool callInstanceFlg_;
#endif

  };
#ifdef _DEBUG
  template< typename T > bool Singleton<T>::callInstanceFlg_ = false;
#endif


}} // end of namespace ::apcl::interface

//-----------------------------------------------------------
// EOF
