/** 
 * @file
 * @brief ファイルを読み込むクラスを記述する。 
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
		
	/// ファイルを読み込むクラス
	class FileLoader
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 生成・破棄
    //@{
		/// 読み込むファイルのパスとタイプを設定する。
		FileLoader( const FilePath& , FileType = FileType_Binary ); 
		virtual ~FileLoader();
    //@}

    //=================================================
    /// @name プロパティの取得
    //@{
		/// 読み込むファイルのパスを取得する。
		const FilePath& path()const;
		/// 読み込むファイルのタイプを取得する。
		FileType type()const;
    //@}

    //=================================================
    /// @name 読み込み
    //@{
		/// 読み込む。成功したらtrue。
		bool load( u32 aBufferSize = 0x10000 );
		/// 読み込んだファイルのデータを取得する。（読み込み成功を確認した後，取得すること）
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
