/**
 * @file
 * @brief 角度を示すクラスを記述する。 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/operators/Addable.hpp>
#include <apcl/operators/Comparable.hpp>
#include <apcl/operators/Dividable.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/operators/Modable.hpp>
#include <apcl/operators/Multipliable.hpp>
#include <apcl/operators/Subtractable.hpp>

//-----------------------------------------------------------
namespace apcl { namespace math
{
  using ::apcl::operators::Addable;
  using ::apcl::operators::Comparable;
  using ::apcl::operators::Dividable;
  using ::apcl::operators::Equalable;
  using ::apcl::operators::Modable;
  using ::apcl::operators::Multipliable;
  using ::apcl::operators::Subtractable;


	/// 角度を示すクラス
  class Angle : 
    public Addable< Angle > ,
    public Comparable< Angle > ,
    public Dividable< Angle > ,
    public Equalable< Angle > ,
    public Modable< Angle > ,
    public Multipliable< Angle > ,
    public Subtractable< Angle >
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 変換
    //@{
		static float degreeToRadian( float aDegree ); ///< Degree値をRadianに変換する。
		static float radianToDegree( float aRadian ); ///< Radian値をDegreeに変換する。
    //@}

    //=================================================
    /// @name ファクトリーメソッド
    //@{
    static Angle createWithDegree( float aDegree ); ///< Degree値を指定して生成
    //@}

    //=================================================
    /// @name 生成・破棄
    //@{
		Angle();
		explicit Angle( float aRadian ); ///< Radian値を指定して初期化
		virtual ~Angle();
    //@}

    //=================================================
    /// @name 等価判定
    //@{
    bool equals( const Angle& )const; ///< 等価かどうか取得する。
    //@}

    //=================================================
    /// @name 比較演算
    //@{
    int compare( const Angle& )const; ///< 大小比較をする。
    //@}

    //=================================================
    /// @name Radian
    //@{
    float radian()const; ///< Radian値として取得する。
    void setRadian( float aRadian ); ///< Radian値として設定する。
    //@}

    //=================================================
    /// @name Degree
    //@{
		float degree()const; ///< Degree値として取得する。
		void setDegree( float aDegree ); ///< Degree値として設定する。
    //@}

    //=================================================
    /// @name 正規化
    //@{
		void normalize(); ///< -PI<=radian<PIになるように正規化する。
		bool isNormalized()const; ///< -PI<=radian<PIになっているか取得する。
    //@}

    //=================================================
    /// @name 演算（Radian値を用いて演算する）
    //@{
    void add( const Angle& aAngle ); ///< 加算する。
    void sub( const Angle& aAngle ); ///< 減算する。
    void mul( const Angle& aAngle ); ///< 乗算する。
    void div( const Angle& aAngle ); ///< 除算する。
    void mod( const Angle& aAngle ); ///< 剰余する。一度整数化して求める。
    //@}

  private:
    float radian_;
	};

}} // end of namespace ::apcl::math

//-----------------------------------------------------------
// EOF
