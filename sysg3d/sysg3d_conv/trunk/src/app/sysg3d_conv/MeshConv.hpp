/**
 * @file
 * @brief MeshConv�^���L�q����B
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

    /// Mesh�R���o�[�g�֐��B
    class MeshConv
    {
    public:
        static int run( const ConvertRecipe& , DAE& );

    private:
        /// Triangles���b�V���B
        static int oneTrianglesProc(
            ByteVector& byteVector
            , const ConvertRecipe&
            , const domGeometry& geo
            , const domMesh& mesh
            , const domTriangles& tri
            );
        /// Polylist���b�V���B
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
