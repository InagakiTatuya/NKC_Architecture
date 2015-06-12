//----------------------------------------------------------
//タイトルのシステム
//更新日 :	06 / 11 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_TITLE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//タイトルのシステム_Begin//--------------------------------
public	partial class TitleSystem : MonoBehaviour{

//パブリックフィールド//------------------------------------

	public	GameObject	canvasObject	= null;

	private	static	float	f_timer;
	public	static	float	timer{
		get{return	f_timer;}
	}

	//初期化////////////////////////////////////////////////
	public	void	Start(){//初期化_Begin//----------------
		updateFunc	= new UpdateFunc[]{//更新関数を初期化
			UpdateNeutral,
			UpdateGoNext,
		};
		TitleSystem.CreateObujectInCanvas("Prefab/Title/TitleBackGround");
		TitleSystem.CreateObujectInCanvas("Prefab/Title/TitleLogo");
		GameObject	buttonObj	= TitleSystem.CreateObujectInCanvas("Prefab/Title/StartButton");
		Button		button		= buttonObj.GetComponent<Button>();
		button.onClick.AddListener(this.OnStartButtonEnter);
		ChangeState(StateNo.Neutral);
		f_timer	= 0.0f;
	}//初期化_End//-----------------------------------------

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		if(stateNo < 0 || stateNo >= (int)StateNo.Length)	ChangeState(StateNo.Neutral);
		if(updateFunc[stateNo] != null)						updateFunc[stateNo]();
		f_timer		+= Time.deltaTime;
		stateTime	+= Time.deltaTime;
	}//更新_End//-------------------------------------------

	//その他関数////////////////////////////////////////////
	//スタートボタンを押した_Begin//------------------------
	public	void	OnStartButtonEnter(){
		ChangeState(StateNo.GoNext);
	}//スタートボタンを押した_End//-------------------------

	//背景を生成する_Begin//--------------------------------
	public	static	GameObject	CreateObujectInCanvas(string fileName){
		GameObject	obj	= Resources.Load<GameObject>(fileName);
		GameObject	bgd	= (GameObject)Instantiate(obj);
		bgd.transform.SetParent(canvasObject.transform,false);
		return	bgd;
	}//背景を生成する_End//---------------------------------

}//タイトルのシステム_End//---------------------------------
