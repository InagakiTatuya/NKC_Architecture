//----------------------------------------------------------
//セレクトのシステム
//更新日 :	06 / 12 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//セレクトのシステム_Begin//--------------------------------
public	partial	class SelectSystem : MonoBehaviour {

	//プライベートフィールド//------------------------------

	//列挙//////////////////////////////////////////////////
	private	enum	StateNo : int{//ステート番号_Beign//----
		Neutral,
		GoNext,
		Length,
	}//ステート番号_End//-----------------------------------

	private	enum	ButtonNo : int{//ボタン番号_Beign//-----
		TutorialButton,
		MainGameButton,
		Length,
	}//ボタン番号_End//-------------------------------------

	//変数//////////////////////////////////////////////////
	private	int				stateNo;
	private	float			stateTime;
	private	Button[]		button	= null;
	
	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;
	
	private	void	UpdateNeutral(){//通常時の更新_Beign//--
	}//通常時の更新_End//-----------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		if(stateTime > 1.0f)	Application.LoadLevel("CardInput");
	}//次のシーンへ_End//-----------------------------------
	
	//その他関数////////////////////////////////////////////
	//ステート遷移_Begin//----------------------------------
	private	void	ChangeState(StateNo value,bool overrapFlg = false){
		int		buf	= (int)value;
		if(stateNo == buf && !overrapFlg)	return;
		stateNo		= buf;
		stateTime	= 0.0f;
	}//ステート遷移_End//-----------------------------------

	//ボタンを無効化する_Begin//----------------------------
	private	void	ButtonCanceler(){
		for(int i = 0;i < button.Length;i ++)	button[i].enabled	= false;
	}//ボタンを無効化する_End//-----------------------------

}//セレクトのシステム_End//---------------------------------
