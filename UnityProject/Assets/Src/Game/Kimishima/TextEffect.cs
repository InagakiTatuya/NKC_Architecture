//-------------------------------------------------
//文字のエフェクト
//-------------------------------------------------

//名前空間//---------------------------------------
using UnityEngine;
using System.Collections;

//クラス//-----------------------------------------
public class TextEffect : MonoBehaviour {

	//変数//---------------------------------------
	public	GameObject	cameraObject;
	public	Vector3		pos		= new Vector3(0.0f,0.0f,100.0f);
	private	Vector3		posBuf;

	//初期化//-------------------------------------
	void Start () {
		posBuf	= pos;
	}
	
	//更新//---------------------------------------
	void Update () {
		posBuf	= pos + new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f));
		transform.localPosition	= posBuf;
	}
}
