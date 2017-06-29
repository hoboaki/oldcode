/**
 * @file
 * @brief ArgumentDataのイテレータクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {
    struct ArgumentData;
}

//------------------------------------------------------------
namespace local {

    /// ArgumentDataのイテレータクラス。
    class ArgumentDataIterator
    {
    public:
        explicit ArgumentDataIterator( const ArgumentData& );
        
        bool hasNext()const;    ///< 次があるか。
        const char* next();     ///< 次の要素へ移動。
        
    private:
        const ArgumentData& data_;
        int index_;
    };

}
//------------------------------------------------------------
// EOF
