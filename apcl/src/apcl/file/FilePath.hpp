/** 
 * @file
 * @brief ファイルパスを示すクラスを記述する。 
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
	 * @brief ファイルパスを示すクラス
	 * <pre>
	 * ディレクトリはスラッシュ'/'で区切られる。
	 * </pre>
	 */
  class FilePath : public Equalable< FilePath >
	{
	public:
    /// テストコード
    static void unitTest();

    //=================================================
    /// @name 生成・破棄
    //@{
		FilePath();
    explicit FilePath( const string& aPath ); ///< パスを指定して作成
		virtual ~FilePath();
    //@}

    //=================================================
    /// @name 等価判定
    //@{
    /// 等価かどうか取得する。
    bool equals( const FilePath& aPath )const;
    //@}

    //=================================================
    /// @name ファイルパス
    //@{
		/// ファイルパスを設定する。
		void setPath( const string& aFilePath );
		/// ファイルパスを取得する。
		const string& path()const;
    //@}

	private:
		string path_;
	};

}} // end of namespace ::apcl::file

//-----------------------------------------------------------
// EOF
