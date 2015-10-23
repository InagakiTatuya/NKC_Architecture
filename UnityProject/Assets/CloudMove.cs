using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour {
	
	private GameSceneSystem system;

	[SerializeField]
	private float rotSpeed;

	void Start () {
		system	=	transform.root.GetComponent<GameSceneSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!system.Pause)	transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
	}
}
