/**
 * @file
 * @brief Conatiner�^���L�q����B
 */
#ifndef ACS_INCLUDE_CONTAINER
#define ACS_INCLUDE_CONTAINER

//------------------------------------------------------------
#include <map>
#include <vector>
#include <acscript/stl_allocator.hpp>

//------------------------------------------------------------
namespace acscript {

    /// Map�R���e�i�̃G�C���A�X�B
    template< typename KeyType , typename ValueType >
    class Map
    {
    public:
        /// BuildTime�p�AMap�R���e�i�B
        typedef std::map< 
            KeyType 
            , ValueType 
            , typename ::std::less< KeyType > 
            , typename ::acscript::STLAllocatorForBuildTime< typename std::pair< KeyType , ValueType > >::Allocator
            > BuildTimeType; 
    };

    /// Vector�R���e�i�̃G�C���A�X�B
    template< typename ValueType >
    class Vector
    {
    public:
        ///BuildTime�p�AVector�R���e�i�B
        typedef ::std::vector< 
            ValueType 
            , typename ::acscript::STLAllocatorForBuildTime< ValueType >::Allocator
            > BuildTimeType;
    };

}
//------------------------------------------------------------
#endif
// EOF
