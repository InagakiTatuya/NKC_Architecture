using UnityEngine;
using System.Collections;

public	partial class GameSceneSystem : MonoBehaviour{

	/// <summary>キャンバスを持つオブジェクト</summary>
	public	GameObject	canvasObject	= null;
	
	void Awake()
	{

	}

	// Use this for initialization
	void Start()
	{
		StartKimishimaSystem();
		StartTanabe();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateKimishimaSystem();
		UpdateTanabe();
	}
}
