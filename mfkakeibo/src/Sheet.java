//
//  Sheet.java
//  Kakeibo
//
//  Created by あっきー on 06/02/17.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.util.*;
import com.akipen.kclib.*;

public class Sheet 
{
	//*クラスのメソッド*******************************
	
	/**
	* アブストラクトシートを取得する
	* @return アブストラクトシート
	*/
	static public Sheet
	abstractSheet()
	{
		Sheet sheet = new Sheet();
		sheet.setName( NSBundle.localizedString( "NameOfAbstractSheet" ) );
		{//---カテゴリを作成
			final String separator = KCCategory.SEPARATOR;
			{//---繰越金
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfCarryMoney() );
				item.addPath( Common.categoryPathOfCarryMoney() );
				sheet.addItem( item );
			}
			{//---預貯金引出
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfCategoryMoveInput() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryInput() + separator + Common.nameOfCategoryMoveInput() + separator ) );
				sheet.addItem( item );
			}
			{//---預貯金預入
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfCategoryMoveOutput() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryOutput() + separator + Common.nameOfCategoryMoveOutput() + separator ) );
				sheet.addItem( item );			
			}
			{//---収入
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfCategoryPureInput() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryInput() + separator + Common.nameOfCategoryPureInput() + separator ) );
				sheet.addItem( item );			
			}
			{//---出金
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfCategoryPureOutput() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryOutput() + separator + Common.nameOfCategoryPureOutput() + separator ) );
				sheet.addItem( item );			
			}
			{//---小計
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfSubTotal() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryInput() + separator ) );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryOutput() + separator ) );
				sheet.addItem( item );						
			}
			{//---残高
				SheetItem item = new SheetItem();
				item.setName( Common.nameOfRestMoney() );
				item.addPath( Common.categoryPathOfCarryMoney() );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryInput() + separator ) );
				item.addPath( new KCCategoryPath( separator + Common.nameOfCategoryOutput() + separator ) );
				sheet.addItem( item );					
			}							
		}
		return sheet;
	}

	//*プロパティ***************************
	private List myItems = new LinkedList();
	private String myName = "";
	
	//*コンストラクタ************************
	
	/**
	* 空のシートを作成する
	*/
	public
	Sheet()
	{
	}
	
	//*メソッド***************************
	
	/**
	* アイテム総数を得る
	* @return アイテム総数
	*/
	public int
	size()
	{
		return myItems.size();
	}
	
	/**
	* アイテムを追加する
	* @param aItem 追加するアイテム
	*/
	public void
	addItem(
		SheetItem aItem
		)
	{
		myItems.add( aItem );
	}
	
	/**
	* アイテムを除去する
	* @param aItem 除去するアイテム
	*/
	public void
	removeItem(
		SheetItem aItem
		)
	{
		myItems.remove( aItem );
	}
	
	/**
	* アイテムを取得する
	* @param aIndex 取得するアイテムのインデックス
	*/
	public SheetItem
	itemAtIndex(
		int aIndex
		)
	{
		return (SheetItem)myItems.get( aIndex );
	}
	
	/**
	* シート名を取得する
	* @return アイテム名
	*/
	public String
	name()
	{
		return myName;
	}
	
	/**
	* シート名を設定する
	* @param aName 設定する名前
	*/
	public void
	setName(
		String aName
		)
	{
		myName = aName;
	}
	
}
