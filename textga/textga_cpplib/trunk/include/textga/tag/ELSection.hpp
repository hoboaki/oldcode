/**
 * @file
 * @brief セクションの要素クラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <textga/tag/AbstractElement.hpp>
#include <vector>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// セクションの要素クラス。
    class ELSection : public AbstractElement
    {
    public:
        /// 無効なインデックス。
        static const uint INVALID_INDEX; 

        /// 名前を指定してセクションを作成。
        explicit ELSection( const char* name );
        virtual ~ELSection();

        /// エレメントをセクションに加える。
        void add( std::auto_ptr< IElement > element );

        /// エレメントの総数を取得する。
        uint count()const;

        /// 指定のindexのエレメントを取得する。
        const IElement& getElementAtIndex( uint index )const;

        /// getElementPtrWithName( name , 0 )のエイリアス。
        const IElement* getElementPtrWithName( const char* name )const;
        /// 指定の名前のエレメントのポインタを取得する。
        const IElement* getElementPtrWithName( const char* name , uint fromIndex )const;

        /// getIndexWithName( name , 0 )のエイリアス。
        uint getIndexWithName( const char* name )const;
        /**
         * @brief 指定の名前のエレメントのindexを取得する。
         * @return index。見つからなければINVALID_INDEXを返す。
         * @param name エレメントの名前。
         * @param fromIndex 探索を開始するインデックス値。
         */
        uint getIndexWithName( const char* name , uint fromIndex )const;

    private:
        std::vector< IElement* > elements_;
    };

}}
//------------------------------------------------------------
// EOF
