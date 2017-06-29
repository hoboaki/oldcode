/**<pre>
* NSBrowserにKCCategoryを表示させるためのドライバクラス．
*
*</pre>
*/
import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class CategoryBrowserDriver
{	
	//*プロパティ*****************************
	private KCStdBook myBook;
	
	//*コンストラクタ*************************

	/**
	* ブラウザに表示させる本を指定すして，ドライバを作成する
	* @param aBook 本
	*/
	public CategoryBrowserDriver(
		KCStdBook aBook
		)
	{
		myBook = aBook;
	}
	
	
	//*メソッド**************************	

	/**
	* 一列を作る
	*/
	protected void
	createRows(
		NSMatrix aMatrix ,
		KCMutableCategory aCategory
		)
	{
		KCCategoryIterator itr = aCategory.childs().iterator();
		while( itr.hasNext() )
		{
			KCMutableCategory category = (KCMutableCategory)itr.next();
		
			NSBrowserCell tmpCell = new NSBrowserCell();
			tmpCell.setObjectValue( new CategoryReference( category ) );
			if ( category.isLeaf() )
				tmpCell.setLeaf( true );
			aMatrix.setPrototype( tmpCell );
			aMatrix.addRow();
		}
	}	
	
	/**
	* 表示する時に呼び出される
	*/
	public void 
	browserWillDisplayCell(
		NSBrowser sender, 
		Object cell, 
		int row, 
		int column
		)
	{	
	}

	/**
	* ブラウザにカテゴリを表示する要求があったときに呼び出される
	*/
	public void 
	browserCreateRowsForColumn(
		NSBrowser sender, 
		int column, 
		NSMatrix matrix
		)
	{		
		KCMutableCategory activeCategory = (KCMutableCategory)myBook.rootCategory();
		if ( column > 0 )
		{//---選択中のセルをゲットする
			activeCategory = (KCMutableCategory)( ( (Reference)sender.selectedCell().objectValue() ).real() );
		}
		createRows( matrix , activeCategory );
		
	}
	
}
