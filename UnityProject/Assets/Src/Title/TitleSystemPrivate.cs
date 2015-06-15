//----------------------------------------------------------
//タイトルのシステム
//更新日 :	06 / 13 / 2015
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
	private	Image		buttonImage	= null;
	private	Vector2		buttonSize;
	private	ColorBlock	buttonColor;
	private	Image		fade		= null;
	private	Color		fadeColor;

	//初期化////////////////////////////////////////////////
	//ボタンを初期化_Begin//--------------------------------
	private	void	StartCreateButton(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Button",canvasObject);
		button			= obj.GetComponent<Button>();
		buttonColor.normalColor		= new Color(0.5f,0.5f,1.0f,1.0f);
		buttonColor.highlightedColor= new Color(0.5f,0.5f,1.0f,1.0f);
		buttonColor.pressedColor	= new Color(1.0f,0.8f,0.0f,1.0f);
		buttonColor.disabledColor	= new Color(0.25f,0.25f,0.5f,1.0f);
		buttonColor.colorMultiplier	= 1;
		buttonColor.fadeDuration	= 0.1f;
		button.colors	= buttonColor;
		buttonImage		= obj.GetComponent<Image>();
		buttonImage.rectTransform.localPosition	= new Vector3(0.0f,-256.0f,0.0f);
		buttonSize		= new Vector2(256.0f,64.0f);
		button.onClick.AddListener(this.OnStartButtonEnter);
		ButtonSystem	buttonSystem	= obj.GetComponent<ButtonSystem>();
		buttonSystem.text		= "タッチしてスタート!!";
		buttonSystem.color		= Color.white;
		buttonSystem.fontSize	= 24;
	}//ボタンを初期化_End//---------------------------------

	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;

	private	void	UpdateNeutral(){//通常時の更新_Beign//--
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		float	n	= Mathf.Min(stateTime * 4.0f,1.0f);
		buttonSize.x*= 1.25f;
		buttonSize.y*= 0.8f;
		fadeColor.a	= n;
		if(stateTime >= 0.5f)	Application.LoadLevel("Select");
	}//次のシーンへ_End//-----------------------------------

	private	void	UpdateButton(){//ボタンを更新_Beign//---
		if(button == null)		return;
		if(buttonImage == null)	return;
		buttonImage.rectTransform.sizeDelta	= buttonSize;
	}//ボタンを更新_End//-----------------------------------

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

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fadeColor		= new Color(0.0f,0.0f,0.0f,0.0f);
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------
	
}//タイトルのシステム_End//---------------------------------
