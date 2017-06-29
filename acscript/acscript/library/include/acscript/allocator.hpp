/**
 * @file
 * @brief Allocator型を記述する。
 */
#ifndef ACS_INCLUDE_ALLOCATOR
#define ACS_INCLUDE_ALLOCATOR

//------------------------------------------------------------
namespace acscript {

    /// Allocator。
    class Allocator
    {
    public:
        typedef void* (*AllocFuncType)( ::acscript::U32 ) ;
        typedef void (*FreeFuncType)( void* ) ;

        Allocator();
        Allocator( const AllocFuncType& , const FreeFuncType& );
        
        void* alloc( U32 size );
        void free( void* ptr );

    private:
        AllocFuncType allocFunc; ///< Alloc function pointer.
        FreeFuncType  freeFunc;  ///< Free function pointer.
    };

}
//------------------------------------------------------------
#endif
// EOF
