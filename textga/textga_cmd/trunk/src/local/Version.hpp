/**
 * @file
 * @brief バージョン情報を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace local {

    /// バージョン情報。
    class Version
    {
    public:
        /// バグフィックスバージョン。バグフィックスされる度にあがる。
        static const ::textga::uint BUGFIX_VERSION;
        
        /// 文字列で取得。（*.*.*）
        static std::string asString();
    };

}
//------------------------------------------------------------
// EOF
