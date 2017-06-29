/**
 * @file
 * @brief �Z�N�V�����̗v�f�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>
#include <textga/tag/AbstractElement.hpp>
#include <vector>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// �Z�N�V�����̗v�f�N���X�B
    class ELSection : public AbstractElement
    {
    public:
        /// �����ȃC���f�b�N�X�B
        static const uint INVALID_INDEX; 

        /// ���O���w�肵�ăZ�N�V�������쐬�B
        explicit ELSection( const char* name );
        virtual ~ELSection();

        /// �G�������g���Z�N�V�����ɉ�����B
        void add( std::auto_ptr< IElement > element );

        /// �G�������g�̑������擾����B
        uint count()const;

        /// �w���index�̃G�������g���擾����B
        const IElement& getElementAtIndex( uint index )const;

        /// getElementPtrWithName( name , 0 )�̃G�C���A�X�B
        const IElement* getElementPtrWithName( const char* name )const;
        /// �w��̖��O�̃G�������g�̃|�C���^���擾����B
        const IElement* getElementPtrWithName( const char* name , uint fromIndex )const;

        /// getIndexWithName( name , 0 )�̃G�C���A�X�B
        uint getIndexWithName( const char* name )const;
        /**
         * @brief �w��̖��O�̃G�������g��index���擾����B
         * @return index�B������Ȃ����INVALID_INDEX��Ԃ��B
         * @param name �G�������g�̖��O�B
         * @param fromIndex �T�����J�n����C���f�b�N�X�l�B
         */
        uint getIndexWithName( const char* name , uint fromIndex )const;

    private:
        std::vector< IElement* > elements_;
    };

}}
//------------------------------------------------------------
// EOF
