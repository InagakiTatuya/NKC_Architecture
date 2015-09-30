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
	private	int		checkFlg	= 0x00000000;

	//更新//////////////////////////////////////////////////
	//チェック用の関数//------------------------------------
	private void UpdateCheckKimishima(){
		floorSize.y = floorSize.y * 0.8f;
		if(floorSize.y < 1.0f)	floorSize.y	= 0.0f;
		if(UpdateCheckKimishimaCollapse())	return;
		if(UpdateCheckKimishimaComplete())	return;
	}

	//ゲームオーバーの判定//--------------------------------
	private	bool	UpdateCheckKimishimaCollapse(){
		if(!collapseFlg)	return	false;
		ChangeState(StateNo.GameOver);
		return	true;
	}
	//次のパーツ選択へのフラグ//----------------------------
	private	bool	UpdateCheckKimishimaComplete(){
		if(!completeFlg)	return	false;
		job = 0;
		if(roofSetFlag){
			roofSetFlag = false;
			ChangeState(StateNo.Result);
		}else{
			AddFloor();
			UpdateCheckKimishimaCompleteCamera();
			//ChangeState(StateNo.CardView);
			ChangeState(StateNo.PartsSelect);
		}
		return	true;
	}
	private	void	UpdateCheckKimishimaCompleteCamera(){
		if(cameraMove == null)	return;
		Vector3	pos		= buildList[buildList.Count - 9].gameObject.transform.position;
		Vector3	look	= new Vector3(0,pos.y + 100,0);
		Vector3	at		= new Vector3(60,pos.y + 50,60);
		cameraMove.look	= look;
		cameraMove.at	= at;
	}
	//落下フラグを反映//------------------------------------
	void	SetCollapseFlg(){
		collapseFlg	= true;
	}

	//GameOver//--------------------------------------------
	private void UpdateGameOverKimishima(){
		if(stateTime >= 1.0f){
			ChangeState(StateNo.Result);
			return;
		}
	}

	//プロパティ//------------------------------------------
	public	bool	completeFlg{
		get{
			return (checkFlg & 0x00000001) != 0x00000000;
		}
		set{
			checkFlg	&= ~0x00000001;
			if(!value)	return;
			checkFlg	|=  0x00000001;
		}
	}
	public	bool	collapseFlg{
		get{
			return (checkFlg & 0x00000002) != 0x00000000;
		}
		set{
			checkFlg	&= ~0x00000002;
			if(!value)	return;
			checkFlg	|=  0x00000002;
		}
	}
}//ゲームシーンのシステム_End//-----------------------------