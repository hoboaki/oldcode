/* MatrixView */

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import java.util.*;

public class MatrixView extends NSView 
{
	//*クラスのプロパティ**********************
	static public final int MoveToTop = 0;
	static public final int MoveToPrev = 1;
	static public final int MoveToNext = 2;
	static public final int MoveToEnd = 3;
	static public final int MoveToToday = 4;
	
	static public final int ViewModeYear = 0;
	static public final int ViewModeMonthOfYear = 1;
	static public final int ViewModeWeakOfYear = 2;
	static public final int ViewModeDayOfMonth = 3;
	static public final int ViewModeFiscalYear = 4;
	static public final int ViewModeHalfOfYear = 5;
	static public final int ViewModeQuarterOfYear = 6;
	
	//*クラスのメソッド**********************
	
	/**
	* デフォルトとなる価格マトリックスのセルを取得する
	* @return テキストセル
	*/
	static public NSCell
	defaultCellForPriceMatrix()
	{
		NSTextFieldCell cell = new NSTextFieldCell();
		cell.setDrawsBackground( true );
		cell.setBackgroundColor( NSColor.whiteColor() );
		cell.setAlignment( NSText.RightTextAlignment );
		return cell;
	}

	/**
	* デフォルトとなる日付マトリックスのセルを取得する
	* @return ボタンセル
	*/
	static public NSCell
	defaultCellForDateMatrix()
	{
		NSButtonCell cell = new NSButtonCell();
		cell.setBezelStyle( NSButtonCell.ShadowlessSquareBezelStyle );
		return cell;
	}

	/**
	* デフォルトとなるグループマトリックスのセルを取得する
	* @return ボタンセル
	*/
	static public NSCell
	defaultCellForGroupMatrix()
	{
		NSButtonCell cell = new NSButtonCell();
		cell.setBezelStyle( NSButtonCell.ShadowlessSquareBezelStyle );
		return cell;
	}
		
	/**
	* デフォルトとなるマトリックスの１セルの大きさを返す
	* @return 大きさ
	*/
	static public NSSize
	defaultSizeForCellOfPriceMatrix()
	{
		return new NSSize( 80.0f , 18.0f );
	}
	
	/**
	* ヘッダーセルの高さを返す
	* @return 高さ
	*/
	static public float
	headerCellHeight()
	{
		return defaultSizeForCellOfPriceMatrix().height();
	}	

	
	//*プロパティ***************************
	private _ScrollView	myScrollView;
	
	private NSButton		myRootButton;
	
	private _ClipView		myClipViewForDateMatrix;
	private NSMatrix		myDateMatrix;
	
	private _ClipView		myClipViewForGroupMatrix;
	private NSMatrix		myGroupMatrix;
	
	private _ClipView		myClipViewForPriceMatrix;
	private NSMatrix		myPriceMatrix;
	
	private _MoveButtonView		myMoveButtonView;
	
	private MainWindow		myWindow = null;
	
	//*コンストラクタ************************
	
	/**
	* MatrixViewを作成する
	*/
	public 
	MatrixView()
	{
		super();
		initialize();
	}

	/**
	* MatrixViewを作成する
	* @param aRect viewの大きさ
	*/	
	public 
	MatrixView(
		NSRect aRect
		)
	{
		super( aRect );
		initialize();
	}
	
	/**
	* 変数類を初期化する
	*/
	private void
	initialize()
	{
		//---表示モード変更ボタン
		myRootButton = new NSButton( new NSRect( 0,0,10,10 ) );
		myRootButton.setBezelStyle( NSButtonCell.ShadowlessSquareBezelStyle );
		myRootButton.setTitle( NSBundle.localizedString( "SelectAllSymbol" ));
		myRootButton.setFrame( new NSRect( 0,0, defaultSizeForCellOfPriceMatrix().width() , defaultSizeForCellOfPriceMatrix().height() ) );
		addSubview( myRootButton );
	
		//---日付のマトリックス
		myDateMatrix = new NSMatrix( new NSRect( 0,0,100,100) );
		myDateMatrix.setMode( NSMatrix.ListMode );
		myDateMatrix.setAllowsEmptySelection( true );
		myDateMatrix.setPrototype( defaultCellForDateMatrix() );
		myDateMatrix.setCellSize( defaultSizeForCellOfPriceMatrix() );
		myDateMatrix.setIntercellSpacing( new NSSize( 1,1 ) );
		
		myDateMatrix.setDrawsBackground( true ) ;
		myDateMatrix.setBackgroundColor( NSColor.grayColor() );
		
		myClipViewForDateMatrix = new _ClipView();
		myClipViewForDateMatrix.setDocumentView( myDateMatrix );
		myClipViewForDateMatrix.setBackgroundColor( backgroundColor() );
		myClipViewForDateMatrix.setDrawsBackground( true );
		addSubview( myClipViewForDateMatrix );

		//---グループのマトリックス
		myGroupMatrix = new NSMatrix( new NSRect( 0,0,100,100) );
		myGroupMatrix.setMode( NSMatrix.ListMode );
		myGroupMatrix.setAllowsEmptySelection( true );
		myGroupMatrix.setPrototype( defaultCellForGroupMatrix() );
		myGroupMatrix.setCellSize( new NSSize( groupCellWidth() , defaultSizeForCellOfPriceMatrix().height() ) );
		myGroupMatrix.setIntercellSpacing( new NSSize( 1,1 ) );
		
		myGroupMatrix.setDrawsBackground( true ) ;
		myGroupMatrix.setBackgroundColor( NSColor.grayColor() );
		
		myClipViewForGroupMatrix = new _ClipView();
		myClipViewForGroupMatrix.setDocumentView( myGroupMatrix );
		myClipViewForGroupMatrix.setBackgroundColor( backgroundColor() );
		myClipViewForGroupMatrix.setDrawsBackground( true );
				
		addSubview( myClipViewForGroupMatrix );
				
		//---価格のマトリックス
		myPriceMatrix = new NSMatrix( new NSRect( 0,0,100,100) );
		myPriceMatrix.setMode( NSMatrix.ListMode );
		myPriceMatrix.setAllowsEmptySelection( true );
		myPriceMatrix.setPrototype( defaultCellForPriceMatrix() );
		myPriceMatrix.setCellSize( defaultSizeForCellOfPriceMatrix() );
		myPriceMatrix.setIntercellSpacing( new NSSize( 1,1 ) );
		
		myPriceMatrix.setDrawsBackground( true ) ;
		myPriceMatrix.setBackgroundColor( NSColor.grayColor() );
		
		myClipViewForPriceMatrix = new _ClipView();
		myClipViewForPriceMatrix.setDocumentView( myPriceMatrix );
		myClipViewForPriceMatrix.setBackgroundColor( backgroundColor() );
		myClipViewForPriceMatrix.setDrawsBackground( true );
				
		//---スクロールバー
		myScrollView = new _ScrollView();
		myScrollView.setContentView( myClipViewForPriceMatrix );
		addSubview( myScrollView );
		
		//---スクロールをシンクロする設定
		myClipViewForDateMatrix.setScrollSyncView( myScrollView );
		myClipViewForDateMatrix.setColumnSyncView( myClipViewForPriceMatrix );
		myClipViewForGroupMatrix.setScrollSyncView( myScrollView );
		myClipViewForGroupMatrix.setRowSyncView( myClipViewForPriceMatrix );
		myClipViewForPriceMatrix.setColumnSyncView( myClipViewForDateMatrix );
		myClipViewForPriceMatrix.setRowSyncView( myClipViewForGroupMatrix );
		
		//_setRowsAndColumns( 10 , 10 ); //--- for debug
		
		//---移動ボタン
		myMoveButtonView = new _MoveButtonView( this );
		addSubview( myMoveButtonView );
		
				
		//---整形を整える
		updatePositionAndSize();
		
	}
	
	
	//*メソッド**********************************

	/**
	* メインウインドウを設定する
	* @param aWindow メインウインドウ
	*/
	public void
	setWindow(
		MainWindow aWindow
		)
	{
		myWindow = aWindow;
	}

	/**
	* オブジェクトの大きさと位置を整える
	*/
	public void
	updatePositionAndSize()
	{
		NSRect allFrame = frame();
		float priceWidth = allFrame.width()-groupCellWidth();
		float priceHeight = allFrame.height()-headerCellHeight();
		
		//---ルートボタン
		rootButton().setFrame(
			new NSRect( 
				0,
				0,
				groupCellWidth(),
				headerCellHeight()
				)
			);
		
		//---スクロールビュー
		scrollView().setFrame( 
			new NSRect( 
				groupCellWidth() ,
				headerCellHeight() , 
				priceWidth,
				priceHeight
				)
			);
				
		//---日付マトリックス
		myClipViewForDateMatrix.setFrame( 
			new NSRect(
				groupCellWidth(),
				0 ,
				scrollView().frame().width(),	
				headerCellHeight()		
				)
			);
			
		//---グループマトリックス
		myClipViewForGroupMatrix.setFrame( 
			new NSRect(
				0,
				headerCellHeight() ,
				groupCellWidth() ,
				scrollView().contentView().frame().height()	
				)
			);
			
		//---移動ボタンビュー
		myMoveButtonView.setFrame( new NSRect( 
				new NSPoint( scrollView().frame().x() , allFrame.height() - NSScroller.scrollerWidth() ) , 
				myMoveButtonView.frame().size() 
				) 
			);
						
	}
	
	/**
	* ルートボタンのタイトルを設定する
	* @param aTitle 設定するタイトル
	*/
	public void
	setRootButtonTitle(
		String aTitle
		)
	{
		rootButton().setTitle( aTitle );
	}
	
	/**
	* ルートボタンを取得する
	* @return ルートボタン
	*/
	public NSButton
	rootButton()
	{
		return myRootButton;
	}
	
	/**
	* 日付マトリックスを取得する
	* @return マトリックス
	*/
	public NSMatrix
	dateMatrix()
	{
		return myDateMatrix;
	}

	/**
	* グループマトリックスを取得する
	* @return マトリックス
	*/
	public NSMatrix
	groupMatrix()
	{
		return myGroupMatrix;
	}
		
	/**
	* 金額マトリックスを取得する
	* @return マトリックス
	*/
	public NSMatrix
	priceMatrix()
	{
		return myPriceMatrix;
	}
	
	/**
	* スクロールビューを取得する
	* @return スクロールビュー
	*/
	public _ScrollView
	scrollView()
	{
		return myScrollView;
	}
	
	/**
	* グループタブの横幅を取得する
	* @return 横幅
	*/
	public float
	groupCellWidth()
	{
		return 100.0f;
	}
	
	/**
	* 背景色を取得する
	* @return 背景色
	*/
	public NSColor
	backgroundColor()
	{
		return NSColor.grayColor();
	}
		
	/**
	* 移動ボタンが押された時に呼び出される
	* @param aParameter 先頭，前，次，最後，今日のいずれか
	*/
	public void
	matrixViewMove(
		int aParameter
		)
	{
		//NSSystem.log( Integer.toString( aParameter ) );
		switch( aParameter )
		{
			case MoveToTop:
				myWindow.matrixSupport().moveToTopPage();
				break;
			case MoveToPrev:
				myWindow.matrixSupport().moveToPrevPage();
				break;
			//case MoveToToday:
			//	myWindow.matrixSupport().moveToToday();
			//	break;
			case MoveToNext:
				myWindow.matrixSupport().moveToNextPage();
				break;
			case MoveToEnd:
				myWindow.matrixSupport().moveToEndPage();
				break;
			default :
				return;
		}
		myWindow.viewModeDidChange();
	}
	
	/**
	* 表示モードが変更された時に呼び出される
	* @param aMode 変更後のモード
	*/
	public void
	matrixViewChangeViewMode(
		int aMode
		)
	{
		//NSSystem.log( Integer.toString( aMode ) );
		myWindow.matrixSupport().setViewMode( aMode );
		myWindow.viewModeDidChange();		
	}

	/**
	* matrixのセルを取得する
	* @return セル
	* @param aRow 行インデックス
	* @param aCol 列インデックス
	*/
	public NSCell
	cellAtIndexForMatrix(
		int aRow ,
		int aCol
		)
	{
		return myPriceMatrix.cellAtLocation( aRow , aCol );
	}

	/**
	* columnのセルを取得する
	* @return セル
	* @param aIndex インデックス
	*/
	public NSCell
	cellAtIndexForColumn(
		int aIndex
		)
	{
		return myDateMatrix.cellAtLocation( 0, aIndex );
	}

	/**
	* rowのセルを取得する
	* @return セル
	* @param aIndex インデックス
	*/
	public NSCell
	cellAtIndexForRow(
		int aIndex
		)
	{
		return myGroupMatrix.cellAtLocation( aIndex ,0 );
	}
	
	
	/**
	* マトリックスのセル数を変更し，新しいフレームサイズも設定する
	* @param aRows グループ数
	* @param aColumns 日付数
	*/
	public void 
	setRowsAndColumns(
		int aRows,
		int aColumns
		)
	{
		//---数の設定
		dateMatrix().renewRowsAndColumns( 1 , aColumns );
		groupMatrix().renewRowsAndColumns( aRows , 1 );
		priceMatrix().renewRowsAndColumns( aRows , aColumns );
		
		//---フレームの大きさの設定
		NSSize cSize = priceMatrix().cellSize();
		float width = aColumns * ( cSize.width()+1 );
		float height = aRows * ( cSize.height()+1 );
		dateMatrix().setFrame( new NSRect( 0,0, width, headerCellHeight() ) );
		groupMatrix().setFrame( new NSRect( 0,0, groupCellWidth() , height ) );
		priceMatrix().setFrame( new NSRect( 0,0, width , height ) );
	}
		
	//*オーバーライドメソッド**************************
	
	public boolean 
	isFlipped()
	{
		return true;
	}

	public void 
	setFrame( 
		NSRect aRect
		)
	{
		super.setFrame( aRect );
		updatePositionAndSize();
	}

	//*その他プライベートなものたち***********************


	//*他のクリップビューにもスクロール情報を送るクリップビュー
	private class _ClipView extends NSClipView
	{
		//*プロパティ***************************
		private _ClipView	colView = null;
		private _ClipView	rowView = null;
		private _ScrollView	scrollView = null;
		
		//*コンストラクタ************************
		public 
		_ClipView()
		{
			super();
		}
		
		//*メソッド**********************************
		
		/**
		* 一緒に横移動するビューを登録する
		* @param aView ビュー
		*/
		public void
		setColumnSyncView(
			_ClipView aView
			)
		{
			colView = aView;
		}

		/**
		* 一緒に縦移動するビューを登録する
		* @param aView ビュー
		*/
		public void
		setRowSyncView(
			_ClipView aView
			)
		{
			rowView = aView;
		}

		/**
		* 移動後にスクロールバーの位置を更新するスクロールビューを登録する
		* @param aView スクロールビュー
		*/
		public void
		setScrollSyncView(
			_ScrollView aView
			)
		{
			scrollView = aView;
		}
				
		//*オーバーライドメソッド**************************

		public void 
		scrollToPoint(
			NSPoint newOrigin
			)
		{
			scrollToPoint( newOrigin , false );
		}
		
		public void 
		scrollToPoint(
			NSPoint newOrigin,
			boolean aTerminateFlg
			)
		{
			super.scrollToPoint( newOrigin );
			if ( !aTerminateFlg )
			{
				if ( colView != null )
				{//---一緒に横移動
					colView.scrollToPoint( new NSPoint( newOrigin.x() , 0 ) , true );
				}			
				if ( rowView != null )
				{//---一緒に縦移動
					rowView.scrollToPoint( new NSPoint( 0 , newOrigin.y() ) , true );
				}
				if ( scrollView != null )
				{//---スクロールバーを更新
					scrollView.updateBar();
				}
			}
		}
		
	}

	//*移動ボタンのビュー
	static private class _MoveButtonView extends NSView
	{
		//*クラスのメソッド***************************
		
		/**
		* コントロールボタンの横幅を取得する
		* @return 横幅
		*/
		static public float
		buttonWidth()
		{
			return NSScroller.scrollerWidth();
		}
		
		/**
		* 表示モードポップアップの横幅を返す
		* @return 高さ
		*/
		static public float
		popupWidth()
		{
			return 56.0f;
		}	
		
		/**
		* コントロールビューの横幅を取得する
		* @return 横幅
		*/
		static public float
		viewWidth()
		{
			return buttonWidth() * 5 + popupWidth();
		}
		
		//*プロパティ***************************
		private MatrixView			myMatrixView = null;
		private NSPopUpButtonCell	myPopup = new NSPopUpButtonCell();
		
		//*コンストラクタ************************
	
		/**
		* @param aMatrixView 所属するマトリックスビュー
		*/
		public 
		_MoveButtonView(
			MatrixView aMatrixView
			)
		{
			super();
			myMatrixView = aMatrixView;
			initialize();
		}
		
		private void
		initialize()
		{			
			//---Matrixの設定
			NSSize cellSize = new NSSize(  buttonWidth(), NSScroller.scrollerWidth()  );
			NSRect cellRect = new NSRect( new NSPoint(0,0) , cellSize );
			
			{//---先頭へ移動ボタン
				CellView cell = new CellView();
				cell.setCell( _moveButton( "moveTop" , "_moveTop" ) );
				cell.setFrame(cellRect );
				addSubview( cell );
			}
			{//---前へ移動ボタン
				CellView cell = new CellView();
				cell.setCell( _moveButton( "movePrev" , "_movePrev" ) );
				cell.setFrame( new NSRect( new NSPoint( cellSize.width()*1 , 0 ) , cellSize ) );
				addSubview( cell );
			}
			{//---次へ移動ボタン
				CellView cell = new CellView();
				cell.setCell( _moveButton( "moveNext" , "_moveNext" ) );
				cell.setFrame( new NSRect( new NSPoint( cellSize.width()*2 , 0 ) , cellSize ) );
				addSubview( cell );
			}
			{//---最後へ移動ボタン
				CellView cell = new CellView();
				cell.setCell( _moveButton( "moveEnd" , "_moveEnd" ) );
				cell.setFrame( new NSRect( new NSPoint( cellSize.width()*3 , 0 ) , cellSize ) );
				addSubview( cell );
			}
			{//---今日へ移動ボタン
				CellView cell = new CellView();
				cell.setCell( _moveButton( "moveToday" , "_moveToday" ) );
				cell.setFrame( new NSRect( new NSPoint( cellSize.width()*4 , 0 ) , cellSize ) );
				addSubview( cell );
			}
			{//---表示切り替えボタン
				CellView cell = new CellView();
				NSPopUpButtonCell popup = myPopup;
				popup.setBezelStyle( NSButtonCell.ShadowlessSquareBezelStyle );
				popup.setArrowPosition( NSPopUpButtonCell.PopUpArrowAtBottom );
				popup.setFont( NSFont.menuBarFontOfSize(10.0f) );
				{//---アクションの設定
					popup.setTarget( this );
					popup.setAction( new NSSelector( "_viewModeDidChange" , new Class[]{ Object.class } ) );
				}
				{//---項目の追加
					popup.addItem( NSBundle.localizedString( "ViewMode_dayOfMonth" ) );
					popup.addItem( NSBundle.localizedString( "ViewMode_monthOfYear" ) );
					//popup.addItem( NSBundle.localizedString( "ViewMode_weakOfYear" ) );
					//popup.addItem( NSBundle.localizedString( "ViewMode_year" ) );
					//popup.addItem( NSBundle.localizedString( "Key_fiscalYear" ) );
					//popup.addItem( NSBundle.localizedString( "Key_half" ) );
					//popup.addItem( NSBundle.localizedString( "Key_quarter" ) );
				}
				cell.setCell( popup );
				cell.setFrame( new NSRect( cellSize.width()*5 , 0 , popupWidth() , NSScroller.scrollerWidth() ) );	
				addSubview( cell );
				
				setFrame( new NSRect( 0,0, viewWidth() ,  NSScroller.scrollerWidth() ) );
				
			}
		}
	
		//*その他プライベートなものたち***********************
		private NSButtonCell
		_moveButton(
			String aImageName,
			String aActionName
			)
		{
			NSButtonCell cell = new NSButtonCell( NSImage.imageNamed( aImageName ) );
			cell.setBezelStyle( NSButtonCell.ShadowlessSquareBezelStyle );
			cell.setTarget( this );
			cell.setAction( new NSSelector( aActionName , new Class[]{ Object.class } ) );
			return cell;
		}
		
		private void
		_moveTop(
			Object anObject
			)
		{
			myMatrixView.matrixViewMove( MatrixView.MoveToTop );
		}

		private void
		_movePrev(
			Object anObject
			)
		{
			myMatrixView.matrixViewMove( MatrixView.MoveToPrev );
		}
		
		private void
		_moveNext(
			Object anObject
			)
		{
			myMatrixView.matrixViewMove( MatrixView.MoveToNext );
		}
		
		private void
		_moveEnd(
			Object anObject
			)
		{
			myMatrixView.matrixViewMove( MatrixView.MoveToEnd );
		}
		
		private void
		_moveToday(
			Object anObject
			)
		{
			myMatrixView.matrixViewMove( MatrixView.MoveToToday );
		}
		
		private void
		_viewModeDidChange( 
			Object anObject 
			)
		{
			int viewMode = -1;
			switch( myPopup.indexOfSelectedItem() )
			{
				case 0:
					viewMode = MatrixView.ViewModeDayOfMonth;
					break;
				case 1:
					viewMode = MatrixView.ViewModeMonthOfYear;
					break;
				case 2:
					viewMode = MatrixView.ViewModeYear;
					break;
				case 3:
					viewMode = MatrixView.ViewModeWeakOfYear;
					break;
				default:
					return;
			}
			myMatrixView.matrixViewChangeViewMode( viewMode );
		}	
		
	}

	//*横バーはずっと消えないというちょっと変わったスクロールビュー
	static private class _ScrollView extends NSScrollView
	{
		//*クラスのメソッド***************************
				
		//*プロパティ***************************
		
		
		//*コンストラクタ************************
		
		/**
		* @param aMatrixView 所属するマトリックスビュー
		*/
		public
		_ScrollView()
		{
			super( );
			initialize();
		}
		
		private void
		initialize()
		{
			setHasHorizontalScroller( true );
			setHorizontalScroller( new _Scroller( new NSRect( 0,0, 20,10 ) ) );
			setHasVerticalScroller( false );
		}
		
		//*メソッド**********************************
		
		/**
		* 整形を整える
		*/
		public void
		updatePositionAndSize(
			NSRect aNewFrame
			)
		{
			//---横スクロールバー
			NSScroller scroller = horizontalScroller();
			float maxWidth = aNewFrame.width();
			if ( hasVerticalScroller()  )
				maxWidth -= NSScroller.scrollerWidth();
			if ( scroller != null )
			{
				NSRect rect = new NSRect( 
						0, 
						scroller.frame().y(),
						maxWidth ,
						scroller.frame().height()
						);
				scroller.setFrame( rect );
			}
					
		}
		
		/**
		* スクロールバーの位置を更新する
		*/
		public void
		updateBar()
		{
			NSClipView content = contentView();
			NSView document = content.documentView();
			{//--横スクロールバー
				NSScroller scroller = horizontalScroller();
				if ( scroller != null )
				{
					float widthMax = document.frame().width()-content.frame().width();
					if ( widthMax > 0.0f )
					{
						float width = content.documentVisibleRect().x() / widthMax;
						scroller.setFloatValueAndKnobProportion(
							width ,
							scroller.knobProportion()
							);
					}
				}
			}	
			{//--縦スクロールバー
				NSScroller scroller = verticalScroller();
				if ( scroller != null )
				{
					float heightMax = document.frame().height()-content.frame().height();
					if ( heightMax > 0.0f )
					{
						float height = content.documentVisibleRect().y() / heightMax;
						scroller.setFloatValueAndKnobProportion(
							height ,
							scroller.knobProportion()
							);
					}
				}
			}
			
		}
		
		//*オーバーライドメソッド**************************
		public void 
		setFrame(
			NSRect aRect
			)
		{
			super.setFrame( aRect );
			updatePositionAndSize( aRect );
			
			//---クリップビューが本物より大きい場合，縦バーを消す
			if ( documentView().frame().height() < contentView().frame().height() )
			{
				setHasVerticalScroller( false );
			}
			else
			{
				setHasVerticalScroller( true );
			}
		}
		
		public boolean 
		isFlipped()
		{
			return true;
		}
	
		//*スクロールバー
		static private class _Scroller extends NSScroller
		{
			//*コンストラクタ************************
			public
			_Scroller(
				NSRect aRect
				)
			{
				super( aRect );
			}
			
			//*オーバーライドメソッド**************************
			public void 
			setFrame(
				NSRect aRect
				)
			{//---左の余白（移動ボタン郡）を考慮してフレーム設定する
				if ( aRect.width() < MatrixView._MoveButtonView.viewWidth() )
					return;
				NSRect rect = new NSRect(
						MatrixView._MoveButtonView.viewWidth(), 
						aRect.y(),
						aRect.width() - MatrixView._MoveButtonView.viewWidth() ,
						aRect.height()			
						);
				super.setFrame( rect );
			}
			
		}
			
	}
	
}
