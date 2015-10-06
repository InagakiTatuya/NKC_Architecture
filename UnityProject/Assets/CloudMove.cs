using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour {

	[SerializeField]
	private float rotSpeed;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
	}
}
