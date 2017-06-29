/* BootClass */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class BootClass 
{
	//*クラスのプロパティ**************************	
	static public BootClass myReal = null;
	static protected MainWindow		myMainWindow = null;
	static protected NSMutableArray	myAllWindow	= new NSMutableArray();
	static protected int				myWindowCnt = 0;

	//*クラスのメソッド******************************
	
	/**
	* ブートクラスを得る
	* @return ブートクラス
	*/
	static public BootClass
	getMainClass()
	{
		return myReal;
	}	

	//*プロパティ********************************
	protected NSMenu			myMenuAccount;
	
	//*コンストラクタ***********************************	
	public BootClass()
	{
		myReal = this;
	}
	
	//*メソッド*****************************************
	
	/**
	* ブートクラスの参照を取得する。
	* @return ブートクラス
	*/
	public BootClass
	ref()
	{
		return this;
	}
	
	public void
	newBook( 
		Object sender 
		)
	{
		KCStdBook book = StartPage.startKakeibo(false);
		if ( book != null )
		{
			myMainWindow = MainWindow.createWindow( book );
			myAllWindow.addObject( myMainWindow );
		}
	}

	public void
	openBook( 
		Object sender 
		)
	{
		KCStdBook book = StartPage.startKakeibo(true);
		if ( book != null )
		{
			myMainWindow = MainWindow.createWindow( book );
			book.setFilepath( StartPage.lastFilepath() );
			myAllWindow.addObject( myMainWindow );
		}
	}

	public void
	testFunction( Object sender )
	{
		//myMainWindow.activeAccount().dataTree().debugOut();
	}

	/**
	* メインメニューの"口座"を返す
	* @return 口座のメニュー
	*/
	public NSMenu
	menuAccount()
	{
		return myMenuAccount;
	}
	
	/**
	* メインウインドウを取得する
	*　@return メインウインドウ
	*/
	static public MainWindow
	mainWindow()
	{
		return myMainWindow;
	}
	
	/**
	* メインウインドウを設定する
	*　@param aWindow 設定するメインウインドウ
	*/
	static public void
	setMainWindow(
		MainWindow aWindow
		)
	{
		myMainWindow = aWindow;
	}
	
	/**
	* ウインドウリストから削除する
	* @param aWindow 削除するウインドウ
	*/
	static public void
	removeMainWindow(
		MainWindow aWindow
		)
	{
		myAllWindow.removeObject( aWindow );
	}
	
	//■デリゲート関数*****************************
	//===NSApplicationのデリゲート関数
	public void applicationDidFinishLaunching (NSNotification notification)
	{//---アプリケーション起動完了時に呼びだされる
		//---サービスを開始する
        NSApplication.sharedApplication().setServicesProvider(this);
		NSApplication.sharedApplication().setDelegate(this);

    }

	public boolean 
	applicationShouldTerminate (NSApplication app)
	{//---アプリケーションを終了するときに呼び出される

		//---未保存ドキュメントのチェック
		MainWindow tmpWindow = myMainWindow;
		while ( myAllWindow.count() > 0 )
		{//---全てのウインドウに対して行う
		
			//---閉じれるか，たずねる
			if ( !tmpWindow.canWindowClose() )
				return false;

			//---今のウインドウを配列から消して次のウインドウを取り出す
			tmpWindow.window().close();
			
		}

		return true;
	}

	public boolean applicationOpenUntitledFile (NSApplication app) 
	{//---アプリケーション起動時に呼び出される
		newBook(null);
		return true;
    }

}
