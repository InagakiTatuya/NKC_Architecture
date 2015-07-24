//----------------------------------------------------------
//ゲームシーンのシステムゲームオーバー
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
	public	bool		completeFlg = false;
	private	bool		collapseFlg	= false;

	//更新//////////////////////////////////////////////////
	//チェック用の関数_Begin//------------------------------
	private void UpdateCheckKimishima(){
		if (completeFlg){
			job	= (job + 1) % 3;
			if(job == 0)	AddFloor();
			completeFlg = false;
			//ChangeState(StateNo.CardView);
			ChangeState(StateNo.PartsSelect);
		}
	}//チェック用の関数_End//-------------------------------

	//落下フラグを反映_Beign//------------------------------
	void	SetCollapseFlg(){
		collapseFlg	= true;
	}//落下フラグを反映_End//-------------------------------

	//GameOverしたら実行しようね。_Begin//------------------
	private void UpdateGameOverKimishima(){
		if(stateTime >= 1.0f){
			ChangeState(StateNo.Result);
			return;
		}
	}//GameOverしたら実行しようね。_End//-------------------

}//ゲームシーンのシステム_End//-----------------------------