using UnityEngine;
using System.Collections;

public partial class FallObject : MonoBehaviour {
	//ステート
	enum STATE
	{
		FALL,
		FALLEND,
		CHECK,
		STOP,
		PAUSE,
	};
	STATE state,prevState;

	private GameSceneSystem system;
	private Rigidbody rBody;
	[SerializeField]
	private LayerMask layerName;

	private float stateTime;
	private Vector3 vel,angVel;

	//初期化
	void Start()
	{
		vel = new Vector3();
		angVel = new Vector3();

		rBody = GetComponent<Rigidbody>();
		system = transform.root.GetComponent<GameSceneSystem>();
		system.Building.Add(this);//建築物一覧に登録

		state = STATE.FALL;
		prevState = state;
		stateTime = 0;
	}

	//更新
	void Update()
	{
		if(system.PauseFlg)
		{
			if(state == STATE.PAUSE) {}
			else
			{
				if(state == STATE.FALL) ObjectSleep(true);
				else if(state == STATE.CHECK) ObjectSleep();
				prevState = state;
				state = STATE.PAUSE;
			}
			return;
		}
		if(state == STATE.PAUSE)
		{
			state = prevState;
			if(state == STATE.FALL) ObjectWakeUp(true);
			else if(state == STATE.CHECK) ObjectWakeUp();
		}

		//ステート変更時ステート内時間を初期化
		if (state != prevState)
		{
			prevState = state;
			stateTime = 0;
		}
		switch (state)
		{
			case STATE.FALL:
				break;
			case STATE.FALLEND:
				rBody.isKinematic = false;
				system.Execute = true;//CHECKシーンへ
				state = STATE.CHECK;
				break;
			case STATE.CHECK:
				if (stateTime == 0.0f)
				{
					//左右への移動と回転を許可
					rBody.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ);
					rBody.useGravity = true;
					//物理演算許可
					ObjectWakeUp();
				}

				if (system.completeFlg)
				{
					state = STATE.STOP;
					//物理演算不許可
					ObjectSleep();
				}
				break;
			case STATE.STOP:
				break;
		}
		stateTime += Time.deltaTime;
	}

	private void ObjectWakeUp(bool isAll = true)
	{
		//建築物の物理計算を許可
		if(isAll)
		{
			system.Building.ForEach(e =>
			{
				e.rBody.isKinematic = false;
				//速度、角速度を戻す
				e.rBody.velocity = e.vel;
				e.rBody.angularVelocity = e.angVel;
			});
		}
		else
		{
			rBody.isKinematic = false;
			//速度、角速度を戻す
			rBody.velocity = vel;
			rBody.angularVelocity = angVel;
		}
	}

	private void ObjectSleep(bool isAll = true)
	{
		if(isAll)
		{
			//建築物の物理計算を不許可
			system.Building.ForEach(e =>
			{
				//速度、角速度を記録する
				e.vel = e.rBody.velocity;
				e.angVel = e.rBody.angularVelocity;
				e.rBody.isKinematic = true;
			});
		}
		else
		{
			rBody.isKinematic = true;
			//速度、角速度を戻す
			vel = rBody.velocity;
			angVel = rBody.angularVelocity;
		}
	}

	//設置判定
	void OnCollisionEnter(Collision col)
	{
		if (state == STATE.FALL)
		{
			state = STATE.FALLEND;
			rBody.isKinematic = true;
		}
	}
}
