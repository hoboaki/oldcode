/**
 * @file
 * @brief ByteCodeDefineを記述する。
 */
#ifndef ACS_INCLUDE_BYTECODEDEFINE
#define ACS_INCLUDE_BYTECODEDEFINE

//------------------------------------------------------------
#include <acscript/template_util.hpp>

//------------------------------------------------------------
namespace acscript {
namespace byte_code_define {
 
#define ACS_BIT_TO_VALUE( b0 , b1 , b2 , b3 , b4 , b5 , b6 , b7 ) ( ::acscript::template_util::BitToValue<b0,b1,b2,b3,b4,b5,b6,b7>::Value )

    /// 型。
    /*enum Type
    {
        Type_S1   = BTV<0,0,0,0>::Value
        , Type_S2 = ::acscript::template_util::BitToValue<1,0,0,0>::Value
        , Type_S4 = ::acscript::template_util::BitToValue<0,1,0,0>::Value
        , Type_S8 = ::acscript::template_util::BitToValue<1,1,0,0>::Value

        , Type_U1 = ::acscript::template_util::BitToValue<0,0,1,0>::Value
        , Type_U2 = ::acscript::template_util::BitToValue<1,0,1,0>::Value
        , Type_U4 = ::acscript::template_util::BitToValue<0,1,1,0>::Value
        , Type_U8 = ::acscript::template_util::BitToValue<1,1,1,0>::Value
        
        , Type_I1 = ::acscript::template_util::BitToValue<0,0,1,0>::Value
        , Type_I2 = ::acscript::template_util::BitToValue<0,0,1,0>::Value
        , Type_I4 = ::acscript::template_util::BitToValue<1,0,1,0>::Value
        , Type_I9 = ::acscript::template_util::BitToValue<0,1,1,0>::Value

        , Type_F4 = ::acscript::template_util::BitToValue<0,1,1,1>::Value
        , Type_F8 = ::acscript::template_util::BitToValue<1,1,1,1>::Value
    };*/

    // 受取方法
    enum ReceiveMethod
    {
        ReceiveMethod_ReferenceData = 0 ///< 参照先の相対アドレス指定受取。
        , ReceiveMethod_Data = 1 ///< データを取得。
        , ReceiveMethod_Address = 2 ///< アドレスを取得。
        , ReceiveMethod_Reference = 3 ///< 参照を取得。
    };

    namespace command {
    /// 命令。
    enum Command
    {
        Zero
        // Add
        , AddI1 = ACS_BIT_TO_VALUE(1,0,0,0,1,0,0,0)
        , AddI2 = ACS_BIT_TO_VALUE(1,0,0,0,1,0,0,1)
        , AddI4 = ACS_BIT_TO_VALUE(1,0,0,0,1,0,1,0)
        , AddI8 = ACS_BIT_TO_VALUE(1,0,0,0,1,0,1,1)
        , AddF4 = ACS_BIT_TO_VALUE(1,0,0,0,1,1,1,0)
        , AddF8 = ACS_BIT_TO_VALUE(1,0,0,0,1,1,1,1)
        // Sub
        , SubI1 = ACS_BIT_TO_VALUE(1,0,0,1,1,0,0,0)
        , SubI2 = ACS_BIT_TO_VALUE(1,0,0,1,1,0,0,1)
        , SubI4 = ACS_BIT_TO_VALUE(1,0,0,1,1,0,1,0)
        , SubI8 = ACS_BIT_TO_VALUE(1,0,0,1,1,0,1,1)
        , SubF4 = ACS_BIT_TO_VALUE(1,0,0,1,1,1,1,0)
        , SubF8 = ACS_BIT_TO_VALUE(1,0,0,1,1,1,1,1)
        // Mul
        , MulS1 = ACS_BIT_TO_VALUE(1,0,1,0,0,0,0,0)
        , MulS2 = ACS_BIT_TO_VALUE(1,0,1,0,0,0,0,1)
        , MulS4 = ACS_BIT_TO_VALUE(1,0,1,0,0,0,1,0)
        , MulS8 = ACS_BIT_TO_VALUE(1,0,1,0,0,0,1,1)
        , MulU1 = ACS_BIT_TO_VALUE(1,0,1,0,0,1,0,0)
        , MulU2 = ACS_BIT_TO_VALUE(1,0,1,0,0,1,0,1)
        , MulU4 = ACS_BIT_TO_VALUE(1,0,1,0,0,1,1,0)
        , MulU8 = ACS_BIT_TO_VALUE(1,0,1,0,0,1,1,1)
        , MulF4 = ACS_BIT_TO_VALUE(1,0,1,0,1,1,1,0)
        , MulF8 = ACS_BIT_TO_VALUE(1,0,1,0,1,1,1,1)
        // Div
        , DivS1 = ACS_BIT_TO_VALUE(1,0,1,1,0,0,0,0)
        , DivS2 = ACS_BIT_TO_VALUE(1,0,1,1,0,0,0,1)
        , DivS4 = ACS_BIT_TO_VALUE(1,0,1,1,0,0,1,0)
        , DivS8 = ACS_BIT_TO_VALUE(1,0,1,1,0,0,1,1)
        , DivU1 = ACS_BIT_TO_VALUE(1,0,1,1,0,1,0,0)
        , DivU2 = ACS_BIT_TO_VALUE(1,0,1,1,0,1,0,1)
        , DivU4 = ACS_BIT_TO_VALUE(1,0,1,1,0,1,1,0)
        , DivU8 = ACS_BIT_TO_VALUE(1,0,1,1,0,1,1,1)
        , DivF4 = ACS_BIT_TO_VALUE(1,0,1,1,1,1,1,0)
        , DivF8 = ACS_BIT_TO_VALUE(1,0,1,1,1,1,1,1)
        // Mod
        , ModS1 = ACS_BIT_TO_VALUE(1,1,0,0,0,0,0,0)
        , ModS2 = ACS_BIT_TO_VALUE(1,1,0,0,0,0,0,1)
        , ModS4 = ACS_BIT_TO_VALUE(1,1,0,0,0,0,1,0)
        , ModS8 = ACS_BIT_TO_VALUE(1,1,0,0,0,0,1,1)
        , ModU1 = ACS_BIT_TO_VALUE(1,1,0,0,0,1,0,0)
        , ModU2 = ACS_BIT_TO_VALUE(1,1,0,0,0,1,0,1)
        , ModU4 = ACS_BIT_TO_VALUE(1,1,0,0,0,1,1,0)
        , ModU8 = ACS_BIT_TO_VALUE(1,1,0,0,0,1,1,1)
        // Compare
        , CmpS1 = ACS_BIT_TO_VALUE(1,1,0,1,0,0,0,0)
        , CmpS2 = ACS_BIT_TO_VALUE(1,1,0,1,0,0,0,1)
        , CmpS4 = ACS_BIT_TO_VALUE(1,1,0,1,0,0,1,0)
        , CmpS8 = ACS_BIT_TO_VALUE(1,1,0,1,0,0,1,1)
        , CmpU1 = ACS_BIT_TO_VALUE(1,1,0,1,0,1,0,0)
        , CmpU2 = ACS_BIT_TO_VALUE(1,1,0,1,0,1,0,1)
        , CmpU4 = ACS_BIT_TO_VALUE(1,1,0,1,0,1,1,0)
        , CmpU8 = ACS_BIT_TO_VALUE(1,1,0,1,0,1,1,1)
        , CmpF4 = ACS_BIT_TO_VALUE(1,1,0,1,1,1,1,0)
        , CmpF8 = ACS_BIT_TO_VALUE(1,1,0,1,1,1,1,1)
        // Increment
        , IncI1 = ACS_BIT_TO_VALUE(1,1,1,0,1,0,0,0)
        , IncI2 = ACS_BIT_TO_VALUE(1,1,1,0,1,0,0,1)
        , IncI4 = ACS_BIT_TO_VALUE(1,1,1,0,1,0,1,0)
        , IncI8 = ACS_BIT_TO_VALUE(1,1,1,0,1,0,1,1)
        , IncF4 = ACS_BIT_TO_VALUE(1,1,1,0,1,1,1,0)
        , IncF8 = ACS_BIT_TO_VALUE(1,1,1,0,1,1,1,1)
        // Decrement
        , DecI1 = ACS_BIT_TO_VALUE(1,1,1,1,1,0,0,0)
        , DecI2 = ACS_BIT_TO_VALUE(1,1,1,1,1,0,0,1)
        , DecI4 = ACS_BIT_TO_VALUE(1,1,1,1,1,0,1,0)
        , DecI8 = ACS_BIT_TO_VALUE(1,1,1,1,1,0,1,1)
        , DecF4 = ACS_BIT_TO_VALUE(1,1,1,1,1,1,1,0)
        , DecF8 = ACS_BIT_TO_VALUE(1,1,1,1,1,1,1,1)
        // Copy
        , Copy1 = ACS_BIT_TO_VALUE(0,0,0,0,0,0,0,0)
        , Copy2 = ACS_BIT_TO_VALUE(0,0,0,0,0,0,0,1)
        , Copy4 = ACS_BIT_TO_VALUE(0,0,0,0,0,0,1,0)
        , Copy8 = ACS_BIT_TO_VALUE(0,0,0,0,0,0,1,1)
        // Memory Copy
        , MCopy4 = ACS_BIT_TO_VALUE(0,0,0,0,0,1,1,0)
        // Shift Left (Logical)
        , SLL1 = ACS_BIT_TO_VALUE(0,0,0,0,1,0,0,0)
        , SLL2 = ACS_BIT_TO_VALUE(0,0,0,0,1,0,0,1)
        , SLL4 = ACS_BIT_TO_VALUE(0,0,0,0,1,0,1,0)
        , SLL8 = ACS_BIT_TO_VALUE(0,0,0,0,1,0,1,1)
        // Shift Right (Logical)
        , SRL1 = ACS_BIT_TO_VALUE(0,0,0,0,1,1,0,0)
        , SRL2 = ACS_BIT_TO_VALUE(0,0,0,0,1,1,0,1)
        , SRL4 = ACS_BIT_TO_VALUE(0,0,0,0,1,1,1,0)
        , SRL8 = ACS_BIT_TO_VALUE(0,0,0,0,1,1,1,1)
        // Bit And
        , BAnd1 = ACS_BIT_TO_VALUE(0,0,0,1,0,0,0,0)
        , BAnd2 = ACS_BIT_TO_VALUE(0,0,0,1,0,0,0,1)
        , BAnd4 = ACS_BIT_TO_VALUE(0,0,0,1,0,0,1,0)
        , BAnd8 = ACS_BIT_TO_VALUE(0,0,0,1,0,0,1,1)
        // Bit Not
        , BNot1 = ACS_BIT_TO_VALUE(0,0,0,1,0,1,0,0)
        , BNot2 = ACS_BIT_TO_VALUE(0,0,0,1,0,1,0,1)
        , BNot4 = ACS_BIT_TO_VALUE(0,0,0,1,0,1,1,0)
        , BNot8 = ACS_BIT_TO_VALUE(0,0,0,1,0,1,1,1)
        // Bit Or
        , BOr1 = ACS_BIT_TO_VALUE(0,0,0,1,1,0,0,0)
        , BOr2 = ACS_BIT_TO_VALUE(0,0,0,1,1,0,0,1)
        , BOr4 = ACS_BIT_TO_VALUE(0,0,0,1,1,0,1,0)
        , BOr8 = ACS_BIT_TO_VALUE(0,0,0,1,1,0,1,1)
        // Bit Xor
        , BXor1 = ACS_BIT_TO_VALUE(0,0,0,1,1,1,0,0)
        , BXor2 = ACS_BIT_TO_VALUE(0,0,0,1,1,1,0,1)
        , BXor4 = ACS_BIT_TO_VALUE(0,0,0,1,1,1,1,0)
        , BXor8 = ACS_BIT_TO_VALUE(0,0,0,1,1,1,1,1)
        // Enter / Leave
        , Enter  = ACS_BIT_TO_VALUE(0,0,1,0,0,0,0,0)
        , Leave  = ACS_BIT_TO_VALUE(0,0,1,0,0,0,1,0)
        // Jump / Call
        , JumpSE  = ACS_BIT_TO_VALUE(0,0,1,1,0,0,0,0)
        , JumpSNE = ACS_BIT_TO_VALUE(0,0,1,1,0,0,0,1)
        , JumpSL  = ACS_BIT_TO_VALUE(0,0,1,1,0,0,1,0)
        , JumpSLE = ACS_BIT_TO_VALUE(0,0,1,1,0,0,1,1)
        , JumpSU  = ACS_BIT_TO_VALUE(0,0,1,1,0,1,0,0)
        , JumpSNU = ACS_BIT_TO_VALUE(0,0,1,1,0,1,0,1)
        , JumpS   = ACS_BIT_TO_VALUE(0,0,1,1,0,1,1,1)
        , CallS   = ACS_BIT_TO_VALUE(0,0,1,1,0,0,0,0)
        , JumpDE  = ACS_BIT_TO_VALUE(0,0,1,1,1,0,0,0)
        , JumpDNE = ACS_BIT_TO_VALUE(0,0,1,1,1,0,0,1)
        , JumpDL  = ACS_BIT_TO_VALUE(0,0,1,1,1,0,1,0)
        , JumpDLE = ACS_BIT_TO_VALUE(0,0,1,1,1,0,1,1)
        , JumpDU  = ACS_BIT_TO_VALUE(0,0,1,1,1,1,0,0)
        , JumpDNU = ACS_BIT_TO_VALUE(0,0,1,1,1,1,0,1)
        , JumpD   = ACS_BIT_TO_VALUE(0,0,1,1,1,1,1,1)
        , CallD   = ACS_BIT_TO_VALUE(0,0,1,1,1,0,0,0)
        // Cast
        , Cast = ACS_BIT_TO_VALUE(0,1,0,0,0,0,0,0)
    };
    } // end of namespace

}}
//------------------------------------------------------------
#endif
// EOF
