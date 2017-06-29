//
//  DateRange.java
//  Kakeibo
//
//  Created by あっきー on 06/02/07.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class DateRange 
{
	//*クラスのプロパティ**********************
	private KCDate myFrom;
	private KCDate myTo;
	
	//*コンストラクタ***********************

	/**
	* 日付範囲を作成する
	* @param aFrom 範囲の始まり
	* @param aTo 範囲の終わり
	*/
	public
	DateRange(
		KCDate aFrom,
		KCDate aTo
		)
	{
		if ( aFrom.compareTo( aTo ) > 0 )
		{//---swap
			myTo = aFrom;
			myFrom = aTo;
		}
		else
		{
			myFrom = aFrom;
			myTo = aTo;
		}
	}
	
	//*メソッド**************************
	
	/**
	* 指定の日付が範囲内かどうか得る
	* @return 範囲内ならtrue
	* @param aDate 指定の日付
	*/
	public boolean
	isInRange(
		KCDate aDate
		)
	{
		return ( myFrom.compareTo( aDate ) <= 0 && myTo.compareTo( aDate ) > 0 );
	}

	/**
	* 指定の日付が範囲内かどうか得る。ただし，タイムゾーン関係なし。日までの判定。
	* @return 範囲ないならtrue
	* @param aDate 指定の日付
	*/
	public boolean
	isInRangeByDay(
		KCDate aDate
		)
	{
		return ( myFrom.compareToDateByDay( aDate ) <= 0 && myTo.compareToDateByDay( aDate ) > 0 );
	}	

}
