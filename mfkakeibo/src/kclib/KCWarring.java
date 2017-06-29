package com.akipen.kclib;
/**<pre>
* 警告を標準出力に出すクラス
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/15 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCWarring extends KCObject
{
	static public void
	log(
		String aMsg
		)
	{
		System.out.println( "Warring:" + aMsg +"\n" );
	}
}
