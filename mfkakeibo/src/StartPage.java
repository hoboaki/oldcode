/* StartPage */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class StartPage 
{
	public NSPanel		myPanel;
    public NSTextField	myNewAccountTitle; /* IBOutlet */
	public NSTextField	myNewAccountMoney;
    public NSTextField	myNewBookTitle; /* IBOutlet */
    public NSButton		myNewDefaultCategoryButton; /* IBOutlet */
	public NSTabView	myTab;
	private int			myResult;
	private String		myFilepath;

	static private String myLastFilepath = null;
	

	static public final int	RESULT_CANCEL = 0;
	static public final int	RESULT_NEW = 1;
	static public final int	RESULT_OPEN = 2;

	//***グローバル関数*****************************
	static public KCStdBook
	startKakeibo(
		boolean viewOpenform
		)
	{
		StartPage page = new StartPage();
		if ( page == null )
			return null;
			
		if ( viewOpenform )
			page.viewOpenform();
		else
			page.viewNewform();
		int result = page.run();			
		
		if ( result == RESULT_NEW )
		{
			KCStdBook book = new KCStdBook();
			book.setName( new KCName( page.newBookTitle() ) ); 
			
			KCMutableAccount account = new KCMutableAccount();
			account.setName( new KCName( page.newAccountTitle() ) );
			book.addAccount( account );
			
			//---初期カテゴリの設定
			KCMutableCategory rootCategory = (KCMutableCategory)book.rootCategory();
			rootCategory.addChild( Common.categoryOutput() );
			rootCategory.addChild( Common.categoryInput() );				
			return book;
			
		}else if ( result == RESULT_OPEN )
		{
			myLastFilepath = page.openFilepath();
			KCStdBook book = (KCStdBook)KCFileManager.loadFrom( page.openFilepath() , new KCStdBookFactory() );
			if ( book == null )
			{//---だめでした
				NSAlertPanel panel = new NSAlertPanel();
				panel.runAlert( 
					NSBundle.localizedString( "Alert_CantOpenFile_Title" ), 
					NSBundle.localizedString( "Alert_CantOpenFile_Msg" ) , 
					NSBundle.localizedString( "Alert_Ok" ) , 
					null , 
					null , 
					null 
					);	
				return null;
			}
			book.setFilepath( myLastFilepath );
			return book;
		}
		return null;
				
	}
	
	static public String
	lastFilepath()
	{
		return myLastFilepath;
	}

	//***コンストラクタ******************************
	protected 
	StartPage()
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("StartPage", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load StartPage.nib");
			return;
        }				
		//---デリゲートの設定
		myPanel.setDelegate( this );
		
	}
	
	//***メソッド************************************
	public void
	viewNewform()
	{
		myTab.selectTabViewItemAtIndex( 0 );
	}

	public void
	viewOpenform()
	{
		myTab.selectTabViewItemAtIndex( 1 );
	}
	
	public int
	run()
	{
		myResult = RESULT_CANCEL;
		NSApplication.sharedApplication().runModalForWindow(myPanel,null);
		myPanel.close();
		return myResult;
	}
	
	//***プロパティアクセス***************************
	public long
	newStartMoney()
	{
		return myNewAccountMoney.intValue();
	}
	
	public String
	newBookTitle()
	{
		return myNewBookTitle.stringValue();
	}
	
	public String
	newAccountTitle()
	{
		return myNewAccountTitle.stringValue();
	}
	
	public boolean
	newDefaultCategory()
	{
		return ( myNewDefaultCategoryButton.state() == NSCell.OnState );
	}
	
	public String
	openFilepath()
	{
		return myFilepath;
	}
	
	//***デリゲート実装*******************************
	public void
	windowWillClose(
		NSNotification aNotification
		)
	{
		NSApplication.sharedApplication().stopModal();
	}
	
	//***アクション***********************************
    public void createBook(Object sender) 
	{ /* IBAction */
		myResult = RESULT_NEW;
		NSApplication.sharedApplication().stopModal();
    }

    public void openBook(Object sender) 
	{ /* IBAction */
		return;
		//myResult = RESULT_OPEN;	
		//NSApplication.sharedApplication().stopModal();		
    }
	
	public void openBookFile(Object sender)
	{
		NSOpenPanel openPanel = new NSOpenPanel();
		NSMutableArray allowTypes = new NSMutableArray();
		allowTypes.addObject( new String( "kxf" ) );
		openPanel.setAllowedFileTypes( allowTypes );
		if ( openPanel.runModal() != NSPanel.OKButton )
			return;
			
		myResult = RESULT_OPEN;	
		myFilepath = openPanel.filename();
		NSApplication.sharedApplication().stopModal();
	}

}
