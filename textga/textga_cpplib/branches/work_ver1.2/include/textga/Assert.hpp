/**
 * @file
 * @brief Assert���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <cassert>
//------------------------------------------------------------

#if defined(TEXTGA_USE_ASSERT)
#define TEXTGA_ASSERT( exp ) assert( exp )

#else
#define TEXTGA_ASSERT( exp ) do{}while(0)

#endif

//------------------------------------------------------------
// EOF
