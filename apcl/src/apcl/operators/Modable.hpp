/** 
 * @file 
 * @brief 余り演算子インターフェースを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>

//-----------------------------------------------------------
namespace apcl { namespace operators
{
  /**
   * @brief 余り演算子インターフェース。
   * <pre>
   * 自分と同じ型or別の型に対しての余りをする演算子インターフェースを提供する。
   * publicで継承する。
   * OWNERは継承したクラス自身，TARGETは判定対象の型である必要がある。
   * OWNERとTARGETが同じ型である場合，TARGETを指定する必要はない。
   * 継承先クラスは，コピーコンストラクタが呼べる必要がある。
   * void mod( const TARGET& )を継承先で実装すること。
   * </pre>
   */
  template< typename OWNER , typename TARGET = OWNER >
  class Modable
  {
  public:  
    //=================================================
    /// @name 余り演算子。
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
    /// @name 生成・破棄
    //@{
    Modable()
    {
    }
    virtual ~Modable()
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
