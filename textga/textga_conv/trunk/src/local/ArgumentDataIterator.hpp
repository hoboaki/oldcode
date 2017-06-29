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
        
        const char* get()const; ///< 今、示している要素を取得。
        bool isEnd()const;      ///< 終わりか。
        void next();            ///< 次の要素へ移動。
        
    private:
        const ArgumentData& data_;
        int index_;
    };

}
//------------------------------------------------------------
// EOF
