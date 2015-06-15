//----------------------------------------------------------
//セレクトのシステム
//更新日 :	06 / 13 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_SELECT
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//セレクトのシステム_Begin//--------------------------------
public	partial	class SelectSystem : MonoBehaviour {

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
			UpdateNeutral,
			UpdateGoNext,
		};
		string[]	tablePrefabName	= new string[]{//プレファブの名前
			"Prefab/Select/BackGround",
			"Prefab/Select/SelectHeader",
		};
		for(int i = 0;i < tablePrefabName.Length;i ++)
			TitleSystem.CreateObujectInCanvas(tablePrefabName[i],canvasObject);
		StartCreateButton();
		CreateHeaderText();
		ChangeState(StateNo.Neutral);
		selectNo	= -1;
		f_timer		= 0.0f;
	}//初期化_End//-----------------------------------------
	
	//更新//////////////////////////////////////////////////
	public	void	Update () {//更新_Begin//---------------
		if(stateNo < 0 || stateNo >= (int)StateNo.Length)	ChangeState(StateNo.Neutral);
		if(updateFunc[stateNo] != null)						updateFunc[stateNo]();
		UpdateButton();
		UpdateFade();
		f_timer		+= Time.deltaTime;
		stateTime	+= Time.deltaTime;
	}//更新_End//-------------------------------------------

	//その他関数////////////////////////////////////////////
	//チュートリアルボタンを押した_Begin//------------------
	public	void	OnTutorialButtonEnter(){
		ChangeState(StateNo.GoNext);
		ButtonCanceler();
		CreateFade();
		selectNo	= 0;
	}//チュートリアルボタンを押した_End//-------------------

	//ゲーム開始ボタンを押した_Begin//----------------------
	public	void	OnMainGameButtonEnter(){
		ChangeState(StateNo.GoNext);
		ButtonCanceler();
		CreateFade();
		selectNo	= 1;
	}//ゲーム開始ボタンを押した_End//-----------------------

}//セレクトのシステム_End//---------------------------------
