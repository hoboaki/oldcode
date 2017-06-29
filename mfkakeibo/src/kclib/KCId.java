package com.akipen.kclib;
/**<pre>
* IDを表すクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
abstract public class KCId extends KCObject
{
	//*メソッド*******************************
	
	/**
	* IDを数値型で得る
	* @return 数値型のID
	*/
	abstract public long
	value();
	
	/**
	* IDを文字列型で得る
	* @return 文字列型のID
	*/
	public String
	string()
	{
		return new String( Long.toString( value() ) );
	}
	
	/**
	* 等価判定をする
	* @return 等しければtrue
	*/
	public boolean
	equals(
		Object aId
		)
	{
		return ( value() == ( (KCId)aId ).value() );
	}		
	
}
