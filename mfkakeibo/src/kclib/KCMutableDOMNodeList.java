package com.akipen.kclib;
/**<pre>
* 書き換え可能なDOMノードリストを表すクラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCMutableDOMNodeList extends KCDOMNodeList
{

	//*コンストラクタ***************************
	
	/**
	 * 空のリストを作成する
	 */
	public 
	KCMutableDOMNodeList()
	{
		super();
	}
	
	//*メソッド**********************************

	/**
	 * ノードの最後尾に要素を追加する
	 * @return 追加に成功したらtrue
	 * @param aNode 追加するノード
	 */
	public boolean
	add(
		KCDOMNode aNode
		)
	{
		return super.add( aNode );
	}

}
