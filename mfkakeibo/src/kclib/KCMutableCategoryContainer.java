package com.akipen.kclib;
/**<pre>
* 可変カテゴリコンテナのクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCMutableCategoryContainer extends KCCategoryContainer
{
	//*コンストラクタ*************************
	
	/**
	* 空のカテゴリコンテナを作成する
	*/
	public
	KCMutableCategoryContainer()
	{
		super();
	}
		
	//*メソッド**********************************

	/**
	* ユニーク判定をして，OKであればカテゴリを追加する
	* @return 追加したらtrue
	* @param aCategory 追加するカテゴリ
	*/
	public boolean
	add(
		KCCategory aCategory
		)
	{
		if ( !find( aCategory ) && willKeepUnique( aCategory ) )
			if ( addProcess( aCategory ) )
				return true;		
		return false;
	}
	
	/**
	* カテゴリを削除する
	* @return 削除できたらtrue
	* @param aCategory 削除するカテゴリ
	*/
	public boolean
	remove(
		KCCategory aCategory
		)
	{
		if ( !find( aCategory ) )
			return false;
		return removeProcess( aCategory );
	}

	/**
	* カテゴリーを追加するとユニークな状態が保てるか判定する
	* @return ユニークである場合true
	* @param aCategory 追加するとするカテゴリー
	*/
	public boolean
	willKeepUnique(
		KCCategory aCategory
		)
	{
		return true;
	}
	
}
