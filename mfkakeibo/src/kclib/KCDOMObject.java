package com.akipen.kclib;
/**<pre>
* 家計簿クラスでXMLのオブジェクトに変換できるクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/05 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
abstract public class KCDOMObject extends KCObject
{
	//*クラスプロパティ*************************
	static public final KCDOMNodeName DOM_ROOT = new KCDOMNodeName( "KCXML" );
	
	static public final KCDOMNodeName DOM_NAME = new KCDOMNodeName( "NAME" );
	static public final KCDOMNodeName DOM_COMMENT = new KCDOMNodeName( "COMMENT" );
	static public final KCDOMNodeName DOM_DATE = new KCDOMNodeName( "DATE" );
	static public final KCDOMNodeName DOM_PRICE = new KCDOMNodeName( "PRICE" );	
	static public final KCDOMNodeName DOM_CATEGORY = new KCDOMNodeName( "CATEGORY" );
	
	static public final KCDOMNodeName DOM_BOOK = new KCDOMNodeName( "BOOK" );
	static public final KCDOMNodeName DOM_ACCOUNTS = new KCDOMNodeName( "ACCOUNTS" );	
	static public final KCDOMNodeName DOM_ACCOUNT = new KCDOMNodeName( "ACCOUNT" );
	static public final KCDOMNodeName DOM_RECORDS = new KCDOMNodeName( "RECORDS" );
	static public final KCDOMNodeName DOM_RECORD = new KCDOMNodeName( "RECORD" );
	static public final KCDOMNodeName DOM_CATEGORIES = new KCDOMNodeName( "CATEGORIES" );
	
	//*メソッド**********************************
	/**
	 * DOMノードに変換する
	 * @return DOMノードに変換したレコード
	 */
	abstract public KCMutableDOMNode
	domNode();
}
