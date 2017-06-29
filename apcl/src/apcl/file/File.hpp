/** 
 * @file 
 * @brief �t�@�C���������N���X���L�q����B 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/file/FileType.hpp>
#include <apcl/file/FilePath.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/util/Data.hpp>

//-----------------------------------------------------------
namespace apcl { namespace file
{
  using ::apcl::operators::Equalable;
	using ::apcl::util::Data;

	/// �t�@�C���������N���X
  class File : public Equalable< File >
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

		//=================================================
		/// @name �����E�j��
		//@{
		File();
		virtual ~File();
		//@}

    //=================================================
    /// @name ��������
    //@{
    /// �t�@�C���p�X�C�t�@�C���^�C�v�C�f�[�^�C�S�ē������擾����B
    bool equals( const File& aFile )const;
    //@}

		//=================================================
		/// @name �t�@�C���p�X
		//@{
		/// �t�@�C���p�X���擾����B
		const FilePath& path()const;
		/// �t�@�C���p�X��ݒ肷��B
		void setPath( const FilePath& );
		//@}

		//=================================================
		/// @name �f�[�^
		//@{
		/// �f�[�^���擾����B
		const Data& data()const;
		/// �f�[�^��ݒ肷��B
		void setData( const Data& );
		//@}

		//=================================================
		/// @name �t�@�C���^�C�v
		//@{
		/// �t�@�C���^�C�v���擾����B
		FileType type()const;
		/// �t�@�C���^�C�v��ݒ肷��B
		void setType( FileType );
		//@}

	private:
		FilePath path_;
		Data data_;
		FileType type_;
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
