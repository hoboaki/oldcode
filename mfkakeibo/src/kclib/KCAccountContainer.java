package com.akipen.kclib;
/**<pre>
* 口座のデータベースクラス．
* 
* 口座のデータベース中に，同じ口座IDを持つレコードは存在しないようにしてください．
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
import java.util.*;
public class KCAccountContainer extends KCObject
{
	//*プロパティ*****************************
	private List myList = new LinkedList();
	
	//*コンストラクタ*************************
	
	/**
	* 空の口座コンテナを作成する
	*/
	public
	KCAccountContainer()
	{
	}
	
	//*メソッド**************************	
	
	/**
	* 要素数を返す
	* @return 要素数
	*/
	public int
	size()
	{
		return myList.size();
	}

	/**
	* 要素数が0かどうか返す
	* @return size()==0
	*/
	public boolean
	isEmpty()
	{
		return ( size() == 0 );
	}
	
	/**
	* 口座を追加する
	* @return 追加できたらtrue
	* @param aAccount 追加する口座
	*/
	protected boolean
	addProcess(
		KCAccount aAccount
		)
	{
		return myList.add( aAccount );
	}

	/**
	* 口座を削除する
	* @return 削除できたらtrue
	* @param aAccount 削除する口座
	*/
	protected boolean
	removeProcess(
		KCAccount aAccount
		)
	{
		Iterator itr = myList.listIterator();
		while( itr.hasNext() )
		{
			KCAccount targetAccount = (KCAccount)itr.next();
			if ( aAccount.equals( targetAccount ) )
			{
				return myList.remove( targetAccount );
			}
		}
		return false;
	}
	
	/**
	* イテレータを得る
	* @return 口座イテレータ
	*/
	public KCAccountIterator
	iterator()
	{
		return new KCStdAccountIterator( myList.listIterator() );
	}
	
	/**
	* KCStdAccountContainerのイテレータクラス
	*/
	static private class KCStdAccountIterator extends KCAccountIterator
	{
		//*プロパティ*****************************
		private Iterator myIterator;	
	
		//*コンストラクタ**************************
		
		/**
		* イテレータを作成する
		* @param aSrcIterator コンテナが所有するイテレータ
		*/
		public
		KCStdAccountIterator(
			Iterator aSrcIterator
			)
		{
			myIterator = aSrcIterator;
		}
	
		//*メソッド*******************************
		
		/**
		* イテレータが終端にいるかどうか返す
		* @return 終端でないならtrue
		*/
		public boolean
		hasNext()
		{
			return myIterator.hasNext();
		}
		
		/**
		* 現在示しているカテゴリを返して，口座を次に進める
		* @return 次に進める前に示しているオブジェクト
		*/
		public KCAccount
		next()
		{
			return (KCAccount)myIterator.next();
		}
		
	}
	
}
