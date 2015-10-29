//----------------------------------------------------
//チュートリアルオブジェクト
//----------------------------------------------------

//名前空間//------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス//--------------------------------------------
public class TutorialChild : MonoBehaviour {

	//変数//------------------------------------------
	private	Image	image;
	
	//初期化//----------------------------------------
	void Start () {
		image			= GetComponent<Image>();
		Color	color	= image.color;
		color.a			= 0.0f;
		image.color		= color;
	}
	
	//更新//------------------------------------------
	void Update () {
		
	}

}
