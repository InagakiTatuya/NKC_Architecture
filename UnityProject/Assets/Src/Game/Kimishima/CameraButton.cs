//----------------------------------------------------------
//カメラの操作用ボタン
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
#define	DEBUG_GAMESCENE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
public class CameraButton : MonoBehaviour {
	
	//変数//////////////////////////////////////////////////
	public	CameraMove	cameraMove	= null;
	public	int			id;
	public	Sprite[]	sprite		= new Sprite[2];

	//初期化////////////////////////////////////////////////
	void Start () {//初期化
		
	}
	
	//更新//////////////////////////////////////////////////
	void Update () {//更新
	
	}
}
