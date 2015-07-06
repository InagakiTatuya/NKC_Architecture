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

	//変数//////////////////////////////////////////////////
	GameObject	beginObject	= null;
	GameObject	endObject	= null;

	//更新//////////////////////////////////////////////////
	//チェック用の関数_Begin//------------------------------
	private void UpdateCheckKimishima(){
		if(beginObject != null && endObject != null){
			Vector2	beginPos	= beginObject.transform.position;
			Vector2	endPos		= endObject.transform.position;
			float	n			= beginPos.x * endPos.x + beginPos.y * endPos.y;
			float	rad			= Mathf.Acos(n);
		}
		if(stateTime >= 3.0f){
			ChangeState(StateNo.CardView);
			return;
		}
	}//チェック用の関数_End//-------------------------------

	//GameOverしたら実行しようね。
	private void UpdateGameOverKimishima(){
	}

}//ゲームシーンのシステム_End//-----------------------------
