/**
 * @file
 * @brief MeshConv型を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace app {
    class ByteVector;
}
namespace app {
namespace sysg3d_conv {
    class ConvertRecipe;
}}

//------------------------------------------------------------
namespace app {
namespace sysg3d_conv {

    /// Meshコンバート関数。
    class MeshConv
    {
    public:
        static int run( const ConvertRecipe& , DAE& );

    private:
        /// Trianglesメッシュ。
        static int oneTrianglesProc(
            ByteVector& byteVector
            , const ConvertRecipe&
            , const domGeometry& geo
            , const domMesh& mesh
            , const domTriangles& tri
            );
        /// Polylistメッシュ。
        static int onePolylistProc(
            ByteVector& byteVector
            , const ConvertRecipe&
            , const domGeometry& geo
            , const domMesh& mesh
            , const domPolylist& poly
            );
    };

}}
//------------------------------------------------------------
// EOF
