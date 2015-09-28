//----------------------------------------------------------
//ボタンのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_BUTTONSYSTEM
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	UnityEngine.EventSystems;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ボタンのシステム_Begin//----------------------------------
public class ButtonSystem : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler {

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////
	public	delegate	void	ButtonEnter(ButtonSystem buttonSystem);
	public	ButtonEnter	buttonEnter	= null;
	public	string		text		= "debug";
	public	Vector3		textPos		= Vector3.zero;
	public	int			buttonID	= -1;
	public	int			fontSize	= 32;
	public	Vector2		buttonSize	= new Vector2(256.0f,64.0f);
	public	Color		color		= Color.white;

	private	Vector2		size;
	private	Text		textObject	= null;
	private	Image		buttonImage	= null;

	//ステートチェックプロパティ
	private	bool		f_press;
	public	bool		press{get{return	f_press;}}
	private	bool		f_down;
	public	bool		down{get{bool buf = f_down;f_down = false;return buf;}}
	private	bool		f_up;
	public	bool		up{get{bool buf = f_up;f_up = false;return buf;}}

	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化_Begin//--------------
		buttonImage		= GetComponent<Image>();
		StartCreateText();
		Button	button	= GetComponent<Button>();
		button.onClick.AddListener(OnButtonEnter);

		size			= buttonSize;
	}//初期化_End//-----------------------------------------

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		if(textObject != null)	textObject.rectTransform.sizeDelta		= size;
		if(buttonImage != null)	buttonImage.rectTransform.sizeDelta		= size;
		if(f_press)	size	= size * 0.5f + buttonSize * 0.4f;
		else 		size	= size * 0.5f + buttonSize * 0.5f;
	}//更新_End//-------------------------------------------

	//初期化////////////////////////////////////////////////
	//ボタンの文字を生成_Begin//----------------------------
	private	void	StartCreateText(){
		if(text == null)	return;
		GameObject	obj			= Instantiate(Resources.Load<GameObject>("Prefab/Select/Text"));
					textObject	= obj.GetComponent<Text>();
		obj.transform.SetParent(this.gameObject.transform);
		textObject.text							= text;
		textObject.fontSize						= fontSize;
		textObject.color						= color;
		textObject.rectTransform.localPosition	= textPos;
		textObject.rectTransform.localScale		= Vector3.one;
	}//ボタンの文字を生成_End//-----------------------------

	//その他関数///////////////////////////////////////////
	//コールバック関数を設定_Beign//------------------------
	public	void	OnPointerDown(PointerEventData eventData){
		f_press	= true;
		f_down	= true;
		f_up	= false;
	}
	public	void	OnDrag(PointerEventData eventData){
	}
	public	void	OnPointerUp(PointerEventData eventData){
		f_press	= false;
		f_down	= false;
		f_up	= true;
	}
	//コールバック関数を設定_Beign//------------------------

	//ボタンが押された_Begin//------------------------------
	private	void	OnButtonEnter(){
		if(buttonEnter == null)	return;
		buttonEnter(this);
	}//ボタンが押された_End//-------------------------------

}//ボタンのシステム_End//-----------------------------------
