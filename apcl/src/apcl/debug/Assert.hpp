/** 
 * @file 
 * @brief assertを記述する。
 */
#pragma once

//-----------------------------------------------------------
#include <cassert>
#include <cstdio>

//-----------------------------------------------------------

/**
 * Releaseモードでも機能するメッセージ付きアサート。
 * <pre>
 * ・Releaseモード時は表示はするがフリーズはしない。
 * ・メッセージはprintfの書式。
 * </pre>
 */
#define apclAssertRMsg( cmd , ... ) \
  if( !(cmd) ) \
  { \
    ::std::printf( "############################################################\n" ); \
    ::std::printf( "[APCL Assertion Failed]\nFile:%s\nLine:%d\n" , __FILE__ , __LINE__ ); \
    ::std::printf( "Command:" ); \
    ::std::printf( #cmd ); \
    ::std::printf( "\n" ); \
    ::std::printf( "Message:" ); \
    ::std::printf( __VA_ARGS__ ); \
    ::std::printf( "\n" ); \
    ::std::printf( "############################################################\n" ); \
    assert( cmd ); \
  }

/// Releaseモードでも機能するアサート。Releaseモード時は表示はするがフリーズはしない。
#define apclAssertR( cmd ) \
  apclAssertRMsg( cmd , "None" )

/// デバッグモード専用メッセージ付きアサート。メッセージはprintfの書式。
//@{
#ifdef _DEBUG
#define apclAssertMsg( cmd , ... ) \
  if( !(cmd) ) \
  { \
    ::std::printf( "############################################################\n" ); \
    ::std::printf( "[APCL Assertion Failed]\nFile:%s\nLine:%d\n" , __FILE__ , __LINE__ ); \
    ::std::printf( "Command:" ); \
    ::std::printf( #cmd ); \
    ::std::printf( "\n" ); \
    ::std::printf( "Message:" ); \
    ::std::printf( __VA_ARGS__ ); \
    ::std::printf( "\n" ); \
    ::std::printf( "############################################################\n" ); \
    assert( cmd ); \
  }

#else
#define apclAssertMsg( cmd , ... ) ((void)0)

#endif // _DEBUG
//@}

/// デバッグモード専用アサート。
//@{
#ifdef _DEBUG
#define apclAssert( cmd ) \
  apclAssertMsg( cmd , "None" )

#else // _DEBUG
#define apclAssert( cmd ) ((void)0)

#endif // _DEBUG
//@}


//-----------------------------------------------------------
// EOF
