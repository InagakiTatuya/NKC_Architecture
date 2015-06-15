//----------------------------------------------------------
//ゲームシーンのシステム
//更新日 :	06 / 15 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_GAMESCENE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ゲームシーンのシステム_Begin//----------------------------
public	partial class GameSceneSystem : MonoBehaviour{

	//変数//////////////////////////////////////////////////
	private	Vector2	FLOORWINDOW_POS	= new Vector2(-192.0f,-384.0f);

	private	int		floor;
	private	Image	floorWindow	= null;
	private	Text	floorText	= null;
	private	Vector2	floorSize;

	//初期化////////////////////////////////////////////////
	//俺の初期化関数_Begin//--------------------------------
	private	void	StartKimishimaSystem(){
		floor		= 1;
		floorSize	= new Vector2(128.0f,128.0f);
		StartKimishimaSystemCreateFloorWindow();
		StartKimishimaSystemCreateFloorText();
	}//俺の初期化関数_End//---------------------------------

	//階層ウィンドウを生成_Begin//--------------------------
	private	void	StartKimishimaSystemCreateFloorWindow(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		floorWindow	= obj.GetComponent<Image>();
		floorWindow.rectTransform.localPosition	= FLOORWINDOW_POS;
	}//階層ウィンドウを生成_End//---------------------------

	//階層テキストを生成_Begin//----------------------------
	private	void	StartKimishimaSystemCreateFloorText(){
		GameObject	obj	= TitleSystem.CreateObujectInCanvas("Prefab/Select/Text",canvasObject);
		floorText	= obj.GetComponent<Text>();
		floorText.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorText.text		= "1\n階層目";
		floorText.fontSize	= 48;
		floorText.color		= Color.white;
	}//階層テキストを生成_End//-----------------------------
	
	//更新//////////////////////////////////////////////////
	//俺の更新関数_Begin//----------------------------------
	private	void	UpdateKimishimaSystem(){
		UpdateFloorWindow();
	}//俺の更新関数_End//-----------------------------------

	//階層ウィンドウを更新_Begin//--------------------------
	private	void	UpdateFloorWindow(){
		if(floorWindow != null)	floorWindow.rectTransform.sizeDelta	= floorSize;
		if(floorText != null)	floorText.rectTransform.sizeDelta	= floorSize;
	}//階層ウィンドウを更新_End//---------------------------

	//その他関数////////////////////////////////////////////
	/// <summary>フロアを上げていく</summary>
	private	void	AddFloor(){//階層を進める_Begin//-------
		floor	++;
		floorText.text	= "" + floor + "\n階層目";
	}//階層を進める_End//-----------------------------------

}//ゲームシーンのシステム_End//-----------------------------