package com.akipen.kclib;
/**<pre>
* レコードのデータベースクラス．
* 
* レコードデータベースは，自動で日付古いが先順でソートします。
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
import java.util.*;
public class KCRecordContainer extends KCObject
{
	//*プロパティ*****************************
	private Collection myContainer = new LinkedList();
	
	//*コンストラクタ*************************
	
	/**
	* 空のレコードコンテナを作成する
	*/
	protected
	KCRecordContainer()
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
		return myContainer.size();
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
	* レコードを追加する
	* @return 追加できたらtrue
	* @param aRecord 追加するレコード
	*/
	protected boolean
	addProcess(
		KCRecord aRecord
		)
	{
    // 最後のレコード取得
    KCRecord lastRecord = null;
    if ( !isEmpty() )
    {
      lastRecord = (KCRecord)( (LinkedList)myContainer ).getLast();
    }		
  
    // コンテナに追加  
    boolean result = myContainer.add( aRecord );
    if ( !result )
    {
      return false;
    }
    
    // 最後のレコードと比較し，ソートする必要があればソートする
    KCRecordComparator comparator = new KCRecordComparator();
    if ( lastRecord != null && comparator.compare( lastRecord , aRecord ) > 0 )
		{//---ソートする
			Collections.sort( (List)myContainer ,  comparator );
		}
		return true;
	}

	/**
	* レコードを削除する
	* @return 削除できたらtrue
	* @param aRecord 削除するレコード
	*/
	protected boolean
	removeProcess(
		KCRecord aRecord
		)
	{
		Iterator itr = myContainer.iterator();
		while( itr.hasNext() )
		{
			KCRecord targetRecord = (KCRecord)itr.next();
			if ( aRecord.equals( targetRecord ) )
			{
				return myContainer.remove( targetRecord );
			}
		}
		return false;
	}
		
	/**
	* レコードを追加するとユニークな状態が保てるか判定する
	* @return ユニークである場合true
	* @param aRecord 追加するとするレコード
	*/
	/*public boolean
	willKeepUnique(
		KCRecord aRecord
		)
	{
		return true;
	}*/
		
	/**
	* イテレータを得る
	* @return レコードイテレータ
	*/
	public KCRecordIterator
	iterator()
	{
		return new KCRecordIteratorImpl( myContainer.iterator() );
	}
	
	/**
	* レコードを日付順でソートするコンパレータ
	*/
	static private class KCRecordComparator implements Comparator
	{
		//*メソッド*******************************
		public int 
		compare(
			Object o1,
			Object o2
			)
		{
			return ( (KCRecord)o1 ).date().compareTo( ( (KCRecord)o2 ).date() );
		}		
	}
	
	/**
	* KCRecordContainerのイテレータクラス
	*/
	static private class KCRecordIteratorImpl extends KCRecordIterator
	{
		//*プロパティ*****************************
		private Iterator myIterator;	
	
		//*コンストラクタ**************************
		
		/**
		* イテレータを作成する
		* @param aSrcIterator コンテナが所有するイテレータ
		*/
		public
		KCRecordIteratorImpl(
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
		* 現在示しているカテゴリを返して，レコードを次に進める
		* @return 次に進める前に示しているオブジェクト
		*/
		public KCRecord
		next()
		{
			return (KCRecord)myIterator.next();
		}
		
	}

	
}
