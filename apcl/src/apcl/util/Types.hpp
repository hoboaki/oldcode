/** 
 * @file
 * @brief Œ^‚Ì—ªÌ‚ğ’è‹`‚·‚éB
 */
#pragma once

//-----------------------------------------------------------
namespace apcl { namespace util 
{
	//=================================================
	/// @name •„†–³‚µŒ^
  //@{
  typedef unsigned char           uint8;
  typedef unsigned short int      uint16;
  typedef unsigned int            uint32;
  typedef unsigned long long int  uint64;
	typedef unsigned char           u8;
	typedef unsigned short int      u16;
	typedef unsigned int            u32;
	typedef unsigned long long int  u64;
	//@}

	//=================================================
	/// @name •„†—L‚èŒ^
	//@{
	typedef signed char             int8;
	typedef signed short int        int16;
	typedef signed int              int32;
	typedef signed long long int    int64;
	typedef signed char             s8;
	typedef signed short int        s16;
	typedef signed int              s32;
	typedef signed long long int    s64;
	//@}

	//=================================================
	/// @name •‚“®¬”Œ^
	//@{
	typedef float                   float32;
	typedef double                  float64;
	typedef float                   f32;
	typedef double                  f64;
	//@}

	//=================================================
	/// @name Šö”­«•„†–³‚µŒ^
	//@{
	typedef volatile uint8          vuint8;
	typedef volatile uint16         vuint16;
	typedef volatile uint32         vuint32;
	typedef volatile uint64         vuint64;
	typedef volatile u8             vu8;
	typedef volatile u16            vu16;
	typedef volatile u32            vu32;
	typedef volatile u64            vu64;
	//@}

	//=================================================
	/// @name Šö”­«•„†—L‚èŒ^
	//@{
  typedef volatile int8           vint8;
  typedef volatile int16          vint16;
  typedef volatile int32          vint32;
  typedef volatile int64          vint64;
  typedef volatile s8             vs8;
  typedef volatile s16            vs16;
  typedef volatile s32            vs32;
  typedef volatile s64            vs64;
  //@}

	//=================================================
	/// @name Šö”­«•‚“®¬”Œ^
	//@{
	typedef volatile float32        vfloat32;
	typedef volatile float64        vfloat64;
	typedef volatile f32			      vf32;
	typedef volatile f64			      vf64;
	//@}

	//=================================================
	/// @name ‚»‚Ì‘¼
	//@{
	typedef uint8                   byte;
	typedef int32                   fixed;
	typedef int64                   dfixed;
	typedef volatile int32          vfixed;
	//@}

}} // end of namespace ::apcl::util

//-----------------------------------------------------------
// EOF
