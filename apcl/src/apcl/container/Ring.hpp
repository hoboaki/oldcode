/** 
* @file 
* @brief リングバッファクラスを記述する。 
*/
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/util/Types.hpp>

//-----------------------------------------------------------
namespace apcl { namespace container
{
  using ::apcl::operators::Equalable;
  using ::apcl::util::u32;

  /// リングバッファのクラス
  template< typename T >
  class Ring : public Equalable< Ring<T> >
  {
  public:
    typedef T ValType; ///< テンプレートの型

    /// テストコード
    static void unitTest()
    {
      const int aSize = 30;

      Ring<int> ring( aSize );
      apclAssert( ring.size() == 1 );
      apclAssert( ring.sizeMax() == aSize );

      for ( int i = 0; i < aSize; ++i )
      {
        apclAssert( ring.index() == i );
        apclAssert( ring.size() == i+1 );
        ring.at(0) = i;
        ring.next();
      }
      apclAssert( ring.index() == 0 );
      apclAssert( ring.size() == ring.sizeMax() );

      apclAssert( ring.at(0) == 0 );
      for ( int i = 1; i < aSize; ++i )
        apclAssert( ring.at(i) == aSize-i );

      Ring<int> ring2( ring );
      for ( int i = 0; i < aSize; ++i )
        apclAssert( ring.at(i) == ring2.at(i) );

      Ring<int> ring3( 1 );
      ring3 = ring;
      for ( int i = 0; i < aSize; ++i )
        apclAssert( ring.at(i) == ring3.at(i) );
      apclAssert( ring3.at(0) == 0 );
    }

    //=================================================
    /// @name 生成・破棄
    //@{
    /// バッファサイズを指定して作成
    explicit Ring( const u32 aSizeMax ):
    size_( aSizeMax ),
      index_( 0 ),
      loopedFlg_( false )
    {
      apclAssert( aSizeMax > 0 );
      buff_ = new T[size_];
    }

    /// コピーコンストラクタ
    explicit Ring( const Ring<T>& aRing ):
    size_( aRing.sizeMax() ),
      index_( 0 ),
      loopedFlg_( false )
    {
      apclAssert( size_ > 0 );
      buff_ = new T[size_];
      copy( aRing );
    }

    /// デストラクタ
    virtual ~Ring()	
    {	
      delete[] buff_; 
    }
    //@}

    //=================================================
    /**
     * @name 等価判定
     * Ringはat(0)からat( size() )までが全て等しければ等価と判定する。
     */
    //@{
    /// 等しいかどうか取得する。
    bool equals( const Ring<T>& aRing )const
    {
      if ( size() == aRing.size() )
      {
        const u32 max = size();
        u32 i;
        for ( i = 0; i < max; ++i )
          if ( at( i ) != aRing.at( i ) )
            break;
        if ( i == max )
          return true;
      }
      return false;
    }
    //@}

    //=================================================
    /// @name 複製
    //@{
    /// 引数の内容をコピーする。
    void copy( const Ring<T>& aRing )
    {
      // 解放
      delete[] buff_;

      // 初期化
      size_ = aRing.sizeMax();
      index_ = aRing.index();
      loopedFlg_ = aRing.size() == aRing.sizeMax();
      buff_ = new T[size_];

      // コピー
      const u32 max = aRing.size();
      for ( u32 i = 0; i < max; ++i )
      {
        const u32 index = index_ >= i ? index_-i : size_-(i-index_);
        buff_[index] = aRing.at(i);
      }
    }
    //@}

    //=================================================
    /// @name サイズ
    //@{
    /// 現在のサイズを取得する。
    u32 size()const	{	return loopedFlg_ ? size_ : index_+1; }
    /// 最大サイズを取得する。
    u32 sizeMax()const	{	return size_; }
    //@}

    //=================================================
    /// @name インデックス
    //@{
    /// 現在指してるバッファのインデックス値を取得する。
    u32 index()const { return index_; }
    /// 現在指しているバッファのインデックスを１つ移動する。
    void next()
    {
      if ( index_ == size_ -1 )
      {
        loopedFlg_ = true;
        index_ = 0;
      }
      else
        ++index_;
      apclAssert( index_ >=0 && index_ < size_ );
    }
    //@}

    //=================================================
    /**
    * @name 参照取得
    * <pre>
    * index = 0が，現在指してるバッファのインデックス。
    * インデックス値が増えるほど，過去にさかのぼる。
    * また，インデックス値は0以上，size()未満である必要がある。
    * </pre>
    */
    //@{
    /// 要素の参照を取得する。
    T& at( const u32 aIndex )
    {
      apclAssert( aIndex >= 0 && aIndex < size() );
      u32 index;
      if ( aIndex <= index_ )
        index = index_ - aIndex;
      else
      {
        const u32 subMemo = aIndex - index_;
        index = size_ - subMemo;
      }
      apclAssert( index < size_ );
      return buff_[index];
    }
    /// at(aIndex)のconst版
    const T& at( const u32 aIndex )const
    {
      apclAssert( aIndex >= 0 && aIndex < size() );
      u32 index;
      if ( aIndex <= index_ )
        index = index_ - aIndex;
      else
      {
        const u32 subMemo = aIndex - index_;
        index = size_ - subMemo;
      }
      apclAssert( index < size_ );
      return buff_[index];
    }
    //@}

    //=================================================
    /// @name 演算子
    //@{
    /// at()のカバー
    T& operator[]( const u32 aIndex ){ return at(aIndex); } 
    /// at()のカバー
    const T& operator[]( const u32 aIndex )const{ return at(aIndex); } 
    /// copy()のカバー
    Ring<T>& operator=( const Ring<T>& aRing )
    { 
      copy( aRing ); 
      return *this; 
    }
    //@}

  private:
    T*	buff_;
    u32 size_;
    u32 index_;
    bool loopedFlg_;
  };


}} // end of namespace ::apcl::container

//-----------------------------------------------------------
// EOF
