/** 
* @file 
* @brief �����O�o�b�t�@�N���X���L�q����B 
*/
#pragma once

//-----------------------------------------------------------
#include <apcl/debug/Assert.hpp>
#include <apcl/operators/Equalable.hpp>
#include <apcl/util/Types.hpp>

//-----------------------------------------------------------
namespace apcl { namespace container
{
  using ::apcl::operators::Equalable;
  using ::apcl::util::u32;

  /// �����O�o�b�t�@�̃N���X
  template< typename T >
  class Ring : public Equalable< Ring<T> >
  {
  public:
    typedef T ValType; ///< �e���v���[�g�̌^

    /// �e�X�g�R�[�h
    static void unitTest()
    {
      const int aSize = 30;

      Ring<int> ring( aSize );
      apclAssert( ring.size() == 1 );
      apclAssert( ring.sizeMax() == aSize );

      for ( int i = 0; i < aSize; ++i )
      {
        apclAssert( ring.index() == i );
        apclAssert( ring.size() == i+1 );
        ring.at(0) = i;
        ring.next();
      }
      apclAssert( ring.index() == 0 );
      apclAssert( ring.size() == ring.sizeMax() );

      apclAssert( ring.at(0) == 0 );
      for ( int i = 1; i < aSize; ++i )
        apclAssert( ring.at(i) == aSize-i );

      Ring<int> ring2( ring );
      for ( int i = 0; i < aSize; ++i )
        apclAssert( ring.at(i) == ring2.at(i) );

      Ring<int> ring3( 1 );
      ring3 = ring;
      for ( int i = 0; i < aSize; ++i )
        apclAssert( ring.at(i) == ring3.at(i) );
      apclAssert( ring3.at(0) == 0 );
    }

    //=================================================
    /// @name �����E�j��
    //@{
    /// �o�b�t�@�T�C�Y���w�肵�č쐬
    explicit Ring( const u32 aSizeMax ):
    size_( aSizeMax ),
      index_( 0 ),
      loopedFlg_( false )
    {
      apclAssert( aSizeMax > 0 );
      buff_ = new T[size_];
    }

    /// �R�s�[�R���X�g���N�^
    explicit Ring( const Ring<T>& aRing ):
    size_( aRing.sizeMax() ),
      index_( 0 ),
      loopedFlg_( false )
    {
      apclAssert( size_ > 0 );
      buff_ = new T[size_];
      copy( aRing );
    }

    /// �f�X�g���N�^
    virtual ~Ring()	
    {	
      delete[] buff_; 
    }
    //@}

    //=================================================
    /**
     * @name ��������
     * Ring��at(0)����at( size() )�܂ł��S�ē�������Γ����Ɣ��肷��B
     */
    //@{
    /// ���������ǂ����擾����B
    bool equals( const Ring<T>& aRing )const
    {
      if ( size() == aRing.size() )
      {
        const u32 max = size();
        u32 i;
        for ( i = 0; i < max; ++i )
          if ( at( i ) != aRing.at( i ) )
            break;
        if ( i == max )
          return true;
      }
      return false;
    }
    //@}

    //=================================================
    /// @name ����
    //@{
    /// �����̓��e���R�s�[����B
    void copy( const Ring<T>& aRing )
    {
      // ���
      delete[] buff_;

      // ������
      size_ = aRing.sizeMax();
      index_ = aRing.index();
      loopedFlg_ = aRing.size() == aRing.sizeMax();
      buff_ = new T[size_];

      // �R�s�[
      const u32 max = aRing.size();
      for ( u32 i = 0; i < max; ++i )
      {
        const u32 index = index_ >= i ? index_-i : size_-(i-index_);
        buff_[index] = aRing.at(i);
      }
    }
    //@}

    //=================================================
    /// @name �T�C�Y
    //@{
    /// ���݂̃T�C�Y���擾����B
    u32 size()const	{	return loopedFlg_ ? size_ : index_+1; }
    /// �ő�T�C�Y���擾����B
    u32 sizeMax()const	{	return size_; }
    //@}

    //=================================================
    /// @name �C���f�b�N�X
    //@{
    /// ���ݎw���Ă�o�b�t�@�̃C���f�b�N�X�l���擾����B
    u32 index()const { return index_; }
    /// ���ݎw���Ă���o�b�t�@�̃C���f�b�N�X���P�ړ�����B
    void next()
    {
      if ( index_ == size_ -1 )
      {
        loopedFlg_ = true;
        index_ = 0;
      }
      else
        ++index_;
      apclAssert( index_ >=0 && index_ < size_ );
    }
    //@}

    //=================================================
    /**
    * @name �Q�Ǝ擾
    * <pre>
    * index = 0���C���ݎw���Ă�o�b�t�@�̃C���f�b�N�X�B
    * �C���f�b�N�X�l��������قǁC�ߋ��ɂ����̂ڂ�B
    * �܂��C�C���f�b�N�X�l��0�ȏ�Csize()�����ł���K�v������B
    * </pre>
    */
    //@{
    /// �v�f�̎Q�Ƃ��擾����B
    T& at( const u32 aIndex )
    {
      apclAssert( aIndex >= 0 && aIndex < size() );
      u32 index;
      if ( aIndex <= index_ )
        index = index_ - aIndex;
      else
      {
        const u32 subMemo = aIndex - index_;
        index = size_ - subMemo;
      }
      apclAssert( index < size_ );
      return buff_[index];
    }
    /// at(aIndex)��const��
    const T& at( const u32 aIndex )const
    {
      apclAssert( aIndex >= 0 && aIndex < size() );
      u32 index;
      if ( aIndex <= index_ )
        index = index_ - aIndex;
      else
      {
        const u32 subMemo = aIndex - index_;
        index = size_ - subMemo;
      }
      apclAssert( index < size_ );
      return buff_[index];
    }
    //@}

    //=================================================
    /// @name ���Z�q
    //@{
    /// at()�̃J�o�[
    T& operator[]( const u32 aIndex ){ return at(aIndex); } 
    /// at()�̃J�o�[
    const T& operator[]( const u32 aIndex )const{ return at(aIndex); } 
    /// copy()�̃J�o�[
    Ring<T>& operator=( const Ring<T>& aRing )
    { 
      copy( aRing ); 
      return *this; 
    }
    //@}

  private:
    T*	buff_;
    u32 size_;
    u32 index_;
    bool loopedFlg_;
  };


}} // end of namespace ::apcl::container

//-----------------------------------------------------------
// EOF
