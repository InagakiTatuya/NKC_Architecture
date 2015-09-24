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
		AddFloor();
		UpdateCheckKimishimaCompleteCamera();
		//ChangeState(StateNo.CardView);
		ChangeState(StateNo.PartsSelect);
		return	true;
	}
	private	void	UpdateCheckKimishimaCompleteCamera(){
		if(cameraMove == null)	return;
		float	maxY	= buildList[0].transform.position.y;
		for(int i = 1;i < buildList.Count;i ++){
			float	y	= buildList[i].transform.position.y;
			if(maxY >= y)	continue;
			maxY	= y;
		}
		cameraMove.look	= new Vector3( 0.0f,maxY + 100, 0.0f);
		cameraMove.at	= new Vector3(60.0f,maxY + 50,60.0f);
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