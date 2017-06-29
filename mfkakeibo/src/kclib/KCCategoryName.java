package com.akipen.kclib;
/**<pre>
* カテゴリーの名前を表すクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCCategoryName extends KCName
{	
	//*コンストラクタ*************************
	
	/**
	* コピーした名前を定義する
	* @param aName コピー元
	*/
	public
	KCCategoryName(
		KCCategoryName aName
		)
	{
		this( aName.string() );
	}
	
	/**
	* 名前を定義する
	* @param aName 設定する名前
	*/
	public
	KCCategoryName(
		String aName
		)
	{
		super( aName );
		if ( aName.indexOf( KCCategory.SEPARATOR ) >= 0  )
		{
			KCWarring.log( new String( "CategoryName include separate character \"\\\\\" [" + aName + "]" ) );
		}
	}
	
	
}
