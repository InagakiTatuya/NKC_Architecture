//----------------------------------------------------------
//タイトルのシステム
//更新日 :	06 / 08 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_TITLE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//タイトルのシステム_Begin//--------------------------------
public	partial class TitleSystem : MonoBehaviour{

	//プライベートフィールド//------------------------------
	
	enum	StateNo : int{//ステート番号_Beign//------------
		Neutral,
		GoNext,
		Length,
	}//ステート番号_End//-----------------------------------
	
	private	int		stateNo;
	private	float	stateTime;
	
	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;

	private	void	UpdateNeutral(){//通常時の更新_Beign//--
		
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		if(stateTime > 1.0f)	Application.LoadLevel("Select");
	}//次のシーンへ_End//-----------------------------------
	
	//その他関数////////////////////////////////////////////
	//ステート遷移_Begin//----------------------------------
	private	void	ChangeState(StateNo value,bool overrapFlg = false){
		int		buf	= (int)value;
		if(stateNo == buf && !overrapFlg)	return;
		stateNo		= buf;
		stateTime	= 0.0f;
	}//ステート遷移_End//-----------------------------------
	
	//背景を生成する_Begin//--------------------------------
	private	GameObject	CreateObujectInCanvas(string fileName){
		GameObject	obj	= Resources.Load<GameObject>(fileName);
		GameObject	bgd	= (GameObject)Instantiate(obj);
		bgd.transform.SetParent(canvasObject.transform,false);
		return	bgd;
	}//背景を生成する_End//---------------------------------
	
}//タイトルのシステム_End//---------------------------------
