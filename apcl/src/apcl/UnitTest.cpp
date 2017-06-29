/** 
 * @file 
 * ユニットテストを記述する。
 */
#include <apcl/UnitTest.hpp>

//-----------------------------------------------------------
#include <apcl.h>
#include <apcl.h> ///< インクルードガードテスト 
#include <cstdio>
#include <iostream>

//-----------------------------------------------------------
#define toStr( testType ) #testType

#define test( testType ) \
  testType::unitTest(); \
  std::cout << "Test is Success. " << "[" toStr(testType) << "]" << std::endl;

//-----------------------------------------------------------
void ::apcl::unitTest()
{
  std::cout << "<<APCL Unit Test Start>>" << std::endl;

  // ::apcl::container
  test( ::apcl::container::Ring<int> );

  // ::apcl::debug
  apclAssert( 1 < 3 );

  // ::apcl::file
  test( ::apcl::file::File );
  test( ::apcl::file::FileLoader );
  test( ::apcl::file::FilePath );
  test( ::apcl::file::FileSaver );

  // ::apcl::interface
  test( ::apcl::interface::Singleton<int> );

  // ::apcl::math
  test( ::apcl::math::Angle );
  test( ::apcl::math::Vector3 );

  // ::apcl::util
  test( ::apcl::util::Data );

  /*{// File Test
    ::apcl::file::File file;
    file.setPath( ::apcl::file::FilePath("unittest.txt") );
    file.setData( ::apcl::util::Data( "HelloWorld" , 10 ) );
    file.setType( ::apcl::file::FileType_Ascii );
    ::apcl::file::FileSaver saver( file );
    apclAssert( saver.save() );

    ::apcl::file::FileLoader loader( file.path() , file.type() );
    apclAssert( loader.load() );
    apclAssert( file.data() == loader.data() );
  }*/
  /*{// Equalable Test
    class A : public ::apcl::interface::Equalable< A >
    {
    public:
      bool equals( const A& a )const
      {
        return x == a.x; 
      }
      float x;
    };
    class B : public ::apcl::interface::Equalable< A >
    {
    public:
      float x;
      bool equals( const A& a )const
      {
        return x == a.x;
      }
    };
    A a1,a2;
    B b;
    b == a1;
    a1 == a2;
  }*/
  /*{// NonCopyable Test
    class A : private ::apcl::interface::NonCopyable
    {
      float x;
    };
    A a;
    A b;
    a = b;
  }*/
  /*{// Singleton Test
    class A : public ::apcl::interface::Singleton< A >
    {
    public:
      A(){}
    };
    A a;
  }*/

  std::cout << "<<APCL Unit Test End>>" << std::endl;
}

//-----------------------------------------------------------
// EOF
