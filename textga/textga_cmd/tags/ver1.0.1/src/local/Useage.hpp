/**
 * @file
 * @brief Useage関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {
    struct ArgumentData;
}

//------------------------------------------------------------
namespace local {

    /// Useage関数を提供するクラス。
    class Useage
    {
    public:
        /// Useageをコンソールに表示する。
        static void print( const ArgumentData& );
    };
}
//------------------------------------------------------------
// EOF
