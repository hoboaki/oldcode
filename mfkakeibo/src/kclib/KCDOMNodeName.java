package com.akipen.kclib;
/**<pre>
* DOMノードの名前を表すクラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCDOMNodeName extends KCObject
{
	//*プロパティ***************************
	private String myName;

	//*コンストラクタ************************
	/**
	 * コピーした文字列を定義する
	 * @param aString コピー元文字列
	 */
	public KCDOMNodeName(
		String aString
		)
	{
		myName = aString;
	}
	
	//*メソッド*****************************
	
	/**
	* 等価判定をする
	* @return 名前が等価ならtrue
	*/
	public boolean
	equals(
		Object aName
		)
	{
		KCDOMNodeName name = (KCDOMNodeName)aName;
		if ( string().equals( name.string() ) )
			return true;
		return false;
	}
	
	/**
	* 文字列型で返す
	* @return 名前の文字列
	*/
	public String
	string()
	{
		return myName;
	}
	
}
