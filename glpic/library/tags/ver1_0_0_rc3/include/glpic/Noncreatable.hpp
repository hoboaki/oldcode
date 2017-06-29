/**
 * @file
 * @brief インスタンス化不可なインターフェースクラスを記述する。
 */
#if defined(GLPIC_INCLUDED_NONCREATABLE_HPP)
#else
#define GLPIC_INCLUDED_NONCREATABLE_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /**
     * @brief インスタンス化不可なインターフェースクラス。
     *
     * このクラスを継承しているクラスはコンストラクトできない。
     * staticな関数のみを提供するクラスが継承する。
     */
    class Noncreatable
    {
    private:
        virtual void privateAbstractFunction()=0;
    };

}
//----------------------------------------------------------------
#endif
// EOF
