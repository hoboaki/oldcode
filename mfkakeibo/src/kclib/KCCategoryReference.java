package com.akipen.kclib;
/**<pre>
* カテゴリーの参照クラス．
*
* <変更履歴>
* 2005/12/11 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCCategoryReference 
{
	//*クラスのプロパティ**********************
	private KCCategory myCategory;
	
	//*コンストラクタ*************************
	
	/**
	* 参照を作成
	* @param aCategory 参照先のカテゴリ
	*/
	public
	KCCategoryReference(
		KCCategory aCategory
		)
	{
		myCategory = aCategory;
	}

	/**
	* コピーを作成
	* @param aCategoryRef コピー元
	*/
	public
	KCCategoryReference(
		KCCategoryReference aCategoryRef
		)
	{
		myCategory = aCategoryRef.real();
	}
	
	//*プロパティアクセス************************
	
	/**
	* 参照先を返す
	* @return 参照先のカテゴリ
	*/
	public KCCategory
	real()
	{
		return myCategory;
	} 

}
