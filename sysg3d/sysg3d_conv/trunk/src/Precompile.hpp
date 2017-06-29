/**
 * @file
 * @brief プリコンパイルヘッダ。
 */
#pragma once

//------------------------------------------------------------
#include <assert.h>
#include <boost/static_assert.hpp>
#include <cstdio>
#include <dae.h>
#include <dom/domCOLLADA.h>
#include <sysg3d/SysG3D.hpp>
#include "Types.hpp"

/// アサート。
#define PJ_ASSERT assert

/// 標準出力。
#define PJ_COUT std::printf

/// コンパイル時のアサート。
#define PJ_COMPILE_ASSERT( cmd ) BOOST_STATIC_ASSERT( cmd )

/// 配列の長さを取得するマクロ。
#define PJ_ARRAY_LENGTH( obj ) ( sizeof(obj)/sizeof(obj[0]) )

/// 指定の配列の長さが、指定の長さかチェックする。
#define PJ_ARRAY_LENGTH_CHECK( arr , len ) PJ_COMPILE_ASSERT( PJ_ARRAY_LENGTH( arr ) == len )

//------------------------------------------------------------
// EOF
