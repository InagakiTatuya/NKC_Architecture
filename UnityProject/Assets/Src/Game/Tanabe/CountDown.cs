using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	private GameSceneSystem system;
	private Text text;

	private int viewTimer;
	private float time;

	void Start () {
		system = transform.root.GetComponent<GameSceneSystem>();
		text = GetComponent<Text>();
		Reset();
	}

	void Update(){
		if (system.Pause) return;
		if (system.stateNo == (int)GameSceneSystem.StateNo.Check){
			text.text = "" + viewTimer;
			text.fontSize = (int)(280 * time);
			if (time >= 1.0f){
				system.seManager.Play(2);
				viewTimer--;
				time = 0;
				if (viewTimer <= 0){
					Reset();
					system.completeFlg = true;
				}
			}
			time += Time.deltaTime;
		}
		else Reset();
	}

	void Reset(){
		text.text = "";
		time = 0;
		viewTimer = 3;
	}
}
