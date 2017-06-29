/** 
 * @file
 * @brief Angle.hpp ‚ÌÀ‘•‚ğ‹Lq‚·‚éB 
 */
#include <apcl/math/Angle.hpp>

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <apcl/math/PI.hpp>
#include <limits>

//-----------------------------------------------------------
using namespace ::apcl::math;

//-----------------------------------------------------------
void Angle::unitTest()
{
  Angle angle1( PI );
  Angle angle2 = Angle::createWithDegree( 180 );
  apclAssert( angle1 == angle2 );
  apclAssert( angle1 <= angle2 );
  apclAssert( angle1 >= angle2 );

  angle2.sub( Angle::createWithDegree(1) );
  apclAssert( angle1 > angle2 );
  apclAssert( angle1 >= angle2 );
  apclAssert( angle2 == Angle::createWithDegree( 179 ) );
  
  angle2.add( Angle::createWithDegree(2) );
  apclAssert( angle1 < angle2 );
  apclAssert( angle1 <= angle2 );
  apclAssert( angle2 == Angle::createWithDegree( 181 ) );

  angle2.setDegree( 180 );
  apclAssert( angle1 == angle2 );

  angle1.div( Angle(0.5) );
  angle2.mul( Angle(2) );
  apclAssert( angle1 == angle2 );

  angle2.setRadian( PI );
  apclAssert( !angle2.isNormalized() );
  angle2.normalize();
  apclAssert( angle2.isNormalized() );
}

//-----------------------------------------------------------
float Angle::degreeToRadian( const float aDegree )
{
	return PI * aDegree / 180.0f;
}

//-----------------------------------------------------------
float Angle::radianToDegree( const float aRadian )
{
	return 180.0f * aRadian / PI;
}

//-----------------------------------------------------------
Angle Angle::createWithDegree( const float aDegree )
{
  return Angle( Angle::degreeToRadian( aDegree ) );
}

//-----------------------------------------------------------
Angle::Angle():
radian_(0)
{
}

//-----------------------------------------------------------
Angle::Angle( const float aRadian ):
radian_( aRadian )
{
}

//-----------------------------------------------------------
Angle::~Angle()
{
}

//-----------------------------------------------------------
bool Angle::equals( const Angle& aAngle )const
{
  return radian_ == radian();
}

//-----------------------------------------------------------
int Angle::compare( const Angle& aAngle )const
{
  const float val = radian_ - aAngle.radian();
  if( val < 0 )
    return -1;
  else if ( val > 0 )
    return 1;
  else
    return 0;
}

//-----------------------------------------------------------
float Angle::radian()const
{
	return radian_;
}

//-----------------------------------------------------------
void Angle::setRadian( const float aRadian )
{
	radian_ = aRadian;
}

//-----------------------------------------------------------
float Angle::degree()const
{
	return radianToDegree( radian_ );
}

//-----------------------------------------------------------
void Angle::setDegree( const float aDegree )
{
	radian_ = degreeToRadian( aDegree );
}

//-----------------------------------------------------------
void Angle::normalize()
{
	const float PI2 = PI*2;
	while( radian_ >= PI )
		radian_ -= PI2;
	while( radian_ < -PI )
		radian_ += PI2;
}

//-----------------------------------------------------------
bool Angle::isNormalized()const
{
	return ( radian_ >= -PI && radian_ < PI );
}

//-----------------------------------------------------------
void Angle::add( const Angle& aAngle )
{
  radian_ += aAngle.radian();
}

//-----------------------------------------------------------
void Angle::sub( const Angle& aAngle )
{
  radian_ -= aAngle.radian();
}

//-----------------------------------------------------------
void Angle::mul( const Angle& aAngle )
{
  radian_ *= aAngle.radian();
}

//-----------------------------------------------------------
void Angle::div( const Angle& aAngle )
{
  apclAssert( aAngle.radian() != 0 );
  radian_ /= aAngle.radian();
}

//-----------------------------------------------------------
void Angle::mod( const Angle& aAngle )
{
  apclAssert( aAngle.radian() != 0 );
  const float radian2 = aAngle.radian();
  if ( radian_ > radian2 )
  {
    float rate = 1;
    if ( radian_ > 0 )
    {// Positive
      const int max = std::numeric_limits< int >::max();
      const int threshold = max * 2;

      int val = static_cast< int >( radian_ * rate );
      while( val < threshold )
      {
        rate *= 2;
        val = static_cast< int >( radian_ * rate );
      }
    }
    else
    {// 
      const int min = std::numeric_limits< int >::min();
      const int threshold = min * 2;

      int val = static_cast< int >( radian_ * rate );
      while( val > threshold )
      {
        rate *= 2;
        val = static_cast< int >( radian_ * rate );
      }
    }
    int rad1 = static_cast< int >( radian_ * rate );
    int rad2 = static_cast< int >( radian2 * rate );
    radian_ = ( rad1 % rad2 ) / rate;
  }
}

//-----------------------------------------------------------
// EOF
