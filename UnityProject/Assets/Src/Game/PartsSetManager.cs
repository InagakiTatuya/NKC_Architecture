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
	}

	void Update()
	{
		if (system.stateNo == (int)GameSceneSystem.StateNo.PartsSet)
		{
		}
	}
}
