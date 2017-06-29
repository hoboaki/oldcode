/**
 * @file
 * @brief BinaryData型を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <map>
#include <vector>
#include "app/BDOLabel.hpp"

//------------------------------------------------------------
namespace app {

    /// バイナリデータ。
    class BinaryData
    {
    public:
        explicit BinaryData( U32 reserveBytes );

        /// データの追加。
        void add( const void* addr , U32 size );
        void addU32( U32 val );
        /// BinaryDataOffsetの追加。
        void addBDOLabel( const BDOLabel& label );
        /// 今から該当ラベルのデータを挿入することを宣言する。
        void reserveAddBDOEntity( const BDOLabel& );
        /// reserveAddBDOEntity + addのエイリアス
        void addBDOEntity( const BDOLabel& , const void* addr , U32 size );
        /// reserveAddBDOEntity + addU32のエイリアス
        void addBDOEntityU32( const BDOLabel& , U32 val );
        /// reserveAddBDOEntity + addBDOLabelのエイリアス
        void addBDOEntityBDOLabel( const BDOLabel& reserve , const BDOLabel& add );

        /// ファイルに書き出す。
        bool write( const char* filepath );

    private:
        typedef std::map< BDOLabel , U32 > LabelMap;
        std::vector< Byte > bytes_;
        LabelMap labelMap_;
        bool isReserved_;
        BDOLabel reservedLabel_;
    };

}
//------------------------------------------------------------
// EOF
