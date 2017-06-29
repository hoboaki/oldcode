/* DrawerFormController */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.awt.Toolkit.*;

import com.akipen.kclib.*;

public class DataEditPanel
{

	//***クラスのプロパティ**************************
    public NSDatePicker		myDateCalender; /* IBOutlet */
    public NSDatePicker		myDateText; /* IBOutlet */
    public NSBrowser		myCategoryBrowser; /* IBOutlet */
    public NSTextView		myCommentText; /* IBOutlet */
    public NSButton			myPriceSwitch; /* IBOutlet */
    public NSTextField		myTitleText; /* IBOutlet */
    public NSTextField		myPriceText; /* IBOutlet */
	public NSPopUpButton	myAccountPopup;
	public NSPanel			myPanel;
	
	public MainWindow		myMainWindow;

	private int myNumOfAddRecord = 0;
	private boolean _myLoopTerminator = false;
	
	//*ファクトリーメソッド**************************
	
	/**
	* 新規レコードを作成する。
	* @return 追加したレコードの数
	*/
	static public int
	newRecordPanel(
		MainWindow aWindow
		)
	{
		DataEditPanel panel = new DataEditPanel( aWindow );
		NSApplication.sharedApplication().runModalForWindow( panel.myPanel , aWindow.window() );
		panel.myPanel.close();
		return panel.numberOfAddedRecord();
	}

	//***コンストラクタ*****************************
	protected 
	DataEditPanel(
		MainWindow	aWindow
		)
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("DataEditPanel", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load DataEditPanel.nib");
			return;
        }
		myMainWindow = aWindow;
		
		//---口座リストを初期化
		updateAccountList();
		
		//---デリゲートの設定
		myPanel.setDelegate( this );
		categoryBrowser().setDelegate( myMainWindow.categoryBrowserDriver() );
		
		//---タイトルの設定
		updatePanelTitle();
		
		//---前面に出す
		//myPanel.makeKeyAndOrderFront(null);
		
	}	
	
	public void
	setMyDateCalender(
		Object anObject
		)
	{
		myDateCalender = (NSDatePicker)anObject;
		myDateCalender.setDelegate( this );
		_myLoopTerminator = true;
		myDateCalender.setDateValue( new NSDate() );
		_myLoopTerminator = false;
	}

	public void
	setMyDateText(
		Object anObject
		)
	{
		myDateText = (NSDatePicker)anObject;
		myDateText.setDelegate( this );
		_myLoopTerminator = true;
		myDateText.setDateValue( new NSDate() );
		_myLoopTerminator = false;
	}
	
	public void
	setMyCategoryBrowser(
		Object anObject
		)
	{
		myCategoryBrowser = (NSBrowser)anObject;
		myCategoryBrowser.setPathSeparator( KCCategory.SEPARATOR );
		
		//---ダブルクリックアクションの設定
		/*NSSelector selector = new NSSelector( "browserDoubleClick", new Class[] { NSBrowser.class } );
		myCategoryBrowser.setDoubleAction( selector );*/
	}
	
	//***メソッド***********************************
    public void 
	setDateToToday(
		Object sender
		) 
	{ /* IBAction */
		setDate( new NSDate() );
    }
	
	public void
	updateAccountList()
	{//---アカウントリストを更新する
	
		NSPopUpButton popup = accountPopup();
		KCAccountIterator itr = mainWindow().book().accountIterator();
		
		//---現在選択中のアカウントを控える
		KCMutableAccount account = selectedAccount();
		
		//---初期化
		popup.removeAllItems();
		while( itr.hasNext() )
		{
			popup.addItem( ( (KCMutableAccount)itr.next() ).name().string() );
		}
		
		//---アカウントを再設定する
		if ( account != null )
			setActiveAccount( account );
		
	}
	
	public boolean
	setActiveAccount(
		KCMutableAccount aAccount
		)
	{
		NSPopUpButton popup = accountPopup();	
		KCAccountIterator itr = mainWindow().book().accountIterator();
		
		//---インデックスを調べる
		int i = 0;
		while( itr.hasNext() )
		{
			KCMutableAccount account = (KCMutableAccount)itr.next();
			if ( account.equals( aAccount ) )
				break;
			i++;
		}
		popup.selectItemAtIndex( i );
		return true;
	}
	
	public void
	browserDoubleClick(
		NSBrowser aBrowser
		)
	{
		renameCategory( null );
	}
	
	public void
	alert(
		String aMessage
		)
	{
		mainWindow().setMessage( aMessage );
		//java.awt.Toolkit.getDefaultToolkit().beep();
	}
	
	public void
	addCategory(
		Object sender
		)
	{	
		//---選択中のパスの取得
		KCCategoryPath selected = selectedPath();
		if ( selected == null )
			return;
		String path = selected.string();
		if ( path.length() == 0 )
			return;
		
		//---追加する名前を取得する
		String newName = 
			CategoryNameEditPanel.getName(
				myPanel ,
				NSBundle.localizedString("Category_Add") ,
				path ,
				""
				);
		if ( newName == null )
			return;

		KCMutableCategory category = (KCMutableCategory)mainWindow().book().rootCategory().search( new KCCategoryPath( path )	 );
		KCMutableCategory addCategory = new KCMutableCategory();
		addCategory.setName( new KCCategoryName( newName ) );
		
		if ( mainWindow().book().addCategory( category , addCategory ) )
		{//---表示を更新する
			mainWindow().setEditFlg( true );
			setCategoryPath( selected );
		}
			
	}
	
	public void
	renameCategory(
		Object sender
		)
	{
		//---選択中のパスの取得
		KCCategory target = (KCCategory)mainWindow().book().rootCategory().search( selectedPath() );
		if ( target == null )
			return;
		if ( target.depth() <= 1 )
			return; //---ルートノードの名前は変更不可能
			
		//---変更する名前を取得する
		String newName = null;
		{
			String oldName = target.name().string();
			newName = CategoryNameEditPanel.getName(
				myPanel ,
				NSBundle.localizedString("Category_Add") ,
				target.parent().path().string() ,
				oldName
				);
	
			if ( newName == null )
				return;
				
			if ( newName.equals( oldName ) )
				return;
		}
		
		KCMutableCategory newCategory = new KCMutableCategory();
		newCategory.setName( new KCCategoryName( newName ) );
		newCategory.setComment( target.comment() );
		if ( ( mainWindow().book() ).updateCategory( target, newCategory  ) ); 
		{//---表示の更新
			mainWindow().setEditFlg( true );
			mainWindow().categoryDataSourceDidChange();
			setCategoryPath( new KCCategoryPath( target.parent().path().string() + newCategory.path().string() ) );
		}
	
	}
	
	public void
	deleteCategory(
		Object sender
		)
	{
		//---選択中のパスの取得
		KCMutableCategory target = (KCMutableCategory)mainWindow().book().rootCategory().search( selectedPath() );
		if ( target == null )
			return;
		if ( target.depth() <= 1 )
			return; //---ルートノードの名前は変更不可能
		
		//---ノードの消去
		KCCategoryPath path = target.parent().path(); //---ひかえ
		if ( !mainWindow().book().removeCategory( target ) )
		{
			NSSystem.log( "Failure" );
			return;
		}
		
		//---表示の更新
		mainWindow().setEditFlg( true );
		mainWindow().categoryDataSourceDidChange();
		setCategoryPath( path );
	
	}
	
	public void
	moveCategory(
		Object sender
		)
	{
	
	}
	
	public void 
	add(
		Object sender
		) 
	{ /* IBAction */
	
		{//---入力チェック
		
			if ( priceText().stringValue().length() == 0 )
			{//---価格
				setFocus( priceText() );
				alert( NSBundle.localizedString("Alert_InputPrice") );
				return;
			}
			if ( categoryBrowser().path().length() == 0 )
			{
				setFocus( categoryBrowser() );
				alert( NSBundle.localizedString("Alert_SelectCategory") );
				return;
			}
		}
	
		{//---データの追加
			KCMutableRecord tmpRecord = new KCMutableRecord();
		
			tmpRecord.setName( new KCRecordName( titleText().stringValue() ) );
			tmpRecord.setMoney( formMoney() );
			tmpRecord.setDate( KCLibDriver.toKCDate( date() ) );
			tmpRecord.setCategoryPath( selectedPath());
			tmpRecord.setComment( commentText().string() );
		
			KCMutableAccount targetAccount = selectedAccount();
			if ( targetAccount == null )
			{
				NSSystem.log( "DataEditPanel : can't found selected Account" );
				return;
			}
			targetAccount.addRecord( tmpRecord );
		}
		
		{//---フォームのリセット
			titleText().setStringValue( "" );
			priceText().setStringValue( "" );
			commentText().setString( "" );
			
			setFocus( titleText() );
			
		}
		
		{//---表示の更新
			//mainWindow().recordDataSourceDidChange();
			myNumOfAddRecord++;
		}
		
    }

	public void
	close(
		NSObject sender
		)
	{
		NSApplication.sharedApplication().stopModal();
	}

	public void
	setFocus(
		NSResponder aTarget
		)
	{
		mainWindow().window().makeFirstResponder( aTarget );
	}

    public void 
	apply(
		Object sender
		) 
	{ /* IBAction */
		
    }

    public void 
	reset(
		Object sender
		) 
	{ /* IBAction */
		titleText().setTitleWithMnemonic( "" );
		priceText().setTitleWithMnemonic( "" );
		commentText().setString( "" );
		priceInvertSwitch().setState(  NSCell.OffState );
    }
	
	//***デリゲート実装******************************
	public NSDate 
	datePickerCellValidateProposedDateValue(
		NSDatePickerCell aDatePickerCell, 
		NSDate proposedDateValue
		)
	{
		if ( !_myLoopTerminator )
		{
			_myLoopTerminator = true;
			setDate( proposedDateValue );
			_myLoopTerminator = false;
		}
		return proposedDateValue;
	}
	
	//***プロパティアクセス***************************
	public MainWindow
	mainWindow()
	{
		return myMainWindow;
	}
	
	public void
	updatePanelTitle()
	{
		myPanel.setTitle( NSBundle.localizedString("DataEditPanel_Title") + " - " + mainWindow().book().name().string() );
	}
	
	public String
	activeAccount()
	{
		return accountPopup().selectedItem().title();
	}
	
	public NSPopUpButton
	accountPopup()
	{
		return myAccountPopup;
	}
	
	public NSDatePicker
	dateCalender()
	{
		return myDateCalender;
	}
	
	public NSDatePicker
	dateText()
	{
		return myDateText;
	}

	public NSGregorianDate
	date()
	{
		return new NSGregorianDate( 0 , dateCalender().dateValue() );
	}

	public void
	setDate(
		NSDate aDate 
		)
	{
		dateCalender().setDateValue( aDate );
		dateText().setDateValue( aDate );
	}
	
	public NSTextField
	titleText()
	{
		return myTitleText;
	}
	
	public NSTextField
	priceText()
	{
		return myPriceText;
	}
	
	public KCMoney
	formMoney()
	{
		double tmp = priceText().doubleValue();
		boolean invert = false;
		if ( selectedPath().rootCategory().equals( Common.categoryOutput().name() ) )
		{
			invert = !invert;
		}
		if ( priceInvertSwitch().state() == NSCell.OnState )
		{
			invert = !invert;
		}
		
		if ( !invert )
		{
			return new KCMoney( tmp );
		}
		else
		{
			return new KCMoney( -1.0 * tmp );
		}
	}
	
	public NSTextView
	commentText()
	{
		return myCommentText;
	}
	
	public NSButton
	priceInvertSwitch()
	{
		return myPriceSwitch;
	}
	
	public KCMutableAccount
	selectedAccount()
	{
		NSPopUpButton popup = accountPopup();
		KCAccountIterator itr = mainWindow().book().accountIterator();
		
		int index = popup.indexOfSelectedItem();		
		for ( int i = 0; i <= index && itr.hasNext(); ++i )
		{
			if ( i == index )
				return (KCMutableAccount)itr.next();
			itr.next();
		}
		return null;
	}
	
	public KCCategoryPath
	selectedPath()
	{
		String path = categoryBrowser().path();
		if ( path.length() == 0 )
			return null;
		return new KCCategoryPath( path + KCCategory.SEPARATOR );
	}
	
	public void
	setCategoryPath(
		KCCategoryPath aPath
		)
	{
		String tmp = aPath.string();
		categoryBrowser().loadColumnZero();
		categoryBrowser().setPath( tmp.substring( 0 , tmp.length() - KCCategory.SEPARATOR.length() )  );
	}
	
	public NSBrowser
	categoryBrowser()
	{
		return myCategoryBrowser;
	}
	
	/**
	* 追加したレコードの数を取得する
	* @return レコードの数
	*/
	public int
	numberOfAddedRecord()
	{
		return myNumOfAddRecord;
	}
	
	
}
