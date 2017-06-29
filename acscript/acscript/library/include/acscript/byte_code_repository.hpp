/**
 * @file
 * @brief ObjectCodeRepository�^���L�q����B
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

    /// �o�C�g�R�[�h�̂܂Ƃ܂�B
    class ByteCodeRepository
    {
    public:

        /// �w���ByteCode�͑��݂��邩�B
        bool isExist( const ByteCodeId id )const
        {
            return mByteCodes.find( id ) != mByteCodes.end();
        }

        /// �w���ByteCode���擾����B���݂��Ȃ����0��Ԃ��B
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
        /// �S�Ẵo�C�g�R�[�h�B
        MapType mByteCodes;
    };
    
}
//------------------------------------------------------------
#endif
// EOF
