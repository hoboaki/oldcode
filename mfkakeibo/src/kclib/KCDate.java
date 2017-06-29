package com.akipen.kclib;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.TimeZone;
/**<pre>
* 日付を表すクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCDate extends KCObject implements Comparable
{
	//*クラスのプロパティ**********************
	private GregorianCalendar myDate = null;
	private KCTimeZone myTimeZone = null;

	/**
	* JAVA形式の月を整数型の月（１〜12）に変換する
	* @return 月（1〜12）
	* @param JAVA形式の月
	*/
	static private int
	javaMonthToInteger(
		int aJavaMonth
		)
	{
		switch( aJavaMonth )
		{
			case Calendar.JANUARY:
				return 1;
			case Calendar.FEBRUARY:
				return 2;
			case Calendar.MARCH:
				return 3;
			case Calendar.APRIL:
				return 4;
			case Calendar.MAY:
				return 5;
			case Calendar.JUNE:
				return 6;
			case Calendar.JULY:
				return 7;
			case Calendar.AUGUST:
				return 8;
			case Calendar.SEPTEMBER:
				return 9;
			case Calendar.OCTOBER:
				return 10;
			case Calendar.NOVEMBER:
				return 11;
			default: // DECEMBER
				return 12;
		}	
	}

	/**
	* 整数型の月（１〜12）をJAVA形式の月に変換する
	* @return JAVA形式の月（0〜11）
	* @param 整数型の月
	*/
	static private int
	integerMonthToJAVA(
		int aIntegerMonth
		)
	{
		return aIntegerMonth-1;
	}
		
	//*コンストラクタ*************************
	
	/**
	* デフォルトタイムゾーンで現在時刻の日付を作成する
	*/
	public 
	KCDate()
	{	
		myDate = new GregorianCalendar();
		myTimeZone = KCTimeZone.defaultTimeZone();
	}
	
	/**
	* コピーした日付を作成する
	*/
	public
	KCDate(
		KCDate aDate
		)
	{
		this( aDate.yearOfCommonEra(),
				aDate.monthOfYear(),
				aDate.dayOfMonth(),
				aDate.hourOfDay(),
				aDate.minuteOfHour(),
				aDate.secondOfMinute(),
				aDate.timeZone()
				);
	}
	
	/**
	* デフォルトタイムゾーンで指定の日付を作成する
	* @param aYear 年
	* @param aMonth 月
	* @param aDay 日
	*/
	public 
	KCDate(
		int aYear,
		int aMonth,
		int aDay
		)
	{
		myDate = new GregorianCalendar( aYear, integerMonthToJAVA(aMonth) , aDay );
		myTimeZone = KCTimeZone.defaultTimeZone();
	}
	
	/**
	* デフォルトタイムゾーンで指定の日付を作成する
	* @param aYear 年
	* @param aMonth 月
	* @param aDay 日
	* @param aHour 時間
	* @param aMinute 分
	* @param aSecond 秒
	*/	
	public
	KCDate(
		int aYear,
		int aMonth,
		int aDay,
		int aHour,
		int aMinute,
		int aSecond
		)
	{
		myDate = new GregorianCalendar( aYear , integerMonthToJAVA(aMonth) , aDay , aHour , aMinute , aSecond );
		myTimeZone = KCTimeZone.defaultTimeZone();
	}
	
	/**
	* 指定したタイムゾーンで指定の日付を作成する
	* @param aYear 年
	* @param aMonth 月
	* @param aDay 日
	* @param aHour 時間
	* @param aMinute 分
	* @param aSecond 秒
	* @param aTimeZone タイムゾーン
	*/	
	public
	KCDate(
		int aYear,
		int aMonth,
		int aDay,
		int aHour,
		int aMinute,
		int aSecond,
		KCTimeZone aTimeZone
		)
	{
		String tmpTimeZoneStr = "GMT" + aTimeZone.string();
		
		TimeZone tmpTimeZone = TimeZone.getTimeZone( tmpTimeZoneStr );
		myDate = new GregorianCalendar( tmpTimeZone );
		myDate.set( aYear , integerMonthToJAVA( aMonth ) , aDay , aHour , aMinute , aSecond );
		myTimeZone = aTimeZone;
	}
	
	/**
	* 文字列から日付を作成する
	* @param aString 日付書式（YYYY-MM-DD HH:MM:SS ±HHMM）の文字列
	*/
	public 
	KCDate(
		String aString
		)
	{		
		String tmpStr = aString;
		int year = ( new Integer( tmpStr.substring(0,4) ) ).intValue();
		int month = ( new Integer( tmpStr.substring(5,7) ) ).intValue();
		int day = ( new Integer( tmpStr.substring(8,10) ) ).intValue();
		int hour  = ( new Integer( tmpStr.substring(11,13) ) ).intValue();
		int min  = ( new Integer( tmpStr.substring(14,16) ) ).intValue();
		int sec  = ( new Integer( tmpStr.substring(17,19) ) ).intValue();
		myTimeZone = new KCTimeZone( aString.substring( 20 , 25 ) );
		
		String tmpTimeZoneStr = "GMT" +myTimeZone.string();
		
		TimeZone tmpTimeZone = TimeZone.getTimeZone( tmpTimeZoneStr );
		myDate = new GregorianCalendar( tmpTimeZone );	
		myDate.set( year ,  integerMonthToJAVA( month )  , day , hour , min , sec );	
	}
	
	//*メソッド***********************************
	
	/**
	* 加算演算した日付を返す
	* @return 加算演算結果
	* @param aYear 加算する年数
	* @param aMonth 加算する月数
	* @param aDay 加算する日数
	* @param aHour 加算する時間数
	* @param aMinute 加算する分数
	* @param aSecond 加算する秒数
	*/
	public KCDate
	add(
		int aYear,
		int aMonth,
		int aDay,
		int aHour,
		int aMinute,
		int aSecond
		)
	{
		String tmpTimeZoneStr = "GMT" + timeZone().string();		
		TimeZone tmpTimeZone = TimeZone.getTimeZone( tmpTimeZoneStr );
		Calendar tmpDate = new GregorianCalendar( tmpTimeZone );
		tmpDate.set( yearOfCommonEra() , integerMonthToJAVA( monthOfYear() ) , dayOfMonth() ,
					hourOfDay() , minuteOfHour() , secondOfMinute() );
								
		if ( aSecond != 0 ) tmpDate.add( Calendar.SECOND , aSecond );
		if ( aMinute != 0 ) tmpDate.add( Calendar.MINUTE , aMinute );
		if ( aHour != 0 ) tmpDate.add( Calendar.HOUR_OF_DAY , aHour );
		if ( aDay != 0 ) tmpDate.add( Calendar.DAY_OF_MONTH , aDay );
		if ( aMonth != 0 ) tmpDate.add( Calendar.MONTH , aMonth );
		if ( aYear != 0 ) tmpDate.add( Calendar.YEAR , aYear );
		
		return new KCDate( tmpDate.get( Calendar.YEAR ) ,
						javaMonthToInteger( tmpDate.get( Calendar.MONTH ) ),
						tmpDate.get( Calendar.DAY_OF_MONTH ),
						tmpDate.get( Calendar.HOUR_OF_DAY ),
						tmpDate.get( Calendar.MINUTE ),
						tmpDate.get( Calendar.SECOND ),
						timeZone()
						);						
	}
	
	/**
	* 比較結果を返す
	* @return 比較結果
	* @param aDate 比較対象
	*/
	public int
	compareTo(
		KCDate aDate 
		)
	{
		if ( myDate.before( aDate.calendar() ) )
		{
			return -1;
		}
		else if ( myDate.after( aDate.calendar() ) )
		{
			return 1;
		}
		return 0;
	}
	
	/**
	* 等価判定を返す
	* @return 同じ日時ならtrue
	* @param aDate 比較対象
	*/
	public boolean
	equals(
		KCDate aDate
		)
	{
		return compareTo( aDate ) == 0 ? true : false;
	}

	/**
	* 年，月，日の等価判定を返す（タイムゾーン関係なし）
	* @return 同じ日時ならtrue
	* @param aDate 比較対象
	*/
	public boolean
	equalsByDay(
		KCDate aDate
		)
	{
		return ( yearOfCommonEra() == aDate.yearOfCommonEra() )
			&& ( monthOfYear() == aDate.monthOfYear() )
			&& ( dayOfMonth() == aDate.dayOfMonth() );
	}

	/**
	* 年，月，日の比較を返す（タイムゾーン関係なし）
	* @return 同じ日時なら0
	* @param aDate 比較対象
	*/
	public int
	compareToDateByDay(
		KCDate aDate
		)
	{
		int year = yearOfCommonEra() - aDate.yearOfCommonEra();
		if ( year != 0 )
			return year;
		int month = monthOfYear() - aDate.monthOfYear();
		if ( month != 0 )
			return month;
		return dayOfMonth() - aDate.dayOfMonth();
	}
	
	/**
	* タイムゾーンを標準時にした日付を返す
	* @return タイムゾーン±0000の日付
	*/
	public KCDate
	standardDate()
	{
		String tmpTimeZoneStr = "GMT+0000";		
		TimeZone tmpTimeZone = TimeZone.getTimeZone( tmpTimeZoneStr );
		Calendar tmpDate = new GregorianCalendar( tmpTimeZone );
		tmpDate.set( yearOfCommonEra() , integerMonthToJAVA( monthOfYear() ) , dayOfMonth() ,
					hourOfDay() , minuteOfHour() , secondOfMinute() );
					
		KCTimeZone tzNow = timeZone();
		{
			int minute = tzNow.minute();
			if ( minute != 0 )
			{
				if ( !tzNow.isPositive() )
					minute *= -1;
				tmpDate.add( Calendar.MINUTE , minute );
			}
		}
		{
			int hour = tzNow.hour();
			if ( hour != 0 )
			{
				if ( !tzNow.isPositive() )
					hour *= -1;
				tmpDate.add( Calendar.HOUR , hour );
			}
		}
		
		return new KCDate( tmpDate.get( Calendar.YEAR ) ,
						javaMonthToInteger( tmpDate.get( Calendar.MONTH ) ),
						tmpDate.get( Calendar.DAY_OF_MONTH ),
						tmpDate.get( Calendar.HOUR_OF_DAY ),
						tmpDate.get( Calendar.MINUTE ),
						tmpDate.get( Calendar.SECOND ),
						KCTimeZone.standardTimeZone()
						);
	}
	
	//*プロパティアクセス***************************

	/**
	* 西暦年数を得る
	* @return 西暦年数
	*/
	public int
	yearOfCommonEra()
	{
		return myDate.get( Calendar.YEAR );
	}
	
	/**
	* 年単位の月数を得る
	* @return 月数（1~12）
	*/
	public int
	monthOfYear()
	{
		return javaMonthToInteger( myDate.get( Calendar.MONTH ) );
	}
	

	
	/**
	* 年単位の日数を得る
	* @return 日数（1〜366）
	*/
	public int
	dayOfYear()
	{
		return myDate.get( Calendar.DAY_OF_YEAR );
	}
	
	/**
	* 月単位の日数を得る
	* @return 日数（1〜31）
	*/
	public int
	dayOfMonth()
	{
		return myDate.get( Calendar.DAY_OF_MONTH );
	}
	
	/**
	* 週単位の日数を得る
	* @return 日数（0〜6）
	*/
	public int
	dayOfWeek()
	{
		switch( myDate.get( Calendar.DAY_OF_WEEK ) )
		{
			case Calendar.SUNDAY:
				return 0;
			case Calendar.MONDAY:
				return 1;
			case Calendar.TUESDAY:
				return 2;
			case Calendar.WEDNESDAY:
				return 3;
			case Calendar.THURSDAY:
				return 4;
			case Calendar.FRIDAY:
				return 5;
			default: //---SATURDAY
				return 6;
		}
	}
	
	/**
	* 日単位の時間を得る
	* @return 時間（0~23）
	*/
	public int
	hourOfDay()
	{
		return myDate.get( Calendar.HOUR_OF_DAY ); 
	}
	
	/**
	* 時間単位の分を得る
	* @return 分（0~59)
	*/
	public int
	minuteOfHour()
	{
		return myDate.get( Calendar.MINUTE );
	}
	
	/**
	* 日単位の秒を得る
	* @return 秒（0〜86399)
	*/
	public int
	secondOfDay()
	{
		return ( hourOfDay() * 3600 + minuteOfHour() * 60 + secondOfMinute() );
	}
	
	/**
	* 分単位の秒を得る
	* @return 秒（0〜59）
	*/
	public int
	secondOfMinute()
	{
		return myDate.get( Calendar.SECOND );
	}
	
	/**
	* タイムゾーンを得る
	* @return タイムゾーン
	*/
	public KCTimeZone
	timeZone()
	{
		return myTimeZone;
	}
	
	/**
	* GregorianCalendarを取得する。
	* @return GregorianCalendar
	*/
	public GregorianCalendar
	calendar()
	{
		return myDate;
	}
	
	/**
	* 文字列で日付を返す
	* @return "YYYY-MM-DD HH:MM:SS ±HHMM" 
	*/
	public String
	string()
	{
		String resStr = "";
		{
			String tmpStr = Integer.toString( yearOfCommonEra() );
			while ( tmpStr.length() < 4 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + "-";
		}
		{
			String tmpStr = Integer.toString( monthOfYear() );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + "-";
		}
		{
			String tmpStr = Integer.toString( dayOfMonth() );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + " ";
		}	
		{
			String tmpStr = Integer.toString( hourOfDay() );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + ":";
		}
		{
			String tmpStr = Integer.toString( minuteOfHour() );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + ":";
		}
		{
			String tmpStr = Integer.toString( secondOfMinute() );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;				
			resStr += tmpStr + " ";
		}
		resStr += timeZone().string();
		
		return new String( resStr );
	}
	
	//*インターフェイスの実装************************
	public int
	compareTo(
		Object o
		)
	{
		return compareTo( (KCDate)o );
	}
	
}
