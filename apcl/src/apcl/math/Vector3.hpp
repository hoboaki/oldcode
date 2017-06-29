/** 
 * @file
 * @brief �R�����x�N�g����\���N���X���L�q����B 
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

	/// �R�����x�N�g����\���N���X
  class Vector3 :
    public Addable< Vector3 > , 
    public Dividable< Vector3 > ,
    public Equalable< Vector3 > ,
    public Multipliable< Vector3 > ,
    public Subtractable< Vector3 >
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �����E�j��
    //@{
		Vector3();
		Vector3( float x , float y , float z ); ///< x,y,z���w�肵�č쐬����B
		virtual ~Vector3();
    //@}

    //=================================================
    /// @name �擾�E�ݒ�
    //@{
    float x()const; ///< x�l���擾����B
    float y()const; ///< y�l���擾����B
    float z()const; ///< z�l���擾����B
    void setX( float x ); ///< x�l��ݒ肷��B
    void setY( float y ); ///< y�l��ݒ肷��B
    void setZ( float z ); ///< z�l��ݒ肷��B
    //@}

    //=================================================
    /// @name ��������
    //@{
		bool equals( const Vector3& )const; ///< �������������B
    //@}

    //=================================================
    /// @name ���Z
    //@{
		void add( float ); ///< �e�v�f�ɒl�����Z����B
		void add( const Vector3& ); ///< �Ή�����l���m�����Z����B
    //@}

    //=================================================
    /// @name ���Z
    //@{
		void sub( float ); ///< �e�v�f����l�����Z����B
		void sub( const Vector3& ); ///< �Ή�����l�ɑ΂��Č��Z����B
    //@}

    //=================================================
    /// @name ��Z
    //@{
		void mul( float ); ///< �e�v�f�ɒl��������B
		void mul( const Vector3& ); ///< �Ή�����l���m��������B
    //@}

    //=================================================
    /// @name ���Z
    //@{
		void div( float ); ///< �e�v�f����l�����B
		void div( const Vector3& ); ///< �Ή�����l�ɑ΂��Ċ���Z������B
    //@}

    //=================================================
    /// @name ����
    //@{
		float length()const; ///< �傫���i�����j���擾����B
		float distance( const Vector3& )const; ///< ���݂��̋������擾����B
    //@}

    //=================================================
    /// @name �P�ʃx�N�g��
    //@{
		void normalize(); ///< �P�ʃx�N�g��������B
		bool isNormalized()const; ///< �P�ʃx�N�g�����ǂ����擾����B
    //@}

    //=================================================
    /// @name ����
    //@{
		float innner( const Vector3& )const; ///< ����x�N�g���Ƃ̓��ς��擾����B
    //@}

    //=================================================
    /// @name �O��
    //@{
		void cross( const Vector3& ); ///< �O�ς��Ƃ�B
		Vector3 getCross( const Vector3& )const; ///< �O�ς��Ƃ����x�N�g�����擾����B
    //@}

    //=================================================
    /// @name ���Z�q�I�[�o�[���[�h
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
