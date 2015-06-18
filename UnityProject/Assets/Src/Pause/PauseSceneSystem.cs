using UnityEngine;
using System.Collections;

public class PauseSceneSystem : MonoBehaviour
{
	private string[] sceneName = new string[]
	{
		"Game",
		"Select"
	};

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void CallRetryScene()
	{
		Application.LoadLevel(sceneName[0]);
	}
	public void CallSelectScene()
	{
		Application.LoadLevel(sceneName[1]);
	}
	public void CallPauseGUI()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}
}
