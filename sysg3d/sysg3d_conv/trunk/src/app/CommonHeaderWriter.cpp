/** 
 * @file
 * @brief CommonHeaderWriter.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "app/CommonHeaderWriter.hpp"

//------------------------------------------------------------
#include "app/ByteVector.hpp"

//------------------------------------------------------------
namespace
{
    const char* T_BINKIND_NAMES[]=
    {
        "Unknown"
        , "Mesh"
        , "Texture"
    };
    PJ_ARRAY_LENGTH_CHECK( T_BINKIND_NAMES , ::sysg3d::BinKind_Terminate );
}

//------------------------------------------------------------
namespace app {
//------------------------------------------------------------
void CommonHeaderWriter::writeXMLBegin( 
    ByteVector& aByteVector
    )
{
    aByteVector.print( "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" );
    aByteVector.print( "<xdata_root \n");
    aByteVector.print( "  xmlns=\"http://10106.net/xdata\"\n" );
    aByteVector.print( "  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n" );
    aByteVector.print( "  xsi:schemaLocation=\"http://10106.net/xdata\"\n" );
    aByteVector.print( "  major_version=\"1\"\n");
    aByteVector.print( "  minor_version=\"0\"\n");
    aByteVector.print( "  >\n");
    aByteVector.indentEnter();
}

//------------------------------------------------------------
void CommonHeaderWriter::writeXMLEnd( 
    ByteVector& aByteVector
    )
{
    aByteVector.indentReturn();
    aByteVector.print( "</xdata_root>\n");
}

//------------------------------------------------------------
void CommonHeaderWriter::write( 
    ByteVector& aByteVector
    , const ::sysg3d::BinKind aBinKind
    )
{
    aByteVector.printCurrentIndent();
    aByteVector.printComment( "begin sysg3d::BinCommonHeader" );
    aByteVector.printLineEnter();
    {
        aByteVector.indentEnter();

        aByteVector.printCurrentIndent();
        aByteVector.printComment( "signature(\"SGBN\")" );
        {
            const U8 VALUE[]={'S','G','B','N'};
            aByteVector.printTagU8Array( VALUE , 4 );
        }
        aByteVector.printLineEnter();

        aByteVector.printCurrentIndent();
        aByteVector.printComment( "version(%u.%u)" , ::sysg3d::BinConstant::VERSION_MAJOR , ::sysg3d::BinConstant::VERSION_MINOR );
        aByteVector.printTagU16( ::sysg3d::BinConstant::VERSION  );
        aByteVector.printLineEnter();

        aByteVector.printCurrentIndent();
        aByteVector.printComment( "kind(%s)" , T_BINKIND_NAMES[aBinKind] );
        aByteVector.printTagU16( aBinKind );
        aByteVector.printLineEnter();
        
        aByteVector.indentReturn();
    }
    aByteVector.printCurrentIndent();
    aByteVector.printComment( "end sysg3d::BinCommonHeader" );
    aByteVector.printLineEnter();
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
