/** 
 * @file 
 * @brief 比較判定演算子インターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief 比較判定演算子インターフェース。
   * <pre>
   * 自分と同じ型or別の型に対しての比較判定する演算子インターフェースを提供する。
   * publicで継承する。
   * OWNERは継承したクラス自身，TARGETは判定対象の型である必要がある。
   * OWNERとTARGETが同じ型である場合，TARGETを指定する必要はない。
   * int compare( const TARGET& )constを継承先で実装すること。
   * compare()の戻り値は 
   *  OWNER < TARGET : -1以下
   *  OWNER > TARGET : 1以上
   *  OWNER <= TARGET : 0以下
   *  OWNER >= TARGET : 0以上
   * を満たしてる必要がある。
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Comparable
  {
  public:  
    //=================================================
    /// @name 比較判定演算子。
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
    /// @name 生成・破棄
    //@{
    Comparable()
    {
    }
    virtual ~Comparable()
    {
    }
    //@}

  private:
    /// オーナー取得
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
