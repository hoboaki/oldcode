/** 
 * @file
 * @brief MeshConv.hppの実装を記述する。
 */
#include "app/sysg3d_conv/MeshConv.hpp"

//------------------------------------------------------------
#include <boost/array.hpp>
#include <memory>
#include "app/Argument.hpp"
#include "app/BinaryData.hpp"
#include "app/ByteVector.hpp"
#include "app/CommonHeaderWriter.hpp"
#include "app/sysg3d_conv/ConvertRecipe.hpp"

//------------------------------------------------------------
#define ARRAY_BINSIZE( arr ) U32( arr.size() * sizeof( arr[0] ) )

namespace
{
    /// 要素の列挙型。
    enum ElementKind
    {
        ElementKind_Pos
        , ElementKind_Normal
        , ElementKind_Color
        // term
        , ElementKind_Terminate
        , ElementKind_Begin = 0
        , ElementKind_End = ElementKind_Terminate
        , ElementKind_Num = ElementKind_End - ElementKind_Begin
    };

    /// オフセット。
    class Offset
    {
    public:
        static const domUint INVALID_OFFSET = 0xFFFFFFFF;
        Offset()
            : val_( INVALID_OFFSET )
        {
        }
        Offset( const domUint aVal )
            : val_( aVal )
        {
        }

        bool isValid()const
        {
            return val_ != INVALID_OFFSET;
        }

        domUint value()const
        {
            PJ_ASSERT( isValid() );
            return val_;
        }

    private:
        domUint val_;
    };

    /// オフセットのセット。
    class OffsetSet
    {
    public:
        OffsetSet()
            : array()
        {
        }

        ::boost::array< Offset , ElementKind_Num > array;
    };

    /// オフセットのセットを返す。
    const OffsetSet t_getOffsetSet( const domInputLocalOffset_Array& aInputArray )
    {
        const char* NAME_ARRAY[]=
        {
            "VERTEX" // Pos
            , "NORMAL" // Normal
            , "COLOR" // Color
        };
        PJ_ARRAY_LENGTH_CHECK( NAME_ARRAY , ElementKind_Num );

        OffsetSet offsetSet;
        for ( U32 i = 0; i < aInputArray.getCount(); ++i )
        {
            const std::string inputName( aInputArray[i]->getSemantic() );
            const Offset offset( aInputArray[i]->getOffset() );
            for ( U32 e = 0; e < ElementKind_Num; ++e )
            {
                if ( inputName == NAME_ARRAY[e] )
                {
                    offsetSet.array[ e ] = offset;
                }
            }
        }
        if ( !offsetSet.array[ ElementKind_Normal ].isValid() )
        {// Normalの指定がなければVertexと同じとする
            PJ_ASSERT( offsetSet.array[ ElementKind_Pos ].isValid() );
            offsetSet.array[ ElementKind_Normal ] = offsetSet.array[ ElementKind_Pos ];
        }
        return offsetSet;
    }

    // オフセットの最大値+1を返す
    domUint t_getMaxOffset( const domInputLocalOffset_Array &input_array ) 
    {
        PJ_ASSERT( input_array.getCount() != 0 );
        domUint maxOffset = 0;
        for ( U32 i = 0; i < input_array.getCount(); i++ ) 
        {
            if ( maxOffset < input_array[i]->getOffset() ) 
            {
                maxOffset = input_array[i]->getOffset();
            }
        }
        return maxOffset+1;
    }
    
    /// １つのインデックスセット。
    class OneIndexSet
    {
    public:
        static const domUint INVALID_INDEX = 0xFFFFFFFF;
        OneIndexSet()
        {
            for ( U32 i = 0; i < ElementKind_Num; ++i )
            {
                array[i] = INVALID_INDEX;
            }
        }

        ::boost::array< domUint , ElementKind_Num > array;

        bool equals( const OneIndexSet& aRHS )const
        {
            for ( U32 i = 0; i < ElementKind_Num; ++i )
            {
                if ( array[i] != aRHS.array[i] )
                {
                    return false;
                }
            }
            return true;
        }

        bool operator ==(const OneIndexSet& aRHS)const
        {
            return equals( aRHS );
        }

        bool operator !=(const OneIndexSet& aRHS)const
        {
            return !equals( aRHS );
        }
    };
    typedef std::vector< OneIndexSet > OneIndexSetArray;

    /// Arrayのindex。
    class ArrayIndex
    {
    public:
        static const U32 INVALID_INDEX = 0xFFFFFFFF;
        ArrayIndex()
            : idx_( INVALID_INDEX )
        {
        }
        ArrayIndex( const U32 aVal )
            : idx_( aVal )
        {
        }

        bool isValid()const
        {
            return idx_ != INVALID_INDEX;
        }

        U32 value()const
        {
            PJ_ASSERT( isValid() );
            return idx_;
        }

    private:
        U32 idx_;
    };

    /// ArrayのIndexセット。
    class ArrayIndexSet
    {
    public:
        ArrayIndexSet()
            : array()
        {
        }

        ::boost::array< ArrayIndex , ElementKind_Num > array;
    };

    /// SourceArrayのArrayIndexSetを取得する。
    const ArrayIndexSet t_getSourceArrayIndexSet(
        const domMesh& aMesh
        )
    {
        const char* NAME_ARRAY[]=
        {
            "position" // Pos
            , "normal" // Normal
            , "colorSet1" // Color
        };
        PJ_ARRAY_LENGTH_CHECK( NAME_ARRAY , ElementKind_Num );

        ArrayIndexSet indexSet;
        for ( U32 i = 0; i < aMesh.getSource_array().getCount(); ++i )
        {
            const std::string name = aMesh.getSource_array().get(i)->getName();
            const ArrayIndex arrIdx(i);
            for ( U32 e = 0; e < ElementKind_Num; ++e )
            {
                if ( name == NAME_ARRAY[e] )
                {
                    indexSet.array[ e ] = arrIdx;
                }
            }
        }
        return indexSet;
    }

    typedef std::vector<U32> U32Vector;
    typedef std::vector<F32> F32Vector;

    /// 変換されたメッシュのデータ。
    class MeshData
    {
    public:
        MeshData()
            : meshKind()
            , name()
            , indexArrays()
            , posArray()
            , normalArray()
            , colorArray()
        {
        }

        ::sysg3d::BinMeshKind meshKind;
        std::string name;
        std::vector< U32Vector > indexArrays;
        F32Vector posArray;
        F32Vector normalArray;
        F32Vector colorArray;
    };
    
    /// 要素の追加。
    void t_addElements( 
        F32Vector& aVecRef 
        , const domMesh& aMesh
        , const ArrayIndex& aSrcArrayIdx
        , const U32 aOffsetPos
        , const U32 aOneElementCount
        )
    {                
        PJ_ASSERT( aMesh.getSource_array()[ aSrcArrayIdx.value() ]->getTechnique_common()->getAccessor()->getStride() == aOneElementCount );
        const domFloat_arrayRef floatArray = aMesh.getSource_array()[ aSrcArrayIdx.value() ]->getFloat_array();
        const U32 baseIndex = U32( aOneElementCount * aOffsetPos );
        for ( U32 i = 0; i < aOneElementCount; ++i )
        {
            aVecRef.push_back(
                F32( floatArray->getValue().get( baseIndex + i ) )
                );
        }
    }

    /// index配列以外の配列を構築。
    void t_buildExtraArrays(
        MeshData& aMeshData
        , const ::domMesh& aMesh
        , const OffsetSet& aOffsetSet
        , const OneIndexSetArray& aOneIndexSetArray
        )
    {
        struct OneElementRecipe
        {
            F32Vector* arrayPtr;
            U32 elementCount;
        };
        const OneElementRecipe storeArrays[] =
        {
            {&aMeshData.posArray , 3} // pos(xyz)
            ,{&aMeshData.normalArray , 3} // normal(xyz)
            ,{&aMeshData.colorArray , 4} //color(rgba)
        };
        PJ_ARRAY_LENGTH_CHECK( storeArrays , ElementKind_Num );

        // reserve
        for ( U32 i = 0; i < ElementKind_Num; ++i )
        {
            storeArrays[i].arrayPtr->reserve(
                storeArrays[i].elementCount * aOneIndexSetArray.size()
                );
        }

        // store
        const ArrayIndexSet srcArrayIndexSet( t_getSourceArrayIndexSet( aMesh ) );
        for ( U32 oneIdx = 0; oneIdx < aOneIndexSetArray.size(); ++oneIdx )
        {
            const OneIndexSet& oneIndexSet = aOneIndexSetArray[oneIdx];
            for ( U32 e = 0; e < ElementKind_Num; ++e )
            {
                if ( !aOffsetSet.array[ e ].isValid() )
                {// 無効ならパス
                    continue;
                }
                t_addElements(
                    *storeArrays[e].arrayPtr
                    , aMesh
                    , srcArrayIndexSet.array[ e ]
                    , U32( oneIndexSet.array[ e ] )
                    , storeArrays[e].elementCount
                    );
            }
        }
    }

    /// MeshKind名。
    const char* T_MESHKIND_NAMES[]=
    {
        "Triangles"
        ,"Polylist"
    };
    PJ_ARRAY_LENGTH_CHECK( T_MESHKIND_NAMES , ::sysg3d::BinMeshKind_Terminate );

    /// ByteVectorに書き出す。
    void t_printToByteVector( 
        ::app::ByteVector& bv
        , const MeshData& aMeshData 
        )
    {       // 共通ヘッダ
        ::app::CommonHeaderWriter::writeXMLBegin( bv );
        ::app::CommonHeaderWriter::write(
            bv
            , ::sysg3d::BinKind_Mesh
            );

        const char* LABEL_NAME = "mesh_name";
        const char* LABEL_COUNT = "mesh_counts";
        const char* LABEL_INDEXBDO = "mesh_index_bdo_array";
        const char* LABEL_INDEX = "mesh_index";
        const char* LABEL_POS = "mesh_pos";
        const char* LABEL_NORMAL = "mesh_normal";
        const char* LABEL_COLOR = "mesh_color";

        // Meshヘッダ
        bv.printCurrentIndent();
        bv.printComment( "begin sysg3d::BinMeshHeader" );
        bv.printLineEnter();
        {
            bv.indentEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoName" );
            bv.printTagReference( LABEL_NAME );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "meshKind(%s)" , T_MESHKIND_NAMES[ aMeshData.meshKind ] );
            bv.printTagU32( aMeshData.meshKind );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "primitiveCount" );
            bv.printTagU32( U32(aMeshData.indexArrays.size()) );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoCount" );
            bv.printTagReference( LABEL_COUNT );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoIndex" );
            bv.printTagReference( LABEL_INDEXBDO );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoPos" );
            bv.printTagReference( LABEL_POS );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoNormal" );
            bv.printTagReference( LABEL_NORMAL );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoColor" );
            bv.printTagReference( LABEL_COLOR );
            bv.printLineEnter();

            bv.indentReturn();
        }
        bv.printCurrentIndent();
        bv.printComment( "end sysg3d::BinMeshHeader" );
        bv.printLineEnter();

        // Name
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_NAME );
        bv.printLineEnter();
        bv.printCurrentIndent();
        bv.printTagString( aMeshData.name.c_str() );
        bv.printLineEnter();

        // Count
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_COUNT );
        bv.printLineEnter();
        bv.printCurrentIndent();
        {
            U32Vector countVector;
            countVector.resize( aMeshData.indexArrays.size() );
            for ( U32 i = 0; i < countVector.size(); ++i )
            {
                countVector[i] = U32(aMeshData.indexArrays[i].size());
            }
            bv.printTagU32Array( &countVector[0] , U32(countVector.size()) );
        }
        bv.printLineEnter();

        // IndexBDOArray
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_INDEXBDO );
        bv.printLineEnter();
        // BDO
        for ( U32 i = 0; i < aMeshData.indexArrays.size(); ++i )
        {
            bv.printCurrentIndent();
            bv.printTagReferenceF( "%s_%lu" , LABEL_INDEX , i );
            bv.printLineEnter();
        }
        // Entity
        for ( U32 i = 0; i < aMeshData.indexArrays.size(); ++i )
        {
            bv.printCurrentIndent();
            bv.printTagLabelF( "%s_%lu" , LABEL_INDEX , i );
            bv.printLineEnter();
            bv.printCurrentIndent();
            bv.printTagU32Array( &aMeshData.indexArrays[i][0] , U32( aMeshData.indexArrays[i].size() ) );
            bv.printLineEnter();
        }
        
        // Pos
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_POS );
        bv.printLineEnter();
        bv.printCurrentIndent();
        bv.printTagF32Array( &aMeshData.posArray[0] , U32( aMeshData.posArray.size() ) );
        bv.printLineEnter();

        // Normal
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_NORMAL );
        bv.printLineEnter();
        bv.printCurrentIndent();
        bv.printTagF32Array( &aMeshData.normalArray[0] , U32( aMeshData.normalArray.size() ) );
        bv.printLineEnter();

        // Color
        if ( aMeshData.colorArray.size() != 0 )
        {
            bv.printCurrentIndent();
            bv.printTagLabel( LABEL_COLOR );
            bv.printLineEnter();
            bv.printCurrentIndent();
            bv.printTagF32Array( &aMeshData.colorArray[0] , U32( aMeshData.colorArray.size() ) );
            bv.printLineEnter();
        }

        // end
        ::app::CommonHeaderWriter::writeXMLEnd( bv );
    }
}

//------------------------------------------------------------
namespace app {
namespace sysg3d_conv {
//------------------------------------------------------------
int MeshConv::run(
    const ConvertRecipe& aConvertRecipe 
    , DAE& aDAE 
    )
{
    // バナー
    PJ_COUT( "<Mesh>\n" );

    // ジオメトリ
    const daeUInt geometryCount = aDAE.getDatabase()->getElementCount(0,"geometry",0);
    for ( daeUInt i = 0; i < geometryCount; ++i )
    {
        // ジオメトリ取得
        domGeometry* geo = 0;
        aDAE.getDatabase()->getElement( 
            reinterpret_cast<daeElement**>(&geo)
            , i
            , 0
            , "geometry"
            );
        PJ_ASSERT( geo != 0 );

        // メッシュ取得
        domMesh* mesh = geo->getMesh();
        if ( mesh == 0 )
        {// メッシュじゃないものはパス            
            PJ_COUT( "Warning: passed geometry named '%s' because not found node named 'mesh'.\n"
                , geo->getName()
                );
            continue;
        }

        // 変数準備
        ByteVector byteVector;

        // Polylist
        if ( mesh->getPolylist_array().getCount() != 0 )
        {
            // 2個以上存在する場合は知らない
            if ( 2 <= mesh->getPolylist_array().getCount() )
            {
                PJ_COUT( "Warning: passed mesh in geometry named '%s' because two or more exist node named 'polylist'.\n"
                    , geo->getName()
                    );
                continue;
            }

            // 処理開始
            const int result = onePolylistProc(
                byteVector
                , aConvertRecipe
                , *geo
                , *mesh
                , *mesh->getPolylist_array().get(0)
                );
            if ( result != 0 )
            {
                return result;
            }
        }
        // Triangles
        else if ( mesh->getTriangles_array().getCount() != 0 )
        {
            // 2個以上存在する場合は知らない
            if ( 2 <= mesh->getTriangles_array().getCount() )
            {
                PJ_COUT( "Warning: passed mesh in geometry named '%s' because two or more exist node named 'triangles'.\n"
                    , geo->getName()
                    );
                continue;
            }

            // 処理開始
            const int result = oneTrianglesProc(
                byteVector
                , aConvertRecipe
                , *geo
                , *mesh
                , *mesh->getTriangles_array().get(0)
                );
            if ( result != 0 )
            {
                return result;
            }
        }

        // write
        std::string outputFileName = "mesh_";
        outputFileName += geo->getName();
        outputFileName += ".xml";
        std::string outputFilePath = aConvertRecipe.outputDir();
        outputFilePath += outputFileName;
        if ( !byteVector.writeToFile( outputFilePath.c_str() ) )
        {
            return -1;
        }
        PJ_COUT( "%s\n" , outputFileName.c_str() );
    }

    return 0;
}

//------------------------------------------------------------
int MeshConv::oneTrianglesProc(
    ByteVector& aByteVector
    , const ConvertRecipe& aRecipe
    , const domGeometry& aGeo
    , const domMesh& aMesh
    , const domTriangles& aTri
    )
{
    if ( aTri.getInput_array().getCount() == 0 )
    {// １つもInputノードがない
        PJ_COUT( "Error: not found  input node in triangles named '%s' in geometry named '%s'.\n"
            , aTri.getName()
            , aGeo.getName()
            );
        return -1;
    }

    // 結果を格納するオブジェクト
    MeshData meshData;
    meshData.meshKind = ::sysg3d::BinMeshKind_Triangles;
    meshData.name = aGeo.getName();
    meshData.indexArrays.resize(1);

    OneIndexSetArray oneIndexSetArray; // OneIndexSet用配列。
    const OffsetSet offsetSet = t_getOffsetSet( aTri.getInput_array() ); // オフセットのセット。
    {// Index配列の生成
        const domUint numOfInput = t_getMaxOffset( aTri.getInput_array() ); // 最大オフセット値。
        const domUint numOfIndex = aTri.getP()->getValue().getCount(); // indexの総数。
        const domUint numOfIndexSet = numOfIndex / numOfInput; // indexSetの数。
        const domListOfUInts srcIndexArray = aTri.getP()->getValue(); // index配列。

        // reserve
        meshData.indexArrays[0].reserve( static_cast< U32 >( numOfIndexSet ) );
        oneIndexSetArray.reserve( static_cast< U32 >( numOfIndexSet ) );
        // index配列生成
        for ( domUint i = 0; i < numOfIndexSet; ++i )
        {
            const domUint baseIndex = i * numOfInput;
            OneIndexSet oneIndexSet;
            for ( U32 e = 0; e < ElementKind_Num; ++e )
            {
                if ( offsetSet.array[ e ].isValid() )
                {
                    oneIndexSet.array[ e ] = srcIndexArray.get(
                        size_t( baseIndex + offsetSet.array[ e ].value() ) 
                        );
                }
            }

            // 既にあればそのindexを格納
            U32 result = OneIndexSet::INVALID_INDEX;
            for ( U32 vecIdx = 0; vecIdx < oneIndexSetArray.size(); ++ vecIdx )
            {
                if ( oneIndexSetArray[vecIdx] == oneIndexSet )
                {// これだ
                    result = vecIdx;
                    break;
                }
            }
            if ( result != OneIndexSet::INVALID_INDEX )
            {// 再利用
                meshData.indexArrays[0].push_back( result );
            }
            else
            {// 追加
                meshData.indexArrays[0].push_back( U32( oneIndexSetArray.size() ) );
                oneIndexSetArray.push_back( oneIndexSet );
            }
        }
    }

    // index以外の配列を生成
    t_buildExtraArrays( 
        meshData
        , aMesh
        , offsetSet
        , oneIndexSetArray 
        );

    // ByteVectorの作成
    t_printToByteVector( aByteVector , meshData );

    return 0;
}
//------------------------------------------------------------
int MeshConv::onePolylistProc(
    ByteVector& aByteVector
    , const ConvertRecipe& aRecipe
    , const domGeometry& aGeo
    , const domMesh& aMesh
    , const domPolylist& aPoly
    )
{
    if ( aPoly.getInput_array().getCount() == 0 )
    {// １つもInputノードがない
        PJ_COUT( "Error: not found  input node in polist named '%s' in geometry named '%s'.\n"
            , aPoly.getName()
            , aGeo.getName()
            );
        return -1;
    }

    // 結果を格納するオブジェクト
    MeshData meshData;
    meshData.meshKind = ::sysg3d::BinMeshKind_Polylist;
    meshData.name = aGeo.getName();
    meshData.indexArrays.resize( aPoly.getVcount()->getValue().getCount() );

    OneIndexSetArray oneIndexSetArray; // OneIndexSet用配列。
    const OffsetSet offsetSet = t_getOffsetSet( aPoly.getInput_array() ); // オフセットのセット。
    {// Index配列の生成
        const domUint numOfInput = t_getMaxOffset( aPoly.getInput_array() ); // 最大オフセット値。
        const domListOfUInts srcIndexArray = aPoly.getP()->getValue(); // index配列。
        const domUint numOfIndex = aPoly.getP()->getValue().getCount(); // indexの総数。
        const domUint numOfIndexSet = numOfIndex / numOfInput; // indexSetの数。
        oneIndexSetArray.reserve( static_cast< U32 >( numOfIndexSet ) );

        // reserve
        U32 totalIndex = 0;
        for ( U32 v = 0; v < aPoly.getVcount()->getValue().getCount(); ++v )
        {
            const domUint numOfIndexInPrimitive = aPoly.getVcount()->getValue()[v];
            meshData.indexArrays[v].reserve( static_cast< U32 >( numOfIndexInPrimitive ) );
        
            for ( U32 p = 0; p < numOfIndexInPrimitive; ++p , ++totalIndex )
            {
                // index配列生成
                const domUint baseIndex = totalIndex * numOfInput;
                OneIndexSet oneIndexSet;
                for ( U32 e = 0; e < ElementKind_Num; ++e )
                {
                    if ( offsetSet.array[ e ].isValid() )
                    {
                        oneIndexSet.array[ e ] = srcIndexArray.get(
                            size_t( baseIndex + offsetSet.array[ e ].value() ) 
                            );
                    }
                }

                // 既にあればそのindexを格納
                U32 result = OneIndexSet::INVALID_INDEX;
                for ( U32 vecIdx = 0; vecIdx < oneIndexSetArray.size(); ++ vecIdx )
                {
                    if ( oneIndexSetArray[vecIdx] == oneIndexSet )
                    {// これだ
                        result = vecIdx;
                        break;
                    }
                }
                if ( result != OneIndexSet::INVALID_INDEX )
                {// 再利用
                    meshData.indexArrays[v].push_back( result );
                }
                else
                {// 追加
                    meshData.indexArrays[v].push_back( U32( oneIndexSetArray.size() ) );
                    oneIndexSetArray.push_back( oneIndexSet );
                }
            }
        }
    }

    // index以外の配列を生成
    t_buildExtraArrays( 
        meshData
        , aMesh
        , offsetSet
        , oneIndexSetArray 
        );

    // ByteVectorの作成
    t_printToByteVector( aByteVector , meshData );

    return 0;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
