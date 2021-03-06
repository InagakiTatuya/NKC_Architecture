﻿//----------------------------------------------------------
//タイトルのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_TITLE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//タイトルのシステム_Begin//--------------------------------
public	partial class TitleSystem : MonoBehaviour{

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////
    private static  bool    screenSizeChanged   = false;
	public	GameObject	canvasObject	= null;
	public	SeManager	seManager		= null;
	public	Material	textMaterial	= null;
	private	static	float	f_timer;
	public	static	float	timer{
		get{return	f_timer;}
	}

	//初期化////////////////////////////////////////////////
	public	void	Start(){//初期化_Begin//----------------
		Database.InitColorBlock();
        ScreenSizeChange();
		updateFunc	= new UpdateFunc[]{//更新関数を初期化
			UpdateNeutral,
			UpdateGoNext,
		};
		string[]	tablePrefabName	= new string[]{//プレファブの名前
			"Prefab/Title/TitleBackGround",
			"Prefab/Title/TitleLogo",
		};
		for(int i = 0;i < tablePrefabName.Length;i ++)
			TitleSystem.CreateObjectInCanvas(tablePrefabName[i],canvasObject);
		StartCreateButton();
		StartCreateText();
		ChangeState(StateNo.Neutral);
		f_timer	= 0.0f;
	}//初期化_End//-----------------------------------------

    private void    ScreenSizeChange(){
        if(screenSizeChanged)   return;
        Screen.SetResolution(408,720,false);
        screenSizeChanged   = true;
    }

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		if(stateNo < 0 || stateNo >= (int)StateNo.Length)	ChangeState(StateNo.Neutral);
		if(updateFunc[stateNo] != null)						updateFunc[stateNo]();
		UpdateFade();
		f_timer		+= Time.deltaTime;
		stateTime	+= Time.deltaTime;
	}//更新_End//-------------------------------------------

	//その他関数////////////////////////////////////////////

	//プレファブを生成する_Begin//--------------------------
	public	static	GameObject	CreateObjectInCanvas(string fileName,GameObject parent){
		GameObject	bgd	= Instantiate(Resources.Load<GameObject>(fileName));
		bgd.transform.SetParent(parent.transform,false);
		return	bgd;
	}//プレファブを生成する_End//---------------------------

}//タイトルのシステム_End//---------------------------------
