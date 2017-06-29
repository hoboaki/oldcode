/**
 * @file
 * @brief Conatiner型を記述する。
 */
#ifndef ACS_INCLUDE_CONTAINER
#define ACS_INCLUDE_CONTAINER

//------------------------------------------------------------
#include <map>
#include <vector>
#include <acscript/stl_allocator.hpp>

//------------------------------------------------------------
namespace acscript {

    /// Mapコンテナのエイリアス。
    template< typename KeyType , typename ValueType >
    class Map
    {
    public:
        /// BuildTime用、Mapコンテナ。
        typedef std::map< 
            KeyType 
            , ValueType 
            , typename ::std::less< KeyType > 
            , typename ::acscript::STLAllocatorForBuildTime< typename std::pair< KeyType , ValueType > >::Allocator
            > BuildTimeType; 
    };

    /// Vectorコンテナのエイリアス。
    template< typename ValueType >
    class Vector
    {
    public:
        ///BuildTime用、Vectorコンテナ。
        typedef ::std::vector< 
            ValueType 
            , typename ::acscript::STLAllocatorForBuildTime< ValueType >::Allocator
            > BuildTimeType;
    };

}
//------------------------------------------------------------
#endif
// EOF
