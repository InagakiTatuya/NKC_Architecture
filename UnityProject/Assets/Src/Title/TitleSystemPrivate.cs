//----------------------------------------------------------
//タイトルのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//タイトルのシステム_Begin//--------------------------------
public	partial class TitleSystem : MonoBehaviour{

	//プライベートフィールド//------------------------------

	//列挙//////////////////////////////////////////////////
	private	enum	StateNo : int{//ステート番号_Beign//----
		Neutral,
		GoNext,
		Length,
	}//ステート番号_End//-----------------------------------

	//変数//////////////////////////////////////////////////
	private	int			stateNo;
	private	float		stateTime;
	private	Button		button		= null;
	private	Image		fade		= null;
	private	Color		fadeColor;

	//初期化////////////////////////////////////////////////
	private	void	StartCreateButton(){//ボタンを初期化
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
		button			= obj.GetComponent<Button>();
		button.colors	= Database.colorBlocks[(int)Database.ColorBlockID.White];
		ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
		Sprite[]	spt	= Resources.LoadAll<Sprite>("Texture/Title/start");
		buttonSystem.sprite.normalSprite	= spt[0];
		buttonSystem.sprite.pushSprite		= spt[1];
		buttonSystem.text.init();
		buttonSystem.buttonPos		= new Vector2(0.0f,-192.0f);
		buttonSystem.buttonSize		= new Vector2(512.0f,128.0f);
		buttonSystem.buttonEnter	= OnStartButtonEnter;
	}

	private	void	StartCreateText(){//著作権表記
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",canvasObject);
		Text		text	= obj.GetComponent<Text>();
		text.text			= "(c)test";
		text.fontSize		= 24;
		text.color			= Color.white;
		text.rectTransform.localPosition		= new Vector3(0.0f,-320.0f,0.0f);
		text.rectTransform.localScale	= Vector3.one;
		text.rectTransform.sizeDelta	= new Vector2(544.0f,128.0f);
		if(textMaterial == null)	return;
		text.material		= textMaterial;
	}

	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;

	private	void	UpdateNeutral(){//通常時の更新_Beign//--
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		float	n	= Mathf.Min(stateTime * 4.0f,1.0f);
		fadeColor.a	= n;
		if(stateTime >= 1.0f)	Application.LoadLevel("Select");
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

	//スタートボタンを押した_Begin//------------------------
	private	void	OnStartButtonEnter(ButtonSystem bs){
		ChangeState(StateNo.GoNext);
		button.enabled	= false;
		CreateFade();
		if(seManager == null)	return;
		seManager.Play(0);
	}//スタートボタンを押した_End//-------------------------

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fadeColor		= new Color(0.0f,0.0f,0.0f,0.0f);
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------
	
}//タイトルのシステム_End//---------------------------------
