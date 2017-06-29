/** 
 * @file
 * @brief �ϒ��f�[�^�������N���X���L�q����B 
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
	 * @brief �ϒ��f�[�^�������N���X
	 * <pre>
	 * �V���Ɋm�ۂ����̈�ɂ͒l0���Z�b�g�����B
	 * </pre>
	 */
  class Data : public Addable< Data > , public Equalable< Data >
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �����E�j��
    //@{
		Data(void);
		explicit Data( const Data& );
		Data( const void* aBytes , u32 aLength ); ///< �R�s�[���̃f�[�^���w�肵�č쐬����B
		virtual ~Data();
    //@}

    //=================================================
    /// @name ����
    //@{
		/// �����̃f�[�^�ƒ����ŏ���������B
		void init( const void* aBytes , u32 aLength ); 
    /// �f�[�^�𕡐�����B
    void copy( const Data& aData );
    //@}

    //=================================================
    /// @name �f�[�^��
    //@{
		/// �������擾����B
		u32 length(void)const;
		/// �f�[�^�̒�����ς���B���̃T�C�Y�����̓��e�͕ێ������B
		void resize( u32 aSize ); 
    //@}

    //=================================================
    /// @name �f�[�^�ύX
    //@{
		/// �f�[�^�𖖒[�ɒǉ�����B
		void add( const Data& );
		/// �w��̒����̃f�[�^���I�[���珜������B
		void remove( u32 aLength );
    //@}

    //=================================================
    /// @name ��������
    //@{
		/// �f�[�^�̓��e�����������ǂ������擾����B
		bool equals( const Data& )const;
    //@}

    //=================================================
    /// @name �f�[�^�擾
    //@{
		/// �w��̃C���f�b�N�X�̎Q�Ƃ��擾����B
		byte& at( u32 aIndex );
		byte at( u32 aIndex )const;
		/// �o�C�g�|�C���^���擾����B
		byte* bytes(void);
		const byte*	bytes(void)const;
    //@}

    //=================================================
    /// @name ���Z�q�I�[�o�[���[�h
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
