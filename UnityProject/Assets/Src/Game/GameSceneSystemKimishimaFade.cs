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

	//列挙/////////////////////////////////////////////////
	//フェードステート_Begin//------------------------------
	private	enum	BackFadeStateNo{
		FadeIn,
		Hide,
		FadeOut,
		Black,
	};//フェードステート_End//------------------------------

	private	UnityAction[]	tableBackFade;
	//変数/////////////////////////////////////////////////
	private	Image	backFadeImage;
	private	Color	backFadeColor;
	private	int		backFadeStateNo;
	private	float	backFadeTimer;

	//初期化///////////////////////////////////////////////
	//初期化_Begin//---------------------------------------
	private	void	BackFadeInit(){
		backFadeColor	= Color.black;
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Game/Fade",canvasObject);
		backFadeImage	= obj.GetComponent<Image>();
		tableBackFade	= new UnityAction[]{
			this.BackFadeUpdateFadeIn,
			this.BackFadeUpdateHide,
			this.BackFadeUpdateFadeOut,
			this.BackFadeUpdateBlack,
		};
	}//初期化_End//----------------------------------------

	//更新/////////////////////////////////////////////////
	//更新_Begin//----------------------------------------
	private	void	BackFadeUpdate(){
		if(tableBackFade[backFadeStateNo] != null)	tableBackFade[backFadeStateNo]();
		backFadeImage.color	= backFadeColor;
		backFadeTimer	+= Time.deltaTime;
	}//更新_End//-----------------------------------------

	//フェードイン_Beign//---------------------------------
	private	void	BackFadeUpdateFadeIn(){
		float	n		= Mathf.Max(backFadeTimer * 4.0f,1.0f);
		backFadeColor.a	= 1.0f - n;
		if(n >= 1.0f)	ChangeBackFadeState(BackFadeStateNo.Hide);
	}//フェードイン_End//----------------------------------

	//見えない_Beign//------------------------------------
	private	void	BackFadeUpdateHide(){
		backFadeColor.a	= 0.0f;
	}//見えない_End//-------------------------------------

	//フェードアウト_Beign//-------------------------------
	private	void	BackFadeUpdateFadeOut(){
		float	n		= Mathf.Max(backFadeTimer * 4.0f,1.0f);
		backFadeColor.a	= n;
		if(n >= 1.0f)	ChangeBackFadeState(BackFadeStateNo.Black);
	}//フェードアウト_End//--------------------------------

	//見えない_Beign//------------------------------------
	private	void	BackFadeUpdateBlack(){
		backFadeColor.a	= 1.0f;
	}//見えない_End//-------------------------------------

	//その他関数///////////////////////////////////////////
	//フェードステートを遷移する_Beign//---------------------
	private	void	ChangeBackFadeState(BackFadeStateNo stateNo){
		int		value	= (int)stateNo;
		if(backFadeStateNo == value)	return;
		backFadeStateNo	= value;
		backFadeTimer	= 0.0f;
	}//フェードステートを遷移する_End//----------------------

}//ゲームシーンのシステム_End//------------------------------