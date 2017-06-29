package com.akipen.kclib;
/**<pre>
* 書き換え可能な口座のデータベースクラス．
* 
* 口座のデータベース中に，同じ口座IDを持つ口座は存在しないようにしてください．
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
public class KCMutableAccountContainer extends KCAccountContainer
{
	//*コンストラクタ*************************
	
	/**
	* 空の口座コンテナを作成する
	*/
	public
	KCMutableAccountContainer()
	{
		super();
	}
	
	//*メソッド**************************		
	
	/**
	* ユニーク判定をして，OKであれば口座を追加する
	* @return 成功したら追加した口座。失敗したらnull。
	* @param aAccount 追加する口座
	*/
	public KCAccount
	add(
		KCAccount aAccount
		)
	{
		KCMutableAccount account = find( aAccount );
		if ( find( aAccount ) == null )
		{
			if ( addProcess( aAccount ) )
			{
				return aAccount;		
			}
		}
		return null;
	}
		
	/**
	* 口座を削除する
	* @return 成功したら削除したら口座。失敗したらnull。
	* @param aAccount 削除する口座
	*/
	public KCAccount
	remove(
		KCAccount aAccount
		)
	{
		KCMutableAccount account = find( aAccount );
		if ( account != null )
		{
			if( removeProcess( account ) )
			{
				return account;
			}
		}
		return null;
	}

	/**
	* 一致する口座があるかどうか得る
	* @return あればその口座の実体
	* @param aAccount 探索する口座
	*/
	protected KCMutableAccount
	find(
		KCAccount aAccount
		)
	{
		if ( aAccount.id() == null )
			return null;
				
		KCAccountIterator itr = iterator();
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
		
}
