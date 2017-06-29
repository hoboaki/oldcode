//
//  Common.java
//  Kakeibo
//
//  Created by あっきー on 05/12/04.
//  Copyright 2005 __MyCompanyName__. All rights reserved.
//

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class Common 
{
	static public final String ApplicationName = NSBundle.localizedString("Common_AppName");
	 
	/**
	* 出金のカテゴリを得る
	* @return カテゴリ
	*/
	static public KCMutableCategory
	categoryOutput()
	{
		KCMutableCategory category = new KCMutableCategory();
		category.setName( new KCCategoryName( nameOfCategoryOutput() ) );
		return category;
	}
	 
	/**
	* 入金のカテゴリを得る
	* @return カテゴリ
	*/
	static public KCMutableCategory
	categoryInput()
	{
		KCMutableCategory category = new KCMutableCategory();
		category.setName( new KCCategoryName( nameOfCategoryInput() ) );
		return category;
	}
	
	/**
	* 繰越金のカテゴリパスを得る
	* @return カテゴリパス
	*/
	static public KCCategoryPath
	categoryPathOfCarryMoney()
	{
		return new KCCategoryPath( KCCategory.SEPARATOR + nameOfCarryMoney() + KCCategory.SEPARATOR );
	}

	/**
	* 小計のローカライズネームを取得する
	* @return "小計"の名前
	*/
	static public String
	nameOfSubTotal()
	{
		return NSBundle.localizedString("NameOfSubTotal");
	}
	
	/**
	* 繰越金のローカライズネームを取得する
	* @return "繰越金"の名前
	*/
	static public String
	nameOfCarryMoney()
	{
		return NSBundle.localizedString("NameOfCarryMoney");
	}

	/**
	* 残金のローカライズネームを取得する
	* @return "残金"の名前
	*/
	static public String
	nameOfRestMoney()
	{
		return NSBundle.localizedString("NameOfRestMoney");
	}
	
	/**
	* 出金のローカライズネームを取得する
	* @return "出金"の名前
	*/
	static public String
	nameOfCategoryOutput()
	{
		return NSBundle.localizedString("Category_Output");
	}
	
	/**
	* 入金のローカライズネームを取得する
	* @return "入金"の名前
	*/
	static public String
	nameOfCategoryInput()
	{
		return NSBundle.localizedString("Category_Input");
	}
	
	/**
	* 収入のローカライズネームを取得する
	* @return "収入"の名前
	*/
	static public String
	nameOfCategoryPureInput()
	{
		return NSBundle.localizedString("Category_PureInput");
	}
	
	/**
	* 支出のローカライズネームを取得する
	* @return "支出"の名前
	*/
	static public String
	nameOfCategoryPureOutput()
	{
		return NSBundle.localizedString("Category_PureOutput");
	}
	
	/**
	* 預貯金引出のローカライズネームを取得する
	* @return "預貯金引出"の名前
	*/
	static public String
	nameOfCategoryMoveInput()
	{
		return NSBundle.localizedString("Category_MoveInput");
	}
	
	/**
	* 預貯金預入のローカライズネームを取得する
	* @return "預貯金預入"の名前
	*/
	static public String
	nameOfCategoryMoveOutput()
	{
		return NSBundle.localizedString("Category_MoveOutput");
	}
	
}
