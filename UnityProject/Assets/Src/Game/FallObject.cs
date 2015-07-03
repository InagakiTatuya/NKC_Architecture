using UnityEngine;
using System.Collections;

public class FallObject : MonoBehaviour {
	//ステート
	enum STATE
	{
		FALL,
		FALLINTERVAL,
		FALLEND,
		STOP,
	};
	STATE state;

	private Rigidbody rBody;
	[SerializeField]
	private LayerMask layerName;

	//初期化
	void Start()
	{
		rBody = GetComponent<Rigidbody>();
		state = STATE.FALL;
		//state = STATE.STOP;
	}

	//更新
	void Update()
	{
		switch (state)
		{
			case STATE.FALL:
				break;
			case STATE.FALLINTERVAL:
				state = STATE.FALLEND;
				break;
			case STATE.FALLEND:
				rBody.isKinematic = false;
				break;
			case STATE.STOP:
				break;
		}
	}

	//当たり判定
	void OnCollisionEnter(Collision col)
	{
		if (state == STATE.FALL)
		{
			state = STATE.FALLEND;
			rBody.isKinematic = true;
			rBody.useGravity = false;
			rBody.velocity = Vector3.zero;
		}
	}
}
