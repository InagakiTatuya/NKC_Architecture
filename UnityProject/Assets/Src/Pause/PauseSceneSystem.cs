using UnityEngine;
using System.Collections;

public class PauseSceneSystem : MonoBehaviour
{
	//移動シーン名
	private string[] sceneName = new string[]
	{
		"Game",		//リトライ
		"Select",	//セレクト
	};

	//初期化
	private void Awake()
	{
		gameObject.SetActive(false);
	}

	//ポーズシーンコール
	public void CallPauseGUI()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}

	//リトライシーンボタンコール
	public void CallRetryScene()
	{
		Application.LoadLevel(sceneName[0]);
	}

	//セレクトシーンボタンコール
	public void CallSelectScene()
	{
		Application.LoadLevel(sceneName[1]);
	}
}
