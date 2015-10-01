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

	//構造体////////////////////////////////////////////////
	public	struct SpriteGroup{
		public	Sprite		normalSprite;	//通常時のスプライト
		public	Sprite		pushSprite;		//押下時のスプライト
	}
	public	struct TextData{//テキストオブジェクト
		public	void	init(){//コンストラクタ
			text		= null;
			pos			= Vector3.zero;
			fontSize	= 32;
			color		= Color.white;
		}
		public	string		text;
		public	Vector3		pos;
		public	int			fontSize;
		public	Color		color;
	}

	//変数//////////////////////////////////////////////////
	public	delegate	void	ButtonEnter(ButtonSystem buttonSystem);
	public	ButtonEnter	buttonEnter	= null;

	public	SpriteGroup	sprite		= new SpriteGroup();
	public	TextData	text		= new TextData();
	public	int			buttonID	= -1;
	public	Vector2		buttonPos	= new Vector2(0.0f,0.0f);
	public	Vector2		buttonSize	= new Vector2(256.0f,64.0f);

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
	public	void	Start () {//初期化
		StartImage();
		StartCreateText();
		StartButton();
		size			= buttonSize;
	}
	private	void	StartImage(){//イメージの初期化
		buttonImage			= GetComponent<Image>();
		if(sprite.normalSprite == null)	return;
		buttonImage.sprite	= sprite.normalSprite;
	}
	private	void	StartButton(){//ボタンを初期化
		Button	button	= GetComponent<Button>();
		button.onClick.AddListener(OnButtonEnter);
	}

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		if(textObject != null)	textObject.rectTransform.sizeDelta		= size;
		if(buttonImage != null)	buttonImage.rectTransform.sizeDelta		= size;
		if(f_press)	size	= size * 0.5f + buttonSize * 0.4f;
		else 		size	= size * 0.5f + buttonSize * 0.5f;
		buttonImage.rectTransform.localPosition	= buttonPos;
	}//更新_End//-------------------------------------------

	//初期化////////////////////////////////////////////////
	//ボタンの文字を生成_Begin//----------------------------
	private	void	StartCreateText(){
		if(text.text == null)	return;
		GameObject	obj			= Instantiate(Resources.Load<GameObject>("Prefab/Select/Text"));
					textObject	= obj.GetComponent<Text>();
		obj.transform.SetParent(this.gameObject.transform);
		textObject.text							= text.text;
		textObject.fontSize						= text.fontSize;
		textObject.color						= text.color;
		textObject.rectTransform.localPosition	= text.pos;
		textObject.rectTransform.localScale		= Vector3.one;
	}//ボタンの文字を生成_End//-----------------------------

	//その他関数///////////////////////////////////////////
	//コールバック関数を設定_Beign//------------------------
	public	void	OnPointerDown(PointerEventData eventData){
		f_press	= true;
		f_down	= true;
		f_up	= false;
		if(buttonImage == null)			return;
		if(sprite.pushSprite == null)	return;
		buttonImage.sprite	= sprite.pushSprite;
	}
	public	void	OnDrag(PointerEventData eventData){
	}
	public	void	OnPointerUp(PointerEventData eventData){
		f_press	= false;
		f_down	= false;
		f_up	= true;
		if(buttonImage == null)			return;
		if(sprite.normalSprite == null)	return;
		buttonImage.sprite	= sprite.normalSprite;
	}
	//コールバック関数を設定_Beign//------------------------

	//ボタンが押された_Begin//------------------------------
	private	void	OnButtonEnter(){
		if(buttonEnter == null)	return;
		buttonEnter(this);
	}//ボタンが押された_End//-------------------------------

}//ボタンのシステム_End//-----------------------------------
