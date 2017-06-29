package com.akipen.kclib;
import java.lang.*;
/**<pre>
* 金額を表すクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/11 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCMoney extends KCObject implements Comparable
{
	//*クラスのプロパティ**********************
	private double	myPrice = 0;

	//*コンストラクタ*************************
	/**
	* 0の金額を定義する
	*/
	public KCMoney()
	{
	}
	
	/**
	* aMoneyの金額を定義する
	* @param aMoney 設定する金額
	*/
	public KCMoney(
		double aMoney
		)
	{
		myPrice = aMoney;
	}
	
	/**
	* 文字列から金額を定義する
	* @param aString 金額書式の文字列
	*/
	public KCMoney(
		String aString
		)
	{
		Double tmp = new Double( aString );
		myPrice = tmp.doubleValue();
	}
	
	/**
	* aMoneyのコピーを定義する
	* @param aMoney 設定する金額
	*/
	public KCMoney(
		KCMoney aMoney
		)
	{
		myPrice = aMoney.price();
	}
	
	//*プロパティアクセス************************
	/**
	* 金額を符号付きで得る
	* @return 金額
	*/
	public double
	price()
	{
		return myPrice;
	}
	
	/**
	* 金額を絶対値で得る
	* @return 金額
	*/
	public double
	absPrice()
	{
		return ( price() >= 0.0 ? price() : -1.0 * price()  );
	}
	
	/**
	* この金額は支出金なのかを返す
	* @return 金額が0以下ならtrue
	*/
	public boolean
	isOutput()
	{
		return ( price() <= 0.0 );
	}
	
	/**
	* 金額を文字列で返す．負の時はマイナス付き
	* @return 文字列
	*/
	public String
	string()
	{
		return new String( Double.toString( price() ) );
	
		/*String retString = new String("");
		String tmpString = new String( Long.toString( price() ) );
		while( tmpString.length() > 3 )
		{
			retString = retString.concat( new String(",") );
			retString = retString.concat( tmpString.substring( tmpString.length()-3 ) );
			tmpString = tmpString.substring( 0 , tmpString.length()-3 );
		}
		retString = tmpString.concat( retString );*/
		//return retString;
	}
	
	/**
	* 絶対値金額を文字列で返す．
	* @return 文字列
	*/
	public String
	absString()
	{
		String tmpString = string();
		if ( tmpString.indexOf( new String("-") ) == 0 )
			return tmpString.substring( 1 );
		return tmpString;
	}
	
	//*インターフェース実装************************
	/**
	* オブジェクトとの比較結果を返す
	* @return 比較結果
	* @param o 比較対象
	*/
	public int
	compareTo( 
		Object o
		)
	{
		try
		{
			KCMoney aObject = (KCMoney)o;
			return compareTo(aObject);			
		}catch( ClassCastException err )
		{	
			throw( err );
		}
					
	}
	
	/**
	* KCMoneyオブジェクトとの比較結果を返す
	* @return 比較結果
	* @param aTarget 比較対象
	*/
	public int
	compareTo( 
		KCMoney aTarget
		)
	{
		if ( price() < aTarget.price() )
			return -1;
		else if ( price() > aTarget.price() )
			return 1;
		else
			return 0;
	}
	
	
}
