/**
 * @file
 * @brief Argument�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace app {

    /// �������܂Ƃ߂�N���X�B
    class Argument
    {
    public:
        Argument( U32 argc , char* argv[] );

        U32 count()const;
        ConstStr argAtIndex( U32 index )const;

        void dump(); ///< �_���v�B

    private:
        U32 argc_;
        char** argv_;
    };

}
//------------------------------------------------------------
// EOF
