/**
 * @file
 * @brief ProcessorException�^���L�q����B
 */
#ifndef ACS_INCLUDE_PROCESSOR_EXCEPTION
#define ACS_INCLUDE_PROCESSOR_EXCEPTION

//------------------------------------------------------------
namespace acscript {

    /// Processor�p��O�B
    enum ProcessorException
    {
        ProcessorException_None ///< ��O����
        , ProcessorException_DivByZero ///< 0����Z
        , ProcessorException_StackOverFlow ///< �X�^�b�N�I�[�o�[�t���[
        , ProcessorException_InvalidSymbol ///< �����ȃV���{��
        , ProcessorException_OutOfByteCode ///< �o�C�g�R�[�h�O�B
    };

}
//------------------------------------------------------------
#endif
// EOF
