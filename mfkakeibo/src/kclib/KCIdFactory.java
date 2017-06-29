package com.akipen.kclib;
/**<pre>
* IDを生成するクラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/05 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCIdFactory extends KCObject
{
	//*プロパティ*****************************
	private long	myId;		//---64bit型
	
	//*コンストラクタ**************************
	
	/**
	* 作成する
	*/
	public
	KCIdFactory()
	{
		myId = 0;
	}
	
	//*メソッド*******************************
	
	/**
	* 新しいIDを取得する
	* @return 新しいID
	*/
	public KCId
	getId()
	{
		return ( new KCIdImpl( myId++ ) );
	}
	
	/**
	* KCIdの実装クラス
	*/
	static private class KCIdImpl extends KCId
	{
		//*プロパティ*****************************
		private long myId;
		
		//*コンストラクタ**************************
		
		/**
		* IDを作成する
		* @param aId 設定するID
		*/
		public 
		KCIdImpl(
			long aId
			)
		{
			myId = aId;
		}
		
		//*メソッド**********************************
		
		/**
		* IDを数値型で得る
		* @return 数値型のID
		*/
		public long
		value()
		{
			return myId;
		}		
		
	}
	
}
