/**<pre>
* 口座の参照を保持するクラス
*
*</pre>
*/

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class AccountReference extends Reference
{
	//*コンストラクタ*************************

	/**
	* 参照を作成する
	* @param aReal 参照先
	*/
	public AccountReference(
		KCMutableAccount aReal
		)
	{
		super( aReal );
	}
	
	
	//*メソッド**************************	

	/**
	* クローンを作成する
	*/
	public Object
	clone()
	{
		return new AccountReference( (KCMutableAccount)real() );
	}
	
	/**
	* 標準文字列を返す
	*/
	public String 
	toString()
	{
		return ( (KCMutableAccount)real() ).name().string();
	}
}
