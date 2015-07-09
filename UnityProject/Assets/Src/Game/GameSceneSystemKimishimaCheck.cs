//----------------------------------------------------------
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
			job	= (job + 1) % 3;
			if(job == 0){
				Vector3	cameraPos				= Camera.main.transform.position;
				cameraPos.y						= endObject.transform.position.y + 30.0f;
				Camera.main.transform.position	= cameraPos;
				AddFloor();
			}
			ChangeState(StateNo.CardView);
		}
		if(beginObject != null && endObject != null){
			Vector3	beginPos= beginObject.transform.position;
			Vector3	endPos	= endObject.transform.position;
			endPos	-= beginPos;
			float	deg	= Mathf.Atan2(endPos.y,endPos.x) * 180 / Mathf.PI;
#if	DEBUG_GAMESCENE
			Debug.Log("角度差 : " + deg);
#endif
			if(deg <= 60.0f || deg >= 120.0f)	ChangeState(StateNo.GameOver);
		}
	}//チェック用の関数_End//-------------------------------

	//GameOverしたら実行しようね。_Begin//------------------
	private void UpdateGameOverKimishima(){
		if(stateTime >= 1.0f){
			ChangeState(StateNo.Result);
			return;
		}
	}//GameOverしたら実行しようね。_End//-------------------

}//ゲームシーンのシステム_End//-----------------------------
