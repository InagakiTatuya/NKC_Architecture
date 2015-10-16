//----------------------------------------------------------
//セレクトのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//セレクトのシステム_Begin//--------------------------------
public	partial	class SelectSystem : MonoBehaviour {

	//プライベートフィールド//------------------------------

	//列挙//////////////////////////////////////////////////
	private	enum	StateNo : int{//ステート番号_Beign//----
		Neutral,
		GoNext,
		Length,
	}//ステート番号_End//-----------------------------------

	private	enum	ButtonNo : int{//ボタン番号_Beign//-----
		TutorialButton,
		MainGameButton,
		Banner,
		Length,
	}//ボタン番号_End//-------------------------------------

	//変数//////////////////////////////////////////////////
	private	int			stateNo;
	private	float		stateTime;
	private	static	int	selectNo	= -1;
	private	Button[]	button		= null;
	private	Image[]		buttonImage	= null;
	private	Image		fade		= null;
	private	Color		fadeColor;

	//初期化////////////////////////////////////////////////
	//ボタンを初期化_Begin//--------------------------------
	private	void	StartCreateButton(){
		UnityAction[]	tableOnButtonEnterFunc;//ボタンを押したときの関数
		tableOnButtonEnterFunc	= new UnityAction[]{
			this.OnTutorialButtonEnter,
			this.OnMainGameButtonEnter,
			this.OnBannerButtonEnter,
		};
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(0.0f, 256.0f,0.0f),
			new Vector3(0.0f,   0.0f,0.0f),
			new Vector3(0.0f,-288.0f,0.0f),
		};
		Vector3[]	tableButtonSize	= new Vector3[]{
			new Vector2(512.0f,256.0f),
			new Vector2(512.0f,256.0f),
			new Vector2(420.0f,315.0f)
		};
		string[]	spriteName		= new string[]{
			"Texture/Select/tutorial",
			"Texture/Select/gamestart",
			"Texture/Select/banner"
		};
		GameObject	obj;
		button		= new Button[tableOnButtonEnterFunc.Length];
		buttonImage	= new Image[button.Length];
		Database.InitColorBlock();
		for(int i = 0;i < button.Length;i ++){
			obj						= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			button[i]				= obj.GetComponent<Button>();
			button[i].colors		= Database.colorBlocks[(int)Database.ColorBlockID.White];
			button[i].onClick.AddListener(tableOnButtonEnterFunc[i]);
			ButtonSystem	buttonSystem	= obj.GetComponent<ButtonSystem>();
			buttonSystem.buttonPos	= tableButtonPos[i];
			buttonSystem.buttonSize	= tableButtonSize[i];
			Sprite[]	sprite		= Resources.LoadAll<Sprite>(spriteName[i]);
			buttonSystem.sprite.normalSprite	= sprite[0];
			buttonSystem.sprite.pushSprite		= sprite[1];
			buttonSystem.text.init();
		}
		obj				= TitleSystem.CreateObjectInCanvas("Prefab/Select/recode",canvasObject);
		Image	image	= obj.GetComponent<Image>();
		image.rectTransform.localPosition	= tableButtonPos[1] + new Vector3(-72.0f,48.0f,0.0f);
		obj				= TitleSystem.CreateObjectInCanvas("Prefab/Select/number",canvasObject);
		NumberDisp	nd	= obj.GetComponent<NumberDisp>();
		nd.pos			= tableButtonPos[1] + new Vector3(-42.0f,48.0f,0.0f);
		nd.size			= new Vector2(24.0f,24.0f);
		nd.offset		= -28.0f;
		nd.color		= new Color(0.0f,0.0f,0.0f,1.0f);
		nd.value		= Database.GetMaxFloor(Database.MainDataName);
	}//ボタンを初期化_End//---------------------------------
	
	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;
	
	private	void	UpdateNeutral(){//通常時の更新_Beign//--
		if(fade == null)	return;
		float	n	= 1.0f - Mathf.Min(stateTime * 4.0f,1.0f);
		fadeColor.a	= n;
		if(stateTime < 0.5f)	return;
		Destroy(fade.gameObject);
		fade		= null;
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		float	n	= Mathf.Min(stateTime * 4.0f,1.0f);
		fadeColor.a	= n;
		if(stateTime < 0.5f)	return;
		Database.obj.StageId	= selectNo;
		Application.LoadLevel("CardInput");
	}//次のシーンへ_End//-----------------------------------

	private	void	UpdateFade(){//フェードを更新_Begin//---
		if(fade == null)	return;
		fade.color		= fadeColor;
	}//フェードを更新_End//---------------------------------
	
	//その他関数////////////////////////////////////////////
	//ステート遷移_Begin//----------------------------------
	private	void	ChangeState(StateNo value,bool overrapFlg = false){
		int		buf	= (int)value;
		if(stateNo == buf && !overrapFlg)	return;
		stateNo		= buf;
		stateTime	= 0.0f;
	}//ステート遷移_End//-----------------------------------

	//ボタンを無効化する_Begin//----------------------------
	private	void	ButtonCanceler(){
		for(int i = 0;i < button.Length;i ++)	button[i].enabled	= false;
	}//ボタンを無効化する_End//-----------------------------

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------

}//セレクトのシステム_End//---------------------------------
