package com.akipen.kclib;
/**<pre>
* 書き換え可能なレコードクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCMutableRecord extends KCRecord
{
	//*コンストラクタ*************************
	
	/**
	* 空のレコードを作成
	*/
	public
	KCMutableRecord()
	{
		super();
	}

	/**
	 * 口座を指定してレコードを作成する
	 * @param aAccount 指定する口座
	 */
	/*public
	KCMutableRecord(
		KCAccount aAccount
		)
	{
		super( aAccount );
	}*/	
	
	//*メソッド***************************

	/**
	* コピーしたレコードを作成する。
	* @return コピーしたレコード
	*/
	public Object
	clone()
	{
		KCMutableRecord record = new KCMutableRecord();
		record.initWithRecord( this );
		return record;
	}

	/**
	 * aRecoardの内容で初期化する（IDと口座はコピーされない）
	 * @param aRecord 初期化する内容
	 */
	public void
	initWithRecord(
		KCRecord aRecord
		)
	{
		super.initWithRecord( aRecord );
	}

	/**
	* レコードIDを設定する
	* @param aId 設定するレコードID
	*/
	/*public void
	setId(
		KCId aId
		)
	{
		super.setId( aId );
	}*/
	
	/**
	* 所属口座を設定する
	* @param aAccount 設定する口座
	*/
	public void
	setAccount(
		KCAccount aAccount
		)
	{
		super.setAccount( aAccount );
	}
	
	/**
	* 品目名を設定する
	* @param aName 設定する名前
	*/
	public void
	setName(
		KCRecordName aName
		)
	{
		super.setName( aName );
	}
	
	/**
	* 金額を設定する
	* @param aMoney 設定する金額
	*/
	public void
	setMoney(
		KCMoney aMoney
		)
	{
		super.setMoney( aMoney );
	}
	
	/**
	* 所属するカテゴリのパスを設定する．
	* @param aCategoryPath 設定するカテゴリのパス
	*/
	public void
	setCategoryPath(
		KCCategoryPath aCategoryPath
		)
	{
		super.setCategoryPath( aCategoryPath );
	}	
	
	/**
	* 日付を設定する
	* @param aDate 設定する日付
	*/
	public void
	setDate(
		KCDate aDate
		)
	{
		super.setDate( aDate );
	}
	
	/**
	* コメント・備考を設定する
	* @param aComment 設定するコメント
	*/
	public void
	setComment(
		String aComment
		)
	{
		super.setComment( aComment );
	}

	
}
