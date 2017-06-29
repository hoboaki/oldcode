/** 
 * @file
 * @brief �t�@�C����ǂݍ��ރN���X���L�q����B 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/file/FilePath.hpp>
#include <apcl/file/FileType.hpp>
#include <apcl/util/Data.hpp>
#include <apcl/util/Types.hpp>

//-----------------------------------------------------------
namespace apcl { namespace file
{
	using ::apcl::util::Data;
	using ::apcl::util::u32;
		
	/// �t�@�C����ǂݍ��ރN���X
	class FileLoader
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �����E�j��
    //@{
		/// �ǂݍ��ރt�@�C���̃p�X�ƃ^�C�v��ݒ肷��B
		FileLoader( const FilePath& , FileType = FileType_Binary ); 
		virtual ~FileLoader();
    //@}

    //=================================================
    /// @name �v���p�e�B�̎擾
    //@{
		/// �ǂݍ��ރt�@�C���̃p�X���擾����B
		const FilePath& path()const;
		/// �ǂݍ��ރt�@�C���̃^�C�v���擾����B
		FileType type()const;
    //@}

    //=================================================
    /// @name �ǂݍ���
    //@{
		/// �ǂݍ��ށB����������true�B
		bool load( u32 aBufferSize = 0x10000 );
		/// �ǂݍ��񂾃t�@�C���̃f�[�^���擾����B�i�ǂݍ��ݐ������m�F������C�擾���邱�Ɓj
		const Data& data()const;
    //@}

	private:
		const FilePath path_;
		const FileType type_;
		Data data_;

	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
