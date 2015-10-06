//------------------------------------------------------
//タイトルエフェクト
//------------------------------------------------------

//名前空間//--------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

//クラス//----------------------------------------------
public class TitleLogo : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler{

	//変数//--------------------------------------------
	private	Image	image;
	private	Vector3	pos;
	private	Vector2	size;

	private	Vector3	posBuf;
	private	float	vel;
	private	Vector2	sizeBuf;

	private	bool	onMouseFlg;
	private	bool	touchFlg;
	
	//初期化//------------------------------------------
	void Start () {
		image		= GetComponent<Image>();
		pos			= image.rectTransform.position;
		size		= image.rectTransform.sizeDelta;
		posBuf		= pos;
		posBuf.y	= 1024.0f;
		sizeBuf		= size;
		onMouseFlg	= false;
	}
	
	//更新//--------------------------------------------
	void Update () {
		GetTouch();
		posBuf.y	+= vel * Time.deltaTime;
		vel			-= 1024.0f * Time.deltaTime;
		sizeBuf		= size * 0.25f + sizeBuf * 0.75f;
		if(posBuf.y <= pos.y){
			posBuf.y	= pos.y;
			if(vel < -128.0f){
				vel	*= -0.5f;
				sizeBuf.x	*= 1.25f;
				sizeBuf.y	*= 0.8f;
			}else{
				vel	= 0.0f;
			}
		}
		image.rectTransform.position	= posBuf;
		image.rectTransform.sizeDelta	= sizeBuf;
		if(onMouseFlg && touchFlg){
			vel	= 512.0f;
		}
	}
	void	GetTouch(){
		touchFlg	= false;
		if(Input.GetMouseButtonDown(0))		touchFlg	= true;
		if(Input.touchCount <= 0)	return;
		Touch	touch	= Input.touches[0];
		if(touch.phase == TouchPhase.Began)	touchFlg	= true;
	}

	public	void	OnPointerEnter(PointerEventData eventData){
		onMouseFlg	= true;
	}
	public	void	OnPointerExit(PointerEventData eventData){
		onMouseFlg	= false;
	}

}
