//----------------------------------------------------------
//ゲームシーンのシステム
//更新日 :	07 / 16 / 2015
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

	//変数//////////////////////////////////////////////////
	private	Vector2	FLOORWINDOW_POS	= new Vector2(-192.0f,-384.0f);

	//フロア関連
	private	int		floor;
	private	Image	floorWindow	= null;
	private	Text	floorText	= null;
	private	Vector2	floorSize;

	//パーツ選択関連
	private	int					partsID;
	private	bool				changeStateFlg2PartsSet	= false;
	private	PartsSelectClass	partsSelectClass;
	//初期化////////////////////////////////////////////////
	//俺の初期化関数_Begin//--------------------------------
	private	void	StartKimishimaSystem(){
		resultUpdateFunc	= new UnityAction[]{
			this.UpdateResultOpen,
			this.UpdateResultLiquidation,
			this.UpdateResultNeutral,
		};
		FallObject.collapseFunc		= this.SetCollapseFlg;
		floor						= 0;
		floorSize					= new Vector2(128.0f,128.0f);
		partsSelectClass			= new PartsSelectClass(this);
		partsSelectClass.Init();
		BackFadeInit();
		StartKimishimaSystemCreateFloorWindow();
		StartKimishimaSystemCreateFloorText();
#if DEBUG_GAMESCENE
		Database.InitColorBlock();
#endif
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
		floorText.text		= "0";
		floorText.fontSize	= 48;
		floorText.color		= Color.black;
	}//階層テキストを生成_End//-----------------------------
	
	//更新//////////////////////////////////////////////////
	//俺の更新関数_Begin//----------------------------------
	private	void	UpdateKimishimaSystem(){
		BackFadeUpdate();
		UpdateFloorWindow();
		PartsSelectClass.MessageID				message	= PartsSelectClass.MessageID.Non;
		if(stateNo == (int)StateNo.PartsSelect)	message	= PartsSelectClass.MessageID.OnPartsSelectState;
		partsSelectClass.Update(message,ref changeStateFlg2PartsSet,job);
		if(changeStateFlg2PartsSet)	ChangeState(StateNo.PartsSet);
		changeStateFlg2PartsSet	= false;
	}//俺の更新関数_End//-----------------------------------

	//パーツ選択ステート_Begin//----------------------------
	private	void	UpdatePartsSelect(){
	}//パーツ選択ステート_End//-----------------------------

	//階層ウィンドウを更新_Begin//--------------------------
	private	void	UpdateFloorWindow(){
		if(floorWindow != null)	floorWindow.rectTransform.sizeDelta	= floorSize;
		if(floorText != null)	floorText.rectTransform.sizeDelta	= floorSize;
	}//階層ウィンドウを更新_End//---------------------------

	//その他関数////////////////////////////////////////////
	/// <summary>フロアを上げていく</summary>
	private	void	AddFloor(){//階層を進める_Begin//-------
		floor	++;
		floorText.text	= floor.ToString();
	}//階層を進める_End//-----------------------------------

}//ゲームシーンのシステム_End//------------------------------

//----------------------------------------------------------
//パーツセレクト
//更新日 :	07 / 24 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//パーツセレクトを管理するクラス_Begin//--------------------
class 	PartsSelectClass{

	//列挙・テーブル////////////////////////////////////////
	//ステート番号_Begin//-----------------------------------
	public	enum	PartsSelectWindowStateNo{
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

	//メッセージID_Beign//-----------------------------------
	public	enum	MessageID : uint{
		Non					= 0x00000000,
		OnPartsSelectState	= 0x00000001,	//パーツセレクトステート内の処理
	}//メッセージID_End//------------------------------------

	private	UnityAction[]	partsSelectWindowUpdateFunc	= null;
	//変数//////////////////////////////////////////////////
	private	int					partsID;
	private	bool				windowOpenFlg;
	private	Image				windowImage		= null;
	private	Vector3				windowSize;
	private	Text[]				text			= null;
	private	Button				button			= null;
	private	int					windowStateNo;
	private float				windowStateTime;
	private	PointerUpSystem[]	pointerUpSystem	= null;
	private	FadeClass			fadeClass		= null;
	private	GameSceneSystem		gameSceneSystem;
	private	GameObject			canvasObject;
	private	bool				nextFlg;

	//コンストラクタ・デストラクタ///////////////////////////
	//コンストラクタ_Begin//---------------------------------
	public	PartsSelectClass(GameSceneSystem gameSceneSystem){
		this.gameSceneSystem	= gameSceneSystem;
		canvasObject			= gameSceneSystem.canvasObject;
		fadeClass				= gameSceneSystem.GetFadeClass();
	}//コンストラクタ_End//----------------------------------

	//初期化////////////////////////////////////////////////
	public	void	Init(){//初期化_Begin//-----------------
		partsSelectWindowUpdateFunc	= new UnityAction[]{
			this.UpdatePartsSelectWindowOpen,
			this.UpdatePartsSelectWindowNeutral,
			this.UpdatePartsSelectWindowClose,
			null,
		};
		windowOpenFlg	= false;
	}//初期化_End//-----------------------------------------

	//更新//////////////////////////////////////////////////
	//更新_Begin//------------------------------------------
	public	void	Update(MessageID message,ref bool changeStateFlg2PartsSet,int job){
		if(windowStateNo < 0 || windowStateNo >= (int)PartsSelectWindowStateNo.Length){
			ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Hide,true);
			return;
		}
		if(partsSelectWindowUpdateFunc[windowStateNo] != null)
			partsSelectWindowUpdateFunc[windowStateNo]();
		if(windowImage != null)	windowImage.rectTransform.localScale	= windowSize;
		windowStateTime	+= Time.deltaTime;
		if(((uint)message & (uint)MessageID.OnPartsSelectState) == 0)	return;
		if(!windowOpenFlg){
			if(fadeClass == null)	fadeClass	= gameSceneSystem.GetFadeClass();
			fadeClass.ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeOut);
			OpenPartsSelectWindow(job);
			windowOpenFlg			= true;
		}
		if(nextFlg){
			windowOpenFlg			= false;
			changeStateFlg2PartsSet	= true;
		}
	}//更新_End//-------------------------------------------

	//ウィンドウを開く_Beign//------------------------------
	private	void	UpdatePartsSelectWindowOpen(){
		float	n	= windowStateTime * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n >= 1.0f)	ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Neutral);
	}//ウィンドウを開く_End//-------------------------------
	
	//待機_Beign//-----------------------------------------
	private	void	UpdatePartsSelectWindowNeutral(){
		button.interactable	= (partsID >= 0);
		for(int i = 0;i < pointerUpSystem.Length;i ++){
			if(partsID == i)	pointerUpSystem[i].defaultColor	= new Color(1.0f,1.0f,0.5f);
			else 				pointerUpSystem[i].defaultColor	= Color.white;
		}
	}//待機_End//------------------------------------------
	
	//ウィンドウを閉じる_Beign//-----------------------------
	private	void	UpdatePartsSelectWindowClose(){
		float	n		= 1.0f - windowStateTime * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n <= 0.0f){
			GameObject.Destroy(windowImage.gameObject);
			windowImage	= null;
			text		= null;
			button		= null;
			fadeClass.ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeIn);
			ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Hide);
			nextFlg					= true;
		}
	}//ウィンドウを閉じる_End//------------------------------

	//ボタン関連////////////////////////////////////////////
	//パーツ選択ウィンドウの決定ボタンが押された_Beign//----
	public	void	OnPartsSelectButtonEnter(){
		if(partsID < 0)	return;
		ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Close);
	}//パーツ選択ウィンドウの決定ボタンが押された_End//-----

	//その他関数/////////////////////////////////////////////
	/// <summary>パーツ選択ウィンドウを表示</summary>_Begin//-
	private	void	OpenPartsSelectWindow(int job){
		partsID		= -1;
		windowSize	= new Vector3(2.0f,0.0f,1.0f);
		ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Open);
		CreatePartsSelectWindow();
		CreatePartsSelectText(job);
		CreatePartsSelectButton();
		CreatePartsSelectScrollView(job);
	}//パーツ選択ウィンドウを表示_End//-----------------------
	
	//パーツ選択ウィンドウのステート番号を変更する_Begin//------
	private	void	ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo stateNo,bool overrapFlg = false){
		int		value	= (int)stateNo;
		if(!overrapFlg && value == windowStateNo)	return;
		windowStateNo	= value;
		windowStateTime	= 0.0f;
	}//パーツ選択ウィンドウのステート番号を変更する_End//-------
	
	//パーツ選択ウィンドウを生成_Beign//---------------------
	private	void	CreatePartsSelectWindow(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		windowImage			= obj.GetComponent<Image>();
		windowImage.sprite	= Resources.Load<Sprite>("Texture/Game/Window");
		windowImage.rectTransform.localPosition	= new Vector3(0.0f,64.0f);
		windowImage.rectTransform.sizeDelta		= new Vector2(480.0f,640.0f);
		windowImage.color						= new Color(1.0f,1.0f,0.75f,0.5f);
	}//パーツ選択ウィンドウを生成_End//----------------------
	
	//パーツウィンドウのテキストを生成_Begin//---------------
	private	void	CreatePartsSelectText(int job){
		GameObject	obj;
		Vector3[]	tableTextPos	= {new Vector3(-64.0f,240.0f,0.0f),new Vector3(96.0f,240.0f,0.0f),};
		Vector2[]	tableSizeDelta	= {new Vector2(256.0f,128.0f),new Vector2(256.0f,128.0f),};
		string[]	tableText		= {
			Database.obj.JOB_NAME[job],
			Database.tableJobDesc[job],
		};
		int[]		tableFontSize	= {48,24,};
		text	= new Text[(int)PartsSelectText.Length];
		for(int i = 0;i < tableText.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",windowImage.gameObject);
			text[i]				= obj.GetComponent<Text>();
			text[i].rectTransform.localPosition	= tableTextPos[i];
			text[i].rectTransform.sizeDelta		= tableSizeDelta[i];
			text[i].text		= tableText[i];
			text[i].alignment	= TextAnchor.MiddleLeft;
			text[i].fontSize	= tableFontSize[i];
			text[i].color		= new Color(0.0f,0.5f,0.0f,1.0f);
		}
	}//パーツウィンドウのテキストを生成_End//----------------
	
	//パーツセレクトウィンドウのボタン_Begin//---------------
	private	void	CreatePartsSelectButton(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",windowImage.gameObject);
		button				= obj.GetComponent<Button>();
		button.colors		= Database.colorBlocks[(int)Database.ColorBlockID.Blue];
		Image	buttonImage	= obj.GetComponent<Image>();
		buttonImage.rectTransform.localPosition	= new Vector3(128.0f,-240.0f);
		buttonImage.rectTransform.sizeDelta		= new Vector2(128.0f,64.0f);
		button.onClick.AddListener(OnPartsSelectButtonEnter);
		button.interactable	= false;
		ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
		buttonSystem.text			= "決定";
		buttonSystem.color			= Color.white;
		buttonSystem.fontSize		= 24;
	}//パーツセレクトウィンドウのボタン_End//----------------
	
	//スクロールビューを生成_Begin//------------------------
	private	void	CreatePartsSelectScrollView(int job){
		string		prefabName	= "Prefab/Game/PartsSelectScrollView";
		GameObject	obj			= TitleSystem.CreateObjectInCanvas(prefabName,windowImage.gameObject);
		GameObject	contents	= obj.transform.GetChild(0).gameObject;
		int			length		= Database.tablePartsName.GetLength(1) / 3;
		if((Database.tablePartsName.GetLength(1) % 3) != 0)	length	++;
		pointerUpSystem			= new PointerUpSystem[Database.tablePartsName.GetLength(1)];
		for(int i = 0;i < length;i ++){
			GameObject	test	= TitleSystem.CreateObjectInCanvas("Prefab/Game/PartsSelectWindowButton",canvasObject);
			test.transform.SetParent(contents.transform);
			for(int j = 0;j < 3;j ++){
				int		id		= i * 3 + j;
				if(id >= Database.tablePartsName.GetLength(1))	break;
				CreatePartsSelectWindowButton(test,job,id);
			}
		}
	}//スクロールビューを生成_End//-------------------------
	
	//パーツ選択ウィンドウのボタンを生成_Begin//-------------
	private	void	CreatePartsSelectWindowButton(GameObject contents,int job,int id){
		GameObject	obj			= TitleSystem.CreateObjectInCanvas("Prefab/Game/PartsSelectPanel",contents);
		Image		image		= obj.GetComponent<Image>();
		Vector3		imagePos	= new Vector3(-128.0f + (id % 3) * 128.0f,0.0f,0.0f);
		image.sprite= Resources.Load<Sprite>("Texture/Game/PartsSelectButton");
		image.rectTransform.localPosition	= imagePos;
		image.rectTransform.sizeDelta		= new Vector2(128.0f,128.0f);
		pointerUpSystem[id]		= obj.GetComponent<PointerUpSystem>();
		pointerUpSystem[id].id				= id;
		pointerUpSystem[id].defaultColor	= Color.white;
		pointerUpSystem[id].pressedColor	= Color.yellow;
		pointerUpSystem[id].scrollViewObject= contents.transform.parent.parent.gameObject;
		pointerUpSystem[id].SetCallBackFunc(GetButtonID);
	}//パーツ選択ウィンドウのボタンを生成_End//--------------
	
	//押されたボタンのIDを受け取る_Beign//------------------
	private	void	GetButtonID(PointerUpSystem pointerUpSystem){
		partsID	= pointerUpSystem.id;
#if DEBUG_GAMESCENE
		Debug.Log("Debug:タッチされたボタンは" + partsID);
#endif
	}//押されたボタンのIDを受け取る_End//-------------------

}//パーツセレクトを管理するクラス_End//---------------------