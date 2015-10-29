using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PauseSceneSystem : MonoBehaviour{

	//移動シーン名
	private string[] sceneName = new string[]{
		"Game",		//リトライ
		"Select",	//セレクト
	};

	enum STATE {
		Retry,
		Select,
		None,
	};
	private STATE			state;

	enum WINDOWSTATE {
		Open,
		Close,
		Neutral,
		None,
	};
	private WINDOWSTATE		windowState,prevWindowState;

	private GameSceneSystem system;
	private	UnityAction		collBack;
	private bool			fadeCompleteFlag;

	private	Transform[]		childObj;
	private	float			time;
	private	UnityAction[]	windowUpdate;
	private	Vector3			windowSize;

	private void Start(){
		windowUpdate = new UnityAction[]{
			this.Open,
			this.Close,
			null,
			null,
		};

		childObj		=	new Transform[transform.childCount];
		for(int i=0;i<transform.childCount;i++)	childObj[i]	=	transform.GetChild(i);

		windowSize		=	Vector3.one;
		windowState		=	WINDOWSTATE.None;
		prevWindowState	=	WINDOWSTATE.None;
		state			=	STATE.None;
		system			=	transform.root.GetComponent<GameSceneSystem>();
		system.Pause	=	false;

		time			=	0;

		gameObject.SetActive(false);
	}

	private void Update(){
		if(fadeCompleteFlag)	Application.LoadLevel(sceneName[(int)state]);
		if(windowState	!=	prevWindowState){
			time			=	0;
			prevWindowState	=	windowState;
		}
		if(windowUpdate[(int)windowState] != null)	windowUpdate[(int)windowState]();
		time			+=	Time.deltaTime;
	}

	private void Open(){
		float	n		= time * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n >= 1.0f){
			windowState	=	WINDOWSTATE.Neutral;
		}
		for(int i=0;i<transform.childCount;i++)	childObj[i].localScale	=	windowSize;
	}

	private void Close(){
		float	n		= 1.0f - time * 8.0f;
		windowSize.x	= Mathf.Max(2.0f - n,1.0f);
		windowSize.y	= Mathf.Min(n,1.0f);
		if(n <= 0.0f){
			windowState		=	WINDOWSTATE.None;
			system.Pause	=	false;
			gameObject.SetActive(false);
		}
		for(int i=0;i<transform.childCount;i++)	childObj[i].localScale	=	windowSize;
	}

	//ポーズシーンコール
	public void CallPauseGUI(){
		if(!gameObject.activeSelf)	{
			windowState	=	WINDOWSTATE.Open;
			system.GetFadeClass().ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeOut);
			system.Pause = true;
			gameObject.SetActive(true);
		}else{
			windowState	=	WINDOWSTATE.Close;
			system.GetFadeClass().ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeIn);
		}
	}

	//リトライシーンボタンコール
	public void CallRetryScene(){
		state =	STATE.Retry;
		system.FadeObj.setFadeOut(this.FadeOutComplete);
	}

	//セレクトシーンボタンコール
	public void CallSelectScene(){
		state =	STATE.Select;
		system.FadeObj.setFadeOut(this.FadeOutComplete);
	}

	void FadeOutComplete(){
		fadeCompleteFlag = true;
	}
}
