//
//  KCLibDriver.java
//  Kakeibo
//
//  Created by あっきー on 06/01/07.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class KCLibDriver 
{
	/**
	* KCDateに変換する
	* @return KCDate 変換されたKCDate
	* @param aDate NSDate型の日付
	*/
	static public KCDate
	toKCDate(
		NSDate aDate
		)
	{
		return toKCDate( new NSGregorianDate( 0 , aDate ) );
	}
	
	/**
	* KCDateに変換する
	* @return KCDate 変換されたKCDate
	* @param aDate NSGregorianDate型の日付
	*/
	static public KCDate
	toKCDate(
		NSGregorianDate aDate
		)
	{
		NSTimeZone srcTZ = aDate.timeZone();
		
		//---タイムゾーン
		KCTimeZone timezone;
		{
			int seconds = srcTZ.secondsFromGMT();
			boolean positive = true;
			if ( seconds < 0 )
			{
				positive = false;
				seconds *= -1;
			}
			
			int hour = seconds / 3600;
			seconds %= 3600;
			int minutes = seconds / 60;
			
			timezone = new KCTimeZone( positive , hour , minutes );
		}
		
		return new KCDate(
			aDate.yearOfCommonEra() ,
			aDate.monthOfYear() ,
			aDate.dayOfMonth() ,
			0 ,
			0,
			0 ,
			timezone
			);
		
	}
	
	/**
	* レコードコンテナをNSArrayに変換する
	* @return 変換したNSArray
	* @param aIterator レコードコンテナのイテレータ
	* @param aRange 絞り込み日付範囲
	* @param aPath 絞り込みカテゴリ
	*/
	static public NSMutableArray
	recordsToNSArray(
		KCRecordIterator aIterator,
		DateRange	aRange,
		KCCategoryPath aPath
		)
	{
		NSMutableArray array = new NSMutableArray();
		
		while( aIterator.hasNext() )
		{
			KCRecord record = aIterator.next();
			
			//---日付範囲で絞り込み
			if ( aRange != null )
				if ( !aRange.isInRange( record.date() ) )
					continue;
			
			//---カテゴリで絞り込み
			if ( aPath != null )
				if ( !record.categoryPath().isBelongTo( aPath ) )
					continue;
			
			array.addObject( record );
		}
		
		return array;
		
	}

	/**
	* 装飾を施した金額文字列を得る
	* @return 装飾文字列
	* @param aMoney 金額
	*/
	static public NSAttributedString
	makeAttributedStringForMoney(
		KCMoney aMoney
		)
	{
		Double dVal = new Double( aMoney.absString() );
		String priceString =  Long.toString( dVal.longValue() );
		NSMutableDictionary tmpDic = new NSMutableDictionary();
		if ( aMoney.isOutput() )
		{//---支出
			return new NSAttributedString( priceString  , (NSDictionary)tmpDic );
		}
		else
		{//---収入
			tmpDic.setObjectForKey( NSColor.blueColor() , NSAttributedString.ForegroundColorAttributeName );
			return new NSAttributedString( priceString , (NSDictionary)tmpDic );
		}
	}
	
	/**
	* 曜日をローカライズする
	* @return ローカライズされた曜日
	* @param aYoubi 曜日
	*/
	static public String
	localizedDay(
		int aYoubi
		)
	{
		switch( aYoubi )
		{
			case 0:
				return NSBundle.localizedString( "Key_sunday" );
			case 1:
				return NSBundle.localizedString( "Key_monday" );
			case 2:
				return NSBundle.localizedString( "Key_tuesday" );
			case 3:
				return NSBundle.localizedString( "Key_wednesday" );
			case 4:
				return NSBundle.localizedString( "Key_thursday" );
			case 5:
				return NSBundle.localizedString( "Key_friday" );
			case 6:
				return NSBundle.localizedString( "Key_saturday" );
			default:
				return "";
		}
	}
	
	/**
	* レコードのチップ文字列を取得する
	* @return チップ文字列
	* @param aRecord 作成元のレコード
	*/
	static public String
	tipForRecord(
		KCRecord aRecord
		)
	{
		String str = "";
		String kakko_start = NSBundle.localizedString( "Tip_Bracket_Start"  );
		String kakko_end = NSBundle.localizedString( "Tip_Bracket_End"  );
		
		//---口座
		str += kakko_start + NSBundle.localizedString( "Record_Account" ) + kakko_end;
		if ( aRecord.account() != null )
			str += aRecord.account().name().string();
		str += "\n";		
		
		//---品目
		str += kakko_start + NSBundle.localizedString( "Record_Name" ) + kakko_end;
		if ( aRecord.name() != null )
			str += aRecord.name().string();
		str += "\n";

		//---金額
		str += kakko_start + NSBundle.localizedString( "Record_Price" ) + kakko_end;
		if ( aRecord.money() != null )
			str += aRecord.money().string();
		str += "\n";

		//---日付
		str += kakko_start + NSBundle.localizedString( "Record_Date" ) + kakko_end;
		if ( aRecord.date() != null )
			str += aRecord.date().string();
		str += "\n";
		
		//---カテゴリ
		str += kakko_start + NSBundle.localizedString( "Record_Category" ) + kakko_end;
		if ( aRecord.categoryPath() != null )
			str += aRecord.categoryPath().string();
		str += "\n";

		//---コメント
		str += kakko_start + NSBundle.localizedString( "Record_Comment" ) + kakko_end;
		if ( aRecord.comment() != null )
			str += aRecord.comment();
				
		return str;
	}
	
}
