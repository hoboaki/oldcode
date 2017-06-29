package com.akipen.kclib;
/**<pre>
* カテゴリーを生成するクラスの抽象クラス
*
* <変更履歴>
* 2006/01/29 1.5作成
* 2006/01/07 とりあえず完成
* 2005/12/12 作成開始
*
* </pre>
* @author 秋野ペンギン
* @version 1.5
*/
public class KCCategoryFactory extends KCObject
{
	/**
	* カテゴリを作成する．
	* @return 作成したカテゴリ，失敗したらnullを返す．
	* @param aBook 所属する本
	* @param aId 設定する口座ID
	* @param aCategory 情報を加えるカテゴリ
	*/
	/*abstract public KCMutableCategory
	createCategory(
		KCBook aBook ,
		KCId aId ,
		KCCategory aCategory
		);*/

	/**
	 * DOMノードから本を作成する
	 * @return 作成したカテゴリ，失敗したらnullを返す．
	 * @param aNode DOMノード
	 */
	public KCMutableCategory
	createFromDOMNode(
		KCDOMNode aNode
		)
	{
		if ( !aNode.name().equals( KCDOMObject.DOM_CATEGORY ) )
		{//---ノード名がCATEGORYでなければ失敗
			return null;
		}

		KCDOMNode[] childs = aNode.childs().nodes();
		KCMutableCategory category = new KCMutableCategory();
		for ( int i = 0; i < childs.length; ++i )
		{
			KCDOMNode node = childs[i];
			KCDOMNodeName name = node.name();

			if ( name.equals( KCDOMObject.DOM_NAME ) )
			{//---カテゴリ名
				category.setName( new KCCategoryName( node.value() ) );
			}
			else if ( name.equals( KCDOMObject.DOM_COMMENT ) )
			{//---コメント
				category.setComment( node.value() );
			}
			else if ( name.equals( KCDOMObject.DOM_CATEGORY ) )
			{//---カテゴリ
				category.addChild( createFromDOMNode( node ) );
			}

		}
		return category;		
	}
	
}
