package com.akipen.kclib;
/**<pre>
* ファイル入出力のクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/06 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
import java.io.*;
import javax.xml.parsers.*;
import javax.xml.transform.*;
import org.w3c.dom.*;
//import org.apache.crimson.*;

import org.w3c.dom.Document;
import org.w3c.dom.DOMImplementation;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.DocumentBuilder;

import javax.xml.transform.TransformerFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

public class KCFileManager extends KCObject
{
	//*クラスのメソッド***********************
	
	/**
	* Bookをファイルに保存する
	* @return 成功すればtrue
	* @param aBook 保存する本
	* @param aFilepath 保存先
	*/
	static public boolean
	saveTo(
		KCBook aBook,
		String aFilepath
		)
	{
	
		Document doc;
		try
		{
			DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance(); 
			DocumentBuilder builder = factory.newDocumentBuilder(); 

			DOMImplementation domImpl=builder.getDOMImplementation();
			doc = domImpl.createDocument("",KCDOMObject.DOM_ROOT.string(),null);
		}catch( Exception e )
		{
			return false;
		}

		Element rootNode = doc.getDocumentElement();
		rootNode.appendChild( aBook.domNode().stdNode( doc ) );
		//doc.appendChild( rootNode );
		//Node childNode = aBook.domNode().stdNode( doc );
		//doc.appendChild( childNode );
		
		//---保存
		try
		{
			BufferedOutputStream stream = new BufferedOutputStream(	new FileOutputStream( aFilepath ) );
			StreamResult result = new StreamResult( stream ); 

			DOMSource source = new DOMSource(doc);			
			
			TransformerFactory transFactory = TransformerFactory.newInstance();
			Transformer transformer = transFactory.newTransformer();
			transformer.setOutputProperty(OutputKeys.METHOD, "xml");
			transformer.setOutputProperty( OutputKeys.ENCODING ,"UTF-8");		
			transformer.setOutputProperty( OutputKeys.INDENT ,"yes");
			transformer.transform(source, result);
			//((XmlDocument)doc).write( stream );
		} catch(Exception woexception) 
		{
			return false;
		}
		
		return true;
	}
	
	/**
	* Bookをファイルから作成する
	* @return ブック．失敗すればnull．
	* @param aFilepath 家計簿ファイルのパス
	* @param aBookFactory 本を生成するクラス
	*/
	static public KCMutableBook
	loadFrom(
		String aFilepath,
		KCBookFactory aBookFactory
		)
	{
		//---ファイルオープン
		Document document;
		try
		{
			document= DocumentBuilderFactory
				.newInstance()
				.newDocumentBuilder()
				.parse( new File( aFilepath ) );
		}catch( Exception err )
		{
			return null;
		}
				
		Node doc = document.getDocumentElement();
		if ( doc.getNodeName().equals( KCDOMObject.DOM_ROOT ) )
		{//---ルートのタグ名が違えば失敗
			return null;
		}
		
		//---KCDOMツリーにデコード
		KCDOMNode root = saikiNode( doc );
		
		//---KCBookにデコード
		KCDOMNode[] nodes = root.childs().nodes();
		for ( int i = 0; i < nodes.length; ++i )
		{
			if ( nodes[i].name().equals( KCDOMObject.DOM_BOOK ) )
			{
				KCMutableBook book = aBookFactory.createFromDOMNode( nodes[i] );
				return book;
			}
		}
		return null;
		
	}
	
	static private KCMutableDOMNode
	saikiNode(
		Node aNode
		)
	{
		if ( aNode.getNodeType() == Node.ATTRIBUTE_NODE )
		{//---属性ノード
			return new KCDOMAttributeNode( 
						new KCDOMNodeName( new String( aNode.getNodeName() ) ),
						new String ( aNode.getNodeValue() )
						);
		}
		else if ( aNode.getNodeType() == Node.ELEMENT_NODE )
		{//---要素ノード
		
			NodeList childs = aNode.getChildNodes();
			
			if ( childs.getLength() == 1 )
			{//---子要素数1の時，テキストノードとみなす
				/*KCWarring.log( "ParentName: " + aNode.getNodeName() + "\n" +
								"Name: " + childs.item(0).getNodeName() + "\n" +
								"Value: " + childs.item(0).getNodeValue() + "\n"
								 );*/
				return new KCDOMTextNode(
						new KCDOMNodeName( new String( aNode.getNodeName() ) ),
						new String( childs.item(0).getNodeValue() )
						);
			}
			
			KCDOMElementNode element = new KCDOMElementNode( new KCDOMNodeName( new String ( aNode.getNodeName() ) ) );
			for ( int i = 1; i < childs.getLength(); /*i+=2*/++i )
			{
				Node targetNode = childs.item(i);
				element.addChild( saikiNode( targetNode ) );				
			}
			return element;
		
		}
		return null;
	}
	
}
