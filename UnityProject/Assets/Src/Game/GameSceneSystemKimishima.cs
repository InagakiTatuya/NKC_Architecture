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
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ゲームシーンのシステム_Begin//----------------------------
public	partial class GameSceneSystem : MonoBehaviour{

	//列挙・テーブル//////////////////////////////////////////
	enum	PartsSelectWindowStateNo{//ステート番号_Begin//--
		Open,		//開く
		Neutral,	//通常
		Close,		//閉じる
		Hide,		//隠れている
		Length,		//長さ
	}//ステート番号_End//------------------------------------

	private	UnityAction[]	partsSelectWindowUpdateFunc	= null;
	//変数//////////////////////////////////////////////////
	private	Vector2	FLOORWINDOW_POS	= new Vector2(-192.0f,-384.0f);

	//フロア関連
	private	int		floor;
	private	Image	floorWindow	= null;
	private	Text	floorText	= null;
	private	Vector2	floorSize;

	//パーツ選択関連
	private	int		partsSelectWindowStateNo;
	private	int		partsSelectWindowStateTime;

	//初期化////////////////////////////////////////////////
	//俺の初期化関数_Begin//--------------------------------
	private	void	StartKimishimaSystem(){
		partsSelectWindowUpdateFunc	= new UnityAction[]{
			this.UpdatePartsSelectWindowOpen,
			this.UpdatePartsSelectWindowNeutral,
			this.UpdatePartsSelectWindowClose,
			null,
		};
		floor		= 1;
		floorSize	= new Vector2(128.0f,128.0f);
		StartKimishimaSystemCreateFloorWindow();
		StartKimishimaSystemCreateFloorText();
	}//俺の初期化関数_End//---------------------------------

	//階層ウィンドウを生成_Begin//--------------------------
	private	void	StartKimishimaSystemCreateFloorWindow(){
		GameObject	obj		= TitleSystem.CreateObujectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		floorWindow			= obj.GetComponent<Image>();
		floorWindow.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorWindow.color	= new Color(0.0f,0.5f,0.5f,1.0f);
	}//階層ウィンドウを生成_End//---------------------------

	//階層テキストを生成_Begin//----------------------------
	private	void	StartKimishimaSystemCreateFloorText(){
		GameObject	obj		= TitleSystem.CreateObujectInCanvas("Prefab/Select/Text",canvasObject);
		floorText			= obj.GetComponent<Text>();
		floorText.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorText.text		= "1\n階層目";
		floorText.fontSize	= 48;
		floorText.color		= Color.white;
	}//階層テキストを生成_End//-----------------------------

	//更新//////////////////////////////////////////////////
	//俺の更新関数_Begin//----------------------------------
	private	void	UpdateKimishimaSystem(){
		if(partsSelectWindowStateNo < 0 || partsSelectWindowStateNo >= (int)PartsSelectWindowStateNo.Length)
			ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Hide,true);
		UpdateFloorWindow();
		if(partsSelectWindowUpdateFunc[partsSelectWindowStateNo] != null)
			partsSelectWindowUpdateFunc[partsSelectWindowStateNo]();
	}//俺の更新関数_End//-----------------------------------

	//階層ウィンドウを更新_Begin//--------------------------
	private	void	UpdateFloorWindow(){
		if(floorWindow != null)	floorWindow.rectTransform.sizeDelta	= floorSize;
		if(floorText != null)	floorText.rectTransform.sizeDelta	= floorSize;
	}//階層ウィンドウを更新_End//---------------------------

	//ウィンドウを開く_Beign//------------------------------
	private	void	UpdatePartsSelectWindowOpen(){
	}//ウィンドウを開く_End//-------------------------------

	//待機_Beign//-----------------------------------------
	private	void	UpdatePartsSelectWindowNeutral(){
	}//待機_End//------------------------------------------

	//ウィンドウを閉じる_Beign//-----------------------------
	private	void	UpdatePartsSelectWindowClose(){
	}//ウィンドウを閉じる_End//------------------------------

	//その他関数////////////////////////////////////////////
	/// <summary>フロアを上げていく</summary>
	private	void	AddFloor(){//階層を進める_Begin//-------
		floor	++;
		floorText.text	= "" + floor + "\n階層目";
	}//階層を進める_End//-----------------------------------

	/// <summary>パーツ選択ウィンドウを表示</summary>_Begin//-
	private	void	OpenPartsSelectWindow(){
		ChangePartsSelectWindowStateNo(PartsSelectWindowStateNo.Open);
	}//パーツ選択ウィンドウを表示_End//-----------------------

	//パーツ選択ウィンドウのステート番号を変更する_Begin//------
	private	void	ChangePartsSelectWindowStateNo(
		PartsSelectWindowStateNo stateNo,bool overrapFlg = false){
		int		value	= (int)stateNo;
		if(!overrapFlg && value == partsSelectWindowStateNo)	return;
		partsSelectWindowStateNo	= value;
		partsSelectWindowStateTime	= -1;
	}//パーツ選択ウィンドウのステート番号を変更する_End//-------

	//パーツ選択ウィンドウを生成_Beign//---------------------
	private	void	CreatePartsSelectWindow(){

	}//パーツ選択ウィンドウを生成_End//----------------------

}//ゲームシーンのシステム_End//------------------------------