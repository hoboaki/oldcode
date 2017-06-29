/**
 * @file
 * @brief メイン関数を記述する。
 */

//------------------------------------------------------------
#include <sysg3d/SysG3d.hpp>
#include <windows.h>
#include "app/Argument.hpp"
#include "app/sysg3d_texconv/EntryPoint.hpp"

//------------------------------------------------------------
int main(int argc, char* argv[])
{
    return ::app::sysg3d_texconv::EntryPoint::run( ::app::Argument(
        argc , argv
        ) );
}

//------------------------------------------------------------
// EOF
