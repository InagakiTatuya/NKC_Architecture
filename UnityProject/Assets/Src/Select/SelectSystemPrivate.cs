//----------------------------------------------------------
//セレクトのシステム
//更新日 :	06 / 13 / 2015
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
		Length,
	}//ボタン番号_End//-------------------------------------

	//変数//////////////////////////////////////////////////
	private	int			stateNo;
	private	float		stateTime;
	private	int			selectNo;
	private	Button[]	button		= null;
	private	Image[]		buttonImage	= null;
	private	Vector2[]	buttonSize	= null;
	private	ColorBlock[]buttonColor	= null;
	private	Image		fade		= null;
	private	Color		fadeColor;

	//初期化////////////////////////////////////////////////
	//ボタンを初期化_Begin//--------------------------------
	private	void	StartCreateButton(){
		UnityAction[]	tableOnButtonEnterFunc;//ボタンを押したときの関数
		tableOnButtonEnterFunc	= new UnityAction[]{
			this.OnTutorialButtonEnter,
			this.OnMainGameButtonEnter,
		};
		string[]	tableButtonText	= new string[]{
			"チュートリアル",
			"こんな漢字",
		};
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(0.0f, 128.0f,0.0f),
			new Vector3(0.0f,-128.0f,0.0f),
		};
		button		= new Button[tableOnButtonEnterFunc.Length];
		buttonImage	= new Image[button.Length];
		buttonSize	= new Vector2[button.Length];
		buttonColor	= new ColorBlock[button.Length];
		for(int i = 0;i < button.Length;i ++){
			GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Button",canvasObject);
			button[i]		= obj.GetComponent<Button>();
			buttonColor[i].normalColor		= new Color(0.5f,0.5f,1.0f,1.0f);
			buttonColor[i].highlightedColor	= new Color(0.5f,0.5f,1.0f,1.0f);
			buttonColor[i].pressedColor		= new Color(1.0f,0.8f,0.0f,1.0f);
			buttonColor[i].disabledColor	= new Color(0.25f,0.25f,0.5f,1.0f);
			buttonColor[i].colorMultiplier	= 1;
			buttonColor[i].fadeDuration		= 0.1f;
			button[i].colors= buttonColor[i];
			buttonImage[i]	= obj.GetComponent<Image>();
			buttonImage[i].rectTransform.localPosition	= tableButtonPos[i];
			buttonSize[i]	= new Vector2(256.0f,64.0f);
			button[i].onClick.AddListener(tableOnButtonEnterFunc[i]);
			ButtonSystem	buttonSystem	= obj.GetComponent<ButtonSystem>();
			buttonSystem.text		= tableButtonText[i];
			buttonSystem.color		= Color.white;
			buttonSystem.fontSize	= 24;
		}
	}//ボタンを初期化_End//---------------------------------
	
	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;
	
	private	void	UpdateNeutral(){//通常時の更新_Beign//--
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		float	n	= Mathf.Min(stateTime * 4.0f,1.0f);
		buttonSize[selectNo].x	*= 1.25f;
		buttonSize[selectNo].y	*= 0.8f;
		fadeColor.a	= n;
		if(stateTime > 0.5f)	Application.LoadLevel("CardInput");
	}//次のシーンへ_End//-----------------------------------

	private	void	UpdateButton(){//ボタンを更新_Beign//---
		if(button == null)		return;
		if(buttonImage == null)	return;
		for(int i = 0;i < buttonImage.Length;i ++)
			buttonImage[i].rectTransform.sizeDelta	= buttonSize[i];
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

	//ボタンを無効化する_Begin//----------------------------
	private	void	ButtonCanceler(){
		for(int i = 0;i < button.Length;i ++)	button[i].enabled	= false;
	}//ボタンを無効化する_End//-----------------------------

	//ヘッダーの文字を生成_Begin//--------------------------
	private	void	CreateHeaderText(){
		GameObject	obj		= Resources.Load<GameObject>("Prefab/Select/Text");
		GameObject	textObj	= (GameObject)Instantiate(obj);
		Text		text	= textObj.GetComponent<Text>();
		text.rectTransform.SetParent(canvasObject.transform,false);
		text.rectTransform.localPosition	= new Vector3(0.0f,424.0f,0.0f);
		text.text			= "モード選択";
		text.fontSize		= 32;
		text.color			= Color.blue;
	}//ヘッダーの文字を生成_End//---------------------------

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fadeColor		= new Color(0.0f,0.0f,0.0f,0.0f);
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------

}//セレクトのシステム_End//---------------------------------
