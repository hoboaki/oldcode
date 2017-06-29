package com.akipen.kclib;
/**<pre>
* 口座のクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCAccount extends KCDOMObject
{
	//*クラスのプロパティ**********************
	static private KCIdFactory myIdFactory = new KCIdFactory(); //---IDファクトリ

	//*プロパティ********************************
	private KCBook						myBook = null;
	private KCId						myId;
	private KCName						myName = new KCName( "" );
	private String						myComment = new String( "" );
	private KCMutableRecordContainer	myRecordContainer = new KCMutableRecordContainer();
	private KCRecordFactory				myRecordFactory = new KCRecordFactory();

	//*コンストラクタ*************************
	
	/**
	* 標準の口座を作成する
	*/
	protected 
	KCAccount()
	{
		myId = myIdFactory.getId();
	}
	
	//*メソッド*******************************

	/**
	* IDで等価判定をする
	* @return IDが等しければtrue
	*/
	public boolean
	equals(
		Object aAccount
		)
	{
		KCAccount account = (KCAccount)aAccount;
		return ( id().equals( account.id() ) );
	}
			
	/**
	* 所属本を得る
	* @return 家計簿本
	*/
	public KCBook
	book()
	{
		return myBook;
	}

	/**
	* 口座IDを得る
	* @return 口座ID
	*/
	public KCId
	id()
	{
		return myId;
	}
	
	/**
	* 口座名を返す
	* @return 口座名
	*/
	public KCName
	name()
	{
		return myName;
	}
			
	/**
	* コメントを得る
	* @return コメント
	*/
	public String
	comment()
	{
		return myComment;
	}
	
	/**
	* レコードコンテナのイテレータを得る
	* @return レコードコンテナのイテレータ
	*/
	public KCRecordIterator
	recordIterator()
	{
		return recordContainer().iterator();
	}
	
	/**
	* レコードコンテナを返す
	* @return レコードコンテナ
	*/
	protected KCRecordContainer
	recordContainer()
	{
		return 	mutableRecordContainer();
	}

	/**
	* 書き換え可能なレコードコンテナを返す
	* @return レコードコンテナ
	*/
	protected KCMutableRecordContainer
	mutableRecordContainer()
	{
		return myRecordContainer;	
	}

	/**
	 * レコードを生成するクラスを得る
	 * @return レコードファクトリー
	 */
	protected KCRecordFactory
	recordFactory()
	{
		return myRecordFactory;
	}
	
	/**
	* 口座の情報をコピーする。
	* @param aAccount コピー元の口座
	*/
	protected void
	initWithAccount(
		KCAccount aAccount
		)
	{
		setBook( aAccount.book() );
		setName( aAccount.name() );
		setComment( aAccount.comment() );
		
	}
	
	/**
	* 所属する本を設定する
	* @param aBook 所属する本
	*/
	protected void
	setBook(
		KCBook aBook
		)
	{
		myBook = aBook;
	}
	
	/**
	* 口座IDを設定する
	* @param aId 口座ID
	*/
	/*protected void
	setId(
		KCId aId
		)
	{
		myId = aId;
	}*/
	
	/**
	 * 口座名を設定する
	 * @param aName 設定する口座名
	 */
	protected void
	setName(
		KCName aName
		)
	{
		myName = aName;
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
	* レコードコンテナを設定する
	* @param aContainer レコードコンテナ
	*/
	protected void
	setRecordContainer(
		KCMutableRecordContainer aContainer
		)
	{
		myRecordContainer = aContainer;
	}
	
	/**
	* レコードファクトリを設定する
	* @param aFactory レコードファクトリ
	*/
	protected void
	setRecordFactory(
		KCRecordFactory aFactory
		)
	{
		myRecordFactory = aFactory;
	}
	
	/**
	 * DOMノードに変換する
	 * @return DOMノードに変換したレコード．必須情報がなければnullを返す．
	 */
	public KCMutableDOMNode
	domNode()
	{
		KCDOMElementNode node = new KCDOMElementNode( DOM_ACCOUNT );
		
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
		
		{//---レコード
			KCDOMElementNode node2 = new KCDOMElementNode( DOM_RECORDS );
		
			KCRecordIterator itr = recordContainer().iterator();
			while( itr.hasNext() )
			{
				KCRecord record = itr.next();
				if ( record == null )
				{
					KCWarring.log( "Record is NULL" );
					continue;
				}
				node2.addChild( record.domNode() );
			}

			node.addChild( node2 );
		}
		return node;
	}

}
