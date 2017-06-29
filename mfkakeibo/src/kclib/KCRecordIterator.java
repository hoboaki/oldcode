package com.akipen.kclib;
/**<pre>
* レコードデータベースのイテレータ抽象クラス．
*
* <変更履歴>
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
abstract public class KCRecordIterator extends KCObject
{
	/**
	* イテレータが終端にいるかどうか返す
	* @return 終端でないならtrue
	*/
	abstract public boolean
	hasNext();

	/**
	* 現在示しているカテゴリを返して，レコードを次に進める
	* @return 次に進める前に示しているオブジェクト
	*/
	abstract public KCRecord
	next();
}
