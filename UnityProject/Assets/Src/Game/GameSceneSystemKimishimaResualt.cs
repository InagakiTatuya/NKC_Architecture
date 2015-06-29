//----------------------------------------------------------
//ゲームシーンのシステム
//更新日 :	06 / 29 / 2015
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
	//リザルトシーンのステート番号_Begin//-------------------
	private	enum	ResultStateNo{
		Open,		//開く
		Liquidation,//清算
		Neutral,	//通常
	}//リザルトシーンのステート番号_End//--------------------

	//リザルトテキスト_Begin//-----------------------------
	private	enum	ResultTextID{
		Kekka,		//建設結果
		Shinkiroku,	//新記録
		Kaiso,		//階層
		Length,
	}//リザルトテキスト_End//------------------------------

	//リザルトボタンの番号_Begin//---------------------------
	private	enum	ResultButtonNo{
		Retry,		//リトライ
		Back,		//バック
		Length,
	}//リザルトボタンの番号_End//----------------------------

	private	UnityAction[]	resultUpdateFunc			= null;
	//変数//////////////////////////////////////////////////
	//リザルト関連
	private	Image		resultWindow		= null;
	private	Vector3		resultWindowSize;
	private	Text[]		resultText			= null;
	private	Button[]	resultButton		= null;
	private	Image[]		resultButtonImage	= null;
	private	int			resultStateNo;
	private	float		resultStateTime;
	private	int			resultDispFloor;
	
	//初期化////////////////////////////////////////////////
	
	//更新//////////////////////////////////////////////////
	//リザルト_Beign//--------------------------------------
	private	void	UpdateResult(){
		if(resultUpdateFunc[resultStateNo] != null)	resultUpdateFunc[resultStateNo]();
		resultStateTime	+= Time.deltaTime;
	}//リザルト_End//---------------------------------------

	//リザルトを開く_Begin//--------------------------------
	private	void	UpdateResultOpen(){
		resultDispFloor	= 0;
		CreateResultWindow();
		CreateResultText();
		CreateResultButton();
		ChangeResultState(ResultStateNo.Liquidation);
	}//リザルトを開く_End//---------------------------------

	//リザルトの清算_Begin//--------------------------------
	private	void	UpdateResultLiquidation(){
		resultDispFloor	= (int)(floor * Mathf.Min(resultStateTime,1.0f));
		resultText[(int)ResultTextID.Kaiso].text	= "" + resultDispFloor + " 階層";
		if(resultStateTime >= 1.0f)	ChangeResultState(ResultStateNo.Neutral);
	}//リザルトの清算_End//---------------------------------
	
	//リザルトのニュートラル_Begin//------------------------
	private	void	UpdateResultNeutral(){
	}//リザルトのニュートラル_End//-------------------------
	
	//ボタン関連////////////////////////////////////////////
	private	void	OnresultButtonRetryEnter(){}
	private	void	OnresultButtonBackEnter(){}

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
		resultWindow.rectTransform.localPosition	= new Vector3(0.0f,192.0f);
		resultWindow.rectTransform.sizeDelta		= new Vector2(480.0f,320.0f);
		resultWindow.color							= new Color(0.0f,1.0f,1.0f,1.0f);
	}//リザルトウィンドウを生成_End//-------------------------

	//リザルトのテキストを生成_Begin//-----------------------
	private	void	CreateResultText(){
		GameObject	obj				= null;
		string[]	tableText		= new string[]{	"建設結果","新記録","" + resultDispFloor + " 階層"};
		Vector3[]	tableTextPos	= new Vector3[]{new Vector3(0.0f,80.0f),new Vector3(-128.0f,16.0f),new Vector3(0.0f,-48.0f)};
		Color[]		tableTextColor	= new Color[]{Color.black,Color.red,Color.blue};
		int[]		tableFontSize	= new int[]{48,48,64};
		resultText	= new Text[(int)ResultTextID.Length];
		for(int i = 0;i < resultText.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",resultWindow.gameObject);
			resultText[i]				= obj.GetComponent<Text>();
			resultText[i].rectTransform.localPosition	= tableTextPos[i];
			resultText[i].rectTransform.sizeDelta		= new Vector2(256.0f,128.0f);
			resultText[i].text			= tableText[i];
			resultText[i].fontSize		= tableFontSize[i];
			resultText[i].color		= tableTextColor[i];
		}
	}//リザルトのテキストを生成_End//------------------------

	//リザルトのボタンを生成_Begin//-------------------------
	private	void	CreateResultButton(){
		GameObject	obj				= null;
		string[]	tableText		= new string[]{	"リトライ","戻る"};
		Vector3[]	tableButtonPos	= new Vector3[]{new Vector3(0.0f,-64.0f),new Vector3(0.0f,-192.0f)};
		UnityAction[]	tableFunc	= new UnityAction[]{OnresultButtonRetryEnter,OnresultButtonBackEnter};
		resultButton		= new Button[(int)ResultButtonNo.Length];
		resultButtonImage	= new Image[(int)ResultButtonNo.Length];
		for(int i = 0;i < resultButton.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			resultButton[i]		= obj.GetComponent<Button>();
			resultButton[i].colors	= partsSelectButtonColor;
			resultButtonImage[i]	= obj.GetComponent<Image>();
			resultButtonImage[i].rectTransform.localPosition	= tableButtonPos[i];
			resultButtonImage[i].rectTransform.sizeDelta		= new Vector2(256.0f,96.0f);
			resultButton[i].onClick.AddListener(tableFunc[i]);
			ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
			buttonSystem.text		= tableText[i];
			buttonSystem.color		= Color.blue;
			buttonSystem.fontSize	= 48;
		}
	}//リザルトのボタンを生成_End//--------------------------


	
}//ゲームシーンのシステム_End//------------------------------