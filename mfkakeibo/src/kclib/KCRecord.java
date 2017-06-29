package com.akipen.kclib;
/**<pre>
* 家計簿のレコードを表すクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2005/12/11 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCRecord extends KCDOMObject
{
	//*クラスのプロパティ**********************
	static private KCIdFactory myIdFactory = new KCIdFactory(); //---IDファクトリ

	//*プロパティ*****************************
	private KCId				myId			= null;		//---レコードID
	private KCAccount			myAccount		= null;		//---所属している口座
	private KCRecordName		myName			= null;		//---品目
	private KCMoney				myMoney			= null;		//---金額
	private KCCategoryPath		myCategoryPath	= null;		//---所属カテゴリのパス
	private KCDate				myDate			= null;		//---日付
	private String				myComment		= null;		//---メモ
	
	//*コンストラクタ*************************
	
	/**
	* 空のレコードを作成する．
	*/
	protected
	KCRecord()
	{
		myId = myIdFactory.getId();
	}
	
	/**
	 * 口座を指定してレコードを作成する。
	 * @param aAccount 指定する口座
	*/
	/*protected
	KCRecord(
		KCAccount aAccount
		)
	{
		myAccount = aAccount;
	}*/

	//*メソッド**************************

	/**
	* IDで等価判定をする
	* @return IDが等しければtrue
	*/
	public boolean
	equals(
		Object aRecord
		)
	{
		KCRecord record = (KCRecord)aRecord;
		return ( id().equals( record.id() ) &&
				account().equals( record.account() ) );
	}
	
	/**
	* レコードIDを得る．
	* @return レコードID
	*/
	public KCId
	id()
	{
		return myId;
	}
	
	/**
	* 所属口座を得る．
	* @return 口座
	*/
	public KCAccount
	account()
	{
		return myAccount;
	}
	
	/**
	* レコードの品目名を得る．
	* @return レコードの品目名
	*/
	public KCRecordName
	name()
	{
		return myName;
	}
	
	/**
	* 金額を得る．
	* @return 金額
	*/
	public KCMoney
	money()
	{
		return myMoney;
	}
	
	/**
	* 所属カテゴリーのパスを得る．
	* @return カテゴリーのパス
	*/
	public KCCategoryPath
	categoryPath()
	{
		return myCategoryPath;
	}
	
	/**
	* 日付を得る．
	* @return 日付
	*/
	public KCDate
	date()
	{
		return myDate;
	}
	
	/**
	* コメント・備考を得る．
	* @return コメント
	*/
	public String
	comment()
	{
		return myComment;
	}

	/**
	 * aRecoardの内容で初期化する．（IDと口座はコピーされない）
	 * @param aRecord 初期化する内容
	 */
	protected void
	initWithRecord(
		KCRecord aRecord
		)
	{
		//setId( aRecord.id() );
		setAccount( aRecord.account() );
		setMoney( aRecord.money() );
		setDate( aRecord.date() );
		setCategoryPath( aRecord.categoryPath() );
		setName( aRecord.name() );
		setComment( aRecord.comment() );
	}

	/**
	* レコードIDを設定する．
	* @param aId 設定するレコードID
	*/
	protected void
	setId(
		KCId aId
		)
	{
		myId = aId;
	}

	/**
	* 所属口座を設定する．
	* @param aAccount 設定する口座
	*/
	protected void
	setAccount(
		KCAccount aAccount
		)
	{
		myAccount = aAccount;
	}
	
	/**
	* 品目名を設定する．
	* @param aName 設定する名前
	*/
	protected void
	setName(
		KCRecordName aName
		)
	{
		myName = aName;
	}
	
	/**
	* 金額を設定する．
	* @param aMoney 設定する金額
	*/
	protected void
	setMoney(
		KCMoney aMoney
		)
	{
		myMoney = aMoney;
	}
	
	/**
	* 所属するカテゴリのパスを設定する．
	* @param aCategoryPath 設定するカテゴリのパス
	*/
	protected void
	setCategoryPath(
		KCCategoryPath aCategoryPath
		)
	{
		myCategoryPath = aCategoryPath;
	}	
	
	/**
	* 日付を設定する．
	* @param aDate 設定する日付
	*/
	protected void
	setDate(
		KCDate aDate
		)
	{
		myDate = aDate;
	}
	
	/**
	* コメント・備考を設定する．
	* @param aComment 設定するコメント
	*/
	protected void
	setComment(
		String aComment
		)
	{
		myComment = aComment;
	}
	
	/**
	 * DOMノードに変換する
	 * @return DOMノードに変換したレコード．必須情報がなければnullを返す．
	 */
	public KCMutableDOMNode
	domNode()
	{
		//---必須情報チェック
		if ( date() == null )
		{
			KCWarring.log( "Date is Null" );
			return null;
		}
		if ( money() == null )
		{
			KCWarring.log( "Money is Null" );
			return null;
		}

		KCDOMElementNode node = new KCDOMElementNode( DOM_RECORD );
		
		if ( name() != null )
		{//---品目
			String str = name().string();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_NAME , str ) );
		}
		if ( date() != null )
		{//---日付
			String str = date().string();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_DATE , str ) );
		}
		if ( money() != null ) 
		{//---金額
			String str = money().string();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_PRICE , str ) );
		}
		if ( categoryPath() != null )
		{//---カテゴリ
			String str = categoryPath().string();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_CATEGORY , str ) );
		}
		if ( comment() != null ) 
		{//---コメント
			String str = comment();
			if ( str.length() > 0 )
				node.addChild( new KCDOMTextNode( DOM_COMMENT , str ) );
		}

		return node;
	}
	
}
