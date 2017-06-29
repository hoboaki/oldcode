package com.akipen.kclib;
/**
* カテゴリーのクラス。
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
public class KCCategory extends KCDOMObject
{
	//*クラスのプロパティ**********************
	static private KCIdFactory myIdFactory = new KCIdFactory(); //---IDファクトリ
	static public final String SEPARATOR = new String( "\\\\" );

	//*プロパティ******************************
	private KCBook						myBook = null;
	private KCId						myId = null;
	private KCCategoryName				myName = new KCCategoryName( new String( "" ) );
	private String						myComment = new String( "" );
	private KCCategory					myParent = null;
	private KCMutableCategoryContainer	myChilds = new KCMutableCategoryContainer();
	
	//*コンストラクタ**************************

	/**
	* 空のカテゴリを作成する
	*/
	protected
	KCCategory()
	{
		myId = myIdFactory.getId();
	}
		
	//*メソッド**********************************

	/**
	* ルートかどうか得る
	* @return ルートならtrue
	*/
	public boolean
	isRoot()
	{
		return ( parent() == null );
	}
	
	/**
	* 終端かどうか得る
	* @return 終端ならtrue
	*/
	public boolean
	isLeaf()
	{
		return ( childs().isEmpty() );
	}


	/**
	* 所属する本を得る
	* @return 所属している本
	*/
	public KCBook
	book()
	{
		return myBook;
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
	*　カテゴリIDを得る．
	* @return ID
	*/
	public KCId
	id()
	{
		return myId;
	}
	
	/**
	* カテゴリIDを設定する
	* @param aId 設定するID
	*/
	protected void
	setId(
		KCId aId
		)
	{
		myId = aId;
	}
		
	/**
	* 親カテゴリを得る．
	* @return 親カテゴリの参照（ルートならnull）
	*/
	public KCCategory
	parent()
	{
		return myParent;
	}
	
	/**
	* 親カテゴリを変更する．
	* @param aCategory 設定する親カテゴリ
	*/
	protected void
	setParent(
		KCCategory aCategory
		)
	{
		myParent = aCategory;
	}

	/**
	* 子カテゴリを得る
	* @return 子カテゴリ
	*/
	public KCCategoryContainer
	childs()
	{
		return mutableChilds();
	}
		
	/**
	* 書き換え可能な子カテゴリコンテナを返す
	* @return 子カテゴリ
	*/
	protected KCMutableCategoryContainer
	mutableChilds()
	{
		return myChilds;
	}

	/**
	* カテゴリの名前を返す
	* @return カテゴリの名前
	*/
	public KCCategoryName
	name()
	{
		return myName;
	}
		
	/**
	 * カテゴリの名前を設定する
	 * @param aName カテゴリの名前
	 */
	protected void
	setName(
		KCCategoryName aName
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
	* 深さを得る
	* @return 深さ(ルート＝0)
	*/
	public int
	depth()
	{
		if ( isRoot() )
			return 0;
		else
			return parent().depth()+1;
	}
	
	/**
	* カテゴリのパスからカテゴリを探す
	* @return カテゴリ．見つからなければnull．
	* @param aPath カテゴリのパス
	*/
	public KCCategory
	search(
		KCCategoryPath aPath
		)
	{
		String path = aPath.string();
		if ( path.equals( SEPARATOR ) )
			return this;
		
		int nextPathEndIndex = path.indexOf( SEPARATOR , SEPARATOR.length() );
		KCCategoryPath nextPath = new KCCategoryPath( path.substring( nextPathEndIndex ) );
		KCCategoryName target = aPath.rootCategory();
		
		KCCategoryIterator itr = childs().iterator();
		while( itr.hasNext() )
		{
			KCCategory category = (KCCategory)itr.next();
			if ( category.name().equals( target ) )
			{
				return category.search( nextPath );
			}
		}
		return null;
	}
	
	/**
	* パスを返す
	* @return カテゴリのフルパス
	*/
	public KCCategoryPath
	path()
	{
		if ( isRoot() )
		{
			return new KCCategoryPath( new String(
				name().string() + SEPARATOR 
				) );
		}
		else
		{
			return new KCCategoryPath( new String(
				parent().path().string() + name().string() + SEPARATOR
				) );
		}		
	}
	
	/**
	* カテゴリのパスを文字列として返す
	* @return カテゴリのフルパス
	*/
	public String
	string()
	{
		return path().string();
	}

	
	/**
	 * DOMノードに変換する
	 * @return DOMノードに変換したレコード
	 */
	public KCMutableDOMNode
	domNode()
	{
		KCDOMElementNode node = new KCDOMElementNode( DOM_CATEGORY );
		
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
		
		{//---子カテゴリ
		
			KCCategoryIterator itr = childs().iterator();
			while( itr.hasNext() )
			{
				node.addChild( itr.next().domNode() );
			}

		}
		return node;
	}
	
					
}
