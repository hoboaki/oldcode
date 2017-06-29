package com.akipen.kclib;
/**<pre>
* DOMノードを表す抽象クラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
import javax.xml.parsers.*;
import org.w3c.dom.*;

abstract public class KCDOMNode extends KCObject
{
	/**
	* ルートノード判定をする
	*　@return ルートノードならtrue
	*/
	public boolean
	isRoot()
	{
		if ( parent() == null )
			return true;
		return false;
	}
	
	/**
	* 終端ノード判定をする
	* @return 終端ノードならtrue
	*/
	public boolean
	isLeaf()
	{
		KCDOMNodeList list = childs();
		if ( list != null )
			if ( !list.isEmpty() )
				return false;
		return true;
	}

	/**
	* ノードの名前を返す
	* @return ノードの名前
	*/
	abstract public KCDOMNodeName
	name();
	
	/**
	* ノードの値を返す
	* @return ノードの値
	*/
	abstract public String
	value();

	/**
	* ノードタイプを返す
	* @return ノードタイプ
	*/
	abstract public KCDOMNodeType
	type();

	/**
	* 親ノードを返す
	* @return 親ノード
	*/
	abstract public KCDOMNode
	parent();
	
	/**
	* 子ノードリストを返す
	* @return 子ノードリスト
	*/
	abstract public KCDOMNodeList
	childs();

	/**
	* 属性ノードリストを返す
	* @return 属性ノードリスト
	*/
	abstract public KCDOMNodeList
	attributes();
	
	/**
	* JAVAライブラリのDOMノードに変換する
	* @return 変換されたノード
	* @param aDocument ノードのドキュメント
	*/
	abstract public Node
	stdNode(
		Document aDocument
		);
	
}
