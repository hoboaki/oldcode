/**
 * @file
 * @brief マクロを定義する。
 */
#ifndef ACS_INCLUDE_MACRO
#define ACS_INCLUDE_MACRO

//------------------------------------------------------------
#include <assert.h>

//------------------------------------------------------------

// BigEndian環境ならこれが定義される。
// #define ACS_IS_BIG_ENDIAN

// LittleEndian環境ならこれが定義される。
#define ACS_IS_LITTLE_ENDIAN

/// アサート。
#define ACS_ASSERT assert

//------------------------------------------------------------
#endif
// EOF
