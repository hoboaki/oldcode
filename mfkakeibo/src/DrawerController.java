/* DrawerController */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;
import java.util.*;

public class DrawerController 
{

	//***クラスのプロパティ**************************
	public MainWindow	myWindow;
    public NSDrawer myDrawer; /* IBOutlet */
	public NSPopUpButton	myAccountPopup;
	public AccountInfo	myAccountInfo;

	public NSButton		myAccountDateButton;
	public NSDatePicker	myAccountDateFrom;
	public NSDatePicker	myAccountDateTo;
	public NSButton		myAccountCategoryButton;
	public NSBrowser	myAccountCategoryBrowser;

	//***コンストラクタ*****************************
	public 
	DrawerController()
	{
	}
	
	public void 
	setMyDrawer(
		Object  anObject
		)
	{
		myDrawer = (NSDrawer)anObject;
		//myDrawer.open();
	}
	
	public void
	setMyWindow(
		Object anObject
		)
	{
		myWindow = (MainWindow)anObject;
	}
	
	public void
	setMyAccountDateFrom(
		Object aObject
		)
	{
		myAccountDateFrom = (NSDatePicker)aObject;
		myAccountDateFrom.setDateValue( new NSDate() );
	}

	public void
	setMyAccountDateTo(
		Object aObject
		)
	{
		myAccountDateTo = (NSDatePicker)aObject;
		myAccountDateTo.setDateValue( new NSDate() );
	}
	
	public void
	setMyAccountCategoryBrowser(
		Object anObject
		)
	{
		myAccountCategoryBrowser = (NSBrowser)anObject;
		myAccountCategoryBrowser.setPathSeparator( KCCategory.SEPARATOR );
	}

	//***メソッド***********************************
	public void
	setCategoryBrowserDriver(
		CategoryBrowserDriver aDriver
		)
	{
		myAccountCategoryBrowser.setDelegate( aDriver );
	}
	
	public void
	updateAccountList()
	{//---アカウントリストを更新する
		
		NSPopUpButton popup = accountPopup();
		if ( myWindow == null )
			return;
		KCAccountIterator itr = myWindow.book().accountIterator();
		
		//---初期化
		popup.removeAllItems();
		while( itr.hasNext() )
		{
			popup.addItem( ( (KCAccount)itr.next() ).name().string() );
		}
		
		updateAccountInfo();
	}
	
	public void
	updateAccountInfo()
	{
		KCAccount account = selectedAccount();
		myWindow.accountEnableManager().setAll( false );
		myWindow.accountEnableManager().setAtIndex( true , accountPopup().indexOfSelectedItem() );

		//---テーブル情報を更新
		myWindow.updateTable();
		
		///---計算
		double carry = 0;
		double input = 0;
		double output = 0;
		int count = 0;
		
		java.util.Enumeration enumerator = myWindow.recordsForTableView().objectEnumerator();

		while ( enumerator.hasMoreElements() ) 
		{
			KCRecord record = (KCRecord)enumerator.nextElement(); 
			if ( !record.account().equals( account ) )
				continue;
			
			count++;
			KCMoney money = record.money();
			if ( money.isOutput() )
				output += money.absPrice();
			else
				input += money.absPrice();
		}
		double restPrice = carry + input - output;
		
		//---表示更新
		myAccountInfo.myRecordCount.setIntValue( count );
		myAccountInfo.myCarryPrice.setIntValue( (int)carry );
		myAccountInfo.myInputTotal.setIntValue( (int)input );
		myAccountInfo.myOutputTotal.setIntValue( (int)output );
		myAccountInfo.myRestPrice.setIntValue( (int)restPrice );
		
	}
	
	//***デリゲート実装******************************


	//***プロパティアクセス***************************
	public NSDrawer
	drawer()
	{
		return myDrawer;
	}

	public NSPopUpButton
	accountPopup()
	{
		return myAccountPopup;
	}
	
	public KCMutableAccount
	selectedAccount()
	{
		return myWindow.accountAtIndex( accountPopup().indexOfSelectedItem() );
	}
	
	//*アクション*************************************
	public void
	actAccountApply( Object sender )
	{
		DateRange range = null;
		if ( myAccountDateButton.state() == NSCell.OnState )
		{//---日付絞り込み
			KCDate fromDate = KCLibDriver.toKCDate( myAccountDateFrom.dateValue() );
			KCDate toDate = KCLibDriver.toKCDate( myAccountDateTo.dateValue() );
			range = new DateRange( fromDate , toDate ) ;
		}
		myWindow.setDateRange( range );		

		//---カテゴリ絞り込み準備
		KCCategoryPath path = null;
		if ( myAccountCategoryButton.state() == NSCell.OnState )
		{
			path = new KCCategoryPath( myAccountCategoryBrowser.path() + KCCategory.SEPARATOR );
		}
		myWindow.setCategoryPath( path );
							
		updateAccountInfo();		
	}
	
	public void
	actAccountNew( Object sender )
	{
		/*AccountEditPanel panel = AccountEditPanel.getAccountInfo( 
										null , 
										NSBundle.localizedString("Account_New") , 
										"",
										0
										);
		if ( panel != null )
		{
			String name = panel.name();
			long price = panel.price();
			if ( name == null )
				return;
				
			KCMutableStdAccount account = new KCMutableStdAccount();
			account.setName( new KCName( new KCString( name ) ) );
			if ( !( (KCMutableStdBook)myWindow.book() ).addAccount( account ) )
			{
				NSSystem.log( "Add Acount Failuer" );
			}
			myWindow.updateAccountList();
		}*/
		
	}
	
	public void
	actAccountEdit( Object sender )
	{
	}
	
	public void
	actAccountDelete( Object sender )
	{
	}
	

}
