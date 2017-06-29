//
//  SheetContainer.java
//  Kakeibo
//
//  Created by あっきー on 06/05/04.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.util.*;

public class SheetContainer 
{
	//*クラスのメソッド************************
	
	/**
	* デフォルトのシートコンテナを取得する。
	* @return デフォルトのシートコンテナ
	*/
	static public
	SheetContainer
	defaultSheetContainer()
	{
		SheetContainer sc = new SheetContainer();
		sc.addSheet( Sheet.abstractSheet() );
		return sc;
	}
	
	//*プロパティ*****************************
	private List myList = new LinkedList();
	
	//*コンストラクタ**************************
	SheetContainer(){}
	
	//*メソッド*******************************
	
	/**
	* シートの総数を得る。
	* @return 総数
	*/
	public 
	int
	size()
	{
		return myList.size();
	}
	
	/**
	* シートを取得する。
	* @param aIndex インデックス
	*/
	public 
	Sheet
	getSheet(
		int aIndex
		)
	{
		return (Sheet)myList.get( aIndex );
	}
	
	/**
	* シートを追加する。
	* @param aSheet 追加するシート
	*/
	public 
	void
	addSheet(
		Sheet aSheet
		)
	{
		myList.add( aSheet );
	}

}
