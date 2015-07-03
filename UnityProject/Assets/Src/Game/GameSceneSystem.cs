using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public	partial class GameSceneSystem : MonoBehaviour{

	//列挙/////////////////////////////////////////////////
	public	enum	StateNo : int{//ステート番号_Begin//---
		Intro,			//イントロ
		CardView,		//カード表示
		PartsSelect,	//パーツ選択
		PartsSet,		//パーツをセット
		Check,			//チェック
		GameOver,		//ゲーム終了
		Result,			//リザルト
		PauseBegin,		//ポーズ開始
		Pause,			//ポーズ
		PauseEnd,		//ポーズ終了
	}//ステート番号_End//----------------------------------

	//変数/////////////////////////////////////////////////
	/// <summary>キャンバスを持つオブジェクト</summary>
	public	GameObject	canvasObject	= null;

	public	int		stateNo;
	public	float	stateTime;

	private bool	chengedFlag;
	
	void Awake(){
		AwakeInagaki();
	}

	//初期化///////////////////////////////////////////////
	void Start (){//初期化_Begin//-------------------------
		updateFunc	= new UnityAction[]{
			this.UpdateIntro,
			this.UpdateCardView,
			this.UpdatePartsSelect,
			this.UpdatePartsSet,
			this.UpdateCheck,
			this.UpdateGameOver,
			this.UpdateResult,
			this.UpdatePauseBegin,
			this.UpdatePause,
			this.UpdatePauseEnd,
		};
		StartStateInit();
		StartInagaki();
		StartKimishimaSystem();
		StartTanabe();
	}//初期化_End//----------------------------------------

	//ステート番号を初期化_Begin//-------------------------
	private	void	StartStateInit(){
		stateNo		= (int)StateNo.Intro;
		stateTime	= 0.0f;
	}//ステート番号を初期化_End//--------------------------

	//更新/////////////////////////////////////////////////
	private	UnityAction[]	updateFunc;
	void Update (){//更新_Begin//--------------------------
		if(updateFunc[stateNo] != null)	updateFunc[stateNo]();
		UpdateInagaki();
		UpdateKimishimaSystem();
		UpdateTanabe();
		if (chengedFlag)	chengedFlag = false;
		else				stateTime += Time.deltaTime;
	}//更新_End//------------------------------------------

	//その他関数///////////////////////////////////////////
	//ステート遷移_Beign//---------------------------------
	private	void	ChangeState(StateNo value,bool overrapFlg = false){
		chengedFlag = true;
		int	buf		= (int)value;
		if(!overrapFlg && stateNo == buf)	return;
		stateNo		= buf;
		stateTime	= 0.0f;
	}//ステート遷移_End//----------------------------------
}
