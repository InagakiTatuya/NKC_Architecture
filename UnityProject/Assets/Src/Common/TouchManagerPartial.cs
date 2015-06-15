using UnityEngine;
using System.Collections;

public partial interface TouchManagerPartial
{
	// タッチした瞬間に呼ばれる。
	void TouchDownOnce();
	// タッチしたときに呼ばれる。
	void TouchDown();
	// スライドしたときに呼ばれる。
	void TouchMove();
	// タッチを離したときに呼ばれる。
	void TouchUp(bool b);
}
