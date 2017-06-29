/**
 * @file
 * @brief 要素のイテレータクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <memory>
#include <textga/Types.hpp>
#include <textga/tag/IElement.hpp>
#include <textga/tag/TagKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// 要素のイテレータクラス。
    class ElementIterator
    {
    public:
        ElementIterator( const byte* , size_t size );
        ~ElementIterator();

        /**
         * @brief 今示している要素を作成して次の要素に移動する。
         * @return 作成に失敗したらNULLを返す。
         */
        std::auto_ptr< IElement > next();
        /// 次の要素があるか。
        bool hasNext()const;

    private:
        const byte* const data_;
        const size_t size_;
        size_t offset_;
        TagKind lastTagKind_;

        //------------------------------------------------------------
        struct NextTagResult
        {
            TagKind     tagKind;
            const char* tagName;
            const byte* data;
        };

        size_t calculateRestSize()const;
        NextTagResult nextTag();
       
    };

}}
//------------------------------------------------------------
// EOF
