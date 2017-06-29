package com.akipen.kclib;
/**<pre>
* カテゴリーのパスを表すクラス．
* 例： (ROOT)\\食費\\お菓子\\
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/07 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCCategoryPath extends KCName
{	
	//*コンストラクタ*************************
	
	/**
	* コピーしたパスを定義する
	* @param aPath コピー元
	*/
	public
	KCCategoryPath(
		KCCategoryPath aPath
		)
	{
		this( aPath.string() );
	}
	
	/**
	* パスを定義する
	* @param aPath 設定するパス
	*/
	public
	KCCategoryPath(
		String aPath
		)
	{
		super( aPath );
		
		if ( aPath.lastIndexOf( KCCategory.SEPARATOR ) != aPath.length()-KCCategory.SEPARATOR.length() 
			)
		{
			KCWarring.log( new String( "CategoryPath is not formated in path [" + aPath + "]" ) );			
		}
	}

	//*メソッド**********************************
	
	/**
	* 一番はじめののカテゴリの名前を取得する
	* @return カテゴリの名前．なければnull
	*/
	public KCCategoryName
	rootCategory()
	{
		String path = string();
		if ( path.equals( KCCategory.SEPARATOR ) )
			return null;
		
		int nextPathEndIndex = path.indexOf( KCCategory.SEPARATOR , KCCategory.SEPARATOR.length() );
		return new KCCategoryName( path.substring( KCCategory.SEPARATOR.length() , nextPathEndIndex ) );		
	}
	
	/**
	* 一番最後ののカテゴリの名前を取得する
	* @return カテゴリの名前．なければnull
	*/
	public KCCategoryName
	leafCategory()
	{
		String path = string();
		if ( path.equals( KCCategory.SEPARATOR ) )
			return null;
		
		int lastPathStartIndex = path.lastIndexOf( KCCategory.SEPARATOR , path.length() - KCCategory.SEPARATOR.length() -1 );
		return new KCCategoryName( path.substring( lastPathStartIndex+KCCategory.SEPARATOR.length() , path.length() - KCCategory.SEPARATOR.length() ) );		
	}
	
	/**
	* カテゴリパスに所属しているか得る
	* @return このクラスが，引数のパス以下のクラスであればtrue．
	* @param aPath 所属しているか判定するパス．
	*/
	public boolean
	isBelongTo(
		KCCategoryPath aPath
		)
	{
		return ( string().indexOf( aPath.string() ) == 0 );
	}
	
}
