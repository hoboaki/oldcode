package com.akipen.kclib;
/**<pre>
* 口座を生成するクラス．
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2006/01/05 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCAccountFactory extends KCObject 
{
	/**
	* 所属本を設定し，口座を作成する．
	* @return 作成した口座，失敗したらnullを返す
	* @param aBook 所属する本
	* @param aAccount 元となる口座
	*/
	public KCMutableAccount
	createAccount(
		KCBook aBook,
		KCAccount aAccount
		)
	{
		KCMutableAccount account = (KCMutableAccount)( (KCMutableAccount)aAccount ).clone();
		account.setBook( aBook );
		return account;
	}

	/**
	* 口座の情報を更新する．IDはコピーされない。
	* @return コピーされた口座
	* @param aAccountSrc コピー元口座
	* @param aAccountDest コピー先口座
	*/
	public KCAccount
	copyAccount(
		KCAccount aAccountSrc ,
		KCMutableAccount aAccountDest
		)
	{
		aAccountDest.initWithAccount( aAccountSrc );
		return aAccountDest;
	}

	/**
	 * DOMノードから口座を作成する
	 * @return 作成した口座，失敗したらnullを返す．
	 * @param aNode DOMノード
	 */
	public KCMutableAccount
	createFromDOMNode(
		KCDOMNode aNode
		)
	{
		if ( !aNode.name().equals( KCDOMObject.DOM_ACCOUNT ) )
		{//---ノード名がACCOUNTでなければ失敗
			return null;
		}

		KCDOMNode[] childs = aNode.childs().nodes();
		KCMutableAccount account = new KCMutableAccount();
		KCRecordFactory recordFactory = new KCRecordFactory();
		for ( int i = 0; i < childs.length; ++i )
		{
			KCDOMNode node = childs[i];
			KCDOMNodeName name = node.name();

			if ( name.equals( KCDOMObject.DOM_NAME ) )
			{//---口座名
				account.setName( new KCName( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_COMMENT ) )
			{//---コメント
				account.setComment( node.value() );
			}
			else if ( name.equals( KCDOMObject.DOM_RECORDS ) )
			{//---レコード
				KCDOMNode[] records = node.childs().nodes();
				for ( int j = 0; j < records.length; ++j )
				{
					KCMutableRecord record =  recordFactory.createFromDOMNode( records[j] );
					if ( account.addRecord( record ) == null )
					{
						KCWarring.log( "Can't add record" );
					}
				}
			}

		}
		return account;		
	}
	
}
