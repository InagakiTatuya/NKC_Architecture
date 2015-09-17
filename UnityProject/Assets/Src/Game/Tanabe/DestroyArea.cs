using UnityEngine;
using System.Collections;

public class DestroyArea : MonoBehaviour {
	private GameSceneSystem system;
	void Start () {
		system = transform.root.GetComponent<GameSceneSystem>();
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "Build"){
			system.BuildList.Remove(col.GetComponent<FallObject>());
			DestroyObject(col.gameObject);
		}
	}
}
