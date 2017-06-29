/**
 * @file
 * @brief ContextStack型を記述する。
 */
#ifndef ACS_INCLUDE_CONTEXTSTACK
#define ACS_INCLUDE_CONTEXTSTACK

//------------------------------------------------------------
#include <acscript/noncopyable.hpp>
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// Context用Stack。
    class ContextStack : public ::acscript::Noncopyable
    {
    public:
        explicit ContextStack( U32 stackSize );
        ~ContextStack();

        /// 先頭アドレスを取得する。(Most lower address pointer)
        U8* head()const
        {
            return mHead;
        }
        /// 最終アドレスを取得する。(Most upper address pointer)
        U8* tail()const
        {
            return mHead + (mSize-1);
        }

        /// スタックサイズを取得する。
        U32 size()const
        {
            return mSize;
        }

    private:
        U8* mHead; ///< Most low address pointer.
        U32 mSize; ///< Stack size.
    };

}
//------------------------------------------------------------
#endif
// EOF
