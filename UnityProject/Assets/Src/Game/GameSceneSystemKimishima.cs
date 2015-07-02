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
	//ステート番号_Begin//-----------------------------------
	private	enum	PartsSelectWindowStateNo{
		Open,		//開く
		Neutral,	//通常
		Close,		//閉じる
		Hide,		//隠れている
		Length,		//長さ
	}//ステート番号_End//------------------------------------

	//パーツ選択ウィンドウのテキストID_Begin//---------------
	private	enum	PartsSelectText{
		Job,		//職種
		DescText,	//説明
		Length,		//長さ
	}//パーツ選択ウィンドウのテキストID_End//----------------

	private	UnityAction[]	partsSelectWindowUpdateFunc	= null;
	//変数//////////////////////////////////////////////////
	private	Vector2	FLOORWINDOW_POS	= new Vector2(-192.0f,-384.0f);

	//フロア関連
	private	int		floor;
	private	Image	floorWindow	= null;
	private	Text	floorText	= null;
	private	Vector2	floorSize;

	//パーツ選択関連
	private	int		partsID;
	private	bool	partsSelectWindowOpenFlg;
	private	Image	partsSelectWindow		= null;
	private	Vector3	partsSelectWindowSize;
	private	Text[]	partsSelectText			= null;
	private	Button	partsSelectButton		= null;
	private	Image	partsSelectButtonImage	= null;
	private	int		partsSelectWindowStateNo;
	private float	partsSelectWindowStateTime;
	//初期化////////////////////////////////////////////////
	//俺の初期化関数_Begin//--------------------------------
	private	void	StartKimishimaSystem(){
		partsSelectWindowUpdateFunc	= new UnityAction[]{
			this.UpdatePartsSelectWindowOpen,
			this.UpdatePartsSelectWindowNeutral,
			this.UpdatePartsSelectWindowClose,
			null,
		};
		resultUpdateFunc	= new UnityAction[]{
			this.UpdateResultOpen,
			this.UpdateResultLiquidation,
			this.UpdateResultNeutral,
		};
		floor						= 1;
		partsSelectWindowOpenFlg	= false;
		floorSize					= new Vector2(128.0f,128.0f);
		BackFadeInit();
		StartKimishimaSystemCreateFloorWindow();
		StartKimishimaSystemCreateFloorText();
#if DEBUG_GAMESCENE
		Database.InitColorBlock();
#endif
	//	CloseCardWind();
	//	CloseCardMiniWind();
	//	CloseNextPleyarWind();
		ChangeState(StateNo.PartsSelect);
	}//俺の初期化関数_End//---------------------------------

	//階層ウィンドウを生成_Begin//--------------------------
	private	void	StartKimishimaSystemCreateFloorWindow(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		floorWindow			= obj.GetComponent<Image>();
		floorWindow.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorWindow.color	= Color.white;
	}//階層ウィンドウを生成_End//---------------------------

	//階層テキストを生成_Begin//----------------------------
	private	void	StartKimishimaSystemCreateFloorText(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",canvasObject);
		floorText			= obj.GetComponent<Text>();
		floorText.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorText.text		= "1";
		floorText.fontSize	= 48;
		floorText.color		= Color.black;
	}//階層テキストを生成_End//-----------------------------
	
	//更新//////////////////////////////////////////////////
	//パーツ選択ステート_Begin//----------------------------
	private	void	UpdatePartsSelect(){
		if(!partsSelectWindowOpenFlg){
			ChangeBackFadeState(BackFadeStateNo.FadeOut);
			OpenPartsSelectWindow();
			partsSelectWindowOpenFlg	= true;
		}
	}//パーツ選択ステート_End//-----------------------------

	//俺の更新関数_Begin//----------------------------------
	private	void	UpdateKimishimaSystem(){
		BackFadeUpdate();
		if(partsSelectWindowStateNo < 0 || partsSelectWindowStateNo >= (int)PartsSelectWindowStateNo.Length)
			ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Hide,true);
		UpdateFloorWindow();
		if(partsSelectWindowUpdateFunc[partsSelectWindowStateNo] != null)
			partsSelectWindowUpdateFunc[partsSelectWindowStateNo]();
		if(partsSelectWindow != null)	partsSelectWindow.rectTransform.localScale	= partsSelectWindowSize;
		partsSelectWindowStateTime	+= Time.deltaTime;
	}//俺の更新関数_End//-----------------------------------

	//階層ウィンドウを更新_Begin//--------------------------
	private	void	UpdateFloorWindow(){
		if(floorWindow != null)	floorWindow.rectTransform.sizeDelta	= floorSize;
		if(floorText != null)	floorText.rectTransform.sizeDelta	= floorSize;
	}//階層ウィンドウを更新_End//---------------------------

	//ウィンドウを開く_Beign//------------------------------
	private	void	UpdatePartsSelectWindowOpen(){
		float	n	= partsSelectWindowStateTime * 8.0f;
		partsSelectWindowSize.x	= Mathf.Max(2.0f - n,1.0f);
		partsSelectWindowSize.y	= Mathf.Min(n,1.0f);
		if(n >= 1.0f)	ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Neutral);
	}//ウィンドウを開く_End//-------------------------------

	//待機_Beign//-----------------------------------------
	private	void	UpdatePartsSelectWindowNeutral(){
	}//待機_End//------------------------------------------

	//ウィンドウを閉じる_Beign//-----------------------------
	private	void	UpdatePartsSelectWindowClose(){
		float	n	= 1.0f - partsSelectWindowStateTime * 8.0f;
		partsSelectWindowSize.x	= Mathf.Max(2.0f - n,1.0f);
		partsSelectWindowSize.y	= Mathf.Min(n,1.0f);
		if(n <= 0.0f){
			GameObject.Destroy(partsSelectWindow.gameObject);
			partsSelectWindow		= null;
			partsSelectText			= null;
			partsSelectButton		= null;
			partsSelectButtonImage	= null;
			partsSelectWindowOpenFlg= false;
			ChangeBackFadeState(BackFadeStateNo.FadeIn);
			ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Hide);
			ChangeState(StateNo.PartsSet);
		}
	}//ウィンドウを閉じる_End//------------------------------

	//ボタン関連////////////////////////////////////////////
	//パーツ選択ウィンドウの決定ボタンが押された_Beign//----
	public	void	OnPartsSelectButtonEnter(){
		if(partsID < 0)	return;
		ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Close);
	}//パーツ選択ウィンドウの決定ボタンが押された_End//-----

	//その他関数////////////////////////////////////////////
	/// <summary>フロアを上げていく</summary>
	private	void	AddFloor(){//階層を進める_Begin//-------
		floor	++;
		floorText.text	= floor.ToString();
	}//階層を進める_End//-----------------------------------

	/// <summary>パーツ選択ウィンドウを表示</summary>_Begin//-
	private	void	OpenPartsSelectWindow(){
		partsID					= -1;
		partsSelectWindowSize	= new Vector3(2.0f,0.0f,1.0f);
		ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Open);
		CreatePartsSelectWindow();
		CreatePartsSelectText();
		CreatePartsSelectButton();
		CreatePartsSelectScrollView();
	}//パーツ選択ウィンドウを表示_End//-----------------------

	//パーツ選択ウィンドウのステート番号を変更する_Begin//------
	private	void	ChangePartsSelectWindowStateNo(
		PartsSelectWindowStateNo stateNo,bool overrapFlg = false){
		int		value	= (int)stateNo;
		if(!overrapFlg && value == partsSelectWindowStateNo)	return;
		partsSelectWindowStateNo	= value;
		partsSelectWindowStateTime	= 0.0f;
	}//パーツ選択ウィンドウのステート番号を変更する_End//-------

	//パーツ選択ウィンドウを生成_Beign//---------------------
	private	void	CreatePartsSelectWindow(){
		GameObject	obj			= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		partsSelectWindow		= obj.GetComponent<Image>();
		partsSelectWindow.sprite= Resources.Load<Sprite>("Texture/Game/Window");
		partsSelectWindow.rectTransform.localPosition	= new Vector3(0.0f,64.0f);
		partsSelectWindow.rectTransform.sizeDelta		= new Vector2(480.0f,640.0f);
		partsSelectWindow.color							= new Color(1.0f,1.0f,0.75f,0.5f);
	}//パーツ選択ウィンドウを生成_End//----------------------

	//パーツウィンドウのテキストを生成_Begin//---------------
	private	void	CreatePartsSelectText(){
		GameObject	obj;
		Vector3[]	tableTextPos	= {new Vector3(-64.0f,240.0f,0.0f),new Vector3(96.0f,240.0f,0.0f),};
		Vector2[]	tableSizeDelta	= {new Vector2(256.0f,128.0f),new Vector2(256.0f,128.0f),};
		string[]	tableText		= {
			Database.obj.JOB_NAME[job],
			Database.tableJobDesc[job],
		};
		int[]		tableFontSize	= {48,24,};
		partsSelectText	= new Text[(int)PartsSelectText.Length];
		for(int i = 0;i < tableText.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",partsSelectWindow.gameObject);
			partsSelectText[i]				= obj.GetComponent<Text>();
			partsSelectText[i].rectTransform.localPosition	= tableTextPos[i];
			partsSelectText[i].rectTransform.sizeDelta		= tableSizeDelta[i];
			partsSelectText[i].text			= tableText[i];
			partsSelectText[i].alignment	= TextAnchor.MiddleLeft;
			partsSelectText[i].fontSize		= tableFontSize[i];
			partsSelectText[i].color		= new Color(0.0f,0.5f,0.0f,1.0f);
		}
	}//パーツウィンドウのテキストを生成_End//----------------

	//パーツセレクトウィンドウのボタン_Begin//---------------
	private	void	CreatePartsSelectButton(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",partsSelectWindow.gameObject);
		partsSelectButton			= obj.GetComponent<Button>();
		partsSelectButton.colors	= Database.colorBlocks[(int)Database.ColorBlockID.Blue];
		partsSelectButtonImage		= obj.GetComponent<Image>();
		partsSelectButtonImage.rectTransform.localPosition	= new Vector3(128.0f,-240.0f);
		partsSelectButtonImage.rectTransform.sizeDelta		= new Vector2(128.0f,64.0f);
		partsSelectButton.onClick.AddListener(OnPartsSelectButtonEnter);
		ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
		buttonSystem.text			= "決定";
		buttonSystem.color			= Color.white;
		buttonSystem.fontSize		= 24;
	}//パーツセレクトウィンドウのボタン_End//----------------

	//スクロールビューを生成_Begin//------------------------
	private	void	CreatePartsSelectScrollView(){
		string		prefabName	= "Prefab/Game/PartsSelectScrollView";
		GameObject	obj			= TitleSystem.CreateObjectInCanvas(prefabName,partsSelectWindow.gameObject);
		GameObject	contents	= obj.transform.GetChild(0).gameObject;
		int			length		= Database.tablePartsName.GetLength(1) / 3;
		if((Database.tablePartsName.GetLength(1) % 3) != 0)	length	++;
		for(int i = 0;i < length;i ++){
			GameObject	test	= TitleSystem.CreateObjectInCanvas("Prefab/Game/PartsSelectWindowButton",canvasObject);
			test.transform.SetParent(contents.transform);
			for(int j = 0;j < 3;j ++){
				int		id		= i * 3 + j;
				if(id >= Database.tablePartsName.GetLength(1))	break;
				CreatePartsSelectWindowButton(test,id);
			}
		}
	}//スクロールビューを生成_End//-------------------------

	//パーツ選択ウィンドウのボタンを生成_Begin//-------------
	private	void	CreatePartsSelectWindowButton(GameObject contents,int id){
		GameObject	obj			= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
		obj.transform.SetParent(contents.transform);
		Button		button		= obj.GetComponent<Button>();
		button.colors			= Database.colorBlocks[(int)Database.ColorBlockID.White];
		Image		image		= obj.GetComponent<Image>();
		Vector3		imagePos	= new Vector3(-128.0f + (id % 3) * 128.0f,0.0f,0.0f);
		image.sprite			= Resources.Load<Sprite>("Texture/Game/PartsSelectButton");
		image.rectTransform.localPosition	= imagePos;
		image.rectTransform.sizeDelta		= new Vector2(128.0f,128.0f);
		ButtonSystem	bs		= obj.GetComponent<ButtonSystem>();
		bs.text					= Database.tablePartsName[job,id];
		bs.textPos				= new Vector3(0.0f,-48.0f,0.0f);
		bs.buttonID				= id;
		bs.color				= Color.black;
		bs.fontSize				= 24;
		bs.buttonEnter			= GetButtonID;
	}//パーツ選択ウィンドウのボタンを生成_End//--------------

	//押されたボタンのIDを受け取る_Beign//------------------
	private	void	GetButtonID(ButtonSystem buttonSystem){
		partsID	= buttonSystem.buttonID;
#if DEBUG_GAMESCENE
		Debug.Log("Debug:タッチされたボタンは" + partsID);
#endif
	}//押されたボタンのIDを受け取る_End//-------------------

}//ゲームシーンのシステム_End//------------------------------