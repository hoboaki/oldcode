/** 
 * @file
 * @brief �t�@�C���p�X�������N���X���L�q����B 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/operators/Equalable.hpp>
#include <apcl/container/Ring.hpp>
#include <string>

//-----------------------------------------------------------
namespace apcl { namespace file
{
  using ::apcl::operators::Equalable;
	using ::std::string;

	/**
	 * @brief �t�@�C���p�X�������N���X
	 * <pre>
	 * �f�B���N�g���̓X���b�V��'/'�ŋ�؂���B
	 * </pre>
	 */
  class FilePath : public Equalable< FilePath >
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �����E�j��
    //@{
		FilePath();
    explicit FilePath( const string& aPath ); ///< �p�X���w�肵�č쐬
		virtual ~FilePath();
    //@}

    //=================================================
    /// @name ��������
    //@{
    /// �������ǂ����擾����B
    bool equals( const FilePath& aPath )const;
    //@}

    //=================================================
    /// @name �t�@�C���p�X
    //@{
		/// �t�@�C���p�X��ݒ肷��B
		void setPath( const string& aFilePath );
		/// �t�@�C���p�X���擾����B
		const string& path()const;
    //@}

	private:
		string path_;
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
