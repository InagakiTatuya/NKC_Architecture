//----------------------------------------------------------
//タイトルのシステム
//更新日 :	06 / 05 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_TITLE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//タイトルのシステム_Begin//--------------------------------
public	class TitleSystem : MonoBehaviour{

//パブリックフィールド//------------------------------------

	public	GameObject	canvasObject	= null;
	private	Canvas		canvas			= null;

	private	static	float	f_timer;
	public	static	float	timer{
		get{return	f_timer;}
	}

	public	void	Start(){//初期化_Begin//----------------
		canvas	= canvasObject.GetComponent<Canvas>();
#if	DEBUG_TITLE
		CreateObujectInCanvas("Title/Debug/DebugBackGround");
#endif
		CreateObujectInCanvas("Title/StartButton");
		f_timer	= 0.0f;
	}//初期化_End//-----------------------------------------

	public	void	Update(){//更新_Beign//-----------------
		f_timer	+= Time.deltaTime;
	}//更新_End//-------------------------------------------

//プライベートフィールド//----------------------------------

	//背景を生成する_Begin//--------------------------------
	private	void	CreateObujectInCanvas(string fileName){
		GameObject	obj	= Resources.Load<GameObject>(fileName);
		GameObject	bgd	= (GameObject)Instantiate(obj);
		bgd.transform.SetParent(canvasObject.transform,false);
	}//背景を生成する_End//---------------------------------

}//タイトルのシステム_End//---------------------------------
