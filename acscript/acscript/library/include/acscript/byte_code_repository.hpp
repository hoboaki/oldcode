/**
 * @file
 * @brief ObjectCodeRepository型を記述する。
 */
#ifndef ACS_INCLUDE_BYTECODEREPOSITORY
#define ACS_INCLUDE_BYTECODEREPOSITORY

//------------------------------------------------------------
#include <acscript/container.hpp>
#include <acscript/macro.hpp>
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {
    struct ByteCode;
}

//------------------------------------------------------------
namespace acscript {

    /// バイトコードのまとまり。
    class ByteCodeRepository
    {
    public:

        /// 指定のByteCodeは存在するか。
        bool isExist( const ByteCodeId id )const
        {
            return mByteCodes.find( id ) != mByteCodes.end();
        }

        /// 指定のByteCodeを取得する。存在しなければ0を返す。
        ByteCode* ptr( const ByteCodeId id )const
        {
            const MapType::const_iterator itr = mByteCodes.find( id );
            if ( itr == mByteCodes.end() )
            {
                return 0;
            }
            else
            {
                return itr->second;
            }
        }

    private:
        typedef ::acscript::Map< ByteCodeId , ByteCode* >::BuildTimeType MapType;
        /// 全てのバイトコード。
        MapType mByteCodes;
    };
    
}
//------------------------------------------------------------
#endif
// EOF
