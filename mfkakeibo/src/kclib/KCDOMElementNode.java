package com.akipen.kclib;
/**<pre>
* DOMの要素ノードを表すクラス
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

public class KCDOMElementNode extends KCMutableDOMNode
{
	//*クラスのプロパティ**********************
	private KCMutableDOMNodeList myAttributes = new KCMutableDOMNodeList();
	private KCMutableDOMNodeList myChilds = new KCMutableDOMNodeList();

	//*コンストラクタ*****************************
	
	/**
	* 要素ノードを作成する
	* @param aName 要素名
	*/
	public 
	KCDOMElementNode(
		KCDOMNodeName aName
		)
	{
		setName( aName );
	}
	
	//*メソッド**********************************

	/**
	* 空の値を返す
	* @return null
	*/
	public String
	value()
	{
		return null;
	}
	
	/**
	* ノードタイプを返す
	* @return ノードタイプ
	*/
	public KCDOMNodeType
	type()
	{
		return KCDOMNodeType.ELEMENT;
	}
	
	/**
	* 子ノードリストを返す
	* @return 子ノードリスト
	*/
	public KCDOMNodeList
	childs()
	{
		return myChilds;
	}

	/**
	* ノードを追加する．追加するノードが属性ノードなら，自動で属性ノードリストに追加する．
	* @return 追加に成功したらtrue
	* @param aNode 追加するノード
	*/
	public boolean
	addChild(
		KCMutableDOMNode aNode
		)
	{
		if ( aNode == null )
			return false;
	
		aNode.setParent( this );
		if ( aNode.type().equals( KCDOMNodeType.ATTRIBUTE ) )
		{
			return addAttribute( (KCDOMAttributeNode)aNode );
		}
		else
		{
			return myChilds.add( aNode );
		}
	}

	/**
	* 属性ノードリストを返す
	* @return 属性ノードリスト
	*/
	public KCDOMNodeList
	attributes()
	{
		return myAttributes;
	}
	
	/**
	* 属性ノードを追加する
	* @return 既に同じ属性名の属性が存在していて追加できなかったらfalse
	* @param aNode 追加する属性ノード
	*/
	public boolean
	addAttribute(
		KCDOMAttributeNode aNode
		)
	{
		if ( aNode == null )
			return false;
			
		KCDOMNodeList tmpList = myAttributes.namedNode( aNode.name().string() );
		if ( tmpList.size() > 0 )
			return false;
	
		aNode.setParent( this ); 
		return myAttributes.add( aNode );
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
		
		{//---属性
			KCDOMNode[] nodes = attributes().nodes();
			for ( int i = 0; i < nodes.length; ++i )
			{
				element.appendChild( nodes[i].stdNode( aDocument ) );
			}
		}
		
		{//---子ノード
			KCDOMNode[] nodes = childs().nodes();
			for ( int i = 0; i < nodes.length; ++i )
			{
				element.appendChild( nodes[i].stdNode( aDocument ) );
			}		
		}
		
		return element;
	}

}
