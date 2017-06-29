package com.akipen.kclib;
/**<pre>
* タイムゾーンを表すのクラス．
* デフォルトのタイムゾーンは初期の段階で±0000になっているので，
* 変更したいのであれば，setDefaultTimeZoneで，デフォルトのタイムゾーンを設定してください．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCTimeZone
{
	//*クラスのプロパティ**********************
	static private KCTimeZone myDefaultTimeZone = standardTimeZone();
	private int		myHour;
	private int		myMinute;
	private boolean	myPositive;
	
	//*グローバル関数**************************
	
	/**
	* 符号HHMMの文字列を作成する
	* @return 作成された文字列
	* @param aPositive ＋ならtrue
	* @param aHour 時間（0~23）
	* @param aMinute 分（0~59）
	*/
	static public String
	makeGMTString(
		boolean aPositive,
		int aHour,
		int aMinute
		)
	{
		String str = "";
		if ( aPositive )
			str += "+";
		else
			str += "-";
		
		{
			String tmpStr = Integer.toString( aHour );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;
			str += tmpStr;
		}
		{
			String tmpStr = Integer.toString( aMinute );
			while ( tmpStr.length() < 2 )
				tmpStr = "0" + tmpStr;
			str += tmpStr;
		}
		
		return new String( str );
	}
	
	/**
	* GMT±0000のタイムゾーンを作成する
	* @return ±0000タイムゾーン
	*/
	static public KCTimeZone
	standardTimeZone()
	{
		return new KCTimeZone( true , 0 , 0);
	}
	
	/**
	* デフォルトのタイムゾーンを作成する
	* @return デフォルトのタイムゾーン
	*/
	static public KCTimeZone
	defaultTimeZone()
	{
		return new KCTimeZone( myDefaultTimeZone );
	}
	
	/**
	* デフォルトのタイムゾーンを設定する
	* @param aTimeZone 設定するデフォルトのタイムゾーン
	*/
	static public void
	setDefaultTimeZone(
		KCTimeZone aTimeZone
		)
	{
		myDefaultTimeZone = aTimeZone;
	}
		
	//*コンストラクタ**************************
		
	/**
	* 指定されたタイムゾーンを作成する
	* @param aPositive ＋ならtrue
	* @param aHour 時間（0~23）
	* @param aMinute 分（0~59）
	*/
	public
	KCTimeZone(
		boolean aPositive,
		int aHour,
		int aMinute
		)
	{
		myHour = aHour;
		myMinute = aMinute;
		myPositive = aPositive;
	}
	
	/**
	* GMT表記の文字列からタイムゾーンを作成する
	* @param aString GMT表記の文字列
	*/
	public
	KCTimeZone(
		String aString
		)
	{
		String tmpStr =  aString.substring( 0,1 );
		if ( tmpStr.equals( new String( "+" ) ) )
			myPositive = true;
		else
			myPositive = false;
			
		myHour = Integer.parseInt( aString.substring(1,3) );
		myMinute = Integer.parseInt( aString.substring(3,5) );
	}
	
	/**
	* コピーしたタイムゾーンを作成する
	* @param aTimeZone コピー元のタイムゾーン
	*/
	public 
	KCTimeZone(
		KCTimeZone aTimeZone
		)
	{
		this( aTimeZone.isPositive() , aTimeZone.hour() , aTimeZone.minute() );
	}
	
	//*プロパティアクセス***************************
	
	/**
	* 時間のずれを返す
	* @return 時間数
	*/
	public int
	hour()
	{
		return myHour;
	}
	
	/**
	* 分のずれを返す
	* @return 分数
	*/
	public int
	minute()
	{
		return myMinute;
	}
	
	/**
	* プラスマイナスかを返す
	* @return trueなら＋
	*/
	public boolean
	isPositive()
	{
		return myPositive;
	}
	
	/**
	* GMT表記の文字列を返す
	* @return GMT表記の文字列
	*/
	public String
	string()
	{
		return makeGMTString( myPositive , myHour , myMinute );
	}
	
	/**
	* 等価か。
	* @return 比較結果
	* @param aTimeZone 比較対象
	*/
	public boolean
	equals(
		KCTimeZone aTimeZone
		)
	{
		return hour() == aTimeZone.hour()
			&& minute() == aTimeZone.minute()
			&& isPositive() == aTimeZone.isPositive();
	}
	
}
