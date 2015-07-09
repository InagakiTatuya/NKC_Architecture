﻿//----------------------------------------------------------
//ゲームシーンのシステムゲームオーバー
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

	//変数//////////////////////////////////////////////////
	public	GameObject	beginObject	= null;
	public	GameObject	endObject	= null;

	//更新//////////////////////////////////////////////////
	//チェック用の関数_Begin//------------------------------
	private void UpdateCheckKimishima(){
		if(stateTime >= 3.0f){
			ChangeState(StateNo.CardView);
			return;
		}
		if(beginObject != null && endObject != null){
			Vector2	beginPos	= beginObject.transform.position;
			Vector2	endPos		= endObject.transform.position;
			float	n			= beginPos.x * endPos.x + beginPos.y * endPos.y;
			float	deg			= Mathf.Acos(n) * Mathf.PI;
			if(deg <= 45.0f)	ChangeState(StateNo.GameOver);
		}
	}//チェック用の関数_End//-------------------------------

	//GameOverしたら実行しようね。_Begin//------------------
	private void UpdateGameOverKimishima(){
		bool	nextFlg	= false;
#if DEBUG_GAMESCENE
		Debug.Log("Debug:タッチされたボタンは" + partsID);
#endif
		if(stateTime >= 1.0f && nextFlg){
			ChangeState(StateNo.Result);
			return;
		}
	}//GameOverしたら実行しようね。_End//-------------------

}//ゲームシーンのシステム_End//-----------------------------
