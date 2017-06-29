/**
 * @file
 * @brief ProcessorEventHandler型を記述する。
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

    /// Processorのイベントハンドラクラス。
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
