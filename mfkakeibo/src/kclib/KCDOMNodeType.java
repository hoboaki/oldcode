package com.akipen.kclib;
/**<pre>
* DOMノードのタイプを表すクラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCDOMNodeType extends KCObject
{
	//*定数***************************************

	/**
	* Elementタイプのプロトタイプ
	*/
	static public final KCDOMNodeType ELEMENT = new KCDOMNodeType(0);
	
	/**
	* Textタイプのプロトタイプ
	*/
	static public final KCDOMNodeType TEXT = new KCDOMNodeType(1);
	
	/**
	* Attributeタイプのプロトタイプ
	*/
	static public final KCDOMNodeType ATTRIBUTE = new KCDOMNodeType(2);	
	
	//*プロパティ***********************************
	private int myTypeNo;
	
	//*コンストラクタ********************************
					
	/**
	* ノードタイプを作成する
	* @param aTypeNo タイプ番号
	*/
	protected
	KCDOMNodeType(
		int aTypeNo
		)
	{
		myTypeNo = aTypeNo;
	}
	
	/**
	* クローンを作成する
	* @return 同じタイプ番号のクローン
	*/
	public KCDOMNodeType
	cloneObject()
	{
		return new KCDOMNodeType( myTypeNo );
	}
	
	
	/**
	* タイプ番号を得る
	* @return タイプ番号
	*/
	public int
	typeNo()
	{
		return myTypeNo;
	}
	
	/**
	* 同じタイプ番号かどうか返す
	* @return 同じタイプ番号ならtrue
	*/
	public boolean
	equals(
		Object aType
		)
	{
		return ( myTypeNo == ( (KCDOMNodeType)aType ).typeNo() );
	}
	
}
