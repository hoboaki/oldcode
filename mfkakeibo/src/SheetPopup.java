//
//  SheetPopup.java
//  Kakeibo
//
//  Created by あっきー on 06/05/04.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;


public class SheetPopup
{
	//*クラスのメソッド*******************************
	
	/**
	* アクティブシートのポップアップボタンを取得する。
	* シートが変更されると，sheetPopupDidChangeが呼び出される。
	* @param target 呼び出し関数のターゲット
	*/
	static public 
	SheetPopup getObj(
		Object target
		)
	{
		SheetPopup popup = new SheetPopup( );
		popup.button().setTarget( target );
		popup.button().setAction( new NSSelector( "sheetPopupDidChange" , new Class[]{ Object.class } ) );
		return popup;
	}
	
	//*プロパティ***********************************
	private NSPopUpButton myPopup;
	
	//*コンストラクタ********************************
	public SheetPopup()
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("SheetPopup", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load SheetPopup.nib");
			return;
        }
	}

	//*メソッド*************************************
	
	/**
	* ポップアップボタン本体を取得する。
	* @return ポップアップボタン
	*/
	public
	NSPopUpButton
	button()
	{
		return myPopup;
	}
	
	/**
	* シートの一覧を更新する。
	* @param aSheetContainer 更新に使用するコンテナ
	*/
	public void
	updateList(
		SheetContainer aSheetContainer
		)
	{
		button().removeAllItems();
		for ( int i = 0; i < aSheetContainer.size(); ++i )
		{
			button().addItem( aSheetContainer.getSheet( i ).name() );
		}
	}
	
}
