﻿using UnityEngine;
using System.Collections;

public partial class FallObject : MonoBehaviour {
	//ステート
	enum STATE{
		FALL,
		FALLEND,
		CHECK,
		STOP,
		PAUSE,
	};
	STATE state,prevState;

	private static int childCount;
	public static int ChildCount{get{ return FallObject.childCount; } set{ FallObject.childCount = value; }}

	private GameSceneSystem system;
	private Rigidbody rBody;
	[SerializeField]
	private LayerMask layerName;

	private float stateTime;
	private Vector3 vel,angVel;

	//初期化
	void Start(){
		vel = new Vector3();
		angVel = new Vector3();

		rBody = GetComponent<Rigidbody>();
		system = transform.root.GetComponent<GameSceneSystem>();
		system.BuildList.Add(this);//建築物一覧に登録

		state = STATE.FALL;
		prevState = state;
		stateTime = 0;
	}

	//更新
	void Update(){
		if(state == STATE.STOP){
			this.enabled = false;
			return;
		}
		//呼び出し回数を規定すること
		if(system.Pause){
			if(state == STATE.PAUSE) {}
			else{
				if(!rBody.isKinematic){
					if(state == STATE.FALL)			ObjectSleep(false);
					else if(state == STATE.CHECK)	ObjectSleep();
				}
				prevState = state;
				state = STATE.PAUSE;
			}
			return;
		}
		if(state == STATE.PAUSE){
			state = prevState;
			if(rBody.isKinematic){
				if(state == STATE.FALL)			ObjectWakeUp(false);
				else if(state == STATE.CHECK)	ObjectWakeUp();
			}
		}

		//ステート変更時ステート内時間を初期化
		if (state != prevState){
			prevState = state;
			stateTime = 0;
		}
		switch (state){
			case STATE.FALL:
				break;
			case STATE.FALLEND:
				//左右への移動と回転を許可
				rBody.constraints &= RigidbodyConstraints.None;
				rBody.isKinematic = true;
				rBody.useGravity = true;

				vel = Vector3.zero;
				angVel = Vector3.zero;
				rBody.velocity = Vector3.zero;
				rBody.angularVelocity = Vector3.zero;

				if(childCount == 0){
					if(system.GetJob == 2){
						state = STATE.CHECK;
						system.Check = true;
					}else{
						state = STATE.STOP;
						system.PartsSet = true;
					}
				}
				break;
			case STATE.CHECK:
				if(system.stateNo == (int)GameSceneSystem.StateNo.GameOver){
					ObjectWakeUpFinish();
					state = STATE.STOP;
				}
				if (stateTime == 0.0f){
					//物理演算許可
					if(rBody.isKinematic) ObjectWakeUp();
				}
				if (system.completeFlg){
					if(system.stateNo == (int)GameSceneSystem.StateNo.PartsSelect){
						state = STATE.STOP;
						system.completeFlg = false;
						//物理演算不許可
						if(!rBody.isKinematic) ObjectSleep();
					}
				}
				if(rBody.isKinematic) state = STATE.STOP;
				break;
		}
		stateTime += Time.deltaTime;
	}
	
	private void ObjectWakeUpFinish(){
		system.BuildList.ForEach(e =>
		{
			e.state = STATE.STOP;
			e.rBody.isKinematic = false;
			e.rBody.AddForce(transform.up * Random.Range(-10.0f,10.0f), ForceMode.Impulse);
			e.rBody.AddForce(transform.right * Random.Range(-10.0f,10.0f), ForceMode.Impulse);
			e.rBody.AddForce(transform.forward * Random.Range(-10.0f,10.0f), ForceMode.Impulse);
		});
	}
	private void ObjectWakeUp(bool isAll = true){
		//建築物の物理計算を許可
		if(isAll){
			system.BuildList.ForEach(e =>
			{
				e.rBody.isKinematic = false;
				//速度、角速度を戻す
				e.rBody.velocity = e.vel;
				e.rBody.angularVelocity = e.angVel;
			});
		}
		else{
			rBody.isKinematic = false;
			//速度、角速度を戻す
			rBody.velocity = vel;
			rBody.angularVelocity = angVel;
		}
	}

	private void ObjectSleep(bool isAll = true){
		if(isAll){
			//建築物の物理計算を不許可
			system.BuildList.ForEach(e =>
			{
				//速度、角速度を記録する
				e.vel = e.rBody.velocity;
				e.angVel = e.rBody.angularVelocity;
				e.rBody.isKinematic = true;
			});
		}
		else{
			//速度、角速度を記録する
			vel = rBody.velocity;
			angVel = rBody.angularVelocity;
			rBody.isKinematic = true;
		}
	}

	//設置判定
	void OnCollisionEnter(Collision col){
		if (state == STATE.FALL){
			childCount--;
			state = STATE.FALLEND;
			rBody.isKinematic = true;
		}
	}
}