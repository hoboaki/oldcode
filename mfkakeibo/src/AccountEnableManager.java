//
//  AccountEnable.java
//  Kakeibo
//
//  Created by あっきー on 06/02/07.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.util.*;

import com.akipen.kclib.*;

public class AccountEnableManager 
{
	//*クラスのプロパティ**********************
	public	List		myAccountList = null;
	public	boolean[]	myEnables = null;
	
	//*コンストラクタ***********************
	
	/**
	* アカウントリストを作成する
	* @param aIterator 口座のイテレータ
	*/
	public 
	AccountEnableManager(
		KCAccountIterator aIterator
		)
	{
		reloadList( aIterator );
	}
	
	//*メソッド******************************
	
	/**
	* アカウントリストを再構築する
	* @param aIterator 口座のイテレータ
	*/
	public void
	reloadList(
		KCAccountIterator aIterator
		)
	{
		myAccountList = new LinkedList();
		while( aIterator.hasNext() )
			myAccountList.add( aIterator.next() );
		myEnables = new boolean[myAccountList.size()];
		setAll( true );
	}
	
	/**
	* 口座数を返す
	* @return 口座数
	*/
	public int
	size()
	{
		return myAccountList.size();
	}
	
	/**
	* 全てを同じ状態にする
	* @return フラグが変化したらtrue
	* @param aEnableFlg イネイブルフラグ
	*/
	public boolean
	setAll(
		boolean aEnableFlg
		)
	{
		boolean changeFlg = false;
		for ( int i = 0; i < size(); ++i )
		{
			if ( myEnables[i] != aEnableFlg )
			{
				myEnables[i] = aEnableFlg;
				changeFlg = true;
			}
		}
		return changeFlg;
	}
	
	/**
	* インデックス番号で指定した口座のイネイブルフラグを設定する
	* @return フラグが変化したらtrue
	* @param aEnableFlg イネイブルフラグ
	* @param aIndex インデックス
	*/
	public boolean
	setAtIndex(
		boolean aEnableFlg,
		int aIndex
		)
	{
		if ( aIndex < 0 || aIndex >= size() )
			return false;
		if ( myEnables[aIndex] == aEnableFlg )
			return false;		
		myEnables[aIndex] = aEnableFlg;
		return true;
	}
	
	/**
	* 指定した口座のイネイブルフラグを設定する
	* @return フラグが変化したらtrue
	* @param aEnableFlg イネイブルフラグ
	* @param aAccount 口座
	*/
	public boolean
	setForAccount(
		boolean aEnableFlg,
		KCAccount aAccount
		)
	{
		int index = getIndexForAccount( aAccount );
		if ( index < 0 )
			return false;
		return setAtIndex( aEnableFlg , index );
	}
	
	/**
	* インデックス番号で指定した口座のイネイブルフラグを取得する
	* @return 取得したフラグ
	* @param aIndex インデックス
	*/
	public boolean
	isEnableAtIndex(
		int aIndex
		)
	{
		if ( aIndex < 0 || aIndex >= size() )
			return false;
		return myEnables[aIndex];
	}
	
	/**
	* 指定した口座のイネイブルフラグを取得する	
	* @return 取得したフラグ
	* @param aIndex インデックス
	*/
	public boolean
	isEnableForAccount(
		KCAccount aAccount
		)
	{
		int index = getIndexForAccount( aAccount );
		if ( index < 0 )
			return false;
		return isEnableAtIndex( index );	
	}
	
	//*オーバーライドメソッド**************************
	
	/**
	* メインメニュー"口座"を開いたときに状態を更新する
	*/
	public void
	menuNeedsUpdate(
		NSMenu menu
		)
	{
		NSMenuItem baseItem = menu.itemAtIndex( 3 );
		
		//---項目数を調整する
		while( menu.numberOfItems() > 3 )
		{
			menu.removeItemAtIndex( 3 );
		}
		Iterator itr = myAccountList.iterator();
		int cnt = 0;
		while( itr.hasNext() )
		{
			KCAccount account = (KCAccount)itr.next();
			NSMenuItem item = new NSMenuItem();
			item.setTitle( account.name().string() );
			item.setTag( cnt );
			item.setAction( baseItem.action() );
			if ( isEnableAtIndex( cnt ) )
				item.setState( NSCell.OnState );
			else
				item.setState( NSCell.OffState );
			menu.addItem( item );
			cnt++;
		}
		
	}
		
	//*プライベートメソッド**************************
	private int
	getIndexForAccount(
		KCAccount aAccount
		)
	{
		int cnt = 0;
		Iterator itr = myAccountList.listIterator();
		while( itr.hasNext() )
		{
			KCAccount account = (KCAccount)itr.next();
			if ( account.equals( aAccount ) )
			{
				return cnt;
			}
			cnt++;
		}	
		return -1;
	}
	
}
