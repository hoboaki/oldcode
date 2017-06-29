/**
 * @file
 * @brief BDOLabel型を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace app {

    /// BinaryDataOffsetラベル。
    class BDOLabel
    {
    public:
        static const U32 INVALID_NO = 0xFFFFFFFF;
        BDOLabel();
        explicit BDOLabel( U32 mainNo , U32 subNo = 0 );

        bool operator <( const BDOLabel& aRHS )const;

    private:
        U32 mainNo_;
        U32 subNo_;
    };

}
//------------------------------------------------------------
// EOF
