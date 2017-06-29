package com.akipen.kclib;
/**<pre>
* DOMのテキストノードを表すクラス
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

public class KCDOMTextNode extends KCMutableDOMNode
{
	//*コンストラクタ*****************************
	
	/**
	* テキストノードを作成する
	* @param aName 設定するノード名
	* @param aText 設定する文字列
	*/
	public 
	KCDOMTextNode(
		KCDOMNodeName aName,
		String aText
		)
	{
		setName( aName );
		myText = aText;
	}
	
	//*プロパティ********************************
	private String myText = null;
	
	//*メソッド**********************************

	/**
	* テキストの値を返す
	* @return ノードの値
	*/
	public String
	value()
	{
		return myText;
	}
	
	/**
	* ノードタイプを返す
	* @return ノードタイプ
	*/
	public KCDOMNodeType
	type()
	{
		return KCDOMNodeType.TEXT;
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
		Element element = aDocument.createElement( name().string() );
		Text text = aDocument.createTextNode( value() );
		element.appendChild( text );
		return element;
	}	
			
}
