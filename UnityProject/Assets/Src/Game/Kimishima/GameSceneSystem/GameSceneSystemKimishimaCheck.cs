﻿//----------------------------------------------------------
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
	private	float	guraCreateTime;
	private	int		guraCount	= 0;

	//更新//////////////////////////////////////////////////
	//チェック用の関数//------------------------------------
	private void UpdateCheckKimishima(){
		if(Time.time - guraCreateTime > 1.1f){
			guraCreateTime			= Time.time;
			guraCount				= (guraCount + 1) % 2;
			Transform	trans		= Camera.main.transform;
			Vector3[,]	offset		= new Vector3[,]{
				{
					trans.forward *  80 + trans.right * -12.0f + trans.up * -14.0f,
					trans.forward * 100 + trans.right * 12.0f + trans.up * 14.0f
				},
				{
					trans.forward * 100 + trans.right * -6.0f + trans.up *  28.0f,
					trans.forward *  80 + trans.right * 6.0f + trans.up * -28.0f
				}
			};
			for(int i = 0;i < 2;i ++){
				GameObject	obj			= Instantiate(textEffectPrefab);
				TextEffectManager	te	= obj.GetComponent<TextEffectManager>();
				te.targetObject			= Camera.main.gameObject;
				te.transform.position	= trans.position + offset[guraCount,i];
				te.id					= TextEffectManager.EffectID.Gura;
				Debug.Log(te.targetObject.name);
			}
		}
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
			ChangeState(StateNo.CardView);
			//ChangeState(StateNo.PartsSelect);
		}
		return	true;
	}
	private	void	UpdateCheckKimishimaCompleteCamera(){
		if(cameraMove == null)	return;
		GameObject	obj	= buildList[buildList.Count - 9].gameObject;
		Debug.Log(obj.transform.position);
		Vector3	pos		= buildList[buildList.Count - 9].gameObject.transform.position;
		Vector3	look	= new Vector3(0,pos.y + 100,0);
		Vector3	at		= new Vector3(60,pos.y + 50,60);
		cameraMove.look	= look;
		cameraMove.at	= at;
		cameraMove.maxY	= look.y + 50.0f;
		Debug.Log(look);
	}
	//落下フラグを反映//------------------------------------
	void	SetCollapseFlg(){
		collapseFlg	= true;
	}

	//GameOver//--------------------------------------------
	private void UpdateGameOverKimishima(){
		if(!gassharnFlg){
			GameObject	obj			= Instantiate(textEffectPrefab);
			TextEffectManager	te	= obj.GetComponent<TextEffectManager>();
			te.targetObject			= Camera.main.gameObject;
			te.transform.position	= Camera.main.transform.position + Camera.main.transform.forward * 80;
			te.id					= TextEffectManager.EffectID.Gasshan;
			gassharnFlg	= true;
			for(int i = 0;i < buildList.Count;i ++){
				if(buildList[i] == null)	continue;
				buildList[i].seManagere		= seManager;
				buildList[i].gameOverFlg	= true;
			}
			seManager.Play(7,1.0f,0.5f);
		}
		if(stateTime < 0.25f){
		//	float	pitch		= Random.Range(0.125f,0.25f);
			if(!seManager.isPlaying(9))
				seManager.Play(9,crackVolume,0.25f);
			crackVolume *= 0.9f;
		}
		if(cameraShakeCount == 0){
			Vector3	shake;
			shake.x	= Random.Range(-1,1) * cameraShakePow * 4.0f;
			shake.z	= Random.Range(-1,1) * cameraShakePow * 4.0f;
			shake.y	= Random.Range(-1,1) * cameraShakePow * 4.0f;
			cameraMove.shake	= shake;
			Vector3	up;
			up.x	= Random.Range(-1,1) * cameraShakePow / 16.0f;
			up.z	= Random.Range(-1,1) * cameraShakePow / 16.0f;
			up.y	= 1.0f;
			cameraMove.up		= up.normalized;
			cameraShakePow		*= 0.9f;
		}
		cameraShakeCount	= (cameraShakeCount + 1) % cameraShakeTiming;
		if(stateTime >= 3.0f){
			gameOverFlg	= true;
			ChangeState(StateNo.Result);
			cameraMove.shake	= Vector3.zero;
			cameraMove.up		= Vector3.up;
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
	public	bool	gameOverFlg{
		get{
			return (checkFlg & 0x00000004) != 0x00000000;
		}
		set{
			checkFlg	&= ~0x00000004;
			if(!value)	return;
			checkFlg	|=  0x00000004;
		}
	}
	public	bool	gassharnFlg{
		get{
			return (checkFlg & 0x00000008) != 0x00000000;
		}
		set{
			checkFlg	&= ~0x00000008;
			if(!value)	return;
			checkFlg	|=  0x00000008;
		}
	}
}//ゲームシーンのシステム_End//-----------------------------