//------------------------------------------------------
//タイトルエフェクト
//------------------------------------------------------

//名前空間//--------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス//----------------------------------------------
public class TitleLogoEffect : MonoBehaviour {

	//変数//--------------------------------------------
	private	Image	image;
	private	Vector2	size;
	private	Vector2	sizeBuf;
	private	float	timer;

	//初期化//------------------------------------------
	void Start () {
		image	= GetComponent<Image>();
		size	= image.rectTransform.sizeDelta;
		sizeBuf	= size;
		timer	= 0.0f;
		gameObject.SetActive(false);
	}
	
	//更新//--------------------------------------------
	void Update () {
		float	a	= Mathf.Max(1.0f - timer,0.0f);
		image.color	= new Color(1.0f,1.0f,1.0f,a);
		sizeBuf	*= 1.00625f;
		timer	+= Time.deltaTime;
		if(timer > 3.0f){
			timer	= 0.0f;
			sizeBuf	= size;
		}
		image.rectTransform.sizeDelta	= sizeBuf;
	}
}
