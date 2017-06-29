package com.akipen.kclib;
/**<pre>
* 名前（改行を許さない文字列）のクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/07 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCName extends KCObject
{
	//*クラスのプロパティ**********************
	private String myName = null;
	
	//*コンストラクタ*************************
	
	/**
	* コピーした名前を定義する
	* @param aName コピー元
	*/
	public
	KCName(
		KCName aName
		)
	{
		this( aName.string() );
	}
	
	/**
	* 名前を定義する
	* @param aName 設定する名前
	*/
	public
	KCName(
		String aName
		)
	{
		if ( aName.indexOf( "\n" ) >= 0 )
		{
			KCWarring.log( "Name include \\n" );
			return;
		}
		set( aName );
	}
	
	//*プロパティアクセス***********************
	
	/**
	* 名前を取得する
	* @return 名前
	*/
	public String
	string()
	{
		return myName;
	}
	
	/**
	* 名前を設定する
	* @param aName 設定する名前
	*/
	protected void
	set(
		String aName
		)
	{
		myName = aName;
	}
	
	/**
	* 等価判定をする
	* @return 文字列が等しければtrue
	* @param aName 比較する名前
	*/
	public boolean
	equals(
		Object aName
		)
	{
		return string().equals( ( (KCName)aName ).string() );
	}
	
}
