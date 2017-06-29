/**
 * @file
 * @brief Processor�^���L�q����B
 */
#ifndef ACS_INCLUDE_PROCESSOR
#define ACS_INCLUDE_PROCESSOR

//------------------------------------------------------------
#include <acscript/context.hpp>
#include <acscript/context_stack.hpp>
#include <acscript/noncopyable.hpp>
#include <acscript/processor_event_handler.hpp>

//------------------------------------------------------------
namespace acscript {
    class ByteCodeRepository;
    struct ByteCode;
    struct SymbolInfo;
}

//------------------------------------------------------------
namespace acscript {

    /// �v���Z�b�T�[�B
    class Processor : public ::acscript::Noncopyable
    {
    public:

        /// �R���X�g���N�^�B
        Processor( const ByteCodeRepository& byteCodeReposRef
            , const ProcessorEventHandler& procEventHandler
            , U32 stackSize 
            );

        void call( const SymbolId& );
        void process();

        /// �v���Z�b�T�̃v���p�e�B�B
        class Property
        {
        public:
            Property( const ByteCodeRepository& byteCodeReposRef
                , const ProcessorEventHandler& procEventHandler
                , U32 stackSize 
                );

            const ByteCodeRepository& byteCodeReposRef;
            const ProcessorEventHandler procEventHandler;
            Context context;
            ContextStack   stack;
        
            /// ���ݏ������̃o�C�g�R�[�h�̎Q�Ƃ��擾����B
            ByteCode& currentByteCodeRef()const;
        };
    private:
        Property mProperty;
        //------------------------------------------------------------
        enum CmdResult
        {
            CmdResult_NoError
            , CmdResult_IllegalOperationCode ///< �����ȃI�y�R�[�h�B
            , CmdResult_IllegalOperationLand ///< �����ȃI�y�����h�B
            , CmdResult_ExceptionOccured ///< ��O�����������B
        };
        /// �R�}���h�֐��̃|�C���^�B
        typedef CmdResult (Processor::* CommandFunc)();
        /// �R�}���h�֐��̃|�C���^�e�[�u���B
        class CommandFuncTable
        {
        public:
            CommandFuncTable();
            CommandFunc func[ 256 ];
        };
        static CommandFuncTable createFirstCommandFuncTable(); // ��1�R�}���h�p�e�[�u���쐬�֐��B
        static CommandFuncTable createCastCommandFuncTable(); // �L���X�g�R�}���h�e�[�u���쐬�֐��B
        /**
         * @name �I�y�����h��́B
         * �߂�l�́A��͂��ꂽ�I�y�����h�̃T�C�Y�B
         * �s���ȃI�y�����h�Ɣ��肵���ꍇ�A0��Ԃ��B
         */
        //@{
        template< int TypeValue , typename ResultType >
        U32 oplandDataTemplate( void* dest , const U8* oplandHead )const;
        U32 calculateRelativeAddr( S32& dest , const U8* oplandHead )const;
        U32 oplandDataTypeA1( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeA2( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeA4( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeA8( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeP1( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeP2( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeP4( void* dest , const U8* oplandHead )const;
        U32 oplandDataTypeP8( void* dest , const U8* oplandHead )const;
        //@}
        /// ���x������V���{���̃A�h���X���擾����B
        void* symbolLabelToAddress( LabelId label )const;
        // 1�̃R�}���h����������B
        CmdResult processOneCommand();
        // OperationTemplate
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplateA();
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplateS();
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplateP();
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplatePA();
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplatePAA();
        template< typename ArgType , typename OperationType >
        CmdResult cmdOperationTemplatePPA();
        // add
        template< typename ArgType >
        CmdResult cmdAddTemplate();
        CmdResult cmdAddI1();
        CmdResult cmdAddI2();
        CmdResult cmdAddI4();
        CmdResult cmdAddI8();
        CmdResult cmdAddF4();
        CmdResult cmdAddF8();
        // sub
        template< typename ArgType >
        CmdResult cmdSubTemplate();
        CmdResult cmdSubI1();
        CmdResult cmdSubI2();
        CmdResult cmdSubI4();
        CmdResult cmdSubI8();
        CmdResult cmdSubF4();
        CmdResult cmdSubF8();
        // mul
        template< typename ArgType >
        CmdResult cmdMulTemplate();
        CmdResult cmdMulS1();
        CmdResult cmdMulS2();
        CmdResult cmdMulS4();
        CmdResult cmdMulS8();
        CmdResult cmdMulU1();
        CmdResult cmdMulU2();
        CmdResult cmdMulU4();
        CmdResult cmdMulU8();
        CmdResult cmdMulF4();
        CmdResult cmdMulF8();
        // div
        template< typename ArgType >
        CmdResult cmdDivTemplate();
        CmdResult cmdDivS1();
        CmdResult cmdDivS2();
        CmdResult cmdDivS4();
        CmdResult cmdDivS8();
        CmdResult cmdDivU1();
        CmdResult cmdDivU2();
        CmdResult cmdDivU4();
        CmdResult cmdDivU8();
        CmdResult cmdDivF4();
        CmdResult cmdDivF8();
        // mod
        template< typename ArgType >
        CmdResult cmdModTemplate();
        CmdResult cmdModS1();
        CmdResult cmdModS2();
        CmdResult cmdModS4();
        CmdResult cmdModS8();
        CmdResult cmdModU1();
        CmdResult cmdModU2();
        CmdResult cmdModU4();
        CmdResult cmdModU8();
        // cmp
        template< typename ArgType , bool IsFloat >
        CmdResult cmdCmpTemplate();
        CmdResult cmdCmpS1();
        CmdResult cmdCmpS2();
        CmdResult cmdCmpS4();
        CmdResult cmdCmpS8();
        CmdResult cmdCmpU1();
        CmdResult cmdCmpU2();
        CmdResult cmdCmpU4();
        CmdResult cmdCmpU8();
        CmdResult cmdCmpF4();
        CmdResult cmdCmpF8();
        // inc
        template< typename ArgType >
        CmdResult cmdIncTemplate();
        CmdResult cmdIncI1();
        CmdResult cmdIncI2();
        CmdResult cmdIncI4();
        CmdResult cmdIncI8();
        CmdResult cmdIncF4();
        CmdResult cmdIncF8();
        // dec
        template< typename ArgType >
        CmdResult cmdDecTemplate();
        CmdResult cmdDecI1();
        CmdResult cmdDecI2();
        CmdResult cmdDecI4();
        CmdResult cmdDecI8();
        CmdResult cmdDecF4();
        CmdResult cmdDecF8();
        // copy
        template< typename ArgType >
        CmdResult cmdCopyTemplate();
        CmdResult cmdCopy1();
        CmdResult cmdCopy2();
        CmdResult cmdCopy4();
        CmdResult cmdCopy8();
        // mcopy
        CmdResult cmdMCopy4();
        // sll
        template< typename ArgType >
        CmdResult cmdSLLTemplate();
        CmdResult cmdSLL1();
        CmdResult cmdSLL2();
        CmdResult cmdSLL4();
        CmdResult cmdSLL8();
        // srl
        template< typename ArgType >
        CmdResult cmdSRLTemplate();
        CmdResult cmdSRL1();
        CmdResult cmdSRL2();
        CmdResult cmdSRL4();
        CmdResult cmdSRL8();
        // band
        template< typename ArgType >
        CmdResult cmdBAndTemplate();
        CmdResult cmdBAnd1();
        CmdResult cmdBAnd2();
        CmdResult cmdBAnd4();
        CmdResult cmdBAnd8();
        // bnot
        template< typename ArgType >
        CmdResult cmdBNotTemplate();
        CmdResult cmdBNot1();
        CmdResult cmdBNot2();
        CmdResult cmdBNot4();
        CmdResult cmdBNot8();
        // bor
        template< typename ArgType >
        CmdResult cmdBOrTemplate();
        CmdResult cmdBOr1();
        CmdResult cmdBOr2();
        CmdResult cmdBOr4();
        CmdResult cmdBOr8();
        // bxor
        template< typename ArgType >
        CmdResult cmdBXorTemplate();
        CmdResult cmdBXor1();
        CmdResult cmdBXor2();
        CmdResult cmdBXor4();
        CmdResult cmdBXor8();
        // enter / leave
        CmdResult cmdEnter();
        CmdResult cmdLeave();
        // call
        CmdResult cmdCallS();
        CmdResult cmdCallD();
        // jump
        CmdResult cmdJumpS();
        CmdResult cmdJumpSE();
        CmdResult cmdJumpSNE();
        CmdResult cmdJumpSL();
        CmdResult cmdJumpSLE();
        CmdResult cmdJumpSU();
        CmdResult cmdJumpSNU();
        CmdResult cmdJumpD();
        CmdResult cmdJumpDE();
        CmdResult cmdJumpDNE();
        CmdResult cmdJumpDL();
        CmdResult cmdJumpDLE();
        CmdResult cmdJumpDU();
        CmdResult cmdJumpDNU();
        // cast
        CmdResult cmdCast();
    };

}
//------------------------------------------------------------
#endif
// EOF
