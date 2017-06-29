/**
 * @file
 * @brief �p�x�������N���X���L�q����B 
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


	/// �p�x�������N���X
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
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �ϊ�
    //@{
		static float degreeToRadian( float aDegree ); ///< Degree�l��Radian�ɕϊ�����B
		static float radianToDegree( float aRadian ); ///< Radian�l��Degree�ɕϊ�����B
    //@}

    //=================================================
    /// @name �t�@�N�g���[���\�b�h
    //@{
    static Angle createWithDegree( float aDegree ); ///< Degree�l���w�肵�Đ���
    //@}

    //=================================================
    /// @name �����E�j��
    //@{
		Angle();
		explicit Angle( float aRadian ); ///< Radian�l���w�肵�ď�����
		virtual ~Angle();
    //@}

    //=================================================
    /// @name ��������
    //@{
    bool equals( const Angle& )const; ///< �������ǂ����擾����B
    //@}

    //=================================================
    /// @name ��r���Z
    //@{
    int compare( const Angle& )const; ///< �召��r������B
    //@}

    //=================================================
    /// @name Radian
    //@{
    float radian()const; ///< Radian�l�Ƃ��Ď擾����B
    void setRadian( float aRadian ); ///< Radian�l�Ƃ��Đݒ肷��B
    //@}

    //=================================================
    /// @name Degree
    //@{
		float degree()const; ///< Degree�l�Ƃ��Ď擾����B
		void setDegree( float aDegree ); ///< Degree�l�Ƃ��Đݒ肷��B
    //@}

    //=================================================
    /// @name ���K��
    //@{
		void normalize(); ///< -PI<=radian<PI�ɂȂ�悤�ɐ��K������B
		bool isNormalized()const; ///< -PI<=radian<PI�ɂȂ��Ă��邩�擾����B
    //@}

    //=================================================
    /// @name ���Z�iRadian�l��p���ĉ��Z����j
    //@{
    void add( const Angle& aAngle ); ///< ���Z����B
    void sub( const Angle& aAngle ); ///< ���Z����B
    void mul( const Angle& aAngle ); ///< ��Z����B
    void div( const Angle& aAngle ); ///< ���Z����B
    void mod( const Angle& aAngle ); ///< ��]����B��x���������ċ��߂�B
    //@}

  private:
    float radian_;
	};

}} // end of namespace ::apcl::math

//-----------------------------------------------------------
// EOF
