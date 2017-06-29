/**
 * @file
 * @brief STL用Allocatorを記述する。
 */
#ifndef ACS_INCLUDE_STLALLOCATOR
#define ACS_INCLUDE_STLALLOCATOR

//------------------------------------------------------------
#include <acscript/allocator.hpp>
#include <acscript/memory.hpp>

//------------------------------------------------------------
namespace acscript {

    /// STLAllocatorのためのテンプレート。
    template <class T,typename AllocatorType>
    class STLAllocatorTmpl
    {
    public:
	    // 型定義
	    typedef size_t size_type;
	    typedef ptrdiff_t difference_type;
	    typedef T* pointer;
	    typedef const T* const_pointer;
	    typedef T& reference;
	    typedef const T& const_reference;
	    typedef T value_type;
    	
	    // アロケータをU型にバインドする
	    template <class U>
	    struct rebind
	    {
		    typedef STLAllocatorTmpl< U, AllocatorType > other;
	    };
    	
	    // コンストラクタ
	    STLAllocatorTmpl() throw(){}
	    STLAllocatorTmpl(const STLAllocatorTmpl&) throw(){}
	    template <class U> STLAllocatorTmpl(const STLAllocatorTmpl<U,AllocatorType>&) throw(){}
	    // デストラクタ
	    ~STLAllocatorTmpl() throw(){}
    	
	    // メモリを割り当てる
	    pointer allocate(size_type num, const_pointer hint = 0)
	    {
            return (pointer)( AllocatorType::allocator().alloc( num * sizeof(T) ) );
	    }
	    // 割当て済みの領域を初期化する
	    void construct(pointer p, const T& value)
	    {
		    new( (void*)p ) T(value);
	    }
    	
	    // メモリを解放する
	    void deallocate(pointer p, size_type num)
	    {
		    AllocatorType::allocator().free( (void*)p );
	    }
	    // 初期化済みの領域を削除する
	    void destroy(pointer p)
	    {
		    p->~T();
	    }
    	
	    // アドレスを返す
	    pointer address(reference value) const { return &value; }
	    const_pointer address(const_reference value) const { return &value; }
    	
	    // 割当てることができる最大の要素数を返す
	    size_type max_size() const throw()
	    {
		    return numeric_limits<size_t>::max() / sizeof(T);
	    }
    };
    template <class TA1, class TA2, class TB1, class TB2>
    bool operator==(const STLAllocatorTmpl<TA1,TA2>&, const STLAllocatorTmpl<TB1,TB2>&) throw() { return true; }

    template <class TA1, class TA2, class TB1, class TB2>
    bool operator!=(const STLAllocatorTmpl<TA1,TA2>&, const STLAllocatorTmpl<TB1,TB2>&) throw() { return false; }

    /// STLAllocatorのためのアダプタクラス群。
    struct STLAllocatorAdapter
    {
        struct BuildTime
        {
            static Allocator& allocator() 
            {
                return ::acscript::Memory::buildTimeAllocator(); 
            }
        };
    };

    /// BuildTime用STLAllocator。
    template< typename ClassType >
    struct STLAllocatorForBuildTime
    {
        typedef STLAllocatorTmpl< ClassType , STLAllocatorAdapter::BuildTime > Allocator;
    };

}
//------------------------------------------------------------
#endif
// EOF
