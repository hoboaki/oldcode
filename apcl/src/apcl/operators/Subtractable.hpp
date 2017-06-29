/** 
 * @file 
 * @brief 減算演算子インターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief 減算演算子インターフェース。
   * <pre>
   * 自分と同じ型or別の型に対しての減算をする演算子インターフェースを提供する。
   * publicで継承する。
   * OWNERは継承したクラス自身，TARGETは判定対象の型である必要がある。
   * OWNERとTARGETが同じ型である場合，TARGETを指定する必要はない。
   * 継承先クラスは，コピーコンストラクタが呼べる必要がある。
   * void sub( const TARGET& )を継承先で実装すること。
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Subtractable
  {
  public:  
    //=================================================
    /// @name 減算演算子。
    //@{
    OWNER& operator -=( const TARGET& aVal )
    {
      OWNER& myOwner = owner();
      myOwner.sub( aVal );
      return myOwner;
    }
    OWNER operator -( const TARGET& aVal )const
    {      
      OWNER tmpOwner( owner() );
      tmpOwner.sub( aVal );
      return tmpOwner;
    }
    //@}

  protected:
    //=================================================
    /// @name 生成・破棄
    //@{
    Subtractable()
    {
    }
    virtual ~Subtractable()
    {
    }
    //@}

  private:
    /// オーナーを取得する。
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
