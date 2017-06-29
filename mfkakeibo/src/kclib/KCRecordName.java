package com.akipen.kclib;
/**<pre>
* レコードの品目名を表すクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/11 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCRecordName extends KCName
{	
	//*コンストラクタ*************************
	
	/**
	* コピーした名前を定義する
	* @param aName コピー元
	*/
	public
	KCRecordName(
		KCRecordName aName
		)
	{
		super( aName.string() );
	}
	
	/**
	* 名前を定義する
	* @param aName 設定する名前
	*/
	public
	KCRecordName(
		String aName
		)
	{
		super( aName );
	}
	
}
