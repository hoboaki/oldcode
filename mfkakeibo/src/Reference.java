/**<pre>
* 参照を保持するクラス
*
*</pre>
*/

import com.apple.cocoa.foundation.*;
import com.apple.cocoa.application.*;

import com.akipen.kclib.*;

public class Reference 
{
	//*プロパティ*****************************
	private Object myReal;
	
	//*コンストラクタ*************************

	/**
	* 参照を作成する
	* @param aReal 参照先
	*/
	public Reference(
		Object aReal
		)
	{
		myReal = aReal;
	}
	
	
	//*メソッド**************************	

	/**
	* クローンを作成する
	*/
	public Object
	clone()
	{
		return new Reference( real() );
	}
	

	/**
	* 参照先を返す
	* @return 実態
	*/
	public Object
	real()
	{
		return myReal;
	}

}
