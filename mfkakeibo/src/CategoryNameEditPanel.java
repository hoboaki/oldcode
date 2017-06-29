/* CategoryNameEditPanel */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

public class CategoryNameEditPanel 
{
	//*これを実行せよ***********************
	static String
	getName(
		NSWindow aWindow,
		String aTitle,
		String aPath,
		String aDefaultName
		)
	{
		CategoryNameEditPanel panel = new CategoryNameEditPanel();
		panel.setTitle( aTitle );
		panel.setPath( aPath );
		panel.setName( aDefaultName );
		if ( panel.modal( aWindow ) )
			return panel.name();
		return null;
	}

	public NSPanel	myPanel;
    public NSTextField myName; /* IBOutlet */
    public NSTextField myPath; /* IBOutlet */
	public boolean myOK = false;
	
	//*コンストラクタ***********************
	protected CategoryNameEditPanel()
	{
		//---nibファイルを読み込む
		if (NSApplication.loadNibNamed("CategoryNameEditPanel.nib", this) == false) 
		{//---読み込みがエラーしたらリターンする
			NSSystem.log("Couldn't load CategoryNameEditPanel.nib");
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
	setPath(
		String aPath
		)
	{
		myPath.setStringValue( aPath );
	}

	public void
	setName(
		String aName
		)
	{
		myName.setStringValue( aName );
	}
	
	public boolean
	modal(
		NSWindow aWindow
		)
	{	
		aWindow.makeFirstResponder( myName );
		NSApplication.sharedApplication().runModalForWindow(myPanel,aWindow);
		return myOK;
	}
	
	public String
	name()
	{
		return myName.stringValue();
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
		if ( myName.stringValue().equals( "" ) )
			return;
		else if ( myName.stringValue().indexOf( "/" ) >= 0 )
			return;
		myOK = true;
		myPanel.close();
		NSApplication.sharedApplication().abortModal();
	}


}
