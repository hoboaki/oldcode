package com.akipen.kclib;
/**<pre>
* カテゴリーのデータベース抽象クラス．
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
import java.util.*;
public class KCCategoryContainer extends KCObject
{
	//*プロパティ*****************************
	private List myList = new LinkedList();
	
	//*コンストラクタ*************************
	
	/**
	* 空のカテゴリコンテナを作成する
	*/
	protected
	KCCategoryContainer()
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
	* カテゴリを追加する
	* @return 追加できたらtrue
	* @param aCategory 追加するカテゴリ
	*/
	protected boolean
	addProcess(
		KCCategory aCategory
		)
	{
		return myList.add( aCategory );
	}

	/**
	* カテゴリを削除する
	* @return 削除できたらtrue
	* @param aCategory 削除するカテゴリ
	*/
	protected boolean
	removeProcess(
		KCCategory aCategory
		)
	{
		Iterator itr = myList.listIterator();
		while( itr.hasNext() )
		{
			KCCategory targetCategory = (KCCategory)itr.next();
			if ( aCategory.equals( targetCategory ) )
			{
				return myList.remove( targetCategory );
			}
		}
		return false;
	}
		
	/**
	* 一致するカテゴリがあるかどうかを得る
	* @return 見つかればtrue
	* @param aCategory 探索するカテゴリ
	*/
	public boolean
	find(
		KCCategory aCategory
		)
	{
		if ( aCategory.id() == null )
			return false;
	
		Iterator itr = myList.listIterator();
		while( itr.hasNext() )
		{
			KCCategory targetCategory = (KCCategory)itr.next();
			if ( targetCategory.id() != null )
			{
				if ( targetCategory.id().equals( aCategory.id() ) )
				{
					return true;
				}
			}
		}
		return false;
	}
	
	/**
	* イテレータを得る
	* @return カテゴリイテレータ
	*/
	public KCCategoryIterator
	iterator()
	{
		return new KCCategoryIteratorImpl( myList.listIterator() );
	}
	
	/**
	* KCCategoryContainerのイテレータクラス
	*/
	static private class KCCategoryIteratorImpl extends KCCategoryIterator
	{
		//*プロパティ*****************************
		private Iterator myIterator;	
	
		//*コンストラクタ**************************
		
		/**
		* イテレータを作成する
		* @param aSrcIterator コンテナが所有するイテレータ
		*/
		public
		KCCategoryIteratorImpl(
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
		* 現在示しているカテゴリを返して，カテゴリを次に進める
		* @return 次に進める前に示しているオブジェクト
		*/
		public KCCategory
		next()
		{
			return (KCCategory)myIterator.next();
		}
		
	}
	
}
