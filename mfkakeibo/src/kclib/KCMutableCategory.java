package com.akipen.kclib;
/**
* 可変カテゴリークラスのクラス
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
public class KCMutableCategory extends KCCategory
{
	//*コンストラクタ**************************

	/**
	* 空のカテゴリを作成する
	*/
	public
	KCMutableCategory()
	{
		super();
	}
		
	//*メソッド**********************************

	/**
	* 所属する本を設定する
	* @param aBook 所属する本
	*/
	public void
	setBook(
		KCBook aBook
		)
	{
		super.setBook( aBook );
	}


	/**
	* カテゴリIDを設定する
	* @param aId 設定するID
	*/
	public void
	setId(
		KCId aId
		)
	{
		super.setId( aId );
	}
		
	/**
	* 親カテゴリを変更する．
	* @param aCategory 設定する親カテゴリ
	*/
	public void
	setParent(
		KCCategory aCategory
		)
	{
		super.setParent( aCategory );
	}
		
	/**
	 * カテゴリの名前を設定する
	 * @param aName 本の名前
	 */
	public void
	setName(
		KCCategoryName aName
		)
	{
		super.setName( aName );
	}
	
	/**
	* コメントを設定する
	* @param aComment コメント
	*/
	public void
	setComment(
		String aComment
		)
	{
		super.setComment( aComment );
	}
			
	/**
	* 子カテゴリを追加する
	* @return 成功したtrue
	* @param aCategory 追加するカテゴリ
	*/
	public boolean
	addChild(
		KCMutableCategory aCategory
		)
	{
		if ( mutableChilds().add( aCategory ) )
		{
			aCategory.setParent( this );
			return true;
		}
		return false;
	}
	
	
}
