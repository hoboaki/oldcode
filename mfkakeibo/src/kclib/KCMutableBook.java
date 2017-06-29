package com.akipen.kclib;
/**<pre>
* 書き換え可能な家計簿本のクラス．
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
public class KCMutableBook extends KCBook
{
	//*コンストラクタ*************************
		
	/**
	* 空の本を作成する
	*/
	public 
	KCMutableBook()
	{
		super();
	}
	
	//*メソッド**********************************

	/**
	 * 口座を追加する
	 * @return 成功したらtrue
	 * @param aAccount 追加する口座
	 */
	public KCAccount
	addAccount(
		KCAccount aAccount
		)
	{
		KCMutableAccountContainer container = mutableAccountContainer();
		KCAccountFactory factory = accountFactory();
		return container.add( aAccount );
	}
	
	/**
	 * 指定の口座を取り除く
	 * @return 成功したらtrue
	 * @param aAccount 取り除く口座
	 */
	public KCAccount
	removeAccount(
		KCAccount aAccount
		)
	{
		return mutableAccountContainer().remove( aAccount );
	}

	/**
	* 指定の口座の情報を更新する
	* @return 成功したらtrue
	* @param aOldAccount 更新対象の口座
	* @param aNewAccount 新しい口座
	*/
	public boolean
	updateAccount(
		KCAccount aOldAccount,
		KCAccount aNewAccount
		)
	{
		KCMutableAccount account = mutableAccountContainer().find( aOldAccount );
		if ( account == null )
			return false;
		accountFactory().copyAccount( aNewAccount , account );
		account.setBook( this );
		return true;
	}

	/**
	* 一致する口座があるかどうか得る
	* @return あればその口座の実体
	* @param aAccount 探索する口座
	*/
	protected KCMutableAccount
	findAccount(
		KCAccount aAccount
		)
	{
		if ( aAccount.id() == null )
			return null;
				
		KCAccountIterator itr = accountIterator();
		while( itr.hasNext() )
		{
			KCMutableAccount targetAccount = (KCMutableAccount)itr.next();
			if ( targetAccount == null )
				continue;
			if ( targetAccount.equals( aAccount ) )
			{
				return targetAccount;
			}
		}
		return null;
	}
		
	/**
	* 指定の口座にレコードを追加する。
	* @return 成功したら追加できたらレコード。失敗したらnull。
	* @param aRecord 追加するレコード
	* @param aAccount 対象となる口座
	*/
	public KCRecord
	addRecordAtAccount(
		KCRecord aRecord,
		KCAccount aAccount
		)
	{
		KCMutableAccount account = findAccount( aAccount );
		if ( account != null )
		{
			return account.addRecord( aRecord );
		}
		return null;
	}

	/**
	* 指定の口座にレコードを削除する。
	* @return 成功したら削除したレコード。失敗したらnull。
	* @param aRecord 削除するレコード
	* @param aAccount 対象となる口座
	*/
	public KCRecord
	removeRecordAtAccount(
		KCRecord aRecord,
		KCAccount aAccount
		)
	{
		KCMutableAccount account = findAccount( aAccount );
		if ( account != null )
		{
			return account.removeRecord( aRecord );
		}
		return null;
	}

	/**
	* 指定の口座のレコードの情報を更新する。
	* @return 成功したら情報を更新したレコード。失敗したらnull。
	* @param aOldRecord 更新対象となるレコード
	* @param aNewRecord 更新後となるレコード	
	* @param aAccount 対象となる口座
	*/
	public KCRecord
	updateRecordAtAccount(
		KCRecord aOldRecord,
		KCRecord aNewRecord,
		KCAccount aAccount
		)
	{
		KCMutableAccount account = findAccount( aAccount );
		if ( account != null )
		{
			return account.updateRecord( aOldRecord , aNewRecord );
		}
		return null;
	}
			
	/**
	* カテゴリを追加する
	* @return 成功したらtrue
	* @param aParentCategory 追加するカテゴリの親カテゴリ
	* @param aCategory 追加するカテゴリ
	*/
	public boolean
	addCategory(
		KCCategory aParentCategory,
		KCCategory aCategory
		)
	{
		if ( rootCategory().search( aParentCategory.path() ) == null )
			return false;
			
		//---更新
		KCMutableCategory parentCategory = (KCMutableCategory)aParentCategory;
		KCMutableCategory category = (KCMutableCategory)aCategory;
		parentCategory.addChild( category );
		
		return true;
		
	}
	
	/**
	* カテゴリの情報を更新する．（名前とコメントがコピーされる）
	* @return 成功したらtrue
	* @param aOldCategory 更新するカテゴリ
	* @param aNewCategory 新しいカテゴリ
	*/
	public boolean
	updateCategory(
		KCCategory aOldCategory,
		KCCategory aNewCategory
		)
	{
		if ( rootCategory().search( aOldCategory.path() ) == null )
			return false;
			
		//---更新
		KCMutableCategory category = (KCMutableCategory)aOldCategory;
		boolean changeName = !aOldCategory.name().equals( aNewCategory.name() );
		KCCategoryPath oldPath = aOldCategory.path();
		category.setName( aNewCategory.name() );
		category.setComment( aNewCategory.comment() );
		
		//---レコードのカテゴリパスを編集する
		if ( changeName )
		{
			KCCategoryPath newPath = category.path();
			KCAccountIterator accountItr = accountContainer().iterator();
			while( accountItr.hasNext() )
			{
				KCAccount account = (KCAccount)accountItr.next();
				KCRecordIterator recordItr = account.recordContainer().iterator();
				while( recordItr.hasNext() )
				{
					KCMutableRecord record = (KCMutableRecord)recordItr.next();
					
					if ( record.categoryPath().isBelongTo( oldPath ) )
					{//---変更
						String tailPath = record.categoryPath().string().substring( oldPath.string().length() );
						KCCategoryPath path = oldPath.string().length() == record.categoryPath().string().length() ?
										newPath : new KCCategoryPath( newPath.string().concat( tailPath ) );
						record.setCategoryPath( path );
					}
					
				}
			
			}
		}
		
		return true;
		
	}
	
	/**
	* カテゴリを削除する．
	* @return 成功したらtrue
	* @param aCategory 削除するカテゴリ
	*/
	public boolean
	removeCategory(
		KCCategory aCategory
		)
	{
		if ( rootCategory().search( aCategory.path() ) == null )
			return false;
			
		//---更新
		KCMutableCategory parentCategory = (KCMutableCategory)( aCategory.parent() );
		KCCategoryPath oldPath = aCategory.path();
		if ( !parentCategory.mutableChilds().remove( aCategory ) )
			return false;
			
		//---レコードのカテゴリパスを編集する
		{
			KCCategoryPath newPath = parentCategory.path();
			KCAccountIterator accountItr = accountContainer().iterator();
			while( accountItr.hasNext() )
			{
				KCAccount account = (KCAccount)accountItr.next();
				KCRecordIterator recordItr = account.recordContainer().iterator();
				while( recordItr.hasNext() )
				{
					KCMutableRecord record = (KCMutableRecord)recordItr.next();
					
					if ( record.categoryPath().isBelongTo( oldPath ) )
					{//---変更
						record.setCategoryPath( newPath );
					}
					
				}
			
			}
		}
		
		return true;	
	}

	/**
	 * 本の名前を設定する
	 * @param aName 本の名前
	 */
	public void
	setName(
		KCName aName
		)
	{
		super.setName( aName );
	}	
	
	/**
	* コメントを設定する
	* @param aComment コメント
	*/
	public void
	setComment(
		String aComment
		)
	{
		super.setComment( aComment );
	}
	
	/**
	* ルートカテゴリを設定する
	* @param aCategory ルートカテゴリ
	*/
	public void
	setRootCategory(
		KCMutableCategory aCategory
		)
	{
		super.setRootCategory( aCategory );
	}
	
}
