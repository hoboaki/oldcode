/** 
 * @file 
 * @brief �t�@�C���^�C�v���ʎq���L�q����B 
 */
#pragma once

//-----------------------------------------------------------
namespace apcl { namespace file 
{
	/// �t�@�C���^�C�v���ʎq
	enum FileType
	{
		FileType_Binary = 0 , ///< �o�C�i��
		FileType_Ascii , ///< �A�X�L�[
		FileType_Terminate,
		FileType_Begin = 0 ,
		FileType_End = FileType_Terminate
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF

