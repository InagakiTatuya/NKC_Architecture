//----------------------------------------------------------
//タッチされたときのイベント
//更新日 :	07 / 24 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_POINTERUP
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	UnityEngine.EventSystems;
using	System.Collections;
using	System.Collections.Generic;

//クラス////////////////////////////////////////////////////
//イベントトリガーを扱うシステム_Begin//--------------------
public class PointerUpSystem : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler{

	//変数//////////////////////////////////////////////////
	//スクロールビューのデータ
	public	GameObject			scrollViewObject;
	private	Scrollbar			scrollbar;
	private	float				decelerationRate;
	private	float				scrollHeight;
	//ID
	public	int					id;
	//色関連
	private	Image				image			= null;
	public	Color				defaultColor	= Color.white;
	public	Color				pressedColor	= Color.white;
	//コールバック関数
	public	delegate	void	CallBackFunc(PointerUpSystem pointerUpSystem);
	private	CallBackFunc		callBackFunc;
	//ステート関連
	private	bool				press;
	private	Vector2				prevPressPos;
	private	float				velY;
	private	float				moveValue;

	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化_Begin//--------------
		image				= GetComponent<Image>();
		this.gameObject.AddComponent<EventTrigger>();
		ScrollRect	sr		= scrollViewObject.GetComponent<ScrollRect>();
		scrollbar			= sr.verticalScrollbar;
		decelerationRate	= sr.decelerationRate;
		Image	sImage		= scrollViewObject.GetComponent<Image>();
		scrollHeight		= sImage.rectTransform.sizeDelta.y;
		press				= false;
	}//初期化_End//-----------------------------------------

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		scrollbar.value		-= velY;
		velY				*= (1.0f - decelerationRate);
		if(image == null)	return;
		if(press)	image.color	= pressedColor;
		else 		image.color	= defaultColor;
	}//更新_End//-------------------------------------------

	//その他関数////////////////////////////////////////////
	//コールバック関数を設定_Beign//------------------------
	public	void	OnPointerDown(PointerEventData eventData){
		prevPressPos	= Camera.main.ScreenToViewportPoint(eventData.position) * 960.0f;
		press			= true;
		moveValue		= 0.0f;
	}
	public	void	OnDrag(PointerEventData eventData){
		float	viewSize	= scrollHeight / scrollbar.size - scrollHeight;
		Vector2	pressPos	= Camera.main.ScreenToViewportPoint(eventData.position) * 960.0f;
		velY				= (pressPos.y - prevPressPos.y) / viewSize;
		prevPressPos		= pressPos;
		moveValue			+= velY;
		if(Mathf.Abs(moveValue) < 0.1f)		return;
		press				= false;
	}
	public	void	OnPointerUp(PointerEventData eventData){
		if(!press)			return;
		if(callBackFunc != null)	callBackFunc(this);
		press				= false;
	}
	//コールバック関数を設定_Beign//------------------------

	public	void	SetCallBackFunc(CallBackFunc callBackFunc){
		this.callBackFunc	= callBackFunc;
	}//コールバック関数を設定_End//-------------------------

}//イベントトリガーを扱うシステム_End//---------------------
