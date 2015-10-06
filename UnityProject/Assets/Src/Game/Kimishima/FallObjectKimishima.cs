//----------------------------------------------------------
//倒壊チェック
//更新日 :	07 / 10 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_GAMESCENE
#endif

//名前空間//////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス////////////////////////////////////////////////////
//落下オブジェクト_Begin//----------------------------------
public partial class FallObject : MonoBehaviour {

	public	static	UnityAction	collapseFunc	= null;
	private	bool	exitFlag;

	//初期化関数//------------------------------------------
	void	StartKimishimaFallObject(){
		exitFlag	=	false;
	}//初期化関数_End//-------------------------------------

	//コリジョンが離れたとき_Begin//------------------------
	void	OnCollisionExit(){
		if(system.stateNo != (int)GameSceneSystem.StateNo.Check) return;
		if(!exitFlag)	exitFlag = true;
		return;
#if	DEBUG_GAMESCENE
		Debug.Log(gameObject.name + " が倒壊!!");
#endif
	}//コリジョンが離れたとき_End//-------------------------

}//落下オブジェクト_End//-----------------------------------
