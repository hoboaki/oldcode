//
//  MainToolbar.java
//  Kakeibo
//
//  Created by あっきー on 05/11/13.
//  Copyright 2005 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;


public class MainToolbar
{//---メインウインドウのツールバーのクラス

	//■グローバル関数*****************************
	public static MainToolbar createToolbar(
		MainWindow	aWindow			//親ウインドウのアドレス
		)
	{//---ツールバーを作成する
		MainToolbar bar = new MainToolbar( aWindow );
		return bar;
	}

	//■グローバル定数******************************
	//===ツールバーアイテムの総数とインデックス番号
	public static final int	toolbarItemMany 			= 10;	//総数
	//---元から用意されているもの
	public static final int	toolbarItemFlexibleSpace	= 0;	//可変スペース
	public static final int	toolbarItemSpace			= 1;	//スペース
	public static final int	toolbarItemSeparator		= 2;	//セパレーター
	//---オリジナルのもの
	protected static int	toolbarItemAppOffset		= 3;	//アプリケーション固有アイテムのオフセット
	//public static final int	toolbarItemUndo				= 4;	//元に戻す
	//public static final int	toolbarItemRedo				= 5;	//やり直す
	public static final int	toolbarItemConfigure		= 3;	//カスタマイズ
	public static final int	toolbarItemDrawer			= 4;	//パネル
	public static final int	toolbarItemSave				= 5;	//保存
	public static final int	toolbarItemInput			= 6;	//入力
	public static final int	toolbarItemEdit				= 7;	//編集
	public static final int	toolbarItemDelete			= 8;	//削除
	public static final int	toolbarItemSheetPopup		= 9;	//シートポップアップ


	//■クラスのプロパティ**************************
    protected MainWindow 		myWindow;					//親ウインドウ
	protected NSToolbar			myToolbar;					//ツールバー
	protected CToolbarItem		myToolbarItem[]		= new CToolbarItem[toolbarItemMany];	//ツールバーアイテム(元からあるもの）
	private	  String			myToolbarItemId[] 	= new String[toolbarItemMany];		//ツールバーアイテムのId文字列

	//■コンストラクタ******************************
	protected MainToolbar(MainWindow aWindow)
	{
		//---親ウインドウの設定
		myWindow = aWindow;

		//===ツールバーのアイテムを生成

		//---固定スペース
		myToolbarItemId[toolbarItemFlexibleSpace] 	= NSToolbarItem.FlexibleSpaceItemIdentifier;
		//---可変スペース
		myToolbarItemId[toolbarItemSpace] 			= NSToolbarItem.SpaceItemIdentifier;
		//---セパレータ
		myToolbarItemId[toolbarItemSeparator] 		= NSToolbarItem.SeparatorItemIdentifier;
		//---アイテムの生成
		for ( int i = 0; i < toolbarItemAppOffset; ++i )
			myToolbarItem[i] = new CToolbarItem( myToolbarItemId[i] );

		//---新規作成
		//myToolbarItem[toolbarItemNew] 	= CToolbarItem.autoCreateItem( "New" , "newDocument" , "filenew" );
		//---開く
		//myToolbarItem[toolbarItemOpen] 	= CToolbarItem.autoCreateItem( "Open" , "openDocument" , "fileopen" );
		//---保存
		myToolbarItem[toolbarItemSave] 	= CToolbarItem.autoCreateItem( "Save" , "saveBook" , "filesave" );
		//---入力
		myToolbarItem[toolbarItemInput] 	= CToolbarItem.autoCreateItem( "InputRecord" , "newRecord" , "newrecord" );
		//---編集
		myToolbarItem[toolbarItemEdit] 	= CToolbarItem.autoCreateItem( "EditRecord" , "editRecord" , "editrecord" );
		//---削除
		myToolbarItem[toolbarItemDelete] = CToolbarItem.autoCreateItem( "DeleteRecord" , "deleteRecord" , "deleterecord" );
		//---シートポップアップ
		myToolbarItem[toolbarItemSheetPopup] = CToolbarItem.autoCreateItem( "SheetPopup" , null , aWindow.sheetPopup().button(), null );
		myToolbarItem[toolbarItemSheetPopup].item().setMinSize( new NSSize( 128,32 ) );
		//---印刷
		//myToolbarItem[toolbarItemPrint] = CToolbarItem.autoCreateItem( "Print" , "Print" , "fileprint" );
		//---切り取り
		//myToolbarItem[toolbarItemCut] 	= CToolbarItem.autoCreateItem( "Cut" , "cut" , "editcut" );
		//---コピー
		//myToolbarItem[toolbarItemCopy] 	= CToolbarItem.autoCreateItem( "Copy" , "copy" , "editcopy" );
		//---貼り付け
		//myToolbarItem[toolbarItemPaste] = CToolbarItem.autoCreateItem( "Paste" , "paste" , "editpaste" );
		//---元に戻す
		//myToolbarItem[toolbarItemUndo] 	= CToolbarItem.autoCreateItem( "Undo" , "undo" , "undo" );
		//---やり直す
		//myToolbarItem[toolbarItemRedo] 	= CToolbarItem.autoCreateItem( "Redo" , "redo" , "redo" );
		//---検索
		//myToolbarItem[toolbarItemFind] 	= CToolbarItem.autoCreateItem( "Find" , "openFindBar" , "find" );
		//---カスタマイズ
		myToolbarItem[toolbarItemConfigure] = CToolbarItem.autoCreateItem( "Configure" , "Configure" , "configure" , this );
		//---パネル
		myToolbarItem[toolbarItemDrawer] = CToolbarItem.autoCreateItem( "Panel" , "Panel" , "info" , this );
		
		//---IDの生成
		for ( int i = toolbarItemAppOffset; i < toolbarItemMany; ++i )
			myToolbarItemId[i] = myToolbarItem[i].item().itemIdentifier();

		//---ツールバーを生成
		myToolbar = new NSToolbar(
			"Toolbar" + Long.toString( 1/*myWindow.windowId()*/ ) 
			//CNSToolbar.AllIconOnly
			);
		myToolbar.setDelegate(this);
		myToolbar.setAllowsUserCustomization(true);
		myToolbar.setAutosavesConfiguration(false);
		this.window().setToolbar(myToolbar);
	}

	//■デリゲート関数*****************************
	//===CNSToolbarのデリゲート関数
	public NSArray toolbarAllowedItemIdentifiers (NSToolbar tb)
	{//---全てのツールバーアイテムを配列にして返す（必須関数）
	        return new NSArray(myToolbarItemId);
    }
    
    
	public NSArray toolbarDefaultItemIdentifiers (NSToolbar tb)
	{//---デフォルトのツールバーアイテムを設定する（必須関数？）
		String	itemList[] = {
			myToolbarItemId[toolbarItemSheetPopup],
			//myToolbarItemId[toolbarItemNew],
			//myToolbarItemId[toolbarItemOpen],
			//myToolbarItemId[toolbarItemSave],
	 		myToolbarItemId[toolbarItemSeparator],
			myToolbarItemId[toolbarItemInput],
			myToolbarItemId[toolbarItemEdit],
			myToolbarItemId[toolbarItemDelete],
	 		//myToolbarItemId[toolbarItemCut],
	 		//myToolbarItemId[toolbarItemCopy],
	 		//myToolbarItemId[toolbarItemPaste],
	 		//myToolbarItemId[toolbarItemUndo],
	 		//myToolbarItemId[toolbarItemRedo],
	 		//myToolbarItemId[toolbarItemFind],
	 		myToolbarItemId[toolbarItemFlexibleSpace],
			myToolbarItemId[toolbarItemDrawer]
			};
		return new NSArray(itemList);
	}
	    
	public NSToolbarItem toolbarItemForItemIdentifier (NSToolbar tb,String s,boolean f)
	{//---ツールバーアイテムをセットする時に呼び出される
		for( int i = 0; i < myToolbarItem.length; i++ ) 
			if( s.equals( myToolbarItem[i].item().itemIdentifier() ) ) 
				return myToolbarItem[i].item();
		return null;
	}

	//===各アクションのデリゲート関数
	/*public void New(Object sender)
	{//---新規作成
  		
	}

	public void Open(Object sender)
	{//---開く
  		
	}

	public void Save(Object sender)
	{//---保存
  		
	}

	public void Print(Object sender)
	{//---印刷
  		
	}

	public void Cut(Object sender)
	{//---切り取り
  		
	}

	public void Copy(Object sender)
	{//---コピー
  		
	}

	public void Paste(Object sender)
	{//---貼り付け
  		
	}

	public void Delete(Object sender)
	{//---削除
  		
	}

	public void Undo(Object sender)
	{//---元に戻す
  		
	}

	public void Redo(Object sender)
	{//---やり直す

	}

	public void Find(Object sender)
	{//---検索

	}*/

	/**
	* アクティブシートポップアップボタンが変更されたら呼び出される
	*/
	public void
	sheetPopup(
		Object sender
		)
	{
		NSSystem.log( "Changed " );
	}

	public void Configure(Object sender)
	{//---カスタマイズ
  		myToolbar.runCustomizationPalette(this);
	}
	
	public void
	Panel(
		Object sender
		)
	{//---パネル
		//myWindow.drawerController().drawer().toggle(this);
	}

	//■オーバーライド関数*************************


	//■メソッド***********************************
	public NSWindow window()
	{//---ウインドウを取得する
		return myWindow.window();
	}
	
	/**
	* ツールバーアイテムを取得する
	* @return 取得したアイテム
	* @param aIndex インデックス番号
	*/
	public NSToolbarItem
	getItem(
		int aIndex
		)
	{
		return myToolbarItem[aIndex].item();
	}

}
