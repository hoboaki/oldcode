/**
 * @file
 * @brief STL�pAllocator���L�q����B
 */
#ifndef ACS_INCLUDE_STLALLOCATOR
#define ACS_INCLUDE_STLALLOCATOR

//------------------------------------------------------------
#include <acscript/allocator.hpp>
#include <acscript/memory.hpp>

//------------------------------------------------------------
namespace acscript {

    /// STLAllocator�̂��߂̃e���v���[�g�B
    template <class T,typename AllocatorType>
    class STLAllocatorTmpl
    {
    public:
	    // �^��`
	    typedef size_t size_type;
	    typedef ptrdiff_t difference_type;
	    typedef T* pointer;
	    typedef const T* const_pointer;
	    typedef T& reference;
	    typedef const T& const_reference;
	    typedef T value_type;
    	
	    // �A���P�[�^��U�^�Ƀo�C���h����
	    template <class U>
	    struct rebind
	    {
		    typedef STLAllocatorTmpl< U, AllocatorType > other;
	    };
    	
	    // �R���X�g���N�^
	    STLAllocatorTmpl() throw(){}
	    STLAllocatorTmpl(const STLAllocatorTmpl&) throw(){}
	    template <class U> STLAllocatorTmpl(const STLAllocatorTmpl<U,AllocatorType>&) throw(){}
	    // �f�X�g���N�^
	    ~STLAllocatorTmpl() throw(){}
    	
	    // �����������蓖�Ă�
	    pointer allocate(size_type num, const_pointer hint = 0)
	    {
            return (pointer)( AllocatorType::allocator().alloc( num * sizeof(T) ) );
	    }
	    // �����čς݂̗̈������������
	    void construct(pointer p, const T& value)
	    {
		    new( (void*)p ) T(value);
	    }
    	
	    // ���������������
	    void deallocate(pointer p, size_type num)
	    {
		    AllocatorType::allocator().free( (void*)p );
	    }
	    // �������ς݂̗̈���폜����
	    void destroy(pointer p)
	    {
		    p->~T();
	    }
    	
	    // �A�h���X��Ԃ�
	    pointer address(reference value) const { return &value; }
	    const_pointer address(const_reference value) const { return &value; }
    	
	    // �����Ă邱�Ƃ��ł���ő�̗v�f����Ԃ�
	    size_type max_size() const throw()
	    {
		    return numeric_limits<size_t>::max() / sizeof(T);
	    }
    };
    template <class TA1, class TA2, class TB1, class TB2>
    bool operator==(const STLAllocatorTmpl<TA1,TA2>&, const STLAllocatorTmpl<TB1,TB2>&) throw() { return true; }

    template <class TA1, class TA2, class TB1, class TB2>
    bool operator!=(const STLAllocatorTmpl<TA1,TA2>&, const STLAllocatorTmpl<TB1,TB2>&) throw() { return false; }

    /// STLAllocator�̂��߂̃A�_�v�^�N���X�Q�B
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

    /// BuildTime�pSTLAllocator�B
    template< typename ClassType >
    struct STLAllocatorForBuildTime
    {
        typedef STLAllocatorTmpl< ClassType , STLAllocatorAdapter::BuildTime > Allocator;
    };

}
//------------------------------------------------------------
#endif
// EOF
