/**
 * @file
 * @brief ContextStack�^���L�q����B
 */
#ifndef ACS_INCLUDE_CONTEXTSTACK
#define ACS_INCLUDE_CONTEXTSTACK

//------------------------------------------------------------
#include <acscript/noncopyable.hpp>
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// Context�pStack�B
    class ContextStack : public ::acscript::Noncopyable
    {
    public:
        explicit ContextStack( U32 stackSize );
        ~ContextStack();

        /// �擪�A�h���X���擾����B(Most lower address pointer)
        U8* head()const
        {
            return mHead;
        }
        /// �ŏI�A�h���X���擾����B(Most upper address pointer)
        U8* tail()const
        {
            return mHead + (mSize-1);
        }

        /// �X�^�b�N�T�C�Y���擾����B
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
