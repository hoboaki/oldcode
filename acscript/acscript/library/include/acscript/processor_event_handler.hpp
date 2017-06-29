/**
 * @file
 * @brief ProcessorEventHandler�^���L�q����B
 */
#ifndef ACS_INCLUDE_PROCESSOREVENTHANDLER
#define ACS_INCLUDE_PROCESSOREVENTHANDLER

//------------------------------------------------------------
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {
    class Engine;
}

//------------------------------------------------------------
namespace acscript {

    /// Processor�̃C�x���g�n���h���N���X�B
    class ProcessorEventHandler
    {
    public:
        ProcessorEventHandler();
        ProcessorEventHandler( Engine& aEngine );

        void externalFunctionCall( const ExtSymbolId id )const;
        void* externalData( const ExtSymbolId id )const;

    private:
        Engine* mEnginePtr;
    };

}
//------------------------------------------------------------
#endif
// EOF
