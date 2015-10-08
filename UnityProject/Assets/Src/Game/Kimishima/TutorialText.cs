//----------------------------------------------------
//チュートリアルオブジェクト
//----------------------------------------------------

//名前空間//------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス//--------------------------------------------
public class TutorialText : MonoBehaviour {

	//変数//------------------------------------------
	private	Text	text;

	//初期化//----------------------------------------
	void Start () {
		text			= GetComponent<Text>();
		Color	color	= text.color;
		color.a			= 0.0f;
		text.color		= color;
	}
	
	//更新//------------------------------------------
	void Update () {
		
	}
}
