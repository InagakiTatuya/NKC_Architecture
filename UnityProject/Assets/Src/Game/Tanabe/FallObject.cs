using UnityEngine;
using System.Collections;

public partial class FallObject : MonoBehaviour {

	//Se用の変数とセッターだぜいby君島//------------
	private	SeManager	f_seManager	= null;
	public	SeManager	seManagere{set{f_seManager	= value;}}
	//----------------------------------------------

	public	delegate	void			DelegateBonPos(Vector3 pos);
	public	static		DelegateBonPos	delegateBonPos;

	static	private bool	breakFlag;
	static	private bool	unBreakFlag;

	//ステート
	public enum STATE{
		Fall,
		FallEnd,
		Check,
		Stop,
		Pause,
		Bound,
	};
	private	STATE	state, prevState;
	public	STATE	State	{get{ return state; } set{ state = value; }}

	//一度に配置されるオブジェクトの数
	private static int childCount;
	public	static int ChildCount	{get{ return FallObject.childCount; } set{ FallObject.childCount = value; }}

	private GameSceneSystem system;
	private Rigidbody		rBody;
	private Vector3			recordVel,recordAngVel;
	private Vector3			temp;

	private bool	boundFlag;
	//state毎の経過時間
	private float	stateTime;
	private float	boundSpeed	=	5;
	//物理動作を停止させ始める階層
	private int		freezeFloor =	2;

	private float	collapseConfirmTime = 0.5f;
	private float	collapseConfirmTimer;

	public	bool	bonFlag;

	void Start(){
		recordVel		=	new Vector3();
		recordAngVel	=	new Vector3();

		rBody			=	GetComponent<Rigidbody>();
		system			=	transform.root.GetComponent<GameSceneSystem>();
		system.BuildList.Add(this);//建築物一覧に登録

		state			=	STATE.Fall;
		prevState		=	state;
		stateTime		=	0;

		unBreakFlag		=	false;
		breakFlag		=	false;
		Physics.gravity =	Vector3.up * -9.81f;
		StartKimishimaFallObject();
	}

	void Update(){
		if(exitFlag){
			if(system.stateNo == (int)GameSceneSystem.StateNo.Check){
				collapseConfirmTimer += Time.deltaTime;
				if(collapseConfirmTime < collapseConfirmTimer){
					collapseFunc();
					Physics.gravity = Vector3.up * -98.1f;
				}
			}
		}
		if(breakFlag){
			if(rBody.velocity.y>0){
				temp = rBody.velocity;
				temp.y = 0;
				rBody.velocity = temp;
			}
			return;
		}
		if(!unBreakFlag){
			if(system.DebugUnBreakFlag){
				UnBreakBuilding();
				unBreakFlag = true;
			}
		}
		//処理の必要がないので停止
		if(state == STATE.Stop)	return;
		if(system.Pause){
			if(state == STATE.Pause) {}
			else{
				if(!rBody.isKinematic){
					//落下中は落下物のみ
					if(state == STATE.Fall)			ObjectSleep(false);
					//チェック中は全て
					else if(state == STATE.Check)	ObjectSleep();
				}
				prevState	=	 state;
				state		=	 STATE.Pause;
			}
			return;
		}
		if(state == STATE.Pause){
			state = prevState;
			if(rBody.isKinematic){
				//落下中は落下物のみ
				if(state == STATE.Fall)			ObjectWakeUp(false);
				//チェック中は全て
				else if(state == STATE.Check)	ObjectWakeUp();
			}
		}

		//ステート変更時に初期化
		if (state != prevState){
			prevState	=	 state;
			stateTime	=	 0;
		}
		switch (state){
			case STATE.Bound:
				break;
			case STATE.Fall:
				break;
			case STATE.FallEnd:
				//左右への移動と回転を許可
				rBody.constraints	&=	RigidbodyConstraints.None;
				if(system.DebugUnBreakFlag){
					rBody.constraints	|=	RigidbodyConstraints.FreezePositionX;
					rBody.constraints	|=	RigidbodyConstraints.FreezePositionZ;
				}
				rBody.isKinematic	=	true;
				rBody.useGravity	=	true;

				//物理演算を正常に行うため一度初期化
				recordVel				=	Vector3.zero;
				recordAngVel			=	Vector3.zero;
				rBody.velocity			=	Vector3.zero;
				rBody.angularVelocity	=	Vector3.zero;

				if(childCount == 0){//全てのオブジェクトの設置を確認
					if(system.GetJob == 2 || system.GetJob == 3){//壁か屋根設置時
						state			=	STATE.Check;
						system.Check	=	true;
					}else{
						state			=	STATE.Stop;
						system.PartsSet	=	true;
					}
				}
				break;
			case STATE.Check:
				if(system.DebugCollapseFlag){
					breakFlag = true;
					ObjectSleep();
					BreakBuilding();
					return;
				}
				if(system.stateNo == (int)GameSceneSystem.StateNo.GameOver){//建築物が崩壊
					ObjectCollapse();
					state = STATE.Stop;
				}
				if (stateTime == 0.0f){//建築物耐久チェック開始
					if(rBody.isKinematic) ObjectWakeUp(true,true);
				}
				if (system.completeFlg){//建築物耐久チェッククリア
					if(system.stateNo	== (int)GameSceneSystem.StateNo.CardView || 
						system.stateNo	== (int)GameSceneSystem.StateNo.Result){
						state				=	STATE.Stop;
						system.completeFlg	=	false;
						if(!rBody.isKinematic)	ObjectSleep();
					}
				}
				if(rBody.isKinematic) state =	STATE.Stop;
				break;
		}
		stateTime += Time.deltaTime;
	}
	
	//崩壊させやすくするため、建造物に力を付与
	private void ObjectCollapse(){
		system.BuildList.ForEach(e =>
		{
			e.state					=	STATE.Stop;
			e.rBody.isKinematic		=	false;
			e.rBody.angularVelocity	=	new Vector3(Random.Range(-2,2),Random.Range(-2,2),Random.Range(-2,2));
			e.rBody.AddForce(Vector3.up			* Random.Range(-15.0f,15.0f), ForceMode.Impulse);
			e.rBody.AddForce(Vector3.right		* Random.Range(-15.0f,15.0f), ForceMode.Impulse);
			e.rBody.AddForce(Vector3.forward	* Random.Range(-15.0f,15.0f), ForceMode.Impulse);
		});
	}

	//物理計算を許可
	private void ObjectWakeUp(bool isAll = true, bool shake = false){
		if(isAll){
			int i = 0;
			//建物の自然な揺れを再現するため、低層の物理演算を無効化
			if(system.GetFloor() >= freezeFloor)	i = (system.GetFloor() - 1) * 9;
			for(;i<system.BuildList.Count;i++){
				FallObject e			=	system.BuildList[i];
				e.rBody.isKinematic		=	false;
				//速度、角速度を戻す
				e.rBody.velocity		=	e.recordVel;
				e.rBody.angularVelocity	=	e.recordAngVel;
			}
		}
		else{
			rBody.isKinematic		=	false;
			//速度、角速度を戻す
			rBody.velocity			=	recordVel;
			rBody.angularVelocity	=	recordAngVel;
		}
	}

	//物理計算を不許可
	private void ObjectSleep(bool isAll = true){
		if(isAll){
			int i = 0;
			//建物の自然な揺れを再現するため、低層の物理演算を無効化
			if(system.GetFloor() >= freezeFloor+1)	i = (system.GetFloor() - 2) * 9;
			for(;i<system.BuildList.Count;i++){
				FallObject e		=	system.BuildList[i];
				//速度、角速度を記録する
				e.recordVel			=	e.rBody.velocity;
				e.recordAngVel		=	e.rBody.angularVelocity;
				e.rBody.isKinematic =	true;
			}
		}
		else{
			//速度、角速度を記録する
			recordVel			=	rBody.velocity;
			recordAngVel		=	rBody.angularVelocity;
			rBody.isKinematic	=	true;
		}
	}

	void UnBreakBuilding(){
		for(int i=0;i<system.BuildList.Count;i++){
			FallObject e			=	system.BuildList[i];
			e.rBody.constraints		|=	RigidbodyConstraints.FreezePositionX;
			e.rBody.constraints		|=	RigidbodyConstraints.FreezePositionZ;
		}
	}

	void BreakBuilding(){
		for(int i=0;i<system.BuildList.Count;i++){
			FallObject e			=	system.BuildList[i];
			e.rBody.constraints		=	RigidbodyConstraints.None;
			e.rBody.useGravity		=	true;
			e.rBody.isKinematic		=	false;
			e.rBody.AddForce(Vector3.right		* 8,	ForceMode.Impulse);
			e.rBody.AddForce(Vector3.forward	* 8,	ForceMode.Impulse);
		}
	}

	void OnCollisionEnter(Collision col){
		if(gameOverFlg && f_seManager != null){
			float	volume		= Random.Range(0.125f,0.5f);
			float	pitch		= Random.Range(0.125f,0.25f);
			int[]	tableSeID	= new int[]{1,8,9};
			int		seID		= Random.Range(0,tableSeID.Length - 1);
			if(!f_seManager.isPlaying(tableSeID[seID]))	f_seManager.Play(tableSeID[seID],volume,pitch);
		}
		if(exitFlag){
			collapseConfirmTimer	=	0;
			exitFlag				=	false;
		}
		if (state == STATE.Fall){//バウンド準備
			system.seManager.Play(1);
			rBody.AddForce(Vector3.up * boundSpeed, ForceMode.VelocityChange);
			rBody.useGravity	=	true;
			rBody.velocity		=	Vector3.zero;
			state				=	STATE.Bound;
			temp				=	transform.position;
			if(bonFlag){
				if(delegateBonPos	!=	null)	delegateBonPos(transform.position);
			}
			switch(system.GetJob){//設置パーティクル
				case 0:
					ParticleManager.obj.Play(ParticleManager.PAR_0_FLOOR,	temp);
					break;
				case 1:
					temp.y -= 8;
					ParticleManager.obj.Play(ParticleManager.PAR_1_PILLAR,	temp);
					break;
				case 2:
					ParticleManager.obj.Play(ParticleManager.PAR_2_WALL,	temp);
					break;
				case 3:
					ParticleManager.obj.Play(ParticleManager.PAR_3_ROOF,	temp);
					break;
			}
		} else if(state == STATE.Bound){//バウンド終わり
			if(system.GetJob == 3) system.RoofSetFlag = true;
			//設置されたことを通達
			childCount--;
			rBody.useGravity		=	false;
			rBody.isKinematic		=	true;
			rBody.velocity			=	Vector3.zero;
			rBody.angularVelocity	=	Vector3.zero;
			state					=	STATE.FallEnd;
		}
	}
}
