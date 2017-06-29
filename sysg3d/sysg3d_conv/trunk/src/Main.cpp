/**
 * @file
 * @brief メイン関数を記述する。
 */

//------------------------------------------------------------
#include <sysg3d/SysG3d.hpp>
#include <windows.h>
#include "app/Argument.hpp"
#include "app/sysg3d_conv/EntryPoint.hpp"

//------------------------------------------------------------
int main(int argc, char* argv[])
{
    return ::app::sysg3d_conv::EntryPoint::run( ::app::Argument(
        argc , argv
        ) );
}

//------------------------------------------------------------
// EOF
