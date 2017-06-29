/** 
 * @file 
 * @brief ファイルを示すクラスを記述する。 
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

	/// ファイルを示すクラス
  class File : public Equalable< File >
	{
	public:
    /// テストコード
    static void unitTest();

		//=================================================
		/// @name 生成・破棄
		//@{
		File();
		virtual ~File();
		//@}

    //=================================================
    /// @name 等価判定
    //@{
    /// ファイルパス，ファイルタイプ，データ，全て等価か取得する。
    bool equals( const File& aFile )const;
    //@}

		//=================================================
		/// @name ファイルパス
		//@{
		/// ファイルパスを取得する。
		const FilePath& path()const;
		/// ファイルパスを設定する。
		void setPath( const FilePath& );
		//@}

		//=================================================
		/// @name データ
		//@{
		/// データを取得する。
		const Data& data()const;
		/// データを設定する。
		void setData( const Data& );
		//@}

		//=================================================
		/// @name ファイルタイプ
		//@{
		/// ファイルタイプを取得する。
		FileType type()const;
		/// ファイルタイプを設定する。
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
