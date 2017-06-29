/**
 * @file
 * @brief CommonHeaderWriter型を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace app {
    class ByteVector;
}

//------------------------------------------------------------
namespace app {

    /// BinCommonHeaderの書き込みを支援するクラス。
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
