//
//  MatrixSupport.java
//  Kakeibo
//
//  Created by あっきー on 06/02/12.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class MatrixSupport 
{
	//*クラスのメソッド***************************

	/**
	* 次の日付を取得する
	* (例）date＝2001/2/4，モード=日，->2001/2/5
	* @param aDate 対象となる日付
	* @param aViewMode モード
	*/
	static public KCDate
	getNextDate(
		KCDate aDate,
		int aViewMode
		)
	{
		switch( aViewMode )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				return aDate.add( 0,0,1,0,0,0 );
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return aDate.add( 0,1,0,0,0,0 );
			default:
				
		}
		return null;
	}
	
	/**
	* ベースとなる日付を取得する
	* (例）date＝2001/2/4，モード=月，->2001/2/1
	* @param aDate 対象となる日付
	* @param aViewMode モード
	*/
	static public KCDate
	getBaseDate(
		KCDate aDate,
		int aViewMode
		)
	{
		switch( aViewMode )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				return new KCDate( 
					aDate.yearOfCommonEra() ,
					aDate.monthOfYear() ,
					1
					);
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return new KCDate( 
					aDate.yearOfCommonEra() ,
					1 ,
					1
					);					
			default:
				
		}
		return null;
	}
	
	/**
	* 前のページとなる日付を取得する
	* (例）date＝2001/2/4，モード=日，->2001/1/1
	* @param aDate 対象となる日付
	* @param aViewMode モード
	*/
	static public KCDate
	getPrevPageDate(
		KCDate aDate,
		int aViewMode
		)
	{
		switch( aViewMode )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				return getBaseDate( aDate , aViewMode ).add( 0,-1,0,0,0,0 );
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return getBaseDate( aDate , aViewMode ).add( -1,0,0,0,0,0 );				
			default:
				
		}
		return null;
	}

	/**
	* 次のページとなる日付を取得する
	* (例）date＝2001/2/4，モード=日，->2001/3/1
	* @param aDate 対象となる日付
	* @param aViewMode モード
	*/
	static public KCDate
	getNextPageDate(
		KCDate aDate,
		int aViewMode
		)
	{
		switch( aViewMode )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				return getBaseDate( aDate , aViewMode ).add( 0,1,0,0,0,0 );
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return getBaseDate( aDate , aViewMode ).add( 1,0,0,0,0,0 );					
			default:
				
		}
		return null;
	}
	
	/**
	* タイトルを取得する
	* (例）date=2001/2/4，モード=日，->2001/2
	* @param aDate 対象となる日付
	* @param aViewMode モード
	*/
	static public String
	getTitleForDateAndMode(
		KCDate aDate,
		int aViewMode
		)
	{
		switch( aViewMode )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				return Integer.toString( aDate.yearOfCommonEra() ) + NSBundle.localizedString( "Key_year" ) + 
						Integer.toString( aDate.monthOfYear() ) + NSBundle.localizedString( "Key_month" );
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return Integer.toString( aDate.yearOfCommonEra() ) + NSBundle.localizedString( "Key_year" );				
			default:
				
		}
		return "";		
	}
	
	//*プロパティ***************************
	private KCDate	myTop = null;
	private KCDate	myEnd = null;
	private KCDate	myToday = null;
	
	private int		myViewMode = MatrixView.ViewModeDayOfMonth;
	private KCDate	myActiveDate = null;
	private KCDate	myNonActiveDate = null;
	
	//private Sheet	mySheet = Sheet.abstractSheet();
	private MainWindow		myMainWindow = null;
	private NSMutableArray	myActiveRecords = null;
	private NSMutableArray	myPreRecords = null;
	
	//*コンストラクタ***********************
	public 
	MatrixSupport(
		MainWindow aWindow
		)
	{
		myMainWindow = aWindow;
		myToday = new KCDate();
		setActiveDate( getBaseDate( myToday , myViewMode ) );
	}
	
	//*メソッド*****************************
	
	/**
	* 表示に必要な列数を返す
	* @return 列数
	*/
	public int
	numberOfColumns()
	{
		switch( viewMode() )
		{
			case MatrixView.ViewModeDayOfMonth :
				//---日
				KCDate date = getNextPageDate( activeDate() , viewMode() ).add( 0,0,-1, 0,0,0 );
				return date.dayOfMonth();
			case MatrixView.ViewModeMonthOfYear :
				//---月
				return 12;			
			default:
		}
		return -1;
	}

	/**
	* 表示に必要な行数を返す
	* @return 行数
	*/
	public int
	numberOfRows()
	{
		return sheet().size();
	}
	
	
	/**
	* データベース上にある一番過去の日付を取得する
	* @return 日付
	*/
	public KCDate
	topDate()
	{
		return myTop;
	}
	
	/**
	* データベース上にある一番古い日付を取得する
	* @param aDate 日付
	*/
	public void
	setTopDate(
		KCDate aDate
		)
	{
		myTop = aDate;
	}

	/**
	* データベース上にある一番新しい日付を取得する
	* @return 日付
	*/
	public KCDate
	endDate()
	{
		return myEnd;
	}
	
	/**
	* データベース上にある一番新しい日付を設定する
	* @param aDate 日付
	*/
	public void
	setEndDate(
		KCDate aDate
		)
	{
		myEnd = aDate;
	}

	/**
	* 表示中の日付を取得する
	* @return 日付
	*/
	public KCDate
	activeDate()
	{
		return myActiveDate;
	}
	
	/**
	* 表示中の日付の終端を取得する。
	* （例：2006/2/1[表示モード：日] → 2006/3/1）
	*/
	public KCDate
	nonActiveDate()
	{
		return myNonActiveDate;
	}
	
	/**
	* 表示中の日付の範囲を取得する
	* @return 範囲
	*/
	public DateRange
	activeDateRange()
	{
		return new DateRange( activeDate() , nonActiveDate() );
	}
	
	/**
	* 表示中の日付を設定する
	* @return 変更があったらtrue
	* @param aDate 日付
	*/
	public boolean
	setActiveDate(
		KCDate aDate
		)
	{
		KCDate date =  getBaseDate( aDate , viewMode() );
		if ( myActiveDate != null )
			if ( myActiveDate.equalsByDay( date ) )
				return false;
		myActiveDate = date;
		myNonActiveDate = getNextPageDate( date , viewMode() );
		return true;
	}

	/**
	* 表示モードを取得する
	* @return モード
	*/
	public int
	viewMode()
	{
		return myViewMode;
	}

	/**
	* 表示モードを設定する
	* @param aMode モード
	*/
	public void
	setViewMode(
		int aMode
		)
	{
		myViewMode = aMode;
	}
	
	/**
	* 先頭に移動する
	*/
	public void
	moveToTopPage()
	{
		setActiveDate( topDate() );
	}
	
	/**
	* 前に移動する
	*/
	public void
	moveToPrevPage()
	{
		setActiveDate( getPrevPageDate( activeDate() , viewMode() ) );
	}
	
	/**
	* 次に移動する
	*/
	public void
	moveToNextPage()
	{
		setActiveDate( getNextPageDate( activeDate() , viewMode() ) );
	}

	/**
	* 最後に移動する
	*/
	public void
	moveToEndPage()
	{
		setActiveDate( endDate() );
	}

	/**
	* 列名を付ける
	* @param aMatrixView 対象となるビュー
	*/
	public void
	setTitleForColumns(
		MatrixView aMatrixView
		)
	{
		int mode = viewMode();
		KCDate date = activeDate();
		for ( int i = 0; i < numberOfColumns(); ++i )
		{
			NSCell cell = aMatrixView.cellAtIndexForColumn( i );
			String title = "";
			switch( mode )
			{
				case MatrixView.ViewModeDayOfMonth :
					//---日
					title = Integer.toString(i+1) + " (" + KCLibDriver.localizedDay( ( date.dayOfWeek()+i)%7 ) + ")";
					break;
				case MatrixView.ViewModeMonthOfYear :
					//---月
					title = Integer.toString(i+1) + NSBundle.localizedString( "Key_month" );					
					break;
				default:
			}
			cell.setTitle( title );
		}
	}

	/**
	* 行名を付ける
	* @param aMatrixView 対象となるビュー
	*/
	public void
	setTitleForRows(
		MatrixView aMatrixView
		)
	{
		for ( int i = 0; i < numberOfRows(); ++i )
		{
			NSCell cell = aMatrixView.cellAtIndexForRow( i );
			String title = "";
			cell.setTitle( sheet().itemAtIndex(i).name() );
		}
	}
	
	/**
	* 表示中のシートを得る
	* @return シート
	*/
	public Sheet
	sheet()
	{
		return mainWindow().activeSheet();
	}
	
	/**
	* シート用のデータベースを作成する
	* @param aAllRecords 全レコード
	* @param aAccontIterator アカウントイテレータ（口座一覧を取得するのに使用）
	*/
	public void
	createDataBase(
		NSArray aAllRecords	,
		KCAccountIterator aAccountIterator
		)
	{
		NSMutableArray records = new NSMutableArray();
		myPreRecords = new NSMutableArray();
		
		{//---アクティブな日付の更新
			setActiveDate( activeDate() );
		}
		
		{//---分ける
			java.util.Enumeration enumerator = aAllRecords.objectEnumerator();
			while( enumerator.hasMoreElements() )
			{
				KCRecord record = (KCRecord)enumerator.nextElement();
				if ( record.date().compareTo( activeDate() ) < 0 )
				{
					myPreRecords.addObject( record );
				}
				else if ( record.date().compareTo( nonActiveDate() ) < 0 )
				{
					records.addObject( record );
				}
			}
		}
		
		//---口座一覧を作成する
		NSMutableArray carryManager = new NSMutableArray();
		while( aAccountIterator.hasNext() )
		{
			KCAccount account = (KCAccount)aAccountIterator.next();
			KCMutableRecord record = new KCMutableRecord();
			record.setAccount( account );
			record.setMoney( new KCMoney( 0.0 ) );
			record.setCategoryPath( Common.categoryPathOfCarryMoney() );
			carryManager.addObject( record );
		}
		
		{//---繰越金を求める
			java.util.Enumeration enumerator = myPreRecords.objectEnumerator();
			while( enumerator.hasMoreElements() )
			{
				KCRecord record = (KCRecord)enumerator.nextElement();
				java.util.Enumeration accountEtr = carryManager.objectEnumerator();
				while( accountEtr.hasMoreElements() )
				{
					KCMutableRecord accountRecord = (KCMutableRecord)accountEtr.nextElement();
					if ( accountRecord.account().equals( record.account() ) )
					{
						accountRecord.setMoney( new KCMoney( accountRecord.money().price() + record.money().price()  ) );
						break;
					}
				}
			}
			
			//---Preに追加
			/*java.util.Enumeration accountEtr = carryManager.objectEnumerator();
			while( accountEtr.hasMoreElements() )
			{
				KCMutableRecord accountRecord = (KCMutableRecord)accountEtr.nextElement();
				accountRecord.setDate( topDate() )
				myPreRecords.addObject(
			}*/			
			
		}
		
		//---シート用にデータベース構築
		myActiveRecords = new NSMutableArray();
		int max = numberOfColumns();
		KCDate fromDate = activeDate();
		for( int i = 0; i < max; ++i )
		{
			NSMutableArray recordsMini = new NSMutableArray();

			{//---繰越金の追加
				java.util.Enumeration accountEtr = carryManager.objectEnumerator();
				while( accountEtr.hasMoreElements() )
				{
					KCMutableRecord accountRecord = (KCMutableRecord)accountEtr.nextElement();
					KCMutableRecord record = (KCMutableRecord)accountRecord.clone();
					record.setDate( fromDate );
					recordsMini.addObject( record );		
				}
			}
			
			//---その日付範囲のデータを抽出・同時に次の繰越金を求める
			KCDate endDate = getNextDate( fromDate , viewMode() );
			DateRange range = new DateRange( fromDate , endDate );
			for ( java.util.Enumeration itr = records.objectEnumerator(); itr.hasMoreElements(); )
			{
				KCRecord record = (KCRecord)itr.nextElement();
				if ( range.isInRangeByDay( record.date() ) )
				{
					recordsMini.addObject( record );
					
					//---次の繰越金を求める
					java.util.Enumeration accountEtr = carryManager.objectEnumerator();
					while( accountEtr.hasMoreElements() )
					{
						KCMutableRecord accountRecord = (KCMutableRecord)accountEtr.nextElement();
						if ( accountRecord.account().equals( record.account() ) )
						{
							accountRecord.setMoney( new KCMoney( accountRecord.money().price() + record.money().price()  ) );
							break;
						}
					}
				}
			}				
						
			//---各セルのデータを抽出
			NSMutableArray col = new NSMutableArray();
			int itemMax = sheet().size();
			for ( int j = 0; j < itemMax; j++ )
			{
				NSMutableArray cell = new NSMutableArray();
				SheetItem item = sheet().itemAtIndex( j );
			
						
				//---その他のカテゴリ
				for ( java.util.Enumeration itr = recordsMini.objectEnumerator(); itr.hasMoreElements(); )
				{
					KCRecord record = (KCRecord)itr.nextElement();
					if ( item.isInclude( record.categoryPath() ) )
					{
						cell.addObject( record );
					}
				}
				
				col.addObject( cell );
			}
			myActiveRecords.addObject( col );
			fromDate = endDate;
			
			
			
		}
		
	}
	
	/**
	* マトリックスのセルのレコードを取り出す
	* @return レコード配列
	* @param rowIndex 行のインデックス
	* @param colIndex 列のインデックス
	*/
	public NSArray
	cellRecords(
		int rowIndex,
		int colIndex
		)
	{
		return (NSArray)( (NSArray)myActiveRecords.objectAtIndex( colIndex) ).objectAtIndex( rowIndex );
	}
	
	/**
	* マトリックスに値を代入する
	* @param aMatrixView 対象のMatrixView
	* @param aAEM アカウントイネイブル
	*/
	public void
	setValueForMatrix(
		MatrixView aMatrixView,
		AccountEnableManager aAEM
		)
	{
		//---タイトル設定
		
		aMatrixView.setRootButtonTitle( getTitleForDateAndMode( activeDate() , viewMode() ) );
	
		//---セル設定
		for ( int col = 0; col < numberOfColumns(); col++ )
		{
			for ( int row = 0; row < numberOfRows(); row++ )
			{
				NSArray records = cellRecords( row , col );
				
				double sum = 0;
				for ( java.util.Enumeration itr = records.objectEnumerator(); itr.hasMoreElements(); )
				{
					KCRecord record = (KCRecord)itr.nextElement();
					if ( aAEM.isEnableForAccount( record.account() ) )
						sum += record.money().price();
				}
				
				NSAttributedString str = KCLibDriver.makeAttributedStringForMoney( new KCMoney( sum ) );
				NSCell cell = aMatrixView.cellAtIndexForMatrix( row , col );
				cell.setObjectValue( str );
				cell.setAlignment( NSText.RightTextAlignment );
			}
		}
	}
	
	/**
	* メインウインドウを取得する。
	* @return メインウインドウ
	*/
	public 
	MainWindow
	mainWindow()
	{
		return myMainWindow;
	}
	
}
