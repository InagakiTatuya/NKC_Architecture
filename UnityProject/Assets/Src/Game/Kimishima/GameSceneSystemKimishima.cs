//----------------------------------------------------------
//ゲームシーンのシステム
//更新者 :	君島一刀
//----------------------------------------------------------

#region//プリプロセッサ/////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_GAMESCENE
#endif
#endregion	//プリプロセッサ

#region//名前空間///////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;
#endregion	//名前空間

#region//ゲームシーンのシステム/////////////////////////////
public	partial class GameSceneSystem : MonoBehaviour{

	//変数//////////////////////////////////////////////////
	public	SeManager	seManager	= null;
	public	CameraMove	cameraMove	= null;

	private	Vector2	FLOORWINDOW_POS	= new Vector2(-192.0f,-384.0f);

	//フロア関連
	private	int		floor;
	public	int		GetFloor(){return	floor;}
	private	Image	floorWindow	= null;
	private	Text	floorText	= null;
	private	Vector2	floorSize;

	//パーツ選択関連
	private	int					partsID;
	public	int					PartsID{get{return	partsID;}}
	private	PartsSelectClass	partsSelectClass;
	private	int					jobBuf;

	private	int					prevStateNo;
	private	bool				execute;

	//初期化////////////////////////////////////////////////
	//俺の初期化関数
	private	void	StartKimishimaSystem(){
		resultUpdateFunc	= new UnityAction[]{
			this.UpdateResultOpen,
			this.UpdateResultLiquidation,
			this.UpdateResultNeutral,
			this.UpdateResultNextScene,
			this.UpdateResultHide,
			this.UpdateResultSSBegin,
			this.UpdateResultSS,
			this.UpdateResultSSEnd,
		};
		FallObject.collapseFunc		= this.SetCollapseFlg;
		floor						= 0;
		floorSize					= new Vector2(128.0f,128.0f);
		partsSelectClass			= new PartsSelectClass(this);
		partsSelectClass.Init();
		BackFadeInit();
		StartKimishimaSystemCreateFloorWindow();
		StartKimishimaSystemCreateFloorText();
#if DEBUG_GAMESCENE
		Database.InitColorBlock();
#endif
	}
	//階層ウィンドウを生成
	private	void	StartKimishimaSystemCreateFloorWindow(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		floorWindow			= obj.GetComponent<Image>();
		floorWindow.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorWindow.color	= Color.white;
	}

	//階層テキストを生成
	private	void	StartKimishimaSystemCreateFloorText(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Select/Text",canvasObject);
		floorText			= obj.GetComponent<Text>();
		floorText.rectTransform.localPosition	= FLOORWINDOW_POS;
		floorText.text		= "0";
		floorText.fontSize	= 48;
		floorText.color		= Color.black;
	}

	//更新//////////////////////////////////////////////////
	//俺の更新関数
	private	void	UpdateKimishimaSystem(){
		if(stateNo == (int)StateNo.PartsSelect && prevStateNo != (int)StateNo.PartsSelect){
			jobBuf	= job;
		}
		prevStateNo	= stateNo;
		BackFadeUpdate();
		UpdateFloorWindow();
		PartsSelectClass.MessageID				message	= PartsSelectClass.MessageID.Non;
		if(stateNo == (int)StateNo.PartsSelect)	message	= PartsSelectClass.MessageID.OnPartsSelectState;
		partsSelectClass.Update(message,ref execute,job);
	}

	//パーツ選択ステート
	private	void	UpdatePartsSelect(){
		if(partsSelectClass.yaneFlg){
			if(job == (int)Database.JobID.Yane)	job	= jobBuf;
			else 								job	= (int)Database.JobID.Yane;
			partsSelectClass.yaneFlg	= false;
		}
		floorSize.y	= Mathf.Min((floorSize.y == 0.0f)?1.0f:floorSize.y * 1.2f,128.0f);
		if(partsSelectClass != null)	partsID	= partsSelectClass.GetPartsID();
		if(!execute)	return;
		ChangeState(StateNo.PartsSet);
		execute	= false;
	}

	//階層ウィンドウを更新
	private	void	UpdateFloorWindow(){
		if(floorWindow != null)	floorWindow.rectTransform.sizeDelta	= floorSize;
		if(floorText != null)	floorText.rectTransform.sizeDelta	= floorSize;
	}

	//その他関数////////////////////////////////////////////
	/// <summary>フロアを上げていく</summary>
	private	void	AddFloor(){//階層を進める_Begin//-------
		floor	++;
		floorText.text	= floor.ToString();
	}//階層を進める_End//-----------------------------------

}
#endregion	//ゲームシーンのシステム