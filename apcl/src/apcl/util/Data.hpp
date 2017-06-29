/** 
 * @file
 * @brief 可変長データを示すクラスを記述する。 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/operators/Addable.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/util/Types.hpp>

//-----------------------------------------------------------
namespace apcl { namespace util
{
  using ::apcl::operators::Addable;
  using ::apcl::operators::Equalable;
	using ::apcl::util::byte;
	using ::apcl::util::u32;

	/**
	 * @brief 可変長データを示すクラス
	 * <pre>
	 * 新たに確保した領域には値0がセットされる。
	 * </pre>
	 */
  class Data : public Addable< Data > , public Equalable< Data >
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 生成・破棄
    //@{
		Data(void);
		explicit Data( const Data& );
		Data( const void* aBytes , u32 aLength ); ///< コピー元のデータを指定して作成する。
		virtual ~Data();
    //@}

    //=================================================
    /// @name 複製
    //@{
		/// 引数のデータと長さで初期化する。
		void init( const void* aBytes , u32 aLength ); 
    /// データを複製する。
    void copy( const Data& aData );
    //@}

    //=================================================
    /// @name データ長
    //@{
		/// 長さを取得する。
		u32 length(void)const;
		/// データの長さを変える。元のサイズ部分の内容は保持される。
		void resize( u32 aSize ); 
    //@}

    //=================================================
    /// @name データ変更
    //@{
		/// データを末端に追加する。
		void add( const Data& );
		/// 指定の長さのデータを終端から除去する。
		void remove( u32 aLength );
    //@}

    //=================================================
    /// @name 等価判定
    //@{
		/// データの内容が等しいかどうかを取得する。
		bool equals( const Data& )const;
    //@}

    //=================================================
    /// @name データ取得
    //@{
		/// 指定のインデックスの参照を取得する。
		byte& at( u32 aIndex );
		byte at( u32 aIndex )const;
		/// バイトポインタを取得する。
		byte* bytes(void);
		const byte*	bytes(void)const;
    //@}

    //=================================================
    /// @name 演算子オーバーロード
    //@{
		byte&	operator []( u32 aIndex ); ///< at()
		byte	operator []( u32 aIndex )const; ///< at()
    Data& operator =( const Data& ); ///< copy()
    //@}

	private:
		byte*	bytes_;
		u32  length_;

		void _releaseBytes();
	};

}} // end of namespace ::apcl::util 

//-----------------------------------------------------------
// EOF
