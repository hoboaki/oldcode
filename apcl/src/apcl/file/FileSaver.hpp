/** 
 * @file
 * @brief ファイルを保存するクラスを記述する。 
 */
#pragma once

//-----------------------------------------------------------
#include <apcl/file/File.hpp>

//-----------------------------------------------------------
namespace apcl { namespace file
{
	/// ファイルを保存するクラス
	class FileSaver
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 生成・破棄
    //@{
		FileSaver( const File& ); ///< 保存するファイルを指定する。
		virtual ~FileSaver();
    //@}

    //=================================================
    /// @name プロパティ取得
    //@{
		/// 保存するファイルを取得する。
		const File& file()const;
    //@}

    //=================================================
    /// @name 保存
    //@{
		/// 保存する。成功したらtrue。
		bool save()const;
    //@}

	private:
		const File file_;
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
