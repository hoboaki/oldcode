/** 
 * @file 
 * @brief かけ算演算子インターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief かけ算演算子インターフェース。
   * <pre>
   * 自分と同じ型or別の型に対してのかけ算をする演算子インターフェースを提供する。
   * publicで継承する。
   * OWNERは継承したクラス自身，TARGETは判定対象の型である必要がある。
   * OWNERとTARGETが同じ型である場合，TARGETを指定する必要はない。
   * 継承先クラスは，コピーコンストラクタが呼べる必要がある。
   * void mul( const TARGET& )を継承先で実装すること。
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Multipliable
  {
  public:  
    //=================================================
    /// @name かけ算演算子。
    //@{
    OWNER& operator *=( const TARGET& aVal )
    {
      OWNER& myOwner = owner();
      myOwner.mul( aVal );
      return myOwner;
    }
    OWNER operator *( const TARGET& aVal )const
    {      
      OWNER tmpOwner( owner() );
      tmpOwner.mul( aVal );
      return tmpOwner;
    }
    //@}

  protected:
    //=================================================
    /// @name 生成・破棄
    //@{
    Multipliable()
    {
    }
    virtual ~Multipliable()
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
