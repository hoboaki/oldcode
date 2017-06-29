//
//  CellView.java
//  Kakeibo
//
//  Created by あっきー on 06/02/09.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;


public class CellView extends NSView
{
	//*プロパティ***************************
	private NSMatrix	myMatrix;
	
	//*コンストラクタ************************
	
	public 
	CellView()
	{
		super();
		initialize();
	}
	
	/**
	* 変数類を初期化する
	*/
	public void
	initialize()
	{
		myMatrix = new NSMatrix( frame() );
		myMatrix.renewRowsAndColumns(1,1);
		myMatrix.setIntercellSpacing( new NSSize(0,0) );
		myMatrix.setAutosizesCells( true );
		addSubview( myMatrix );
	}
	
	//*メソッド**********************************
	
	/**
	* セルをセットする
	* @param セットするセル
	*/
	public void
	setCell(
		NSCell aCell
		)
	{
		myMatrix.putCellAtLocation( aCell , 0,0 );
	}
	
	/**
	* セルを取得する
	* @return 設置済みのセル
	*/
	public NSCell
	cell()
	{
		return myMatrix.cellAtLocation(0,0);
	}
	
	//*オーバーライドメソッド**************************
	
	public void
	setFrame(
		NSRect aFrame
		)
	{
		super.setFrame( aFrame );
		if ( myMatrix != null )
		{
			myMatrix.setCellSize( new NSSize( aFrame.size() ) );
			myMatrix.setFrame( new NSRect( new NSPoint(0,0) , aFrame.size() ) );
		}
	}
	
	
}
