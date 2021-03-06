/**
 * @file
 * @brief Assertを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <cassert>
//------------------------------------------------------------

#if defined(USE_ASSERT)
#define PJ_ASSERT( exp ) assert( exp )

#else
#define PJ_ASSERT( exp ) do{}while(0)

#endif

//------------------------------------------------------------
// EOF
