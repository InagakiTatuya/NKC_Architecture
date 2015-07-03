using UnityEngine;
using System.Collections;

public class PartsSetManager : MonoBehaviour
{
	private GameSceneSystem system;
	private GameObject touchAbleArea;

	void Start()
	{
		system = transform.root.GetComponent<GameSceneSystem>();
		touchAbleArea = GameObject.Find("BACK_GAMEGUI").transform.GetChild(1).gameObject;
		touchAbleArea.SetActive(false);
	}

	void Update()
	{
		if (system.stateNo == (int)GameSceneSystem.StateNo.PartsSet)
		{
			Debug.Log(system.stateTime);
			if (system.stateTime == 0) touchAbleArea.SetActive(true);
		}
	}
}
