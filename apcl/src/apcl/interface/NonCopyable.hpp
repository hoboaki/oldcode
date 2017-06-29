/**
 * @file
 * @brief コピーできないインターフェース。
 */
#pragma once

//-----------------------------------------------------------
namespace apcl { namespace interface
{
  /// コピーできなくするインターフェース。privateで継承する。
  class NonCopyable
  {
  protected:
    //=================================================
    /// @name 生成・破棄
    //@{
    NonCopyable();
    virtual ~NonCopyable();
    //@}

  private:
    //=================================================
    /// @name 実装しない関数
    //@{
    NonCopyable( const NonCopyable& aObject ); ///< コピーコンストラクタ
    const NonCopyable& operator = ( const NonCopyable& aObject ); ///< 代入演算子
    //@}
  };

}} // end of namespace ::apcl::interface

//-----------------------------------------------------------
// EOF
