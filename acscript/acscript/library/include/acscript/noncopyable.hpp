/**
 * @file
 * @brief Noncopyable�^���L�q����B
 */
#ifndef ACS_INCLUDE_NONCOPYABLE
#define ACS_INCLUDE_NONCOPYABLE

//------------------------------------------------------------
namespace acscript {

    /// Noncopyable�B
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
