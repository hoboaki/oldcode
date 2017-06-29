/**
 * @file
 * @brief デザインパターンSingletonインターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace interface
{
  /**
   * @brief デザインパターンSingletonインターフェース。
   * <pre>
   * public継承して使う。
   * 継承するクラスはデフォルトコンストラクタを持つ必要がある。
   * インスタンスにアクセスする場合、クラス名::instance()でアクセスする。
   * Singletonを継承したクラスを継承するのは推奨しない。
   *
   * 例：正確なSingleton（コンストラクタを公開しない）
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
   * 例：正確でないSingleton（Aのインスタンスを作らない保証が必要）
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
    /// テストコード
    static void unitTest()
    {
      int objA = Singleton<int>::instance();
      int objB = Singleton<int>::instance();
      apclAssert( objA == objB );
      int objC = Singleton<char>::instance();
    }

    //=================================================
    /// @name インスタンス取得
    /// インスタンスを取得する。
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
    /// @name 生成・破棄
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
