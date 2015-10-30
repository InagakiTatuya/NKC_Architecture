//----------------------------------------------------------
//ゲームシーンのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

#region//プリプロセッサ/////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_GAMESCENE
#endif
#endregion	//プリプロセッサ

#region//名前空間///////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;
#endregion	//名前空間

#region//パーツセレクトを管理するクラス////////////////////
class 	PartsSelectClass{
	
	//列挙・テーブル///////////////////////////////////////
	//ステート番号
	public	enum	StateNo{Open,Neutral,Close,Hide,YaneChange,YaneWait,Length}
	//パーツ選択ウィンドウのテキスト
	private	enum	PartsSelectText{Job,DescText,Length}
	//メッセージID
	public	enum	MessageID : uint{
		Non					= 0x00000000,
		OnPartsSelectState	= 0x00000001,	//パーツセレクトステート内の処理
	}
	//ボタンのID
	public	enum	ButtonID{Enter,Yane,Length}
	private	UnityAction[]	partsSelectWindowUpdateFunc	= null;
	
	//変数//////////////////////////////////////////////////
	private	int					partsID;
	private	bool				windowOpenFlg;
	private	Image				windowImage		= null;
	private	Vector3				windowSize;
	private	Text[]				text			= null;
	private	Button[]			button			= null;
	public	int					stateNo{get{return	windowStateNo;}}
	private	int					windowStateNo;
	private float				windowStateTime;
	private	PointerUpSystem[]	pointerUpSystem	= null;
	private	FadeClass			fadeClass		= null;
	private	GameSceneSystem		gameSceneSystem;
	private	GameObject			canvasObject;
	private	bool				nextFlg;
	public	bool				yaneFlg;
	public	SeManager			seManager;
	
	//コンストラクタ・デストラクタ//////////////////////////
	//コンストラクタ
	public	PartsSelectClass(GameSceneSystem gameSceneSystem){
		this.gameSceneSystem	= gameSceneSystem;
		canvasObject			= gameSceneSystem.canvasObject;
		fadeClass				= gameSceneSystem.GetFadeClass();
	}
	
	//初期化////////////////////////////////////////////////
	public	void	Init(){//初期化_Begin//-----------------
		partsSelectWindowUpdateFunc	= new UnityAction[]{
			this.Open,	this.Neutral,	this.Close,	null,	this.YaneChange,	this.YaneWait,
		};
		windowOpenFlg	= false;
		windowSize		= Vector3.zero;
	}//初期化_End//-----------------------------------------
	
	//更新//////////////////////////////////////////////////
	//更新_Begin//------------------------------------------
	public	void	Update(MessageID message,ref bool execute,int job){
		if(UpdateCheckStateNo())	return;
		if(partsSelectWindowUpdateFunc[windowStateNo] != null)
			partsSelectWindowUpdateFunc[windowStateNo]();
		if(windowImage != null)	windowImage.rectTransform.localScale	= windowSize;
		windowStateTime	+= Time.deltaTime;
		if(((uint)message & (uint)MessageID.OnPartsSelectState) == 0)	return;
		UpdateWindowOpenCheck(job);
		UpdateNextCheck(ref execute);
	}
	private	bool	UpdateCheckStateNo(){//ステート番号を確認
		if(windowStateNo >= 0 && windowStateNo < (int)StateNo.Length)	return	false;
		ChangeState(StateNo.Hide,true);									return	true;
	}
	private	void	UpdateWindowOpenCheck(int job){//ウィンドウを開く処理
		if(windowOpenFlg)	return;
		if(fadeClass == null)	fadeClass	= gameSceneSystem.GetFadeClass();
		fadeClass.ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeOut);
		OpenWindow(job);
		windowOpenFlg	= true;
	}
	private	void	UpdateNextCheck(ref bool execute){//次のシーンに進む
		if(!nextFlg)	return;
		nextFlg			= false;
		windowOpenFlg	= false;
		execute			= true;
	}
	
	//ウィンドウを開く_Beign//------------------------------
	private	void	Open(){
		float	n	= windowStateTime * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n >= 1.0f)	ChangeState(StateNo.Neutral);
	}//ウィンドウを開く_End//-------------------------------
	
	//待機_Beign//-----------------------------------------
	private	void	Neutral(){
		if(button == null)	return;
		button[(int)ButtonID.Enter].interactable	= (partsID >= 0);
		for(int i = 0;i < pointerUpSystem.Length;i ++){
			if(partsID == i)	pointerUpSystem[i].defaultColor	= new Color(1.0f,1.0f,0.5f);
			else 				pointerUpSystem[i].defaultColor	= Color.white;
		}
	}//待機_End//------------------------------------------
	
	//ウィンドウを閉じる_Beign//-----------------------------
	private	void	Close(){
		float	n		= 1.0f - windowStateTime * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n <= 0.0f){
			GameObject.Destroy(windowImage.gameObject);
			windowImage	= null;
			text		= null;
			button		= null;
			fadeClass.ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeIn);
			ChangeState(StateNo.Hide);
			nextFlg		= true;
			yaneFlg		= false;
		}
	}//ウィンドウを閉じる_End//------------------------------

	private	void	YaneChange(){//屋根ウィンドウ出す//------
		float	n		= 1.0f - windowStateTime * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n <= 0.0f){
			GameObject.Destroy(windowImage.gameObject);
			windowImage	= null;
			text		= null;
			button		= null;
			ChangeState(StateNo.YaneWait);
		}
	}
	private	void	YaneWait(){//ウィンドウを出すための待機//
		windowSize		= Vector3.zero;
		if(windowStateTime < 0.25f)	return;
		windowOpenFlg	= false;
		ChangeState(StateNo.Open);
	}

	//ボタン関連////////////////////////////////////////////
	//パーツ選択ウィンドウの決定ボタンが押された
	public	void	OnPartsSelectButtonEnter(){
		if(partsID < 0)	return;
		seManager.Play(6);
		ChangeState(StateNo.Close);
	}
	//屋根のボタンが押された
	public	void	OnYaneButtonEnter(){
		yaneFlg	= true;
		seManager.Play(6);
		ChangeState(StateNo.YaneChange);
	}
	
	//その他関数/////////////////////////////////////////////
	/// <summary>パーツ選択ウィンドウを表示</summary>_Begin//-
	private	void	OpenWindow(int job){
		partsID		= -1;
		windowSize	= new Vector3(2.0f,0.0f,1.0f);
		ChangeState(StateNo.Open);
		CreateWindow();
		CreatePartsSelectText(job);
		CreatePartsSelectButton(job);
		CreatePartsSelectScrollView(job);
	}

	public	void	ChangeState(StateNo stateNo,bool overrapFlg = false){//パーツ選択ウィンドウのステート番号を変更する
		int		value	= (int)stateNo;
		if(!overrapFlg && value == windowStateNo)	return;
		windowStateNo	= value;
		windowStateTime	= 0.0f;
	}

	private	void	CreateWindow(){//パーツ選択ウィンドウを生成
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		windowImage			= obj.GetComponent<Image>();
		windowImage.sprite	= Resources.Load<Sprite>("Texture/Game/Window");
		windowImage.rectTransform.localPosition	= new Vector3(0.0f,64.0f);
		windowImage.rectTransform.sizeDelta		= new Vector2(480.0f,640.0f);
		windowImage.color						= new Color(0.0f,0.0f,0.0f,0.5f);
	}

	private	void	CreatePartsSelectText(int job){//パーツウィンドウのテキストを生成
		GameObject	obj;
		Vector3[]	tableTextPos	= {new Vector3(-176.0f,240.0f,0.0f),new Vector3(-16.0f,240.0f,0.0f),};
		Vector2[]	tableSizeDelta	= {new Vector2(256.0f,128.0f),new Vector2(256.0f,128.0f),};
		string[]	tableText		= {
			Database.obj.JOB_NAME[job],
			Database.tableJobDesc[job],
		};
		int[]		tableFontSize	= {48,24,};
		text	= new Text[(int)PartsSelectText.Length];
		for(int i = 0;i < tableText.Length;i ++){
			Vector3	scale	= Vector3.one;
			if(i == 0 && job == 3)	scale.x	= 3.0f / 4.0f;
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",windowImage.gameObject);
			text[i]				= obj.GetComponent<Text>();
			text[i].rectTransform.pivot			= new Vector2(0.0f,0.5f);
			text[i].rectTransform.localPosition	= tableTextPos[i];
			text[i].rectTransform.localScale	= scale;
			text[i].rectTransform.sizeDelta		= tableSizeDelta[i];
			text[i].text		= tableText[i];
			text[i].alignment	= TextAnchor.MiddleLeft;
			text[i].fontSize	= tableFontSize[i];
			text[i].color		= new Color(0.0f,0.75f,0.75f,1.0f);
			text[i].material	= Resources.Load<Material>("Material/Game/TextMaterial");
		}
	}

	private	void	CreatePartsSelectButton(int job){//パーツセレクトウィンドウのボタン
		button	= new Button[(int)ButtonID.Length];
		Vector3[]		tablePos	= new Vector3[]{new Vector3(128.0f,-240.0f),new Vector3(-128.0f,-240.0f)};
		string[]		tebleText	= new string[2];
		string[]		tablejob	= new string[]{"屋根","屋根","屋根","戻る"};
		tebleText[0]	= "決定";
		tebleText[1]	= tablejob[job];
		UnityAction[]	tableAction	= new UnityAction[]{OnPartsSelectButtonEnter,OnYaneButtonEnter};
		for(int i = 0;i < (int)ButtonID.Length;i ++){
			GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",windowImage.gameObject);
			button[i]			= obj.GetComponent<Button>();
			button[i].colors	= Database.colorBlocks[(int)Database.ColorBlockID.Blue];
			button[i].onClick.AddListener(tableAction[i]);
			button[i].interactable		= (i == 0)?false:true;
			ButtonSystem	buttonSystem= obj.GetComponent<ButtonSystem>();
			buttonSystem.text.text			= tebleText[i];
			buttonSystem.text.color			= Color.white;
			buttonSystem.text.fontSize		= 24;
			buttonSystem.buttonPos		= tablePos[i];
			buttonSystem.buttonSize		= new Vector2(128.0f,64.0f);
		}
	}

	private	void	CreatePartsSelectScrollView(int job){//スクロールビューを生成
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
	}

	private	void	CreatePartsSelectWindowButton(GameObject contents,int job,int id){//パーツ選択ウィンドウのボタンを生成
		GameObject	obj			= TitleSystem.CreateObjectInCanvas("Prefab/Game/PartsSelectPanel",contents);
		Image		image		= obj.GetComponent<Image>();
		Vector3		imagePos	= new Vector3(-128.0f + (id % 3) * 128.0f,0.0f,0.0f);
		string[]	buttonName	= new string[]{"yuka","hashira","kabe","yane"};
		image.sprite= Resources.Load<Sprite>("Texture/Game/PartsSelectButton/" + buttonName[job] + id);
		image.rectTransform.localPosition	= imagePos;
		image.rectTransform.sizeDelta		= new Vector2(128.0f,128.0f);
		pointerUpSystem[id]		= obj.GetComponent<PointerUpSystem>();
		pointerUpSystem[id].id				= id;
		pointerUpSystem[id].defaultColor	= Color.white;
		pointerUpSystem[id].pressedColor	= Color.yellow;
		pointerUpSystem[id].scrollViewObject= contents.transform.parent.parent.gameObject;
		pointerUpSystem[id].SetCallBackFunc(GetButtonID);
	}

	private	void	GetButtonID(PointerUpSystem pointerUpSystem){//押されたボタンのIDを受け取る
		partsID	= pointerUpSystem.id;
		seManager.Play(6);
		#if DEBUG_GAMESCENE
		Debug.Log("Debug:タッチされたボタンは" + partsID);
		#endif
	}
	
	public	int		GetPartsID(){
		return	partsID;
	}
	
}
#endregion	//パーツセレクトを管理するクラス