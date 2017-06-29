/**
 * @file
 * @brief Engine�^���L�q����B
 */
#ifndef ACS_INCLUDE_ENGINE
#define ACS_INCLUDE_ENGINE

//------------------------------------------------------------
namespace acscript {

    class Engine
    {
    private:
        /// �V���{�������V���{��ID�̃e�[�u���B
        Map< SymbolName , SymbolId >::BuildTimeType mGlobalSymbolNameToIdTable;
    };

}
//------------------------------------------------------------
#endif
// EOF
