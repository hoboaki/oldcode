/** 
 * @file 
 * @brief 等価判定演算子インターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief 等価判定演算子インターフェース。
   * <pre>
   * 自分と同じ型or別の型に対しての等価判定する演算子インターフェースを提供する。
   * publicで継承する。
   * OWNERは継承したクラス自身，TARGETは判定対象の型である必要がある。
   * OWNERとTARGETが同じ型である場合，TARGETを指定する必要はない。
   * bool equals( const TARGET& )constを継承先で実装すること。
   * equals()の戻り値は OWNER == TARGET のときtrue。
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Equalable
  {
  public:  
    //=================================================
    /// @name 等価判定演算子。
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
    /// @name 生成・破棄
    //@{
    Equalable()
    {
    }
    virtual ~Equalable()
    {
    }
    //@}

  private:
    /// オーナーを取得する。
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
