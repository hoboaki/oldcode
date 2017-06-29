/**
 * @file
 * @brief BinaryData�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <map>
#include <vector>
#include "app/BDOLabel.hpp"

//------------------------------------------------------------
namespace app {

    /// �o�C�i���f�[�^�B
    class BinaryData
    {
    public:
        explicit BinaryData( U32 reserveBytes );

        /// �f�[�^�̒ǉ��B
        void add( const void* addr , U32 size );
        void addU32( U32 val );
        /// BinaryDataOffset�̒ǉ��B
        void addBDOLabel( const BDOLabel& label );
        /// ������Y�����x���̃f�[�^��}�����邱�Ƃ�錾����B
        void reserveAddBDOEntity( const BDOLabel& );
        /// reserveAddBDOEntity + add�̃G�C���A�X
        void addBDOEntity( const BDOLabel& , const void* addr , U32 size );
        /// reserveAddBDOEntity + addU32�̃G�C���A�X
        void addBDOEntityU32( const BDOLabel& , U32 val );
        /// reserveAddBDOEntity + addBDOLabel�̃G�C���A�X
        void addBDOEntityBDOLabel( const BDOLabel& reserve , const BDOLabel& add );

        /// �t�@�C���ɏ����o���B
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
