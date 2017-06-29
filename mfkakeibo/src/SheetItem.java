//
//  SheetItem.java
//  Kakeibo
//
//  Created by あっきー on 06/02/17.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.util.*;
import com.akipen.kclib.*;

public class SheetItem 
{
	//*プロパティ***************************
	private List myCategories = new LinkedList();
	private String myName = "";
	
	//*コンストラクタ************************
	
	/**
	* 空のアイテムを作成する。
	*/
	public 
	SheetItem()
	{
	}
		
	//*メソッド***************************
	
	/**
	* カテゴリを追加する
	* @param aCategoryPath 追加するカテゴリのパス
	*/
	public void
	addPath(
		KCCategoryPath aCategory
		)
	{
		myCategories.add( aCategory );
	}

	/**
	* カテゴリを除去する
	* @param aCategory 除去するカテゴリのパス
	*/
	public void
	removePath(
		KCCategoryPath aCategory
		)
	{
		myCategories.remove( aCategory );
	}
	
	/**
	* カテゴリがアイテム内に含まれているものか判定する
	* @return 内包する時，true
	*/
	public boolean
	isInclude(
		KCCategoryPath aCategory
		)
	{
		Iterator itr = myCategories.iterator();
		while( itr.hasNext() )
		{
			KCCategoryPath path = (KCCategoryPath)itr.next();
			if ( aCategory.isBelongTo( path ) )
				return true;
		}
		return false;
	}
	
	/**
	* アイテム名を取得する
	* @return アイテム名
	*/
	public String
	name()
	{
		return myName;
	}
	
	/**
	* アイテム名を設定する
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
