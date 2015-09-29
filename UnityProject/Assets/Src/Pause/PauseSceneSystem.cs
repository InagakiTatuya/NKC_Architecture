using UnityEngine;
using System.Collections;

public class PauseSceneSystem : MonoBehaviour
{
	private GameSceneSystem system;

	//移動シーン名
	private string[] sceneName = new string[]{
		"Game",		//リトライ
		"Select",	//セレクト
	};

	//初期化
	private void Start(){
		system = transform.root.GetComponent<GameSceneSystem>();
		system.Pause = false;
		gameObject.SetActive(false);
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
		Application.LoadLevel(sceneName[0]);
	}

	//セレクトシーンボタンコール
	public void CallSelectScene(){
		Application.LoadLevel(sceneName[1]);
	}
}
