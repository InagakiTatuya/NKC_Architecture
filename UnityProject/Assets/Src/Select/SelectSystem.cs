//----------------------------------------------------------
//セレクトのシステム
//更新日 :	06 / 12 / 2015
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
		TitleSystem.CreateObujectInCanvas("Prefab/Select/BackGround");
		TitleSystem.CreateObujectInCanvas("Prefab/Select/SelectHeader");
		GameObject	buttonObj	= null;
		Button		button		= null;
		buttonObj	= TitleSystem.CreateObujectInCanvas("Prefab/Select/TutorialButton");
		button		= buttonObj.GetComponent<Button>();
		button.onClick.AddListener(this.OnTutorialButtonEnter);
		buttonObj	= TitleSystem.CreateObujectInCanvas("Prefab/Select/MainGameButton");
		button		= buttonObj.GetComponent<Button>();
		button.onClick.AddListener(this.OnMainGameButtonEnter);
		//ChangeState(StateNo.Neutral);
		f_timer	= 0.0f;
	}//初期化_End//-----------------------------------------
	
	//更新//////////////////////////////////////////////////
	public	void	Update () {//更新_Begin//---------------
	
	}//更新_End//-------------------------------------------

	//その他関数////////////////////////////////////////////
	//チュートリアルボタンを押した_Begin//------------------
	public	void	OnTutorialButtonEnter(){
		ChangeState(StateNo.GoNext);
	}//チュートリアルボタンを押した_End//-------------------

	//ゲーム開始ボタンを押した_Begin//----------------------
	public	void	OnMainGameButtonEnter(){
		ChangeState(StateNo.GoNext);
	}//ゲーム開始ボタンを押した_End//-----------------------

}//セレクトのシステム_End//---------------------------------
