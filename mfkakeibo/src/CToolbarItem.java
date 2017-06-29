import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;


public class CToolbarItem
{//---NSToolbarItemを含んだクラス

	//■クラス定数*********************************

	//■クラス関数*********************************
	static public CToolbarItem autoCreateItem(
		String itemIdentifier,	//アイテムID
		String funcName,		//アクション関数の名前,
		NSView viewObj,			//ビュークラス
		Object targetObj		//ターゲットクラス
		)
	{//---自動でラベルを生成してクラスを作成する（ローカライズ対応）
		return new CToolbarItem(
			itemIdentifier,
			funcName,
			targetObj,
			NSBundle.localizedString( itemIdentifier + "_Label",null) ,
			NSBundle.localizedString( itemIdentifier + "_PaletteLabel",null) ,
			NSBundle.localizedString( itemIdentifier + "_Tip",null) ,
			viewObj
			);
	}
	
	static public CToolbarItem autoCreateItem(
		String itemIdentifier,	//アイテムID
		String funcName,		//アクション関数の名前,
		String imageName,		//アイコンイメージの名前
		Object targetObj		//ターゲットクラス
		)
	{//---自動でラベルを生成してクラスを作成する（ローカライズ対応）
		return new CToolbarItem(
			itemIdentifier,
			funcName,
			targetObj,
			NSBundle.localizedString( itemIdentifier + "_Label",null) ,
			NSBundle.localizedString( itemIdentifier + "_PaletteLabel",null) ,
			NSBundle.localizedString( itemIdentifier + "_Tip",null) ,
			imageName
			);
	}

	static public CToolbarItem autoCreateItem(
		String itemIdentifier,	//アイテムID
		String funcName,		//アクション関数の名前,
		String imageName		//アイコンイメージの名前
		)
	{//---自動でラベルを生成してクラスを作成する（ローカライズ対応）
		return new CToolbarItem(
			itemIdentifier,
			funcName,
			null,
			NSBundle.localizedString( itemIdentifier + "_Label",null) ,
			NSBundle.localizedString( itemIdentifier + "_PaletteLabel",null) ,
			NSBundle.localizedString( itemIdentifier + "_Tip",null) ,
			imageName
			);
	}

	//■プロパティ*********************************
	protected	NSToolbarItem	myItem;				//アイテム本体
	protected	String			myActionFuncName;	//アクション関数の名前

	//■コンストラクタ*****************************
	CToolbarItem( String itemIdentifier )
	{
		//---初期化する
		myItem = new NSToolbarItem( itemIdentifier );
		myActionFuncName = null;
	}

	CToolbarItem(String itemIdentifier,
		String funcName,		//アクション関数の名前
		Object targetObj		//ターゲットクラス
		)
	{
		this( itemIdentifier );

		//---初期化する
		if ( funcName != null )
			setActionFunc( funcName );
		myItem.setTarget( targetObj );

	}

	CToolbarItem(String itemIdentifier,
		String funcName,	//---アクション関数の名前
		Object targetObj,	//---ターゲットクラス
		String label,		//---ラベル
		String palette,		//---カスタマイズパレットのラベル
		String tip,			//---ツールチップ
		String imageName	//---イメージ名
		)
	{
		this( itemIdentifier , funcName , targetObj );

		//---初期化する
		myItem.setLabel( label );
		myItem.setPaletteLabel( palette );
		myItem.setToolTip( tip );
		myItem.setImage( NSImage.imageNamed(imageName) );

	}

	CToolbarItem(String itemIdentifier,
		String funcName,	//---アクション関数の名前
		Object targetObj,	//---ターゲットクラス
		String label,		//---ラベル
		String palette,		//---カスタマイズパレットのラベル
		String tip,			//---ツールチップ
		NSView viewObj		//---ビューオブジェ
		)
	{
		this( itemIdentifier , funcName , targetObj );

		//---初期化する
		myItem.setLabel( label );
		myItem.setPaletteLabel( palette );
		myItem.setToolTip( tip );
		myItem.setView( viewObj );

	}

	//■オーバーライド関数**************************

	//■メソッド************************************
	public NSToolbarItem item()
	{//---アイテムを返す
		return myItem;
	}

	public boolean setActionFunc( String funcName )
	{//---アクション関数を設定する
		myActionFuncName = funcName;
		boolean flg = true;
		if ( funcName == null )
			flg = false;
		return setActionEnable( flg );
	}

	public boolean setActionEnable(
		boolean enableFlg	//アクション有効フラグ
		)
	{//---アクション関数を有効・無効を設定する

		if ( enableFlg )
		{
			String funcName = myActionFuncName;
			if ( funcName == null )
				return false;
			try
			{
				Class	c[] = new Class[1];
				c[0] = Class.forName("java.lang.Object");
				NSSelector selector = new NSSelector( funcName ,c);
				myItem.setAction(selector);
				myItem.setTarget(this);
			}catch(ClassNotFoundException e)
			{
				if ( enableFlg )
				NSSystem.log(e.toString()+"\n");	//---エラー出力
				return false;
			}		
		}
		else
		{
			myItem.setAction( null );
		}
		
		return true;
	}

}
