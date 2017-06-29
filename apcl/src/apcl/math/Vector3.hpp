/** 
 * @file
 * @brief ３次元ベクトルを表すクラスを記述する。 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/operators/Addable.hpp>
#include <apcl/operators/Dividable.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/operators/Multipliable.hpp>
#include <apcl/operators/Subtractable.hpp>

//-----------------------------------------------------------
namespace apcl { namespace math
{
  using ::apcl::operators::Addable;
  using ::apcl::operators::Dividable;
  using ::apcl::operators::Equalable;
  using ::apcl::operators::Multipliable;
  using ::apcl::operators::Subtractable;

	/// ３次元ベクトルを表すクラス
  class Vector3 :
    public Addable< Vector3 > , 
    public Dividable< Vector3 > ,
    public Equalable< Vector3 > ,
    public Multipliable< Vector3 > ,
    public Subtractable< Vector3 >
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 生成・破棄
    //@{
		Vector3();
		Vector3( float x , float y , float z ); ///< x,y,zを指定して作成する。
		virtual ~Vector3();
    //@}

    //=================================================
    /// @name 取得・設定
    //@{
    float x()const; ///< x値を取得する。
    float y()const; ///< y値を取得する。
    float z()const; ///< z値を取得する。
    void setX( float x ); ///< x値を設定する。
    void setY( float y ); ///< y値を設定する。
    void setZ( float z ); ///< z値を設定する。
    //@}

    //=================================================
    /// @name 等価判定
    //@{
		bool equals( const Vector3& )const; ///< 等価判定をする。
    //@}

    //=================================================
    /// @name 加算
    //@{
		void add( float ); ///< 各要素に値を加算する。
		void add( const Vector3& ); ///< 対応する値同士を加算する。
    //@}

    //=================================================
    /// @name 減算
    //@{
		void sub( float ); ///< 各要素から値を減算する。
		void sub( const Vector3& ); ///< 対応する値に対して減算する。
    //@}

    //=================================================
    /// @name 乗算
    //@{
		void mul( float ); ///< 各要素に値をかける。
		void mul( const Vector3& ); ///< 対応する値同士をかける。
    //@}

    //=================================================
    /// @name 除算
    //@{
		void div( float ); ///< 各要素から値をわる。
		void div( const Vector3& ); ///< 対応する値に対して割り算をする。
    //@}

    //=================================================
    /// @name 長さ
    //@{
		float length()const; ///< 大きさ（長さ）を取得する。
		float distance( const Vector3& )const; ///< お互いの距離を取得する。
    //@}

    //=================================================
    /// @name 単位ベクトル
    //@{
		void normalize(); ///< 単位ベクトル化する。
		bool isNormalized()const; ///< 単位ベクトルかどうか取得する。
    //@}

    //=================================================
    /// @name 内積
    //@{
		float innner( const Vector3& )const; ///< あるベクトルとの内積を取得する。
    //@}

    //=================================================
    /// @name 外積
    //@{
		void cross( const Vector3& ); ///< 外積をとる。
		Vector3 getCross( const Vector3& )const; ///< 外積をとったベクトルを取得する。
    //@}

    //=================================================
    /// @name 演算子オーバーロード
    //@{
    
    //@}

  private:
		float x_;
		float y_;
		float z_;
	};

}} // end of namespace ::apcl::math

//-----------------------------------------------------------
// EOF
