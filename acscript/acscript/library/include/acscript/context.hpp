/**
 * @file
 * @brief Contextå^ÇãLèqÇ∑ÇÈÅB
 */
#ifndef ACS_INCLUDE_CONTEXT
#define ACS_INCLUDE_CONTEXT

//------------------------------------------------------------
#include <acscript/types.hpp>
#include <acscript/processor_exception.hpp>

//------------------------------------------------------------
namespace acscript {
    struct ByteCode;
}

//------------------------------------------------------------
namespace acscript {

    /// Processor Context.
    class Context
    {
    public:
        Context()
            : bcp()
            , pc()
            , bp()
            , sp()
            , cs()
            , ri()
            , gr()
            , cr()
            , exception()
        {
        }

        enum { GR_COUNT = 32 };
        /// General register.
        struct GeneralRegister
        {
            union
            {
                U8  r[ GR_COUNT ];
                U8  r1[ GR_COUNT ];
                U16 r2[ GR_COUNT/2 ];
                U32 r4[ GR_COUNT/4 ];
                U64 r8[ GR_COUNT/8 ];
            };
        };
        enum 
        {
            CR_Equal // ìôÇµÇ¢
            , CR_Less // ÇÊÇËè¨Ç≥Ç¢
            , CR_NaN // Ç«ÇøÇÁÇ©àÍï˚Ç™NaN
            , CR_Empty // ãÛÇ´
            //
            , CR_Terminate
        };
        /// Compare register.
        struct CompareRegister
        {
            U8 r[ CR_Terminate ];
        };
        /// Return info.
        struct ReturnInfo
        {
            SymbolId symbol;
            U8* pc;
        };
        ByteCode* bcp; ///< Current byte code pointer.
        U8* pc; ///< Program counter.
        U8* bp; ///< Base pointer.
        U8* sp; ///< Stack pointer.
        SymbolId cs; ///< Current symbol.
        ReturnInfo ri; ///< Return info.
        GeneralRegister gr; ///< General register.
        CompareRegister cr; ///< Compare register.
        ProcessorException exception; ///< Processor exception.
    };

}
//------------------------------------------------------------
#endif
// EOF
