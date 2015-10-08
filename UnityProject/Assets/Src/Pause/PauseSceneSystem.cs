using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PauseSceneSystem : MonoBehaviour
{
	enum STATE {
		Retry,
		Select,
		None,
	};
	private STATE			state;

	private GameSceneSystem system;
	private	UnityAction		collBack;
	private bool			fadeCompleteFlag;

	//移動シーン名
	private string[] sceneName = new string[]{
		"Game",		//リトライ
		"Select",	//セレクト
	};

	private void Start(){
		state			=	STATE.None;
		system			=	transform.root.GetComponent<GameSceneSystem>();
		system.Pause	=	false;
		gameObject.SetActive(false);
	}

	private void Update(){
		if(fadeCompleteFlag)	Application.LoadLevel(sceneName[(int)state]);
	}

	//ポーズシーンコール
	public void CallPauseGUI(){
		if(!gameObject.activeSelf)	system.GetFadeClass().ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeOut);
		else						system.GetFadeClass().ChangeBackFadeState(FadeClass.BackFadeStateNo.FadeIn);
		system.Pause = !system.Pause;
		gameObject.SetActive(!gameObject.activeSelf);
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
