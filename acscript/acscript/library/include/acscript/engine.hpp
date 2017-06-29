/**
 * @file
 * @brief Engine型を記述する。
 */
#ifndef ACS_INCLUDE_ENGINE
#define ACS_INCLUDE_ENGINE

//------------------------------------------------------------
namespace acscript {

    class Engine
    {
    private:
        /// シンボル名→シンボルIDのテーブル。
        Map< SymbolName , SymbolId >::BuildTimeType mGlobalSymbolNameToIdTable;
    };

}
//------------------------------------------------------------
#endif
// EOF
