/**
 * @file
 * @brief ProcessorException型を記述する。
 */
#ifndef ACS_INCLUDE_PROCESSOR_EXCEPTION
#define ACS_INCLUDE_PROCESSOR_EXCEPTION

//------------------------------------------------------------
namespace acscript {

    /// Processor用例外。
    enum ProcessorException
    {
        ProcessorException_None ///< 例外無し
        , ProcessorException_DivByZero ///< 0割り算
        , ProcessorException_StackOverFlow ///< スタックオーバーフロー
        , ProcessorException_InvalidSymbol ///< 無効なシンボル
        , ProcessorException_OutOfByteCode ///< バイトコード外。
    };

}
//------------------------------------------------------------
#endif
// EOF
