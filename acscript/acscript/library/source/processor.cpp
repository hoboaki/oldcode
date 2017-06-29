/** 
 * @file
 * @brief processor.hppの実装を記述する。
 */
#include <acscript/processor.hpp>

//------------------------------------------------------------
#include <limits>
#include <acscript/byte_code.hpp>
#include <acscript/byte_code_define.hpp>
#include <acscript/byte_code_repository.hpp>
#include <acscript/macro.hpp>
#include <acscript/symbol_info.hpp>

//------------------------------------------------------------
#define ACS_LOCAL_ADD_CMD( cmdName ) table.func[ cmdName ] = &cmd##cmdName

//------------------------------------------------------------
using ::acscript::Context;
using ::acscript::U32;
typedef ::acscript::Processor::Property Property;

//------------------------------------------------------------
namespace
{
    // Opland,Typeの種類。
    enum TypeValueKind
    {
        TypeValueKind_TypeA
        , TypeValueKind_TypeP
    };

    // Operator
    template< typename Type >
    struct OperatorAdd
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs + rhs;
        }
    };
    template< typename Type >
    struct OperatorSub
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs - rhs;
        }
    };
    template< typename Type >
    struct OperatorMul
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs * rhs;
        }
    };
    template< typename Type >
    struct OperatorDiv
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = false;
        static void operate( Property& prop , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            if ( rhs == 0 )
            {// 0割り算
                prop.context.exception = ::acscript::ProcessorException_DivByZero;
                return;
            }
            dest = lhs / rhs;
        }
    };
    template< typename Type >
    struct OperatorMod
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs % rhs;
        }
    };
    template< typename Type >
    struct OperatorInc
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest )
        {
            ++dest;
        }
    };
    template< typename Type >
    struct OperatorDec
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest )
        {
            --dest;
        }
    };
    template< typename Type >
    struct OperatorCopy
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& val )
        {
            dest = val;
        }
    };
    template< typename Type >
    struct OperatorMCopy
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , Type& src , const size_t val )
        {
            std::memcpy( &dest , &src , val );
        }
    };
    template< typename Type >
    struct OperatorSLL
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs << rhs;
        }
    };
    template< typename Type >
    struct OperatorSRL
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs >> rhs;
        }
    };
    template< typename Type >
    struct OperatorBAnd
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs & rhs;
        }
    };
    template< typename Type >
    struct OperatorBNot
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& val )
        {
            dest = ~val;
        }
    };
    template< typename Type >
    struct OperatorBOr
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate( Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs | rhs;
        }
    };
    template< typename Type >
    struct OperatorBXor
    {
        static const bool THROW_EXCEPTION = false;
        static const bool MOVE_PC = false;
        static void operate(Property& , const U32 oplandSize , Type& dest , const Type& lhs , const Type& rhs )
        {
            dest = lhs ^ rhs;
        }
    };
    struct OperatorEnter
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = false;
        static void operate( Property& prop , const U32 oplandSize , const ::acscript::U32& val )
        {
            // over flow check
            if ( prop.context.sp < ( prop.stack.head() + sizeof(prop.context.bp) + sizeof(prop.context.ri) ) )
            {// スタックオーバーフロー
                prop.context.exception = ::acscript::ProcessorException_StackOverFlow;
                return;          
            }

            // push bp and rs
            prop.context.sp -= sizeof(prop.context.bp);
            *reinterpret_cast<::acscript::U8**>( prop.context.sp ) = prop.context.bp;
            prop.context.sp -= sizeof(prop.context.ri);
            *reinterpret_cast<Context::ReturnInfo*>( prop.context.sp ) = prop.context.ri;
        }
    };
    struct OperatorLeave
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = false;
        static void operate( Property& prop , const U32 oplandSize , const ::acscript::U32& val )
        {
            // over flow check
            if ( ( prop.stack.tail() - sizeof(prop.context.bp) - sizeof(prop.context.ri) ) < prop.context.bp )
            {// スタックオーバーフロー
                prop.context.exception = ::acscript::ProcessorException_StackOverFlow;
                return;
            }

            // pop rs and bp
            prop.context.ri = *reinterpret_cast<Context::ReturnInfo*>( prop.context.sp );
            prop.context.sp += sizeof(prop.context.ri);
            prop.context.bp = *reinterpret_cast<::acscript::U8**>( prop.context.sp );
            prop.context.sp += sizeof(prop.context.bp);
        }
    };
    template< typename ComparatorType >
    struct OperatorJump
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = true;
        static void operate( Property& prop , const U32 oplandSize , const ::acscript::LabelId& val )
        {
            // ローカルのシンボルしか許されない
            if ( !prop.currentByteCodeRef().isLocalSymbolLabel( val ) )
            {
                prop.context.exception = ::acscript::ProcessorException_InvalidSymbol;
                return;
            }

            // jump
            if ( ComparatorType::isTrue( prop.context ) )
            {
                const ::acscript::U32 offset = prop.currentByteCodeRef().labelIdToLocalSymbolOffset( val );
                if ( !prop.currentByteCodeRef().isOffsetValid( offset ) )
                {// 無効なオフセット
                    prop.context.exception = ::acscript::ProcessorException_OutOfByteCode;
                    return;
                }
                // 更新
                prop.context.pc = reinterpret_cast< ::acscript::U8* >(
                    prop.currentByteCodeRef().offsetToAddress( offset )
                    );
            }
        }
    };
    struct ComparatorAlways
    {
        static bool isTrue( const Context& )
        {
            return true;
        }
    };
    struct ComparatorEqual
    {
        static bool isTrue( const Context& context )
        {
            return context.cr.r[ Context::CR_Equal ] != 0;
        }
    };
    struct ComparatorNotEqual
    {
        static bool isTrue( const Context& context )
        {
            return !ComparatorEqual::isTrue( context );
        }
    };
    struct ComparatorLess
    {
        static bool isTrue( const Context& context )
        {
            return context.cr.r[ Context::CR_Less ] != 0;
        }
    };
    struct ComparatorLessEqual
    {
        static bool isTrue( const Context& context )
        {
            return ComparatorLess::isTrue( context )
                || ComparatorEqual::isTrue( context ) ;
        }
    };
    struct ComparatorUnvalue
    {
        static bool isTrue( const Context& context )
        {
            return context.cr.r[ Context::CR_NaN ] != 0;
        }
    };
    struct ComparatorNotUnvalue
    {
        static bool isTrue( const Context& context )
        {
            return !ComparatorUnvalue::isTrue( context );
        }
    };
    struct OperatorCallDynamic
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = true;
        static void operate( Property& prop , const U32 oplandSize , const ::acscript::SymbolId& val )
        {
            // エラーチェック
            const ::acscript::SymbolInfo nextSymbol = { val };
            if ( !nextSymbol.isValidInfo() )
            {// 無効なシンボル
                prop.context.exception = ::acscript::ProcessorException_InvalidSymbol;
                return;
            }

            // 内部シンボル
            if ( nextSymbol.isInternalSymbol() )
            {// 
                ::acscript::ByteCode* nextByteCode = prop.byteCodeReposRef.ptr( nextSymbol.internal.byteCodeId );
                if ( nextByteCode == 0 
                    || !nextByteCode->isLabelValid( nextSymbol.internal.labelId ) 
                    || !nextByteCode->isLocalSymbolLabel( nextSymbol.internal.labelId )
                    )
                {// 無効なシンボル・ラベル
                    prop.context.exception = ::acscript::ProcessorException_InvalidSymbol;
                    return;
                }
                const ::acscript::U32 offset = nextByteCode->labelIdToLocalSymbolOffset( nextSymbol.internal.labelId );
                if ( !nextByteCode->isOffsetValid( offset ) )
                {// 無効なオフセット
                    prop.context.exception = ::acscript::ProcessorException_OutOfByteCode;
                    return;
                }
                // ReturnInfoの更新
                prop.context.ri.symbol = prop.context.cs;
                prop.context.ri.pc = prop.context.pc;
                // 移動
                prop.context.bcp = nextByteCode;
                prop.context.cs = nextSymbol.id;
                prop.context.pc = reinterpret_cast<::acscript::U8*>( nextByteCode->offsetToAddress( val ) );
                return;
            }

            // 外部シンボル
            {// Contextを保存しておく
                Context backupContext = prop.context;

                // コール
                prop.procEventHandler.externalFunctionCall( nextSymbol.extSymbolId() );

                // ロールバック
                prop.context = backupContext;

                // プログラムカウンタを移動
                prop.context.pc += oplandSize;
            }
        }
    };
    struct OperatorCallStatic
    {
        static const bool THROW_EXCEPTION = true;
        static const bool MOVE_PC = true;
        static void operate( Property& prop , const U32 oplandSize , const ::acscript::LabelId& val )
        {
            if ( !prop.currentByteCodeRef().isLabelValid( val ) )
            {// 無効なラベル
                prop.context.exception = ::acscript::ProcessorException_InvalidSymbol;
                return;
            }

            // SymbolInfo作成
            ::acscript::SymbolInfo si = { prop.context.cs };
            si.internal.labelId = val;

            // 処理委譲
            OperatorCallDynamic::operate( prop , oplandSize , si.id );
        }
    };
}

//------------------------------------------------------------
namespace acscript {
//------------------------------------------------------------
Processor::Property::Property(
    const ByteCodeRepository& aByteCodeReposRef
    , const ProcessorEventHandler& aProcEventHandler
    , const U32 aStackSize
    )
: byteCodeReposRef( aByteCodeReposRef )
, procEventHandler( aProcEventHandler )
, context()
, stack( aStackSize )
{
    context.sp = stack.head();
    context.bp = context.sp;
}

//------------------------------------------------------------
ByteCode& Processor::Property::currentByteCodeRef()const
{
    ACS_ASSERT( context.bcp != 0 );
    return *context.bcp;
}

//------------------------------------------------------------
Processor::Processor(
    const ByteCodeRepository& aByteCodeReposRef
    , const ProcessorEventHandler& aProcEventHandler
    , const U32 aStackSize
    )
: mProperty( aByteCodeReposRef , aProcEventHandler , aStackSize )
{
}

//------------------------------------------------------------
void Processor::call( const SymbolId&  )
{
}

//------------------------------------------------------------
Processor::CmdResult Processor::processOneCommand()
{
    static const CommandFuncTable FIRST_COMMAND_FUNC_TABLE = createFirstCommandFuncTable();

    const U8 opcode = *mProperty.context.pc;
    const CommandFunc func = FIRST_COMMAND_FUNC_TABLE.func[ opcode ];
    if ( func == 0 )
    {
        return CmdResult_IllegalOperationCode;
    }
    return (*this.*func)();
}

//------------------------------------------------------------
Processor::CommandFuncTable::CommandFuncTable()
: func()
{
}

//------------------------------------------------------------
Processor::CommandFuncTable Processor::createFirstCommandFuncTable()
{
    using namespace ::acscript::byte_code_define::command;

    CommandFuncTable table;
    // add
    ACS_LOCAL_ADD_CMD( AddI1 );
    ACS_LOCAL_ADD_CMD( AddI2 );
    ACS_LOCAL_ADD_CMD( AddI4 );
    ACS_LOCAL_ADD_CMD( AddI8 );
    ACS_LOCAL_ADD_CMD( AddF4 );
    ACS_LOCAL_ADD_CMD( AddF8 );
    // sub
    ACS_LOCAL_ADD_CMD( SubI1 );
    ACS_LOCAL_ADD_CMD( SubI2 );
    ACS_LOCAL_ADD_CMD( SubI4 );
    ACS_LOCAL_ADD_CMD( SubI8 );
    ACS_LOCAL_ADD_CMD( SubF4 );
    ACS_LOCAL_ADD_CMD( SubF8 );
    // mul
    ACS_LOCAL_ADD_CMD( MulS1 );
    ACS_LOCAL_ADD_CMD( MulS2 );
    ACS_LOCAL_ADD_CMD( MulS4 );
    ACS_LOCAL_ADD_CMD( MulS8 );
    ACS_LOCAL_ADD_CMD( MulU1 );
    ACS_LOCAL_ADD_CMD( MulU2 );
    ACS_LOCAL_ADD_CMD( MulU4 );
    ACS_LOCAL_ADD_CMD( MulU8 );
    ACS_LOCAL_ADD_CMD( MulF4 );
    ACS_LOCAL_ADD_CMD( MulF8 );
    // div
    ACS_LOCAL_ADD_CMD( DivS1 );
    ACS_LOCAL_ADD_CMD( DivS2 );
    ACS_LOCAL_ADD_CMD( DivS4 );
    ACS_LOCAL_ADD_CMD( DivS8 );
    ACS_LOCAL_ADD_CMD( DivU1 );
    ACS_LOCAL_ADD_CMD( DivU2 );
    ACS_LOCAL_ADD_CMD( DivU4 );
    ACS_LOCAL_ADD_CMD( DivU8 );
    ACS_LOCAL_ADD_CMD( DivF4 );
    ACS_LOCAL_ADD_CMD( DivF8 );
    // mod
    ACS_LOCAL_ADD_CMD( ModS1 );
    ACS_LOCAL_ADD_CMD( ModS2 );
    ACS_LOCAL_ADD_CMD( ModS4 );
    ACS_LOCAL_ADD_CMD( ModS8 );
    ACS_LOCAL_ADD_CMD( ModU1 );
    ACS_LOCAL_ADD_CMD( ModU2 );
    ACS_LOCAL_ADD_CMD( ModU4 );
    ACS_LOCAL_ADD_CMD( ModU8 );
    // cmp
    ACS_LOCAL_ADD_CMD( CmpS1 );
    ACS_LOCAL_ADD_CMD( CmpS2 );
    ACS_LOCAL_ADD_CMD( CmpS4 );
    ACS_LOCAL_ADD_CMD( CmpS8 );
    ACS_LOCAL_ADD_CMD( CmpU1 );
    ACS_LOCAL_ADD_CMD( CmpU2 );
    ACS_LOCAL_ADD_CMD( CmpU4 );
    ACS_LOCAL_ADD_CMD( CmpU8 );
    ACS_LOCAL_ADD_CMD( CmpF4 );
    ACS_LOCAL_ADD_CMD( CmpF8 );
    // inc
    ACS_LOCAL_ADD_CMD( IncI1 );
    ACS_LOCAL_ADD_CMD( IncI2 );
    ACS_LOCAL_ADD_CMD( IncI4 );
    ACS_LOCAL_ADD_CMD( IncI8 );
    ACS_LOCAL_ADD_CMD( IncF4 );
    ACS_LOCAL_ADD_CMD( IncF8 );
    // dec
    ACS_LOCAL_ADD_CMD( DecI1 );
    ACS_LOCAL_ADD_CMD( DecI2 );
    ACS_LOCAL_ADD_CMD( DecI4 );
    ACS_LOCAL_ADD_CMD( DecI8 );
    ACS_LOCAL_ADD_CMD( DecF4 );
    ACS_LOCAL_ADD_CMD( DecF8 );
    // copy
    ACS_LOCAL_ADD_CMD( Copy1 );
    ACS_LOCAL_ADD_CMD( Copy2 );
    ACS_LOCAL_ADD_CMD( Copy4 );
    ACS_LOCAL_ADD_CMD( Copy8 );
    // mcopy
    ACS_LOCAL_ADD_CMD( MCopy4 );
    // sll
    ACS_LOCAL_ADD_CMD( SLL1 );
    ACS_LOCAL_ADD_CMD( SLL2 );
    ACS_LOCAL_ADD_CMD( SLL4 );
    ACS_LOCAL_ADD_CMD( SLL8 );
    // srl
    ACS_LOCAL_ADD_CMD( SRL1 );
    ACS_LOCAL_ADD_CMD( SRL2 );
    ACS_LOCAL_ADD_CMD( SRL4 );
    ACS_LOCAL_ADD_CMD( SRL8 );
    // band
    ACS_LOCAL_ADD_CMD( BAnd1 );
    ACS_LOCAL_ADD_CMD( BAnd2 );
    ACS_LOCAL_ADD_CMD( BAnd4 );
    ACS_LOCAL_ADD_CMD( BAnd8 );
    // bnot
    ACS_LOCAL_ADD_CMD( BNot1 );
    ACS_LOCAL_ADD_CMD( BNot2 );
    ACS_LOCAL_ADD_CMD( BNot4 );
    ACS_LOCAL_ADD_CMD( BNot8 );
    // bor
    ACS_LOCAL_ADD_CMD( BOr1 );
    ACS_LOCAL_ADD_CMD( BOr2 );
    ACS_LOCAL_ADD_CMD( BOr4 );
    ACS_LOCAL_ADD_CMD( BOr8 );
    // bxor
    ACS_LOCAL_ADD_CMD( BXor1 );
    ACS_LOCAL_ADD_CMD( BXor2 );
    ACS_LOCAL_ADD_CMD( BXor4 );
    ACS_LOCAL_ADD_CMD( BXor8 );
    // enter / leave
    ACS_LOCAL_ADD_CMD( Enter );
    ACS_LOCAL_ADD_CMD( Leave );
    // call
    ACS_LOCAL_ADD_CMD( CallS );
    ACS_LOCAL_ADD_CMD( CallD );
    // jump
    ACS_LOCAL_ADD_CMD( JumpS );
    ACS_LOCAL_ADD_CMD( JumpSE );
    ACS_LOCAL_ADD_CMD( JumpSNE );
    ACS_LOCAL_ADD_CMD( JumpSL );
    ACS_LOCAL_ADD_CMD( JumpSLE );
    ACS_LOCAL_ADD_CMD( JumpSU );
    ACS_LOCAL_ADD_CMD( JumpSNU );
    ACS_LOCAL_ADD_CMD( JumpD );
    ACS_LOCAL_ADD_CMD( JumpDE );
    ACS_LOCAL_ADD_CMD( JumpDNE );
    ACS_LOCAL_ADD_CMD( JumpDL );
    ACS_LOCAL_ADD_CMD( JumpDLE );
    ACS_LOCAL_ADD_CMD( JumpDU );
    ACS_LOCAL_ADD_CMD( JumpDNU );
    // Cast
    ACS_LOCAL_ADD_CMD( Cast );
    
    return table;
}

//------------------------------------------------------------
template< int TypeValue , typename ResultType >
U32 Processor::oplandDataTemplate( void* aDest , const U8* aOplandHead )const
{
    using namespace ::acscript::byte_code_define;

    U8 opland = *aOplandHead;

    if ( TypeValue == TypeValueKind_TypeA 
        && opland == 0 
        )
    {// num
        *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<ResultType*>( aOplandHead[1] );
        return 1+sizeof(ResultType);
    }

    if ( opland & 1 )
    {// bp or symbol

        // bp or symbol
        const bool isSymbol = ( ( opland >>= 1 ) & 1 ) != 0;

        // receive method
        opland >>= 1;
        const ReceiveMethod receiveMethod = static_cast<ReceiveMethod>( opland & 3 );

        // 相対アドレス取得
        S32 relativeAddr = S32();
        U32 oplandSize = calculateRelativeAddr( relativeAddr , aOplandHead );
        if ( oplandSize == 0 )
        {// 不正なオペランド
            return 0;
        }            

        const U8* baseAddr = 0;
        if ( isSymbol )
        {
            // シンボルアドレス取得
            const LabelId symbolLabel = *reinterpret_cast<const U16*>( &aOplandHead[ oplandSize ] );
            baseAddr = reinterpret_cast<const U8*>( symbolLabelToAddress( symbolLabel ) );
            oplandSize += 2;
        }
        else
        {// bp
            baseAddr = mProperty.context.bp;
        }
        
        if ( baseAddr == 0 )
        {// 不正なシンボルラベル
            return 0;
        }

        // 代入
        switch( receiveMethod )
        {
        case ReceiveMethod_Data:
            // データ受取
            *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<const ResultType*>( &baseAddr[ relativeAddr ] );
            break;

        case ReceiveMethod_Address:
            // アドレス受取
            if ( sizeof(ResultType) != sizeof(void*) )
            {// 無効な結果タイプ
                return 0;
            }
            *reinterpret_cast<const void**>(aDest) = &baseAddr[ relativeAddr ];
            break;

        case ReceiveMethod_Reference:
            // 参照受取
            *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<const ResultType* const*>( &baseAddr[ relativeAddr ] )[0];
            break;

        default:
            // 無効な受取方法
            return 0;
        }
        
        // 成功
        return oplandSize;
    }
    else
    {// reg
        
        // receive method
        opland >>= 1;
        const ReceiveMethod receiveMethod = static_cast<ReceiveMethod>( opland & 3 );

        // register index
        if ( TypeValue == TypeValueKind_TypeA 
            && receiveMethod == byte_code_define::ReceiveMethod_ReferenceData 
            )
        {// TypeAのときのみ、4bitに制約される
            opland >>= 3;
        }
        else
        {// そのほか
            opland >>= 2;
        }
        const U8 registerIdx = opland;
        
        // 代入
        U32 oplandSize = 1;
        switch( receiveMethod )
        {
        case ReceiveMethod_ReferenceData:
            {// 参照先受取
                // 相対アドレス取得
                const S16 relativeAddr = *reinterpret_cast<const S16*>( &aOplandHead[1] );
                *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<const ResultType* const*>( &mProperty.context.gr.r[ registerIdx ] )[ relativeAddr ];
                oplandSize += 2;
            }
            break;

        case ReceiveMethod_Data:
            // データ受取
            *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<const ResultType*>( &mProperty.context.gr.r[ registerIdx ] );
            break;

        case ReceiveMethod_Address:
            // アドレス受取
            if ( sizeof(ResultType) != sizeof(void*) )
            {// 無効な結果タイプ
                return 0;
            }
            *reinterpret_cast<const void**>(aDest) = &mProperty.context.gr.r[ registerIdx ];
            break;

        case ReceiveMethod_Reference:
            // 参照受取
            *reinterpret_cast<ResultType*>(aDest) = *reinterpret_cast<const ResultType* const*>( &mProperty.context.gr.r[ registerIdx ] )[0];
            break;

        default:
            // 無効な受取方法
            return 0;
        }
        
        // 成功
        return oplandSize;
    }
}

//------------------------------------------------------------
U32 Processor::calculateRelativeAddr( S32& aDest , const U8* aOplandHead )const
{
    U8 opland = aOplandHead[0];
    opland >>= 4;

    U32 oplandSize = 0;
    if ( opland & 1 )
    {
        if ( ( opland >>= 1 ) & 1 )
        {
            if ( ( opland >>= 1 ) & 1 )
            {// 32bit
                aDest = *reinterpret_cast<S32*>( aOplandHead[1] );
                oplandSize = 5;
            }
            else
            {// 24bit
#if defined(ACS_IS_LITTLE_ENDIAN)
                U32 tmpVal = *reinterpret_cast<U32*>( aOplandHead[0] );
                tmpVal = ( ( tmpVal >> 8 ) & 0x007FFFFF ) | ( tmpVal & 0x80000000 ); // 23bit + 1bit(sign bit)
                aDest = *reinterpret_cast<S32*>(&tmpVal);
#else
                U32 tmpVal = *reinterpret_cast<U32*>( aOplandHead[0] );
                tmpVal = ( tmpVal & 0x007FFFFF ) | ( ( tmpVal << 8 ) & 0x80000000 ); // 23bit + 1bit(sign bit)
                aDest = *reinterpret_cast<S32*>(&tmpVal);
#endif
                oplandSize = 4;
            }
        }
        else
        {// 16 bit
            aDest = *reinterpret_cast<S16*>( aOplandHead[1] );
            oplandSize = 3;
        }
    }
    else
    {// 11bit
        U32 tmpVal = aOplandHead[1] + ( static_cast<U16>( aOplandHead[0] >> 5 ) << 8 );
        tmpVal = ( tmpVal & 0x00003FFF ) | ( ( tmpVal << 21 ) & 0x80000000 );
        aDest = *reinterpret_cast<S32*>( &tmpVal );
        oplandSize = 3;
    }
    return oplandSize;
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeA1( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeA , U8 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeA2( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeA , U16 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeA4( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeA , U32 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeA8( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeA , U64 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeP1( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeP , U8 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeP2( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeP , U16 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeP4( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeP , U32 >( aDest , aOplandHead );
}

//------------------------------------------------------------
U32 Processor::oplandDataTypeP8( void* aDest , const U8* aOplandHead )const
{
    return oplandDataTemplate< TypeValueKind_TypeP , U64 >( aDest , aOplandHead );
}

//------------------------------------------------------------
void* Processor::symbolLabelToAddress( const LabelId aLabel )const
{
    ByteCode& byteCode = mProperty.currentByteCodeRef();
    if ( byteCode.isLocalSymbolLabel( aLabel ) )
    {// ローカルシンボル
        return byteCode.offsetToAddress( byteCode.labelIdToLocalSymbolOffset( aLabel ) );
    }

    // グローバルシンボル
    const SymbolInfo symbolInfo = byteCode.labelIdToGlobalSymbolInfo( aLabel );
    if ( symbolInfo.isInternalSymbol() )
    {// 内部シンボル
        ByteCode* byteCodePtr = mProperty.byteCodeReposRef.ptr( symbolInfo.byteCodeId() );
        if ( byteCodePtr == 0 )
        {// 無効なバイトコードID。
            return 0;
        }
        if ( !byteCodePtr->isLocalSymbolLabel( symbolInfo.labelId() ) )
        {// ローカルシンボルじゃない。
            return 0;
        }
        const U32 offset = byteCodePtr->labelIdToLocalSymbolOffset( symbolInfo.labelId() );
        if ( !byteCodePtr->isOffsetValid( offset ) )
        {// 無効なオフセット値
            return 0;
        }
        return byteCodePtr->offsetToAddress( offset );
    }

    // 外部シンボル
    return mProperty.procEventHandler.externalData( symbolInfo.extSymbolId() );
}

//------------------------------------------------------------
template< typename ArgType , typename OperationType >
Processor::CmdResult Processor::cmdOperationTemplateA()
{
    ArgType* val;

    U32 oplandSize = 1;
    {// addr
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( &val , &mProperty.context.pc[oplandSize] );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }

    // Calculate
    OperationType::operate( mProperty , oplandSize , *val );
    if ( OperationType::THROW_EXCEPTION 
        && mProperty.context.exception != ProcessorException_None
        )
    {// 例外発生
        return CmdResult_ExceptionOccured;
    }
    // Move pc
    if ( !OperationType::MOVE_PC )
    {// 自動で進める
        mProperty.context.pc += oplandSize;
    }

    return CmdResult_NoError;
}

//------------------------------------------------------------
template< typename ArgType , typename OperationType >
Processor::CmdResult Processor::cmdOperationTemplateS()
{
    ArgType label;

    U32 oplandSize = 1;
    {// label
        label = *reinterpret_cast<const ArgType*>( &mProperty.context.pc[oplandSize] );
        oplandSize += sizeof(ArgType);
    }

    // Calculate
    OperationType::operate( mProperty , oplandSize , label );
    if ( OperationType::THROW_EXCEPTION 
        && mProperty.context.exception != ProcessorException_None
        )
    {// 例外発生
        return CmdResult_ExceptionOccured;
    }
    // Move pc
    if ( !OperationType::MOVE_PC )
    {// 自動で進める
        mProperty.context.pc += oplandSize;
    }

    return CmdResult_NoError;
}

//------------------------------------------------------------
template< typename ArgType , typename OperationType >
Processor::CmdResult Processor::cmdOperationTemplatePA()
{
    ArgType* addr;
    ArgType  val;

    U32 oplandSize = 1;
    {// addr
        const U32 currentOplandSize = oplandDataTypeP4( &addr , &mProperty.context.pc[oplandSize] );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// val
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &val , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }

    // Calculate
    OperationType::operate( mProperty , oplandSize , *addr , val );
    if ( OperationType::THROW_EXCEPTION 
        && mProperty.context.exception != ProcessorException_None
        )
    {// 例外発生
        return CmdResult_ExceptionOccured;
    }
    // Move pc
    if ( !OperationType::MOVE_PC )
    {// 自動で進める
        mProperty.context.pc += oplandSize;
    }

    return CmdResult_NoError;
}

//------------------------------------------------------------
template< typename ArgType , typename OperationType >
Processor::CmdResult Processor::cmdOperationTemplatePAA()
{
    ArgType* addr;
    ArgType  lhs,rhs;

    U32 oplandSize = 1;
    {// addr
        const U32 currentOplandSize = oplandDataTypeP4( &addr , &mProperty.context.pc[oplandSize] );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// lhs
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &lhs , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// rhs
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &rhs , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }

    // Calculate
    OperationType::operate( mProperty , oplandSize , *addr , lhs , rhs );
    if ( OperationType::THROW_EXCEPTION 
        && mProperty.context.exception != ProcessorException_None
        )
    {// 例外発生
        return CmdResult_ExceptionOccured;
    }
    // Move pc
    if ( !OperationType::MOVE_PC )
    {// 自動で進める
        mProperty.context.pc += oplandSize;
    }

    return CmdResult_NoError;
}

//------------------------------------------------------------
template< typename ArgType , typename OperationType >
Processor::CmdResult Processor::cmdOperationTemplatePPA()
{
    ArgType* addr1;
    ArgType* addr2;
    ArgType  val;

    U32 oplandSize = 1;
    {// addr1
        const U32 currentOplandSize = oplandDataTypeP4( &addr1 , &mProperty.context.pc[oplandSize] );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// addr2
        const U32 currentOplandSize = oplandDataTypeP4( &addr2 , &mProperty.context.pc[oplandSize] );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// val
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &val , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }

    // Calculate
    OperationType::operate( mProperty , oplandSize , *addr1 , *addr2 , val );
    if ( OperationType::THROW_EXCEPTION 
        && mProperty.context.exception != ProcessorException_None
        )
    {// 例外発生
        return CmdResult_ExceptionOccured;
    }
    // Move pc
    if ( !OperationType::MOVE_PC )
    {// 自動で進める
        mProperty.context.pc += oplandSize;
    }

    return CmdResult_NoError;
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdAddTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorAdd< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddI1()
{
    return cmdAddTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddI2()
{
    return cmdAddTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddI4()
{
    return cmdAddTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddI8()
{
    return cmdAddTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddF4()
{
    return cmdAddTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdAddF8()
{
    return cmdAddTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdSubTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorSub< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubI1()
{
    return cmdSubTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubI2()
{
    return cmdSubTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubI4()
{
    return cmdSubTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubI8()
{
    return cmdSubTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubF4()
{
    return cmdSubTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSubF8()
{
    return cmdSubTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdMulTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorMul< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulS1()
{
    return cmdMulTemplate< S8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulS2()
{
    return cmdMulTemplate< S16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulS4()
{
    return cmdMulTemplate< S32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulS8()
{
    return cmdMulTemplate< S64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulU1()
{
    return cmdMulTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulU2()
{
    return cmdMulTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulU4()
{
    return cmdMulTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulU8()
{
    return cmdMulTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulF4()
{
    return cmdMulTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMulF8()
{
    return cmdMulTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdDivTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorDiv< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivS1()
{
    return cmdDivTemplate< S8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivS2()
{
    return cmdDivTemplate< S16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivS4()
{
    return cmdDivTemplate< S32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivS8()
{
    return cmdDivTemplate< S64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivU1()
{
    return cmdDivTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivU2()
{
    return cmdDivTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivU4()
{
    return cmdDivTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivU8()
{
    return cmdDivTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivF4()
{
    return cmdDivTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDivF8()
{
    return cmdDivTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdModTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorMod< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModS1()
{
    return cmdModTemplate< S8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModS2()
{
    return cmdModTemplate< S16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModS4()
{
    return cmdModTemplate< S32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModS8()
{
    return cmdModTemplate< S64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModU1()
{
    return cmdModTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModU2()
{
    return cmdModTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModU4()
{
    return cmdModTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdModU8()
{
    return cmdModTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType , bool IsFloat >
Processor::CmdResult Processor::cmdCmpTemplate()
{
    ArgType  lhs,rhs;

    U32 oplandSize = 1;
    {// lhs
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &lhs , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }
    {// rhs
        const U32 currentOplandSize = oplandDataTemplate< TypeValueKind_TypeA , ArgType >( 
            &rhs , &mProperty.context.pc[oplandSize] 
            );
        if ( currentOplandSize == 0 )
        {
            return CmdResult_IllegalOperationLand;
        }
        oplandSize += currentOplandSize;
    }

    // Calculate
    mProperty.context.cr.r[ Context::CR_Equal ] = lhs == rhs;
    mProperty.context.cr.r[ Context::CR_Less ]  = lhs < rhs;
    if ( IsFloat )
    {
        mProperty.context.cr.r[ Context::CR_NaN ] = 
            lhs == std::numeric_limits< ArgType >::quiet_NaN()
            || rhs == std::numeric_limits< ArgType >::quiet_NaN();
    }
    else
    {
        mProperty.context.cr.r[ Context::CR_NaN ] = 0;
    }
    mProperty.context.pc += oplandSize;

    return CmdResult_NoError;
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpS1()
{
    return cmdCmpTemplate< S8 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpS2()
{
    return cmdCmpTemplate< S16 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpS4()
{
    return cmdCmpTemplate< S32 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpS8()
{
    return cmdCmpTemplate< S64 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpU1()
{
    return cmdCmpTemplate< U8 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpU2()
{
    return cmdCmpTemplate< U16 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpU4()
{
    return cmdCmpTemplate< U32 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpU8()
{
    return cmdCmpTemplate< U64 , false >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpF4()
{
    return cmdCmpTemplate< F32 , true >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCmpF8()
{
    return cmdCmpTemplate< F64 , true >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdIncTemplate()
{
    return cmdOperationTemplateP< ArgType , OperatorInc< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncI1()
{
    return cmdIncTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncI2()
{
    return cmdIncTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncI4()
{
    return cmdIncTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncI8()
{
    return cmdIncTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncF4()
{
    return cmdIncTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdIncF8()
{
    return cmdIncTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdDecTemplate()
{
    return cmdOperationTemplateP< ArgType , OperatorDec< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecI1()
{
    return cmdDecTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecI2()
{
    return cmdDecTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecI4()
{
    return cmdDecTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecI8()
{
    return cmdDecTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecF4()
{
    return cmdDecTemplate< F32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdDecF8()
{
    return cmdDecTemplate< F64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdCopyTemplate()
{
    return cmdOperationTemplatePA< ArgType , OperatorCopy< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCopy1()
{
    return cmdCopyTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCopy2()
{
    return cmdCopyTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCopy4()
{
    return cmdCopyTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCopy8()
{
    return cmdCopyTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdMCopy4()
{
    return cmdOperationTemplatePPA< U32 , OperatorMCopy<U32> >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdSLLTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorSLL< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSLL1()
{
    return cmdSLLTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSLL2()
{
    return cmdSLLTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSLL4()
{
    return cmdSLLTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSLL8()
{
    return cmdSLLTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdSRLTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorSRL< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSRL1()
{
    return cmdSRLTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSRL2()
{
    return cmdSRLTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSRL4()
{
    return cmdSRLTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdSRL8()
{
    return cmdSRLTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdBAndTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorBAnd< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBAnd1()
{
    return cmdBAndTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBAnd2()
{
    return cmdBAndTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBAnd4()
{
    return cmdBAndTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBAnd8()
{
    return cmdBAndTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdBNotTemplate()
{
    return cmdOperationTemplatePA< ArgType , OperatorBNot< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBNot1()
{
    return cmdBNotTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBNot2()
{
    return cmdBNotTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBNot4()
{
    return cmdBNotTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBNot8()
{
    return cmdBNotTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdBOrTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorBOr< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBOr1()
{
    return cmdBOrTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBOr2()
{
    return cmdBOrTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBOr4()
{
    return cmdBOrTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBOr8()
{
    return cmdBOrTemplate< U64 >();
}

//------------------------------------------------------------
template< typename ArgType >
Processor::CmdResult Processor::cmdBXorTemplate()
{
    return cmdOperationTemplatePAA< ArgType , OperatorBXor< ArgType > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBXor1()
{
    return cmdBXorTemplate< U8 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBXor2()
{
    return cmdBXorTemplate< U16 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBXor4()
{
    return cmdBXorTemplate< U32 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdBXor8()
{
    return cmdBXorTemplate< U64 >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdEnter()
{
    return cmdOperationTemplateA< U32 , OperatorEnter >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdLeave()
{
    return cmdOperationTemplateA< U32 , OperatorLeave >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCallS()
{
    return cmdOperationTemplateS< LabelId , OperatorCallStatic >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdCallD()
{
    return cmdOperationTemplateA< SymbolId , OperatorCallDynamic >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpS()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorAlways > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSE()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSNE()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorNotEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSL()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorLess > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSLE()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorLessEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSU()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorUnvalue > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpSNU()
{
    return cmdOperationTemplateS< LabelId , OperatorJump< ComparatorNotUnvalue > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpD()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorAlways > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDE()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDNE()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorNotEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDL()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorLess > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDLE()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorLessEqual > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDU()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorUnvalue > >();
}

//------------------------------------------------------------
Processor::CmdResult Processor::cmdJumpDNU()
{
    return cmdOperationTemplateA< LabelId , OperatorJump< ComparatorNotUnvalue > >();
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
