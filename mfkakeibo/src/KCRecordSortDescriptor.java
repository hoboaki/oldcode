//
//  KCRecordSortDiscriptor.java
//  Kakeibo
//
//  Created by あっきー on 06/01/09.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class KCRecordSortDescriptor extends NSSortDescriptor
{

	//***コンストラクタ*****************************
	/**
	* 作成する
	* @param aKey キー
	* @param aAscending 昇順ならtrue
	*/
	public KCRecordSortDescriptor(
		String aKey,
		boolean aAscending
		)
	{
		super( aKey, aAscending );
	}
	
	

	//***メソッド***********************************
	
	public int
	compareObjects(
		Object object1, 
		Object object2
		)
	{
		KCRecord data1 = (KCRecord)object1;
		KCRecord data2 = (KCRecord)object2;
		
		String key = key();
		int result = 0;
		if ( key.equals( "date" ) )
		{
			result = data1.date().compareTo( data2.date() );
		}
		else if ( key.equals( "name" ) )
		{
			result = data1.name().string().compareTo( data2.name().string() );
		}
		else if ( key.equals( "category" ) )
		{
			result = data1.categoryPath().leafCategory().string().compareTo( data2.categoryPath().leafCategory().string() );
		}
		else if ( key.equals( "price" ) )
		{
			result = data1.money().compareTo( data2.money() );
		}
		
		return ascending() ? result : -1 * result;
	}
	
	public Object 
	reversedSortDescriptor()
	{
		return new KCRecordSortDescriptor( key() , !ascending() );
	}
	
}
