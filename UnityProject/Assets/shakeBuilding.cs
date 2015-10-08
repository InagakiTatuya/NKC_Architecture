using UnityEngine;
using System.Collections;

public class shakeBuilding : MonoBehaviour {

	GameSceneSystem system;
	Quaternion q;
	Vector3 rot,rot2;
	float time,timer,timerMax=3.0f;
	float shakeSynthesisRate;
	float shakePower;
	bool b;
	bool reverse;

	// Use this for initialization
	void Start () {
		reverse = false;
		b = false;
		timer = timerMax;
		time = 0;
		system = transform.root.GetComponent<GameSceneSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!b){
			if(system.BuildList.Count > 0){
				if(system.BuildList[0].State == FallObject.STATE.Stop){
					transform.position = system.BuildList[0].transform.position;
					b=true;
				}
			}
		}
		if(system.stateNo == (int)GameSceneSystem.StateNo.Check){
			if(!system.DebugCollapseFlag){
				timer -= Time.deltaTime;
				if(!reverse)	time += Time.deltaTime/2;
				else			time -= Time.deltaTime/4;
				if(time > 1.0f || time < 0.0f){
					if(time < 0.0f) rot *= -1;
					reverse = !reverse;
					Mathf.Clamp01(time);
				}
				shakeSynthesisRate = time*(timer/timerMax);
				transform.rotation = Quaternion.Slerp(q,Quaternion.Euler(rot),shakeSynthesisRate);
			}else{
				time += Time.deltaTime/2;
				if(time > 1.0)	Mathf.Clamp01(time);
				shakeSynthesisRate = time*(timer/timerMax);
				transform.rotation = Quaternion.Slerp(q,Quaternion.Euler(rot2),shakeSynthesisRate);
			}
		}else if(system.stateNo == (int)GameSceneSystem.StateNo.GameOver){
			if(system.DebugCollapseFlag){
				time += Time.deltaTime/2;
				if(time > 1.0)	Mathf.Clamp01(time);
				shakeSynthesisRate = time*(timer/timerMax);
				transform.rotation = Quaternion.Slerp(q,Quaternion.Euler(rot2),shakeSynthesisRate);
			}
		}else{
			time = 0;
			timer = timerMax;
			q = Quaternion.identity;
			if(system.DebugUnBreakFlag)	shakePower = 10.0f;
			else						shakePower = 5.0f;
			rot		= new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-0.1f,0.1f),Random.Range(-1.0f,1.0f)) * shakePower;
			rot2	= new Vector3(1,0,1) * 15;
		}
	}
}
