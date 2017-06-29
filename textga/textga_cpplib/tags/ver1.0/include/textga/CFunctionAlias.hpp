/**
 * @file
 * @brief C言語の関数のエイリアスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Compiler.hpp>

//------------------------------------------------------------

// sprintf
#if defined(TEXTGA_COMPILER_MSC)
#define TEXTGA_C_SPRINTF sprintf_s
#else
#define TEXTGA_C_SPRINTF std::sprintf
#endif

//------------------------------------------------------------
// EOF
