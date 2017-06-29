/** 
 * @file
 * @brief Vector3.hpp ÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <apcl/math/Vector3.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <cmath>
#include <limits>

//-----------------------------------------------------------
using namespace ::apcl::math;
using namespace ::std;

//-----------------------------------------------------------
namespace
{
  bool epsilonEqual( const float aVal1 , const float aVal2 )
  {
    const float epsilon = std::numeric_limits< float >::epsilon();
    const float absVal =  aVal1 > aVal2 ? aVal1-aVal2 : aVal2-aVal1;
    return absVal < epsilon;
  }
}

//-----------------------------------------------------------
void Vector3::unitTest()
{
  const Vector3 vec1(1,2,3);
  Vector3 vec2( vec1 * Vector3(2,2,2) );
  vec2.div( 2.0f );
  apclAssert( vec1 == vec2 );
  apclAssert( vec1.distance( vec2 ) == 0 );

  vec2 += vec1;
  apclAssert( vec2 == vec1+vec1 );
  vec2 -= vec1;

  Vector3 vec3( vec1 - vec2 );
  apclAssert( vec3.length() == 0 );

  vec3 = vec1;
  apclAssert( !vec3.isNormalized() );
  vec3.normalize();
  apclAssertMsg( vec3.isNormalized() , 
    "Vec:(%f,%f,%f) Length:%f\n" , vec3.x(),vec3.y(),vec3.z(),vec3.length() );

  apclAssert( vec3.innner( vec1 ) > 0 );
  apclAssert( Vector3(1,1,1).innner( Vector3(0,0,0) ) == 0 );

  apclAssert( Vector3(1,0,0).getCross( Vector3(0,1,0) ) == Vector3(0,0,1) );


}

//-----------------------------------------------------------
Vector3::Vector3():
x_(0),
y_(0),
z_(0)
{
}

//-----------------------------------------------------------
Vector3::Vector3( const float aX , const float aY , const float aZ ):
x_( aX ),
y_( aY ),
z_( aZ )
{
}

//-----------------------------------------------------------
Vector3::~Vector3()
{
}

//-----------------------------------------------------------
float Vector3::x()const
{
  return x_;
}

//-----------------------------------------------------------
float Vector3::y()const
{
  return y_;
}

//-----------------------------------------------------------
float Vector3::z()const
{
  return z_;
}

//-----------------------------------------------------------
void Vector3::setX( const float x )
{
  x_ = x;
}

//-----------------------------------------------------------
void Vector3::setY( const float y )
{
  y_ = y;
}

//-----------------------------------------------------------
void Vector3::setZ( const float z )
{
  z_ = z;
}

//-----------------------------------------------------------
void Vector3::add( const float aVal )
{
	x_ += aVal;
	y_ += aVal;
	z_ += aVal;
}

//-----------------------------------------------------------
void Vector3::add( const Vector3& aVec )
{
	x_ += aVec.x();
	y_ += aVec.y();
	z_ += aVec.z();
}
//-----------------------------------------------------------
void Vector3::sub( const float aVal )
{
	x_ -= aVal;
	y_ -= aVal;
	z_ -= aVal;
}

//-----------------------------------------------------------
void Vector3::sub( const Vector3& aVec )
{
	x_ -= aVec.x();
	y_ -= aVec.y();
	z_ -= aVec.z();
}

//-----------------------------------------------------------
void Vector3::mul( const float aVal )
{
	x_ *= aVal;
	y_ *= aVal;
	z_ *= aVal;
}

//-----------------------------------------------------------
void Vector3::mul( const Vector3& aVec )
{
	x_ *= aVec.x();
	y_ *= aVec.y();
	z_ *= aVec.z();
}

//-----------------------------------------------------------
void Vector3::div( const float aVal )
{
	assert( aVal != 0 );
	x_ /= aVal;
	y_ /= aVal;
	z_ /= aVal;
}

//-----------------------------------------------------------
void Vector3::div( const Vector3& aVec )
{
	assert( aVec.x() != 0 );
	assert( aVec.y() != 0 );
	assert( aVec.z() != 0 );
	x_ /= aVec.x();
	y_ /= aVec.y();
	z_ /= aVec.z();
}

//-----------------------------------------------------------
bool Vector3::equals( const Vector3& aVec )const
{
	return ( 
    epsilonEqual( x_ , aVec.x() ) && 
    epsilonEqual( y_ , aVec.y() ) && 
    epsilonEqual( z_ , aVec.z() )
    );
}

//-----------------------------------------------------------
float Vector3::length()const
{
	return sqrt( x_*x_ + y_*y_ + z_*z_ );
}

//-----------------------------------------------------------
float Vector3::distance( const Vector3& aVec )const
{
	const float subx = x_ - aVec.x();
	const float suby = y_ - aVec.y();
	const float subz = z_ - aVec.z();
	return sqrt( subx*subx + suby*suby + subz*subz );
}

//-----------------------------------------------------------
void Vector3::normalize()
{
	const float tmplength = length();
	assert( tmplength != 0 );
	div( tmplength );
}

//-----------------------------------------------------------
bool Vector3::isNormalized()const
{
	return epsilonEqual( length() , 1.0f );
}

//-----------------------------------------------------------
float Vector3::innner( const Vector3& aVec )const
{
	return x_ * aVec.x() + y_ * aVec.y() + z_ * aVec.z();
}

//-----------------------------------------------------------
void Vector3::cross( const Vector3& aVec )
{
	const float tmpx = y_*aVec.z() - z_*aVec.y();
	const float tmpy = z_*aVec.x() - x_*aVec.z();
	const float tmpz = x_*aVec.y() - y_*aVec.x();
	x_ = tmpx;
	y_ = tmpy;
	z_ = tmpz;
}

//-----------------------------------------------------------
Vector3 Vector3::getCross( const Vector3& aVec )const
{
	const float x = y_*aVec.z() - z_*aVec.y();
	const float y = z_*aVec.x() - x_*aVec.z();
	const float z = x_*aVec.y() - y_*aVec.x();
	return Vector3(x,y,z);
}

//-----------------------------------------------------------
// EOF
