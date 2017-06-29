/** 
 * @file
 * @brief �t�@�C����ۑ�����N���X���L�q����B 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/file/File.hpp>

//-----------------------------------------------------------
namespace apcl { namespace file
{
	/// �t�@�C����ۑ�����N���X
	class FileSaver
	{
	public:
    /// �e�X�g�R�[�h
    static void unitTest();

    //=================================================
    /// @name �����E�j��
    //@{
		FileSaver( const File& ); ///< �ۑ�����t�@�C�����w�肷��B
		virtual ~FileSaver();
    //@}

    //=================================================
    /// @name �v���p�e�B�擾
    //@{
		/// �ۑ�����t�@�C�����擾����B
		const File& file()const;
    //@}

    //=================================================
    /// @name �ۑ�
    //@{
		/// �ۑ�����B����������true�B
		bool save()const;
    //@}

	private:
		const File file_;
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
