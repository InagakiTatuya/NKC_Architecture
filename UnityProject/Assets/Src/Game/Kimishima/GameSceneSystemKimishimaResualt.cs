//----------------------------------------------------------
//ゲームシーンのシステムのリザルト
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
#define	DEBUG_GAMESCENE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ゲームシーンのシステム_Begin//----------------------------
public	partial class GameSceneSystem : MonoBehaviour{
	
	//列挙・テーブル//////////////////////////////////////////
	private	enum	ResultStateNo{//リザルトのステート番号
		Open,		//開く
		Liquidation,//清算
		Neutral,	//通常
		NextScene,	//次のシーンへ
	}
	private	enum	ResultButtonNo{//リザルトボタンの番号
		Retry,		//リトライ
		Back,		//バック
		Length,
	}
	private	UnityAction[]	resultUpdateFunc= null;

	//変数//////////////////////////////////////////////////
	//リザルト関連
	private	Image		resultWindow		= null;
	private	Vector3		resultWindowSize;
	private	Text		resultText			= null;
	private	Button[]	resultButton		= null;
	private	Image[]		resultButtonImage	= null;
	private	int			resultStateNo;
	private	float		resultStateTime;
	private	int			resultDispFloor;
	private	bool		newRecodeFlg;
	private	int			resultSelectID;
	
	//初期化////////////////////////////////////////////////
	
	//更新//////////////////////////////////////////////////
	private	void	UpdateResult(){//リザルトの更新中枢
		if(resultUpdateFunc[resultStateNo] != null)	resultUpdateFunc[resultStateNo]();
		resultStateTime	+= Time.deltaTime;
	}
	private	void	UpdateResultOpen(){//リザルトを開く
		cameraMove.touchPermit	= true;
		resultDispFloor			= 0;
		resultSelectID			= -1;
		CreateResultWindow();
		CreateResultText();
		CreateResultButton();
		ChangeResultState(ResultStateNo.Liquidation);
	}
	private	void	UpdateResultLiquidation(){//精算ステート
		resultDispFloor	= (int)(floor * Mathf.Min(resultStateTime,1.0f));
		resultText.text	= resultDispFloor + " 階層";
		if(resultStateTime < 1.0f)	return;
		ChangeResultState(ResultStateNo.Neutral);
		if(Database.MaxFloor < floor){
			Database.MaxFloor	= floor;
			newRecodeFlg		= true;
		}else{
			newRecodeFlg		= false;
		}
	}
	private	void	UpdateResultNeutral(){//リザルトシーンの通常時
		floorSize.y	= 0.0f;
		cameraMove.touchPermit	= true;
	}
	private	void	UpdateResultNextScene(){//次のシーンへ遷移
		if(resultStateTime < 0.5f)	return;
		string[]	nextSceneName	= new string[]{"Game","Select"};
		Application.LoadLevel(nextSceneName[resultSelectID]);
	}
	
	//ボタン関連////////////////////////////////////////////
	private	void	OnResultButtonEnter(ButtonSystem bs){
		resultSelectID	= bs.buttonID;
		ChangeResultState(ResultStateNo.NextScene);
	}

	//その他関数////////////////////////////////////////////
	//リザルトのステートを遷移する_Begin//--------------------
	void	ChangeResultState(ResultStateNo value){
		resultStateNo		= (int)value;
		resultStateTime	= 0.0f;
	}//リザルトのステートを遷移する_End//---------------------

	//リザルトウィンドウを生成_Begin//------------------------
	private	void	CreateResultWindow(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		resultWindow		= obj.GetComponent<Image>();
		resultWindow.sprite	= Resources.Load<Sprite>("Texture/Result/window");
		resultWindow.rectTransform.localPosition	= new Vector3(0.0f,192.0f);
		resultWindow.rectTransform.sizeDelta		= new Vector2(512.0f,512.0f);
		resultWindow.color							= Color.white;
	}//リザルトウィンドウを生成_End//-------------------------

	//リザルトのテキストを生成_Begin//-----------------------
	private	void	CreateResultText(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",resultWindow.gameObject);
		resultText				= obj.GetComponent<Text>();
		resultText.rectTransform.localPosition	= new Vector3(0.0f,-60.0f);
		resultText.rectTransform.sizeDelta		= new Vector2(256.0f,128.0f);
		resultText.text		= resultDispFloor + " 階層";
		resultText.fontSize	= 64;
		resultText.color		= Color.blue;
	}//リザルトのテキストを生成_End//------------------------

	//リザルトのボタンを生成_Begin//-------------------------
	private	void	CreateResultButton(){
		GameObject	obj				= null;
		Vector3[]	tableButtonPos	= new Vector3[]{new Vector3(0.0f,-64.0f),new Vector3(0.0f,-192.0f)};
		resultButton		= new Button[(int)ResultButtonNo.Length];
		resultButtonImage	= new Image[(int)ResultButtonNo.Length];
		for(int i = 0;i < resultButton.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			resultButton[i]		= obj.GetComponent<Button>();
			resultButton[i].colors	= Database.colorBlocks[(int)Database.ColorBlockID.White];
			resultButtonImage[i]	= obj.GetComponent<Image>();
			resultButtonImage[i].sprite	= Resources.Load<Sprite>("Texture/Result/button" + i);
			resultButtonImage[i].rectTransform.localPosition	= tableButtonPos[i];
			ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
			buttonSystem.text		= null;
			buttonSystem.buttonSize	= new Vector2(512.0f,128.0f);
			buttonSystem.buttonID	= i;
			buttonSystem.buttonEnter= OnResultButtonEnter;
		}
	}//リザルトのボタンを生成_End//--------------------------
	
}//ゲームシーンのシステム_End//------------------------------