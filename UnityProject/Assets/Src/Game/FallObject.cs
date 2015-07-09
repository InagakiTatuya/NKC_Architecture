using UnityEngine;
using System.Collections;

public class FallObject : MonoBehaviour {
	//ステート
	enum STATE
	{
		FALL,
		FALLEND,
		CHECK,
		STOP,
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

					//建築物の物理計算を許可
					system.Building.ForEach(e =>
					{
						e.rBody.isKinematic = false;
						//速度、角速度を戻す
						e.rBody.velocity = e.vel;
						e.rBody.angularVelocity = e.angVel;
					});
				}


				if (stateTime >= 3.0f || Input.GetMouseButtonDown(0))
				{
					system.Execute = true;//PARTSSETシーンへ
					state = STATE.STOP;

					//建築物の物理計算を不許可
					system.Building.ForEach(e =>
					{
						//速度、角速度を記録する
						e.vel = e.rBody.velocity;
						e.angVel = e.rBody.angularVelocity;
						e.rBody.isKinematic = true;
					});
				}
				break;
			case STATE.STOP:
				break;
		}
		stateTime += Time.deltaTime;
	}

	void OnCollisionEnter(Collision col)
	{
		if (state == STATE.FALL)
		{
			state = STATE.FALLEND;
			rBody.isKinematic = true;
			//rBody.velocity = Vector3.zero;
		}
	}
}
