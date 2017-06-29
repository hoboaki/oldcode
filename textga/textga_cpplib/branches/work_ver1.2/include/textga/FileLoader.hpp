/**
 * @file
 * @brief �t�@�C�������[�h����N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {
    
    /// �t�@�C�������[�h����N���X���L�q����B
    class FileLoader
    {
    public:
        FileLoader( const char* filePath );
    
        /// �J�������B
        bool isLoaded()const;
        
        /// �f�[�^���擾����B
        const ::textga::byte* data()const;
        /// �f�[�^�T�C�Y���擾����B
        size_t dataSize()const;
        
    private:
        bool isLoaded_;
        std::vector< ::textga::byte > bytes_;
    };
}
//------------------------------------------------------------
// EOF
