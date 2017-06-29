//
//  KCStdBook.java
//  Kakeibo
//
//  Created by あっきー on 06/01/29.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class KCStdBook extends KCMutableBook
{
	//*プロパティ***************************
	private String	myFilepath = null;
	private SheetContainer	mySheetContainer = SheetContainer.defaultSheetContainer();
	
	//*コンストラクタ************************
	
	/**
	* 空の本を作成する
	*/
	public 
	KCStdBook()
	{
		super();
	}

	//*メソッド**********************************

	/**
	* ファイルパスを設定する
	*/
	public void 
	setFilepath(
		String aFilepath
		)
	{
		myFilepath = aFilepath;
	}
	
	/**
	* ファイルパスを得る
	* @return ファイルパス
	*/
	public String
	filepath()
	{
		return myFilepath;
	}

	/**
	* シートコンテナを設定する。
	* @param aSheetContainer 設定するコンテナ
	*/
	public void 
	setSheetContainer(
		SheetContainer aSheetContainer
		)
	{
		mySheetContainer = aSheetContainer;
	}
	
	/**
	* シートコンテナを得る
	* @return シートコンテナ
	*/
	public SheetContainer
	sheetContainer()
	{
		return mySheetContainer;
	}

}
