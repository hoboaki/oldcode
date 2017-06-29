/**
 * @file
 * @brief バージョン情報を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <string>

//------------------------------------------------------------
namespace textga {

    /**
     * @brief バージョン情報。
     * TexTargaLibのバージョンは*.*の形式で表される。
     * 1つめはFORMAT_VERSION。TexTargaフォーマットが大きく変更される度に増える。
     * 2つめはLIBRARY_VERSION。TexTargaC++Libがバグフィックスされる度に増える。
     */
    class Version
    {
    public:
        static const ::textga::u8   FORMAT_VERSION;
        static const ::textga::uint LIBRARY_VERSION;
        
        static std::string asString();
    };
}
//------------------------------------------------------------
// EOF
