//
//  MainWindow.java
//  Kakeibo
//
//  Created by あっきー on 05/11/13.
//  Copyright 2005 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class MainWindow extends Object 
{
	//*クラスのプロパティ******************************
	public static final int drawerHeight = 560;

	//*クラスのメソッド*******************************
	public static 
	MainWindow 
	createWindow(
		KCStdBook aBook
		)
	{
		MainWindow newWindow = new MainWindow( aBook );
		if ( newWindow != null )
			newWindow.window().makeKeyAndOrderFront(null);		//キーウインドウにして。前面に出す
		return newWindow;
		
	}
	
	//*プロパティ**************************************
    protected NSWindow				myWindow;			//ウインドウ(nibファイルに有）
	protected DrawerController		myDrawerController;	//ドロー
	protected NSTextField			myMessageTextView;
	protected NSTextField			myStatusTextView;

	protected MainToolbar			myToolbar;				//ツールバー
	//protected DataEditPanel			myDataEditPanel = null;	//入力パネル

	protected KCStdBook				myBook;
	protected CategoryBrowserDriver	myCategoryBrowserDriver;
	
	private boolean					myEditFlg = false;
	private NSMutableArray			myRecords = null;
	private NSMutableArray			myDataForTableView = null;
	
	private boolean					myMainWindowFlg = true;
	//private KCMutableAccount			myActiveAccount = null;

	private SheetPopup				mySheetPopup = SheetPopup.getObj( this );

	//---絞り込み関係
	private	AccountEnableManager	myAccountEnableManager;
	private DateRange				myDateRange = null;
	private KCCategoryPath			myCategoryPath = null;
	
	private MatrixView				myMatrixView;
	private MatrixSupport			myMatrixSupport = new MatrixSupport( this );
	protected NSTableView			myTableView;
	
	

	//***コンストラクタ*****************************
	protected 
	MainWindow(
		KCStdBook aBook
		)
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("MainWindow", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load Window.nib");
			return;
        }
		
		//---ツールバーの作成
		myToolbar = MainToolbar.createToolbar( this );
		
		//----ブックの設定
		myBook = aBook;
		myCategoryBrowserDriver = new CategoryBrowserDriver( myBook );
		myAccountEnableManager = new AccountEnableManager( myBook.accountIterator() );
		
		//---入力パネルの表示
		//myDataEditPanel = DataEditPanel.createPanel( this );
		
		//---デリゲートの設定
		myDrawerController.drawer().setDelegate( this );
		myDrawerController.setCategoryBrowserDriver( categoryBrowserDriver() );
		BootClass.getMainClass().menuAccount().setDelegate( myAccountEnableManager );
		
		//---各ビューの更新
		sheetPopup().updateList( book().sheetContainer() );
		updateWindowTitle();
		recordDataSourceDidChange();
		//updateMatrix();
		//updateTable();
		
	}

	public void 
	setMyWindow( 
		Object anObject
		)
	{
		myWindow = (NSWindow)anObject;
		myWindow.setDelegate( this );
	}
	
	public void 
	setMyMatrixView(
		Object  anObject
		)
	{
		myMatrixView = (MatrixView) anObject;
		myMatrixView.setWindow( this );
	}

	public void 
	setMyTableView(
		Object  anObject
		)
	{
		myTableView = (NSTableView)anObject;
		myTableView.setDelegate( this );
		
		{//---ソートクラスの作成
			NSMutableArray array = new NSMutableArray();
			//myDetailTableView.setSortDescriptors( array );			
			{
				NSTableColumn column = myTableView.tableColumnWithIdentifier( new String("date" ) );
				KCRecordSortDescriptor sort = new KCRecordSortDescriptor("date",false);
				if ( column != null )
				{
					column.setSortDescriptorPrototype( sort );
					array.addObject( sort );
				}
			}
			{
				NSTableColumn column = myTableView.tableColumnWithIdentifier( new String("price" ) );
				KCRecordSortDescriptor sort = new KCRecordSortDescriptor("price",true);
				if ( column != null )
				{
					column.setSortDescriptorPrototype( sort );
					array.addObject( sort );
				}
			}			
			myTableView.setSortDescriptors( array );
		}
	}		

	//***メソッド***********************************

	public NSMutableArray
	recordsForTableView()
	{
		if ( myDataForTableView == null )
		{
			myDataForTableView = new NSMutableArray();
			AccountEnableManager aem = accountEnableManager();
			DateRange range = matrixSupport().activeDateRange();
			if ( aem != null && myRecords != null )
			{
				for ( java.util.Enumeration itr = myRecords.objectEnumerator(); itr.hasMoreElements(); )
				{
					KCRecord record = (KCRecord)itr.nextElement();
					if ( !aem.isEnableForAccount( record.account() ) )
					{
						continue;
					}
					if ( !range.isInRange( record.date() ) )
					{
						continue;
					}
					myDataForTableView.addObject( record );
				}
			}
		}
		return myDataForTableView;
		/*if ( myDataForDetailTableView == null )
		{
			if ( book() != null )
			{//---全ての口座のデータを返す
				NSMutableArray tmpArray = new NSMutableArray();
				KCAccountIterator itr = book().accountIterator();
				int cnt = 0;
				while( itr.hasNext() )
				{
					KCAccount account = itr.next();
					if ( accountEnableManager().isEnableAtIndex( cnt ) )
					{
						tmpArray.addObjectsFromArray(
							KCLibDriver.recordsToNSArray( 
								account.recordIterator() ,
								dateRange() ,
								categoryPath() 
								)
							);
					}
					cnt++;
				}
				myDataForDetailTableView = tmpArray;
			}
		}
		return myDataForDetailTableView;*/
	}

	/**
	* ウインドウタイトルを更新する
	*/
	public void
	updateWindowTitle()
	{
		window().setTitle( Common.ApplicationName + " - " + book().name().string() );
	}

	/**
	* 表示モードが変更したときこれを必ず呼び出す
	*/
	public void
	viewModeDidChange()
	{
		updateMatrix();	
		updateTable();		
	}

	/**
	* シート情報が変更したときこれを必ず呼び出す
	*/
	public void
	sheetDidChange()
	{
		sheetPopup().updateList( book().sheetContainer() );
		updateMatrix();			
	}
	
	/**
	* アクティブシートが変更したときこれを必ず呼び出す
	*/
	public void
	activeSheetDidChange()
	{
		updateMatrix();			
	}

	/**
	* レコードの追加・削除・変更があったときにこれを必ず呼び出す
	*/
	public void
	recordDataSourceDidChange()
	{
		//---レコードデータベースを再構築する
		_reloadRecords();
	
		updateMatrix();	
		updateTable();
	}

	/**
	* カテゴリの追加・削除・変更があったときにこれを必ず呼び出す
	*/
	public void
	categoryDataSourceDidChange()
	{
		updateMatrix();	
		updateTable();
	}
	
	/**
	* 口座を追加，削除，変更があったときにこれを必ず呼び出す
	*/
	public void
	accountDataSourceDidChange()
	{
		//---イネイブルリストの初期化
		accountEnableManager().reloadList( book().accountIterator() );
		
		//---新規入力フォームの口座一覧を更新
		//if ( myDataEditPanel != null )
		//	myDataEditPanel.updateAccountList();
			
		//---ドローの口座一覧を更新
		drawerController().updateAccountList();
	}

	
	/**
	* 口座の表示・非表示の設定が変更したときにこれを必ず呼び出す
	*/
	public void
	accountEnableDidChange()
	{
		updateMatrix();
		updateTable();
	}
	
	/**
	* テーブルを更新する必要があるときに呼び出す
	*/
	public void
	updateTable()
	{
		myDataForTableView = null;		
		//recordsForTableView().sortUsingDescriptors( tableView().sortDescriptors() );
		tableView().reloadData();
		setStatus( numberOfRowsInTableView( tableView() ) );
		
		//---選択解除
		NSIndexSet set = tableView().selectedRowIndexes();
		for ( int i = set.lastIndex(); set.containsIndex( i ); i = set.indexLessThanIndex( i ) )
		{
			tableView().deselectRow( i );
		}
	}
	
	/**
	* マトリックスを更新する必要があるときに呼び出す
	*/
	public void
	updateMatrix()
	{
		MatrixView matrix = matrixView();
		MatrixSupport support = matrixSupport();
		matrix.setRowsAndColumns( support.numberOfRows() , support.numberOfColumns() );
		support.setTitleForColumns( matrix );
		support.setTitleForRows( matrix );
		support.createDataBase( myRecords , book().accountIterator() );
		support.setValueForMatrix( matrix , accountEnableManager() );
	}

	//***デリゲート実装******************************
	//==NSWindowのデリゲート関数
	public void windowDidBecomeMain(NSNotification aNotification)
	{//---メインウインドウになったら呼び出される
		setMainWindowFlg( true );
		BootClass.setMainWindow( this );
	}
	
	public void windowDidResignMain(NSNotification aNotification)
	{//---メインウインドウでなくなった時に呼び出される
		setMainWindowFlg( false );
		if ( BootClass.mainWindow() == this )
			BootClass.setMainWindow( null );
	}

	public boolean 
	windowShouldClose(Object sender)
	{//---ウインドウが閉じようとする時に呼び出される
		return canWindowClose();
	}

	public void windowWillClose(NSNotification aNotification)
	{//---ウインドウが閉じる時に呼び出される
		BootClass.removeMainWindow( this );
	}
		
	public NSSize 
	windowWillResize(
		NSWindow sender, 
		NSSize proposedFrameSize
		)
	{
		/*{//---Drawerの下スペースを変更
			float tmp = proposedFrameSize.height() - drawerHeight;
			if ( tmp < 0.0f )
				tmp = 0.0f;
			drawer().setTrailingOffset( tmp );
		}*/
		return proposedFrameSize;
	}
	
	//---NSDrawer
	public void 
	drawerDidOpen(
		NSDrawer sender
		)
	{
		drawerController().updateAccountList();
	}
	
	public NSSize 
	drawerWillResizeContents(
		NSDrawer sender,
		NSSize contentSize
		)
	{
		if ( contentSize.height() > drawerHeight )
		{
			sender.setTrailingOffset( contentSize.height() - drawerHeight );
			return new NSSize( contentSize.width() , drawerHeight );
		}
		return contentSize;
	}
	
	//===NSResponderのデリゲート関数
	public boolean acceptsFirstResponder()
	{//---ファーストレスポンダになることを許可する
		return isMainWindow();
	}
	
	//===NSTableView
	public int 
	numberOfRowsInTableView( 
		NSTableView aTableView 
		) 
	{
		if ( recordsForTableView() == null )
		{
			return 0;
		}
		int count = recordsForTableView().count();
		return count;
	}
	
	public Object 
	tableViewObjectValueForLocation( 
		NSTableView aTableView, 
		NSTableColumn aTableColumn, 
		int rowIndex 
		)
	{
		KCRecord data = (KCRecord)recordsForTableView().objectAtIndex( rowIndex );
	
		if ( aTableView == tableView() )
		{
			if ( aTableColumn.identifier().equals( "name" ) )
			{
				//if ( data.name().string().length() == 0 )
				{//---カテゴリを名前欄に表示
					//return "<<"  + data.categoryName() + ">>";
				}
				if ( data.name() != null )
					return data.name().string();
			}
			else if ( aTableColumn.identifier().equals( "category" ) )
			{
				/*if ( data.title().length() == 0 )
				{
					if ( data.categoryFullPath().lastIndexOf( "/" ) != 0 )
						return data.categoryNameSecond();
				}*/
				return data.categoryPath().leafCategory().string();
			}
			else if ( aTableColumn.identifier().equals( "date" ) )
			{
				return data.date().string();
			}
			else if ( aTableColumn.identifier().equals( "price" ) )
			{
				KCMoney money = data.money();
				return KCLibDriver.makeAttributedStringForMoney( money );
			}
			else if ( aTableColumn.identifier().equals( "account" ) )
			{
			
				return ( (KCMutableAccount)data.account() ).name().string();
			}
			
		}
		return null;
	}
	
	public void tableViewSetObjectValueForLocation( 
		NSTableView aTableView, 
		Object anObject, 
		NSTableColumn aTableColumn, 
		int rowIndex 
		)
	{
	}
	
	
	/**
	* 列タイトルをクリックした時に呼び出される
	*/
	public void
	tableViewDidClickTableColumn(
		NSTableView tableView, 
		NSTableColumn tableColumn
		)
	{
		recordsForTableView().sortUsingDescriptors( tableView.sortDescriptors() );
		tableView.reloadData();
	}
	
	/**
	* チップが表示される時に呼び出される
	*/
	public String 
	tableViewToolTipForCell(
		NSTableView aTableView, 
		NSCell aCell, 
		NSMutableRect rect, 
		NSTableColumn aTableColumn, 
		int row, 
		NSPoint mouseLocation
		)
	{
		KCRecord record = (KCRecord)recordsForTableView().objectAtIndex( row );
		return KCLibDriver.tipForRecord( record );
	}
	
	/**
	* レコードが選択された時に呼び出される
	*/
	public void 
	tableViewSelectionDidChange(
		NSNotification aNotification
		)
	{
				
		/*{//---ツールバーアイテムの状態の更新
			NSToolbarItem edit = myToolbar.getItem( MainToolbar.toolbarItemEdit );
			NSToolbarItem delete = myToolbar.getItem( MainToolbar.toolbarItemDelete );
			edit.setAutovalidates( false );
			delete.setAutovalidates( false );
		
			if ( numOfSelected == 1 )
				edit.setEnabled( true );
			else
				edit.setEnabled( false );
		
			if ( numOfSelected > 0 )
				delete.setEnabled( true );
			else
				delete.setEnabled( false );
			
		}*/
		
	}
	
	/**
	* ツールバーのvalidationメソッド
	*/
	public boolean 
	validateToolbarItem (
		NSToolbarItem toolbarItem
		) 
	{
		boolean retFlg = true;
		
		String id = toolbarItem.itemIdentifier();
		
		if ( id.equals( "EditRecord" ) )
		{
			int numOfSelected = tableView().numberOfSelectedRows();
			if ( numOfSelected != 1 )
				return false;
		}
		else if ( id.equals( "DeleteRecord" ) )
		{
			int numOfSelected = tableView().numberOfSelectedRows();
			if ( numOfSelected == 0 )
				return false;			
		}
				
		return true;
	}
	
	/**
	* アクティブシートが変更されたとき呼び出される
	*/
	public void
	sheetPopupDidChange(
		Object sender
		)
	{
		activeSheetDidChange();
	}
	
	//***アクション*********************************
	public void
	saveBook(
		Object sender
		)
	{//---上書き保存
		saveBookProcess();
	}
	
	public void
	saveBookAs(
		Object sender
		)
	{
		saveBookAsProcess();
	}
	
	/**
	* 上書き保存処理
	* @return 保存できたらtrue
	*/
	protected boolean
	saveBookProcess()
	{
		if ( book().filepath() != null )
		{
			if ( !saveProcess( book().filepath() ) )
			{
				NSSystem.log( "Can't Save" );
				return false;
			}
			return true;
		}
		else 
			return saveBookAsProcess();	
	}

	/**
	* 別名保存処理
	* @return 保存できたらtrue
	*/
	protected boolean
	saveBookAsProcess()
	{
		//--名前の取得
		NSSavePanel savePanel = new NSSavePanel();
		savePanel.setRequiredFileType( "kxf" );
		if ( savePanel.runModal() ==  NSPanel.OKButton )
		{
			if ( !saveProcess( savePanel.filename() ) )
			{
				return false;
			}
			else
			{
				if ( book().filepath() == null )
					book().setFilepath( savePanel.filename() );
				return true;
			}
		}
		return false;
	}
		
	/**
	* ファイル保存処理
	* @return trueなら保存できた
	* @param aFilepath 保存先
	*/
	protected boolean
	saveProcess(
		String aFilepath
		)
	{
		if( KCFileManager.saveTo( book() , aFilepath ) )
		{
			setEditFlg( false );
			return true;
		}
		return false;		
	}
	
	/**
	*　新規レコードの入力
	*/
	public void
	newRecord(
		Object sender
		)
	{
		if ( DataEditPanel.newRecordPanel( this ) > 0 )
		{//---データ更新
			setEditFlg( true );
			recordDataSourceDidChange();
		}
	}
	
	/**
	*　既存レコードの編集
	*/
	public void
	editRecord(
		Object sender
		)
	{
	}
	
	/**
	*　既存レコードの削除
	*/
	public void
	deleteRecord(
		Object sender
		)
	{
		//---選択項目がなければリターン
		if ( tableView().numberOfSelectedRows() == 0 )
			return;
	
		NSIndexSet set = tableView().selectedRowIndexes();
		for ( int i = set.lastIndex(); set.containsIndex( i ); i = set.indexLessThanIndex( i ) )
		{
			KCRecord record = (KCRecord)recordsForTableView().objectAtIndex( i );
			book().removeRecordAtAccount( record , record.account() );
		}
		/*NSEnumerator itr = tableView().selectedRowEnumerator();
		while( itr.hasMoreElements() )
		{
			Object obj = itr.nextElement();
			NSSystem.log( obj.getClass().getName() );
			KCRecord record = (KCRecord)obj;
			book().removeRecordAtAccount( record , record.account() );
		}*/
		
		setEditFlg( true );
		recordDataSourceDidChange();
		
	}
	
	/**
	* 口座の作成
	*/
	public boolean
	addAccount(
		Object sender
		)
	{
		AccountEditPanel panel = AccountEditPanel.getAccountInfo( 
										null , 
										NSBundle.localizedString("Account_New") , 
										"",
										""
										);
		if ( panel != null )
		{
			String name = panel.name();
			String comment = panel.comment();
			if ( name == null )
				return false;
				
			KCMutableAccount account = new KCMutableAccount();
			account.setName( new KCName( name ) );
			if ( comment != null )
				account.setComment( comment );
			if ( book().addAccount( account ) == null )
			{
				NSSystem.log( "Add Acount Failure" );
			}
			setEditFlg( true );
			accountDataSourceDidChange();
		}
		return true;
	}
	
	/**
	* すべての口座を選択する
	*/
	public void
	checkAccountAll(
		Object aSender
		)
	{
		if( accountEnableManager().setAll( true ) )
		{
			accountEnableDidChange();
		}
	}
	
	/**
	* すべての口座の選択を解除する
	* @return 状態が変化したらtrue
	*/
	public void
	uncheckAccountAll(
		Object aSender
		)
	{
		if ( accountEnableManager().setAll( false ) )
		{
			accountEnableDidChange();
		}
	}	

	/**
	* 口座を選択する
	*/
	public void
	checkAccount(
		Object aSender
		)
	{
		NSMenuItem item = (NSMenuItem)aSender;
		accountEnableManager().setAtIndex( 
			!accountEnableManager().isEnableAtIndex( item.tag() ) ,
			item.tag()
			);
		
		accountEnableDidChange();
	}	
	
	//***プロパティアクセス***************************	
	public KCStdBook
	book()
	{
		return myBook;
	}
	
	public CategoryBrowserDriver
	categoryBrowserDriver()
	{
		return myCategoryBrowserDriver;
	}
	
	public NSWindow
	window()
	{
		return myWindow;
	}
	
	public NSTableView
	tableView()
	{
		return myTableView;
	}
	
	public MatrixView
	matrixView()
	{
		return myMatrixView;
	}
	
	public MatrixSupport
	matrixSupport()
	{
		return myMatrixSupport;
	}
	
	public DrawerController
	drawerController()
	{
		return myDrawerController;
	}
	
	public void
	setMessage(
		String aMessage
		)
	{
		String tmpString = aMessage;
		if ( tmpString == null )
			tmpString = "";
		myMessageTextView.setStringValue( tmpString );
	}
	
	public void
	setStatus(
		long aMany
		)
	{
		long tmp = aMany;
		if ( tmp < 0 )
			tmp = 0;
		myStatusTextView.setStringValue( Long.toString( tmp ) + " " + NSBundle.localizedString("MainWindow_Unit") );
	}

	public boolean
	isMainWindow()
	{
		return myMainWindowFlg;
	}
	
	public void
	setMainWindowFlg(
		boolean aFlg
		)
	{
		myMainWindowFlg = aFlg;
	}

	/**
	* 変更フラグを取得する
	* @return 変更フラグ
	*/
	public boolean
	editFlg()
	{
		return myEditFlg;
	}
	
	/**
	* 変更フラグを設定する
	* @param aFlg 設定するフラグ
	*/
	public void
	setEditFlg(
		boolean aFlg
		)
	{
		myEditFlg = aFlg;
	}
	
	public AccountEnableManager
	accountEnableManager()
	{
		return myAccountEnableManager;
	}

	/**
	* インデックス番号で口座を返す
	*/
	public KCMutableAccount
	accountAtIndex(
		int index
		)
	{
		KCAccountIterator itr = book().accountIterator();
		for ( int i = 0; i < index && itr.hasNext(); ++i )
		{
			itr.next();
		}
		return (KCMutableAccount)itr.next();
		
	}

	/**
	* 絞り込み日付範囲を設定する
	* @param aRange 設定する日付範囲
	*/
	public void
	setDateRange(
		DateRange aRange
		)
	{
		myDateRange = aRange;
	}
	
	/**
	* 絞り込み日付範囲を取得する
	* @return 日付範囲
	*/
	public DateRange
	dateRange()
	{
		return myDateRange;
	}
	
	/**
	* 絞り込みカテゴリを設定する
	* @param aPath 設定するカテゴリパス
	*/
	public void
	setCategoryPath(
		KCCategoryPath aPath
		)
	{
		myCategoryPath = aPath;
	}
	
	/**
	* 絞り込みカテゴリを取得する
	* @return カテゴリパス
	*/
	public KCCategoryPath
	categoryPath()
	{
		return myCategoryPath;
	}
	
	
	/**
	* ウインドウが閉じれるか調べる。
	* @return trueなら閉じれる
	*/
	public boolean
	canWindowClose()
	{
		//---ファイル保存チェック
		if ( editFlg() )
		{//---変更されてる
			//---アラートを出す
			NSAlertPanel panel = new NSAlertPanel();
			int ret = panel.runAlert( 
				NSBundle.localizedString( "Alert_NotSaveYet_Title" ) ,	
				NSBundle.localizedString( "Alert_NotSaveYet_Msg" ) ,			
				NSBundle.localizedString( "Alert_Save" ) , 
				NSBundle.localizedString( "Alert_DontSave" ) , 
				NSBundle.localizedString( "Alert_Cancel" ) , 
				window() 
				);	
				
			if ( ret == NSAlertPanel.DefaultReturn )
			{//---はい（保存処理）
				if ( !saveBookProcess() )
					return false;
			}
			else if ( ret == NSAlertPanel.AlternateReturn )
			{//---いいえ（保存しない）
			}
			else
			{//---キャンセル
				return false;
			}
		}
		return true;	
	}
	
	/**
	* アクティブシートポップアップボタンを取得する。
	* @return アクティブシートポップアップボタン
	*/
	public 
	SheetPopup
	sheetPopup()
	{
		return mySheetPopup;
	}
	
	/**
	* アクティブなシートを取得する。
	* @return シート
	*/
	public 
	Sheet
	activeSheet()
	{
		int index = sheetPopup().button().indexOfSelectedItem();
		if ( index < 0 )
			index = 0;
 		return book().sheetContainer().getSheet( index );
	}
	
	//*プライベートなもの*******************************
	
	/**
	* レコードデータベースを再構築する
	*/
	private void
	_reloadRecords()
	{
		myRecords = new NSMutableArray();
		KCAccountIterator itr = book().accountIterator();
		int cnt = 0;
		while( itr.hasNext() )
		{
			KCAccount account = itr.next();
			myRecords.addObjectsFromArray(
				KCLibDriver.recordsToNSArray( 
					account.recordIterator() ,
					null,
					null
					)
				);
		}
		
		//---日付を過去から新しいもの順にソートする
		NSMutableArray descriptor = new NSMutableArray();
		descriptor.addObject( new KCRecordSortDescriptor("date",false) );
		myRecords.sortUsingDescriptors( descriptor );
		
	}
	
}
