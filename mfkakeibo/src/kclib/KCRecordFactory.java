package com.akipen.kclib;
/**<pre>
* レコードを生成するクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2005/12/11 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.0
*/
public class KCRecordFactory extends KCObject 
{
	/**
	* 所属口座を設定し，レコードを作成する．
	* @return 作成したレコード，失敗したらnullを返す
	* @param aAccount 所属する口座
	* @param aRecord 元となるレコード
	*/
	public KCMutableRecord
	createRecord(
		KCAccount aAccount,
		KCRecord aRecord
		)
	{
		KCMutableRecord record = (KCMutableRecord)( (KCMutableRecord)aRecord ).clone();
		record.setAccount( aAccount );
		return record;
	}

	/**
	* レコードの情報を更新する．IDはコピーされない。
	* @return コピーされたレコード
	* @param aRecordSrc コピー元レコード
	* @param aRecordDest コピー先レコード
	*/
	public KCRecord
	copyRecord(
		KCRecord aRecordSrc ,
		KCMutableRecord aRecordDest
		)
	{
		aRecordDest.initWithRecord( aRecordSrc );
		return aRecordDest;
	}
	
	/**
	 * DOMノードからレコードを作成する
	 * @return 作成したレコード，失敗したらnullを返す．
	 * @param aNode DOMノード
	 */
	public KCMutableRecord
	createFromDOMNode(
		KCDOMNode aNode
		)
	{
		if ( !aNode.name().equals( KCDOMObject.DOM_RECORD ) )
		{//---ノード名がレコードでなければ失敗
			return null;
		}

		KCDOMNode[] childs = aNode.childs().nodes();
		if ( childs.length == 0 )
			return null;

		KCMutableRecord record = new KCMutableRecord();
		for ( int i = 0; i < childs.length; ++i )
		{
			KCDOMNode node = childs[i];
			KCDOMNodeName name = node.name();

			if ( name.equals( KCDOMObject.DOM_NAME ) )
			{//---品目名
				record.setName( new KCRecordName( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_PRICE ) )
			{//---金額
				record.setMoney( new KCMoney( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_DATE ) )
			{//---日付
				record.setDate(	new KCDate( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_CATEGORY ) )
			{//---カテゴリ
				record.setCategoryPath( new KCCategoryPath( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_COMMENT ) )
			{//---コメント
				record.setComment( node.value() );
			}

		}
		return record;

	}

}
