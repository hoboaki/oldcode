package com.akipen.kclib;
/**<pre>
* 本を生成するクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2006/01/05 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCBookFactory extends KCObject 
{
	//*プロパティ**************************************
	private KCAccountFactory myAccountFactory = new KCAccountFactory();
	private KCCategoryFactory myCategoryFactory = new KCCategoryFactory();
	
	//*メソッド****************************************

	/**
	* 本を返す。
	* @return 本
	*/
	public KCMutableBook
	getBook()
	{
		return new KCMutableBook();
	}

	/**
	* 口座ファクトリーを返す。
	* @return 口座ファクトリー
	*/
	public KCAccountFactory
	getAccountFactory()
	{
		return myAccountFactory;
	}

	/**
	* カテゴリファクトリーを返す。
	* @return カテゴリファクトリー
	*/
	public KCCategoryFactory
	getCategoryFactory()
	{
		return myCategoryFactory;
	}
	
	/**
	 * DOMノードから本を作成する
	 * @return 作成した本，失敗したらnullを返す．
	 * @param aNode DOMノード
	 */
	public KCMutableBook
	createFromDOMNode(
		KCDOMNode aNode
		)
	{
		if ( !aNode.name().equals( KCDOMObject.DOM_BOOK ) )
		{//---ノード名が本でなければ失敗
			return null;
		}

		KCDOMNode[] childs = aNode.childs().nodes();
		KCMutableBook book = getBook();
		for ( int i = 0; i < childs.length; ++i )
		{
			KCDOMNode node = childs[i];
			KCDOMNodeName name = node.name();
			if ( name.equals( KCDOMObject.DOM_NAME ) )
			{//---名前
				book.setName( new KCName( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_COMMENT ) )
			{//---コメント
				book.setComment( node.value() );
			}
			else if ( name.equals( KCDOMObject.DOM_ACCOUNTS ) )
			{//---口座
				KCDOMNode[] accounts = node.childs().nodes();
				KCAccountFactory accountFactory = getAccountFactory();
				for ( int j = 0; j < accounts.length; ++j )
				{
					KCMutableAccount tmpAccount = ( accountFactory.createFromDOMNode( accounts[j] ) );
					/*if ( tmpAccount == null )
					{
						KCWarring.log( "Account is null" );
					}*/
					if ( book.addAccount( tmpAccount ) == null )
					{
						KCWarring.log( "Can't add account" );
					}
				}
			}
			else if ( name.equals( KCDOMObject.DOM_CATEGORIES ) )
			{//---カテゴリ
				KCDOMNode[] categories = node.childs().nodes();
				KCMutableCategory root = (KCMutableCategory)book.rootCategory();
				KCCategoryFactory categoryFactory = getCategoryFactory();
				for ( int j = 0; j < categories.length; ++j )
				{
					KCMutableCategory tmpCategory = ( categoryFactory.createFromDOMNode( categories[j] ) );
					root.addChild( tmpCategory );
				}				
			}

		}
		return book;		
	}
}
