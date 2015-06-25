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

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////
	public	GameObject	canvasObject	= null;
	private	static	float	f_timer;
	public	static	float	timer{
		get{return	f_timer;}
	}
	
	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化_Begin//--------------
		updateFunc	= new UpdateFunc[]{//更新関数を初期化
			UpdateGideIn,	UpdateGide,		UpdateGideOut,
			UpdateRuncIn,	UpdateRunc,		UpdateRuncOut,
			UpdateButtonIn,	UpdateButton,	UpdateGoNext,
		};
		string[]	tablePrefabName	= new string[]{//プレファブの名前
			"Prefab/Select/BackGround",
			"Prefab/Select/SelectHeader",
		};
		for(int i = 0;i < tablePrefabName.Length;i ++){
			TitleSystem.CreateObjectInCanvas(tablePrefabName[i],canvasObject);
		}
		CreateHeaderText();
		StartCreateGide();
		StartCreateText();
		StartCreateRuncGide();
		StartCreateRuncText();
		selectNo	= -1;
		ChangeState(StateNo.GideIn);
		f_timer		= 0.0f;
	}//初期化_End//-----------------------------------------
	
	//更新//////////////////////////////////////////////////
	public	void	Update () {//更新_Begin//---------------
		if(stateNo < 0 || stateNo >= (int)StateNo.Length)	ChangeState(StateNo.GideIn);
		if(updateFunc[stateNo] != null)						updateFunc[stateNo]();
		UpdateGideAndText();
		UpdateRuncGideAndRuncText();
		UpdateButtonImage();
		UpdateFade();
		f_timer		+= Time.deltaTime;
		stateTime	+= Time.deltaTime;
	}//更新_End//-------------------------------------------
	
	//その他関数////////////////////////////////////////////
	//リトライボタンを押した_Begin//------------------------
	public	void	OnRetryButtonEnter(){
		ChangeState(StateNo.GoNext);
		ButtonCanceler();
		CreateFade();
		selectNo	= 0;
	}//リトライボタンを押した_End//-------------------------
	
	//セレクトボタンを押した_Begin//------------------------
	public	void	OnSelectButtonEnter(){
		ChangeState(StateNo.GoNext);
		ButtonCanceler();
		CreateFade();
		selectNo	= 1;
	}//セレクトボタンを押した_End//-------------------------

	//社員証ボタンを押した_Begin//--------------------------
	public	void	OnCardInputButtonEnter(){
		ChangeState(StateNo.GoNext);
		ButtonCanceler();
		CreateFade();
		selectNo	= 2;
	}//社員証ボタンを押した_End//---------------------------

}//リザルトのシステム_End//---------------------------------
