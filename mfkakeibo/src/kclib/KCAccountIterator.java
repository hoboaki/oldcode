package com.akipen.kclib;
/**<pre>
* 口座データベースのイテレータ抽象クラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2006/01/05 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
abstract public class KCAccountIterator extends KCObject
{
	/**
	* イテレータが終端にいるかどうか返す
	* @return 終端でないならtrue
	*/
	abstract public boolean
	hasNext();

	/**
	* 現在示しているカテゴリを返して，口座を次に進める
	* @return 次に進める前に示しているオブジェクト
	*/
	abstract public KCAccount
	next();
}
