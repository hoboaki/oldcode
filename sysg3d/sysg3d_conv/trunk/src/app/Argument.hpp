/**
 * @file
 * @brief Argument型を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace app {

    /// 引数をまとめるクラス。
    class Argument
    {
    public:
        Argument( U32 argc , char* argv[] );

        U32 count()const;
        ConstStr argAtIndex( U32 index )const;

        void dump(); ///< ダンプ。

    private:
        U32 argc_;
        char** argv_;
    };

}
//------------------------------------------------------------
// EOF
