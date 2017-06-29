package com.akipen.kclib;
/**<pre>
* DOMの属性ノードを表すクラス
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
//import org.apache.crimson.tree.*;
public class KCDOMAttributeNode extends KCMutableDOMNode
{
	//*コンストラクタ*****************************
	
	/**
	* 属性ノードを作成する
	* @param aName 設定する属性名
	* @param aValue 設定する値
	*/
	public 
	KCDOMAttributeNode(
		KCDOMNodeName aName,
		String aValue
		)
	{
		setName( aName );
		myValue = aValue;
	}
	
	//*プロパティ********************************
	private String myValue = null;
	
	//*メソッド**********************************

	/**
	* 属性の値を返す
	* @return 属性の値
	*/
	public String
	value()
	{
		return myValue;
	}
	
	/**
	* ノードタイプを返す
	* @return ノードタイプ
	*/
	public KCDOMNodeType
	type()
	{
		return KCDOMNodeType.ATTRIBUTE;
	}
	
	/**
	* 空の子ノードリストを返す
	* @return null
	*/
	public KCDOMNodeList
	childs()
	{
		return null;
	}

	/**
	* 空の属性ノードリストを返す
	* @return null
	*/
	public KCDOMNodeList
	attributes()
	{
		return null;
	}
	
	/**
	* JAVAライブラリのDOMノードに変換する
	* @return 変換されたノード
	* @param aDocument ノードのドキュメント
	*/
	public Node
	stdNode(
		Document aDocument
		)
	{
		Attr attribute = aDocument.createAttribute( name().string() );
		attribute.setValue( value() );
		return attribute;
	}
}
