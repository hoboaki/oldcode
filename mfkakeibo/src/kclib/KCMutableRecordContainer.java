package com.akipen.kclib;
/**<pre>
* 書き換え可能なレコードのデータベースクラス．
* 
* レコードのデータベース中に，同じレコードIDを持つレコードは存在しないようにしてください．
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
public class KCMutableRecordContainer extends KCRecordContainer
{
	//*コンストラクタ*************************
	
	/**
	* 空のレコードコンテナを作成する
	*/
	public
	KCMutableRecordContainer()
	{
		super();
	}
	
	//*メソッド**************************	

	/**
	* ユニーク判定をして，OKであればレコードを追加する。レコードは所属口座が設定済みである必要がある。
	* @return 追加したらtrue
	* @param aRecord 追加するレコード
	*/
	public boolean
	add(
		KCRecord aRecord
		)
	{
		if ( find( aRecord ) == null )
			if ( addProcess( aRecord ) )
				return true;		
		return false;
	}
		
	/**
	* レコードを削除する
	* @return 成功したら削除したレコード。失敗したらnull。
	* @param aRecord 削除するレコード
	*/
	public KCRecord
	remove(
		KCRecord aRecord
		)
	{
		KCMutableRecord record = find( aRecord );
		if ( record != null )
		{
			if ( removeProcess( record ) )
			{
				return record;
			}
		}
		return null;
	}
	
	/**
	* 一致するレコードがあるかどうか探す
	* @return あればその実体を返す
	* @param aRecord 探索するレコード（IDをキーとして探す）
	*/
	public KCMutableRecord
	find(
		KCRecord aRecord
		)
	{
		if ( aRecord.id() == null || aRecord.account() == null )
			return null;
			
		KCRecordIterator itr = iterator();
		while( itr.hasNext() )
		{
			KCMutableRecord targetRecord = (KCMutableRecord)itr.next();
			if ( aRecord.equals( targetRecord ) )
			{
				return targetRecord;
			}
		}
		return null;	
	}
			
}
