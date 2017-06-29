package com.akipen.kclib;
/**<pre>
* DOMノードリストを表すクラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
import java.util.*;
public class KCDOMNodeList extends KCObject
{
	//*クラスのプロパティ**********************
	private List myList = new LinkedList();

	//*コンストラクタ***************************
	
	/**
	 * 空のリストを作成する
	 */
	public 
	KCDOMNodeList()
	{
	}
	
	//*メソッド**********************************

	/**
	 * リストのサイズを返す
	 * @return リストの長さ
	 */
	public int
	size()
	{
		return myList.size();
	}

	/**
	* 要素数が0かどうか返す
	* @return size()==0
	*/
	public boolean
	isEmpty()
	{
		return ( size() == 0 );
	}

	/**
	* 指定した名前のノードのリストを返す
	* @return ノードリスト
	* @param aName ノードの名前
	*/
	public KCDOMNodeList
	namedNode(
		String aName
		)
	{
		Iterator itr = myList.listIterator();
		
		KCDOMNodeList tmpList = new KCMutableDOMNodeList();
		while( itr.hasNext() )
		{
			KCDOMNode node = (KCDOMNode)itr.next();
			if ( node.name().equals( aName ) )
			{
				tmpList.add( node );
			}
		}
		
		return tmpList;				
	}
		
	/**
	* 指定したインデックスのノードを返す
	* @return ノード（なければnull）
	* @param aIndex インデックス値
	*/
	public KCDOMNode
	nodeAtIndex(
		int aIndex
		)
	{
		if ( aIndex < 0 || aIndex >= size() )
			return null;
		return (KCDOMNode)( myList.get( aIndex ) );
	}
	
	/**
	 * ノード配列を返す
	 * @return ノード配列
	 */
	public KCDOMNode[]
	nodes()
	{
		if ( size() == 0 )
			return new KCDOMNode[0];
	
		Iterator itr = myList.listIterator();
		
		KCDOMNode[] tmpNodes = new KCDOMNode[size()];
		int i = 0;
		while( itr.hasNext() )
		{
			tmpNodes[i] = (KCDOMNode)itr.next();
			i++;
		}	
		return tmpNodes;	
	}

	/**
	 * ノードの最後尾に要素を追加する
	 * @return 追加に成功したらtrue
	 * @param aNode 追加するノード
	 */
	protected boolean
	add(
		KCDOMNode aNode
		)
	{
		return myList.add( aNode );
	}

}
