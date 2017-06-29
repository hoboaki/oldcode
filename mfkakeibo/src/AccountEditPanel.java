/* AccountEditPanel */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

public class AccountEditPanel {

    public NSTextField myName; /* IBOutlet */
    public NSTextView myComment; /* IBOutlet */
	public NSPanel		myPanel;
	public boolean myOK = false;
	
	//*これを実行せよ***********************
	static public AccountEditPanel
	getAccountInfo(
		NSWindow aWindow,
		String aTitle,
		String aName,
		String aComment
		)
	{
		AccountEditPanel panel = new AccountEditPanel();
		panel.setTitle( aTitle );
		panel.setName( aName );
		panel.setComment( aComment );
		if ( panel.modal( aWindow ) )
			return panel;
		return null;
	}
	
	//*コンストラクタ***********************
	protected AccountEditPanel()
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("AccountEditPanel.nib", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load AccountEditPanel.nib");
			return;
        }
	}
	
	//*メソッド*****************************
	public void
	setTitle(
		String aTitle
		)
	{
		myPanel.setTitle( aTitle );
	}
	
	public void
	setName(
		String aName
		)
	{
		myName.setStringValue( aName );
	}
	
	public String
	name()
	{
		return myName.stringValue();
	}
	
	public void
	setComment(
		String aComment
		)
	{
		myComment.setString( aComment );
	}
	
	public String
	comment()
	{
		return myComment.string();
	}
	
	public boolean
	modal(
		NSWindow aWindow
		)
	{	
		//myPanel.makeFirstResponder( myName );
		NSApplication.sharedApplication().runModalForWindow(myPanel,null);
		return myOK;
	}
	
	//*呼び出し系****************************
	public void
	cancel(
		Object sender
		)
	{
		myPanel.close();
		NSApplication.sharedApplication().stopModal();
	}
	
	public void
	ok(
		Object sender
		)
	{
		if ( name().equals( "" ) )
			return;
		myOK = true;
		myPanel.close();
		NSApplication.sharedApplication().abortModal();
	}

}
