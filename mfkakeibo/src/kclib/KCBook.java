package com.akipen.kclib;
/**<pre>
* 家計簿本のクラス．
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
public class KCBook extends KCDOMObject
{
	//*クラスのプロパティ**********************
	static private KCIdFactory myIdFactory = new KCIdFactory(); //---IDファクトリ
		
	//*プロパティ******************************
	private KCId myId;
	private KCName						myName = new KCName( "" );
	private String						myComment = new String( "" );
	private KCAccountFactory			myAccountFactory = new KCAccountFactory();
	private KCMutableAccountContainer	myAccountContainer = new KCMutableAccountContainer();
	private KCMutableCategory			myRootCategory = new KCMutableCategory();

	//*コンストラクタ******************************
	protected
	KCBook()
	{
		myId = myIdFactory.getId();
	}
	
	//*メソッド**********************************

	/**
	* IDで等価判定をする
	* @return IDが等しければtrue
	*/
	public boolean
	equals(
		Object aBook
		)
	{
		return ( id().equals( ( (KCBook)aBook ).id() ) );
	}
	
	/**
	* 本IDを得る
	* @return 本ID
	*/
	public KCId
	id()
	{
		return myId;
	}
	
	/**
	* 本の名前を返す
	* @return 本の名前
	*/
	public KCName
	name()
	{
		return myName;
	}
		
	/**
	 * 本の名前を設定する
	 * @param aName 本の名前
	 */
	protected void
	setName(
		KCName aName
		)
	{
		myName = aName;
	}
	
	/**
	* コメントを返す
	* @return コメント
	*/
	public String
	comment()
	{
		return myComment;
	}
	
	/**
	* コメントを設定する
	* @param aComment コメント
	*/
	protected void
	setComment(
		String aComment
		)
	{
		myComment = aComment;
	}

	/**
	* 口座のイテレータを得る
	* @return 口座のイテレータ
	*/
	public KCAccountIterator
	accountIterator()
	{
		return accountContainer().iterator();
	}
	
	/**
	* 口座コンテナを得る
	* @return 口座コンテナ
	*/
	protected KCAccountContainer
	accountContainer()
	{
		return mutableAccountContainer();
	}
	
	/**
	* 書き換え可能な口座コンテナを得る
	* @return 口座コンテナ
	*/
	protected KCMutableAccountContainer
	mutableAccountContainer()
	{
		return myAccountContainer;
	}

	/**
	 * 口座を生成するクラスを得る
	 * @return アカウントファクトリー
	 */
	protected KCAccountFactory
	accountFactory()
	{
		return myAccountFactory;
	}
	
	/**
	* ルートカテゴリを得る
	* @return カテゴリ
	*/
	public KCMutableCategory
	rootCategory()
	{
		return myRootCategory;
	}
	
	/**
	* ルートカテゴリを設定する
	* @param aCategory ルートカテゴリ
	*/
	protected void
	setRootCategory(
		KCMutableCategory aCategory
		)
	{
		aCategory.setParent( null );
		myRootCategory = aCategory;
	}
	
	/**
	 * DOMノードに変換する
	 * @return DOMノードに変換したレコード
	 */
	public KCMutableDOMNode
	domNode()
	{
		KCDOMElementNode node = new KCDOMElementNode( DOM_BOOK );
		
		if ( name() != null )
		{//---品目
			String str = name().string();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_NAME , str ) );
		}
		if ( comment() != null ) 
		{//---コメント
			String str = comment();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_COMMENT , str ) );
		}
		
		if ( rootCategory() != null )
		{//---カテゴリ
			KCDOMElementNode node2 = new KCDOMElementNode( DOM_CATEGORIES );
		
			KCCategoryIterator itr = rootCategory().childs().iterator();
			while( itr.hasNext() )
			{
				node2.addChild( itr.next().domNode() );
			}

			node.addChild( node2 );		
		}
		
		{//---口座
			KCDOMElementNode node2 = new KCDOMElementNode( DOM_ACCOUNTS );
		
			KCAccountIterator itr = accountContainer().iterator();
			while( itr.hasNext() )
			{
				KCAccount account = itr.next();
				if ( account == null )
				{
					KCWarring.log( "Account is Null" );
					continue;
				}
				node2.addChild( account.domNode() );
			}

			node.addChild( node2 );
		}
		return node;
	}
	


}
