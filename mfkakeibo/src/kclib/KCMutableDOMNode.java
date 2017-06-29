package com.akipen.kclib;
/**<pre>
* 書き換え可能なDOMノードを表す抽象クラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
abstract public class KCMutableDOMNode extends KCDOMNode
{
	//*プロパティ***********************************
	private KCDOMNodeName myName = null;
	private KCDOMNode	myParent = null;
	
	//*メソッド************************************

	/**
	* ノードの名前を返す
	* @return ノードの名前
	*/
	public KCDOMNodeName
	name()
	{
		return myName;
	}
	
	/**
	* ノードの名前を設定する
	* @param aName 設定する名前
	*/
	protected void
	setName(
		KCDOMNodeName aName
		)
	{
		myName = aName;
	}
	
	/**
	* 親ノードを返す
	* @return 親ノード
	*/
	public KCDOMNode
	parent()
	{
		return myParent;
	}
	
	/**
	* 親ノードを設定する
	* @param aNode 親ノード
	*/
	public void
	setParent(
		KCDOMNode aNode
		)
	{
		myParent = aNode;
	}
	
}
