using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameSceneSystem : MonoBehaviour{
	private GameObject pauseGUI;

	private	bool	startFadeFlag;

	private bool	debugUnBreakFlag;
	public	bool	DebugUnBreakFlag	{get{ return debugUnBreakFlag; } set{ debugUnBreakFlag = value; }}

	private bool	debugCollapseFlag;
	public	bool	DebugCollapseFlag	{get{ return debugCollapseFlag; } set{ debugCollapseFlag = value; }}

	private bool	pause;
	public	bool	Pause	{get{ return pause; } set{ pause = value; }}

	private bool	check;
	public	bool	Check	{get{ return check; } set{ check = value; }}
	
	private bool	roofSetFlag;
	public	bool	RoofSetFlag	{get{ return roofSetFlag; } set{ roofSetFlag = value; }}

	private bool	partsSet;
	public	bool	PartsSet	{get{ return partsSet; } set{ partsSet = value; }}

	private	FadeManager	fadeObj;
	public	FadeManager	FadeObj	{get{ return fadeObj; } set{ fadeObj = value; }}

	private List<FallObject>	buildList;
	public	List<FallObject>	BuildList {get{return buildList;} set{buildList = value;}}


	void StartTanabe(){
		buildList			=	new List<FallObject>();
		pauseGUI			=	transform.GetChild(3).gameObject;
		fadeObj				=	transform.GetChild(10).GetChild(0).GetComponent<FadeManager>();

		pause				=	false;
		check				=	false;
		roofSetFlag			=	false;
		partsSet			=	false;
		startFadeFlag		=	false;
		debugCollapseFlag	=	false;
		debugUnBreakFlag	=	false;
	}

	//更新関数
	void UpdateTanabe(){
		if(!startFadeFlag){
			fadeObj.setFadeIn();
			startFadeFlag = true;
		}
		if(pauseGUI.activeSelf){
			if(stateNo == (int)StateNo.GameOver || 
				stateNo == (int)StateNo.Result) pauseGUI.SetActive(false);
		}
	}

	//初期化
	private void UpdateIntro(){
		ChangeState(StateNo.CardView);
	}

	//パーツ配置
	private void UpdatePartsSet(){
		if(check){
			check = false;
			ChangeState(StateNo.Check);
		}
		if(partsSet){
			job++;
			partsSet = false;
			ChangeState(StateNo.CardView);
		}
	}

	//建物倒壊チェック
	private void UpdateCheck(){
		UpdateCheckKimishima();
	}

	//建物倒壊
	private void UpdateGameOver(){
		UpdateGameOverKimishima();
	}

	//ポーズ初期化
	private void UpdatePauseBegin(){

	}

	//ポーズ実行中
	private void UpdatePause(){

	}

	//ポーズ終わり
	private void UpdatePauseEnd(){

	}

	public void DebugCollapse(){
		DebugCollapseFlag = true;
	}

	public void DebugUnBreak(){
		DebugUnBreakFlag = true;
	}
}
