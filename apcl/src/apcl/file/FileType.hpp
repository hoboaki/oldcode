/** 
 * @file 
 * @brief ファイルタイプ識別子を記述する。 
 */
#pragma once

//-----------------------------------------------------------
namespace apcl { namespace file 
{
	/// ファイルタイプ識別子
	enum FileType
	{
		FileType_Binary = 0 , ///< バイナリ
		FileType_Ascii , ///< アスキー
		FileType_Terminate,
		FileType_Begin = 0 ,
		FileType_End = FileType_Terminate
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF

