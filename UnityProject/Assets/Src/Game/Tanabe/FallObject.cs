using UnityEngine;
using System.Collections;

public partial class FallObject : MonoBehaviour {
	//ステート
	enum STATE{
		FALL,
		FALLEND,
		CHECK,
		STOP,
		PAUSE,
		BOUND,
	};
	STATE state,prevState;

	//一度に配置されるオブジェクトの数
	private static int childCount;
	public static int ChildCount{get{ return FallObject.childCount; } set{ FallObject.childCount = value; }}

	private GameSceneSystem system;
	private Rigidbody rBody;
	private Vector3 recordVel,recordAngVel;

	private bool boundFlag;
	//state毎の経過時間
	private float stateTime;
	private float boundSpeed = 5; 

	private float	collapseConfirmTime = 0.5f;
	private float	collapseConfirmTimer;

	void Start(){
		recordVel		= new Vector3();
		recordAngVel	= new Vector3();

		rBody			= GetComponent<Rigidbody>();
		system			= transform.root.GetComponent<GameSceneSystem>();
		system.BuildList.Add(this);//建築物一覧に登録

		state		= STATE.FALL;
		prevState	= state;
		stateTime	= 0;
		StartKimishimaFallObject();
	}

	void Update(){
		if(exitFlag){
			if(system.stateNo == (int)GameSceneSystem.StateNo.Check){
				collapseConfirmTimer += Time.deltaTime;
				if(collapseConfirmTime<collapseConfirmTimer) collapseFunc();
			}
		}
		if(state == STATE.STOP){//処理の必要がないので停止
			return;
		}
		if(system.Pause){
			if(state == STATE.PAUSE) {}
			else{
				if(!rBody.isKinematic){
					//落下中は落下物のみ
					if(state == STATE.FALL)			ObjectSleep(false);
					//チェック中は全て
					else if(state == STATE.CHECK)	ObjectSleep();
				}
				prevState	= state;
				state		= STATE.PAUSE;
			}
			return;
		}
		if(state == STATE.PAUSE){
			state = prevState;
			if(rBody.isKinematic){
				//落下中は落下物のみ
				if(state == STATE.FALL)			ObjectWakeUp(false);
				//チェック中は全て
				else if(state == STATE.CHECK)	ObjectWakeUp();
			}
		}

		//ステート変更時に初期化
		if (state != prevState){
			prevState = state;
			stateTime = 0;
		}
		switch (state){
			case STATE.BOUND:
				break;
			case STATE.FALL:
				break;
			case STATE.FALLEND:
				//左右への移動と回転を許可
				rBody.constraints	&= RigidbodyConstraints.None;
				//rBody.constraints	|= RigidbodyConstraints.FreezeRotationX;
				rBody.isKinematic	= true;
				rBody.useGravity	= true;

				//物理演算を正常に行うため一度初期化
				recordVel				= Vector3.zero;
				recordAngVel			= Vector3.zero;
				rBody.velocity			= Vector3.zero;
				rBody.angularVelocity	= Vector3.zero;

				if(childCount == 0){//全てのオブジェクトの設置を確認
					system.seManager.Play(1);
					if(system.GetJob == 2 || system.GetJob == 3){//壁か屋根設置時
						state			= STATE.CHECK;
						system.Check	= true;
					}else{
						state			= STATE.STOP;
						system.PartsSet	= true;
					}
				}
				break;
			case STATE.CHECK:
				if(system.stateNo == (int)GameSceneSystem.StateNo.GameOver){//建築物が崩壊
					ObjectCollapse();
					state = STATE.STOP;
				}
				if (stateTime == 0.0f){//建築物耐久チェック開始
					if(rBody.isKinematic) ObjectWakeUp(true,true);
				}
				if (system.completeFlg){//建築物耐久チェッククリア
					if(system.stateNo == (int)GameSceneSystem.StateNo.CardView || 
						system.stateNo == (int)GameSceneSystem.StateNo.Result){
						state				= STATE.STOP;
						system.completeFlg	= false;
						if(!rBody.isKinematic) ObjectSleep();
					}
				}
				if(rBody.isKinematic) state = STATE.STOP;
				break;
		}
		stateTime += Time.deltaTime;
	}
	
	//崩壊させやすくするため、建造物に力を付与
	private void ObjectCollapse(){
		system.BuildList.ForEach(e =>
		{
			e.state				= STATE.STOP;
			e.rBody.isKinematic	= false;
			e.rBody.AddForce(transform.up * Random.Range(-15.0f,15.0f), ForceMode.Impulse);
			e.rBody.AddForce(transform.right * Random.Range(-15.0f,15.0f), ForceMode.Impulse);
			e.rBody.AddForce(transform.forward * Random.Range(-15.0f,15.0f), ForceMode.Impulse);
		});
	}

	//物理計算を許可
	private void ObjectWakeUp(bool isAll = true, bool shake = false){
		if(isAll){
			int i = 0;
			//建物の自然な揺れを再現するため、低層の物理演算を無効化
			if(system.GetFloor() >= 2)	i = (system.GetFloor() - 1) * 9;
			for(;i<system.BuildList.Count;i++){
				FallObject e = system.BuildList[i];
				e.rBody.isKinematic		= false;
				//速度、角速度を戻す
				e.rBody.velocity		= e.recordVel;
				e.rBody.angularVelocity	= e.recordAngVel;
			}
		}
		else{
			rBody.isKinematic		= false;
			//速度、角速度を戻す
			rBody.velocity			= recordVel;
			rBody.angularVelocity	= recordAngVel;
		}
	}

	//物理計算を不許可
	private void ObjectSleep(bool isAll = true){
		if(isAll){
			int i = 0;
			//建物の自然な揺れを再現するため、低層の物理演算を無効化
			if(system.GetFloor() >= 3)	i = (system.GetFloor() - 2) * 9;
			for(;i<system.BuildList.Count;i++){
				FallObject e = system.BuildList[i];
				//速度、角速度を記録する
				e.recordVel			= e.rBody.velocity;
				e.recordAngVel		= e.rBody.angularVelocity;
				e.rBody.isKinematic = true;
			}
		}
		else{
			//速度、角速度を記録する
			recordVel			= rBody.velocity;
			recordAngVel		= rBody.angularVelocity;
			rBody.isKinematic	= true;
		}
	}

	void OnCollisionEnter(Collision col){
		if(exitFlag){
			collapseConfirmTimer	= 0;
			exitFlag				= false;
		}
		if (state == STATE.FALL){//バウンド準備
			rBody.AddForce(Vector3.up * boundSpeed, ForceMode.VelocityChange);
			rBody.useGravity	= true;
			rBody.velocity		= Vector3.zero;
			state				= STATE.BOUND;
		} else if(state == STATE.BOUND){//バウンド終わり
			if(system.GetJob == 3) system.RoofSetFlag = true;
			//設置されたことを通達
			childCount--;
			rBody.useGravity		= false;
			rBody.isKinematic		= true;
			rBody.velocity			= Vector3.zero;
			rBody.angularVelocity	= Vector3.zero;
			state					= STATE.FALLEND;
		}
	}
}
