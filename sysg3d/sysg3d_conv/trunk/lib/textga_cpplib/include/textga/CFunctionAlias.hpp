/**
 * @file
 * @brief C����̊֐��̃G�C���A�X���L�q����B
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
