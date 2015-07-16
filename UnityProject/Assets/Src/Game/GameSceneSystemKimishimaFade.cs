//----------------------------------------------------------
//ゲームシーンのシステムフェードアウト
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

	//変数/////////////////////////////////////////////////
	private	FadeClass	fadeClass;

	//初期化///////////////////////////////////////////////
	//初期化_Begin//---------------------------------------
	private	void	BackFadeInit(){
		fadeClass	= new FadeClass(this,canvasObject);
		fadeClass.Init();
	}//初期化_End//----------------------------------------

	//更新/////////////////////////////////////////////////
	//更新_Begin//----------------------------------------
	private	void	BackFadeUpdate(){
		fadeClass.Update();
	}//更新_End//-----------------------------------------

}//ゲームシーンのシステム_End//------------------------------

//----------------------------------------------------------
//フェードアウト
//更新日 :	07 / 16 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

class	FadeClass{//フェードを管理するクラス_Begin//---------

	//列挙/////////////////////////////////////////////////
	//フェードステート_Begin//------------------------------
	public	enum	BackFadeStateNo{
		FadeIn,
		Hide,
		FadeOut,
		Black,
	};//フェードステート_End//------------------------------
	
	private	UnityAction[]	tableBackFade;
	//変数/////////////////////////////////////////////////
	private	Image			backFadeImage;
	private	Color			backFadeColor;
	private	int				backFadeStateNo;
	private	float			backFadeTimer;
	private	MonoBehaviour	sceneSystem;
	private	GameObject		canvasObject;

	//コンストラクタ・デストラクタ///////////////////////////
	//コンストラクタ_Begin//---------------------------------
	public	FadeClass(MonoBehaviour sceneSystem,GameObject canvasObject){
		this.sceneSystem	= sceneSystem;
		this.canvasObject	= canvasObject;
	}//コンストラクタ_End//----------------------------------

	//初期化/////////////////////////////////////////////////
	//初期化_Begin//-----------------------------------------
	public	void	Init(){
		backFadeColor	= Color.black;
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Game/Fade",canvasObject);
		backFadeImage	= obj.GetComponent<Image>();
		tableBackFade	= new UnityAction[]{
			this.BackFadeUpdateFadeIn,
			this.BackFadeUpdateHide,
			this.BackFadeUpdateFadeOut,
			this.BackFadeUpdateBlack,
		};
	}//初期化_End//------------------------------------------
	
	//更新///////////////////////////////////////////////////
	//更新_Begin//-------------------------------------------
	public	void	Update(){
		if(tableBackFade[backFadeStateNo] != null)	tableBackFade[backFadeStateNo]();
		backFadeImage.color	= backFadeColor;
		backFadeTimer	+= Time.deltaTime;
	}//更新_End//--------------------------------------------

	//フェードイン_Beign//---------------------------------
	private	void	BackFadeUpdateFadeIn(){
		float	n		= Mathf.Max(backFadeTimer * 4.0f,0.5f);
		backFadeColor.a	= 0.5f - n;
		if(n >= 0.5f)	ChangeBackFadeState(BackFadeStateNo.Hide);
	}//フェードイン_End//----------------------------------
	
	//見えない_Beign//------------------------------------
	private	void	BackFadeUpdateHide(){
		backFadeColor.a	= 0.0f;
	}//見えない_End//-------------------------------------
	
	//フェードアウト_Beign//-------------------------------
	private	void	BackFadeUpdateFadeOut(){
		float	n		= Mathf.Max(backFadeTimer * 4.0f,0.5f);
		backFadeColor.a	= n;
		if(n >= 0.5f)	ChangeBackFadeState(BackFadeStateNo.Black);
	}//フェードアウト_End//--------------------------------
	
	//見えない_Beign//------------------------------------
	private	void	BackFadeUpdateBlack(){
		backFadeColor.a	= 0.5f;
	}//見えない_End//-------------------------------------
	
	//その他関数///////////////////////////////////////////
	//フェードステートを遷移する_Beign//---------------------
	public	void	ChangeBackFadeState(BackFadeStateNo stateNo){
		int		value	= (int)stateNo;
		if(backFadeStateNo == value)	return;
		backFadeStateNo	= value;
		backFadeTimer	= 0.0f;
	}//フェードステートを遷移する_End//----------------------

}//フェードを管理するクラス_End//----------------------------