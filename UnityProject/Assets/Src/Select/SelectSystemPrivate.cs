//----------------------------------------------------------
//セレクトのシステム
//更新日 :	07 / 02 / 2015
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
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(0.0f, 128.0f,0.0f),
			new Vector3(0.0f,-128.0f,0.0f),
		};
		string[]	spriteName		= new string[]{
			"Texture/Select/tutorial",
			"Texture/Select/gamestart"
		};
		button		= new Button[tableOnButtonEnterFunc.Length];
		buttonImage	= new Image[button.Length];
		buttonSize	= new Vector2[button.Length];
		Database.InitColorBlock();
		for(int i = 0;i < button.Length;i ++){
			GameObject	obj			= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			button[i]				= obj.GetComponent<Button>();
			button[i].colors		= Database.colorBlocks[(int)Database.ColorBlockID.White];
			buttonImage[i]			= obj.GetComponent<Image>();
			buttonImage[i].sprite	= Resources.Load<Sprite>(spriteName[i]);
			buttonImage[i].rectTransform.localPosition	= tableButtonPos[i];
			buttonSize[i]			= new Vector2(512.0f,256.0f);
			button[i].onClick.AddListener(tableOnButtonEnterFunc[i]);
			ButtonSystem	buttonSystem	= obj.GetComponent<ButtonSystem>();
			buttonSystem.text		= " ";
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
		if(stateTime < 0.5f)	return;
		Database.obj.StageId	= selectNo;
		Application.LoadLevel("CardInput");
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

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fadeColor		= new Color(0.0f,0.0f,0.0f,0.0f);
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------

}//セレクトのシステム_End//---------------------------------
