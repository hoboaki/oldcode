/** 
 * @file 
 * @brief assert���L�q����B
 */
#pragma once

//-----------------------------------------------------------
#include <cassert>
#include <cstdio>

//-----------------------------------------------------------

/**
 * Release���[�h�ł��@�\���郁�b�Z�[�W�t���A�T�[�g�B
 * <pre>
 * �ERelease���[�h���͕\���͂��邪�t���[�Y�͂��Ȃ��B
 * �E���b�Z�[�W��printf�̏����B
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

/// Release���[�h�ł��@�\����A�T�[�g�BRelease���[�h���͕\���͂��邪�t���[�Y�͂��Ȃ��B
#define apclAssertR( cmd ) \
  apclAssertRMsg( cmd , "None" )

/// �f�o�b�O���[�h��p���b�Z�[�W�t���A�T�[�g�B���b�Z�[�W��printf�̏����B
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

/// �f�o�b�O���[�h��p�A�T�[�g�B
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
