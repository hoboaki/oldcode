/**
 * @file
 * @brief Allocator�^���L�q����B
 */
#ifndef ACS_INCLUDE_ALLOCATOR
#define ACS_INCLUDE_ALLOCATOR

//------------------------------------------------------------
namespace acscript {

    /// Allocator�B
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
