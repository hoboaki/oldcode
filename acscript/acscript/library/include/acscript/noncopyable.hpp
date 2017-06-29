/**
 * @file
 * @brief Noncopyable型を記述する。
 */
#ifndef ACS_INCLUDE_NONCOPYABLE
#define ACS_INCLUDE_NONCOPYABLE

//------------------------------------------------------------
namespace acscript {

    /// Noncopyable。
    class Noncopyable
    {
    protected:
        Noncopyable(){}
    private:
        Noncopyable( const Noncopyable& );
    };

}
//------------------------------------------------------------
#endif
// EOF
