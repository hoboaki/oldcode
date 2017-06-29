package com.akipen.kclib;
/**<pre>
* 可変口座のクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2006/01/02 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCMutableAccount extends KCAccount
{
	//*コンストラクタ*************************
	
	/**
	* 空の口座を作成する
	*/
	public
	KCMutableAccount()
	{
		super();
	}
		
	//*メソッド******************************

	/**
	* コピーした口座を作成する
	* @return コピーした口座
	*/
	public Object
	clone()
	{
		KCMutableAccount account = new KCMutableAccount();
		account.initWithAccount( this );
		
		KCRecordIterator itr = mutableRecordContainer().iterator();
		while( itr.hasNext() )
		{
			account.addRecord( itr.next() );
		}
		return account;
	}

	/**
	 * レコードを追加する
	 * @return 成功したら追加したレコード。失敗したらnull。
	 * @param aRecord 追加するレコード
	 */
	public KCRecord
	addRecord(
		KCRecord aRecord
		)
	{
		KCMutableRecordContainer container = mutableRecordContainer();
		KCRecordFactory factory = recordFactory();
		KCMutableRecord record = factory.createRecord( this , aRecord );
		if ( container.add( record ) )
			return record;
		return null;
	}
	
	/**
	 * 指定のレコードを取り除く
	 * @return 成功したら取り除いたレコード。失敗したらnull。
	 * @param aRecord 取り除くレコード
	 */
	public KCRecord
	removeRecord(
		KCRecord aRecord
		)
	{
		return mutableRecordContainer().remove( aRecord );
	}

	/**
	* 指定のレコードの情報を更新する
	* @return 成功したら更新したレコード。失敗したらnull。
	* @param aOldRecord 更新対象のレコード
	* @param aNewRecord 新しいレコード
	*/
	public KCRecord
	updateRecord(
		KCRecord aOldRecord,
		KCRecord aNewRecord
		)
	{
		KCMutableRecord record = mutableRecordContainer().find( aOldRecord );
		if ( record != null )
		{
			recordFactory().copyRecord( aNewRecord , record );
			record.setAccount( this );
			return record;
		}
		return null;
	}

	/**
	* 口座の情報をコピーする。
	* @param aAccount コピー元の口座
	*/
	public void
	initWithAccount(
		KCAccount aAccount
		)
	{
		super.initWithAccount( aAccount );
	}
	
	/**
	* 所属する本を設定する
	* @param aBook 所属する本
	*/
	public void
	setBook(
		KCBook aBook
		)
	{
		super.setBook( aBook );
	}
	
	/**
	* 口座IDを設定する
	* @param aId 口座ID
	*/
	/*public void
	setId(
		KCId aId
		)
	{
		super.setId( aId );
	}*/

	/**
	 * 口座名を設定する
	 * @param aName 設定する口座名
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
	* レコードコンテナを設定する
	* @param aContainer レコードコンテナ
	*/
	public void
	setRecordContainer(
		KCMutableRecordContainer aContainer
		)
	{
		super.setRecordContainer( aContainer );
	}
	
	/**
	* レコードファクトリを設定する
	* @param aFactory レコードファクトリ
	*/
	public void
	setRecordFactory(
		KCRecordFactory aFactory
		)
	{
		super.setRecordFactory( aFactory );
	}
	

}
