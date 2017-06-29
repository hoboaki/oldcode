//
//  KCStdBookFactory.java
//  Kakeibo
//
//  Created by あっきー on 06/01/29.
//  Copyright 2006 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class KCStdBookFactory extends KCBookFactory
{
	/**
	* 本を返す。
	* @return 本
	*/
	public KCMutableBook
	getBook()
	{
		return new KCStdBook();
	}
}
