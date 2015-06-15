//----------------------------------------------------------
//リザルトのシステム
//更新日 :	06 / 13 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_RESULT
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//リザルトのシステム_Begin//--------------------------------
public	partial	class ResultSystem : MonoBehaviour {
	
//プライベートフィールド//----------------------------------
	
	//列挙//////////////////////////////////////////////////
	private	enum	StateNo : int{//ステート番号_Beign//----
		GideIn,		Gide,		GideOut,
		RuncIn,		Runc,		RuncOut,
		ButtonIn,	Button,		GoNext,
		Length,
	}//ステート番号_End//-----------------------------------
	
	private	enum	ButtonNo : int{//ボタン番号_Beign//-----
		TutorialButton,
		MainGameButton,
		Length,
	}//ボタン番号_End//-------------------------------------
	
	//変数//////////////////////////////////////////////////
	private	int				stateNo;
	private	float			stateTime;
	private	int				selectNo;
	private	Image[]			gide		= null;
	private	Vector3[]		gidePos		= null;
	private	Text[]			text		= null;
	private	Image[]			runcGide	= null;
	private	Vector3[]		runcGidePos	= null;
	private	Text[]			runcText	= null;
	private	Button[]		button		= null;
	private	Image[]			buttonImage	= null;
	private	Vector2[]		buttonSize	= null;
	private	ColorBlock		buttonColor;
	private	Image			fade		= null;
	private	Color			fadeColor;

	//初期化////////////////////////////////////////////////
	private	void	StartCreateGide(){//ガイドを生成_Begin//
		gide	= new Image[2];
		gidePos	= new Vector3[gide.Length];
		for(int i = 0;i < gide.Length;i ++){
			GameObject	obj;
			obj			= TitleSystem.CreateObujectInCanvas("Prefab/Result/Gide",canvasObject);
			gide[i]		= obj.GetComponent<Image>();
			gidePos[i]	= new Vector3(544.0f,192.0f - i * 128.0f,0.0f);
			gide[i].rectTransform.localPosition	= gidePos[i];
		}
	}//ガイドを生成_End//-----------------------------------

	private	void	StartCreateText(){//文字を生成_Begin//--
		string	[]	tableText	= new string[]{	"0 階層","- 級"	};
		text	= new Text[gide.Length];
		for(int i = 0;i < gide.Length;i ++){
			GameObject	obj;
			obj				= TitleSystem.CreateObujectInCanvas("Prefab/Select/Text",canvasObject);
			text[i]			= obj.GetComponent<Text>();
			text[i].rectTransform.localPosition	= gidePos[i] + new Vector3(0.0f,64.0f,0.0f);
			text[i].text	= tableText[i];
			text[i].rectTransform.sizeDelta	=new Vector2(256,128);
			text[i].fontSize= 64;
			text[i].color	= Color.black;
		}
	}//文字を生成_End//-------------------------------------

	//ランクのガイドを生成_Begin//--------------------------
	private	void	StartCreateRuncGide(){
		runcGide	= new Image[5];
		runcGidePos	= new Vector3[runcGide.Length];
		for(int i = 0;i < runcGide.Length;i ++){
			GameObject	obj;
			obj				= TitleSystem.CreateObujectInCanvas("Prefab/Result/Gide",canvasObject);
			runcGide[i]		= obj.GetComponent<Image>();
			runcGidePos[i]	= new Vector3(544.0f,192.0f - i * 128.0f,0.0f);
			runcGide[i].rectTransform.localPosition	= runcGidePos[i];
		}
	}//ランクのガイドを生成_End//---------------------------

	//ランクのテキストを生成_Begin//------------------------
	private	void	StartCreateRuncText(){
		runcText	= new Text[runcGide.Length];
		for(int i = 0;i < runcGide.Length;i ++){
			GameObject	obj;
			obj					= TitleSystem.CreateObujectInCanvas("Prefab/Select/Text",canvasObject);
			runcText[i]			= obj.GetComponent<Text>();
			runcText[i].rectTransform.localPosition	= runcGidePos[i] + new Vector3(0.0f,64.0f,0.0f);
			runcText[i].text	= "0 階層\n- 級";
			runcText[i].rectTransform.sizeDelta	=new Vector2(256,128);
			runcText[i].fontSize= 32;
			runcText[i].color	= Color.black;
		}
	}//ランクのテキストを生成_End//-------------------------
	
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
		text.text			= "結果集計";
		text.fontSize		= 32;
		text.color			= Color.blue;
	}//ヘッダーの文字を生成_End//---------------------------

	//ボタンを生成_Begin//----------------------------------
	private	void	CreateButton(){
		UnityAction[]	tableOnButtonEnterFunc;//ボタンを押したときの関数
		tableOnButtonEnterFunc	= new UnityAction[]{
			this.OnRetryButtonEnter,
			this.OnSelectButtonEnter,
			this.OnCardInputButtonEnter,
		};
		string[]	tableButtonText	= new string[]{
			"リトライ",
			"セレクトへ",
			"社員証入力へ",
		};
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(0.0f, 128.0f,0.0f),
			new Vector3(0.0f,   0.0f,0.0f),
			new Vector3(0.0f,-128.0f,0.0f),
		};
		button		= new Button[tableOnButtonEnterFunc.Length];
		buttonImage	= new Image[button.Length];
		buttonSize	= new Vector2[button.Length];
		buttonColor.normalColor		= new Color(0.5f,0.5f,1.0f,1.0f);
		buttonColor.highlightedColor= new Color(0.5f,0.5f,1.0f,1.0f);
		buttonColor.pressedColor	= new Color(1.0f,0.8f,0.0f,1.0f);
		buttonColor.disabledColor	= new Color(0.25f,0.25f,0.5f,1.0f);
		buttonColor.colorMultiplier	= 1;
		buttonColor.fadeDuration	= 0.1f;
		for(int i = 0;i < button.Length;i ++){
			GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Button",canvasObject);
			button[i]		= obj.GetComponent<Button>();
			button[i].colors= buttonColor;
			buttonImage[i]	= obj.GetComponent<Image>();
			buttonImage[i].rectTransform.localPosition	= tableButtonPos[i];
			buttonSize[i]	= Vector2.zero;
			button[i].onClick.AddListener(tableOnButtonEnterFunc[i]);
			ButtonSystem	buttonSystem	= obj.GetComponent<ButtonSystem>();
			buttonSystem.text		= tableButtonText[i];
			buttonSystem.color		= Color.white;
			buttonSystem.fontSize	= 24;
		}
	}//ボタンを生成_End//-----------------------------------

	//フェード関連を初期化_Begin//--------------------------
	private	void	CreateFade(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/Fade",canvasObject);
		fade			= obj.GetComponent<Image>();
		fadeColor		= new Color(0.0f,0.0f,0.0f,0.0f);
		fade.rectTransform.sizeDelta	= new Vector2(1024.0f,1024.0f);
		fade.color		= fadeColor;
	}//フェード関連を初期化_End//---------------------------
	
}//リザルトのシステム_End//---------------------------------
