/**
 * @file
 * @brief CommonHeaderWriter�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace app {
    class ByteVector;
}

//------------------------------------------------------------
namespace app {

    /// BinCommonHeader�̏������݂��x������N���X�B
    class CommonHeaderWriter
    {
    public:
        static void writeXMLBegin( ByteVector& );
        static void writeXMLEnd( ByteVector& );
        static void write( ByteVector& 
            , ::sysg3d::BinKind binKind
            );
    };

}
//------------------------------------------------------------
// EOF
