using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StateMachineManager {
	private UnityAction[]	Func;

	private int		stateNo;
	private	int		prevStateNo;
	private float	stateTimer;
	public	float	StateTimer{get{return stateTimer;} set{stateTimer = value;}}

	public StateMachineManager(UnityAction[] u, int state){
		if(u != null){
			Func	= new UnityAction[u.Length];
			Func	= u;
		}
		stateNo		= state;
		prevStateNo	= stateNo;
		stateTimer  = 0.0f;
	}
	public void UpdateFunc(){
		if(Func == null){
			Debug.LogWarning("Stateが登録されていません。");
			return;
		}
		if(stateNo < 0 || stateNo >= Func.Length) return;
		Func[stateNo]();

		stateTimer += Time.deltaTime;
	}
	public void ChangeState(int state, bool overrideFlg = false){
		if(state != stateNo || overrideFlg){
			prevStateNo = stateNo;
			stateNo		= state;
			stateTimer	= 0;
		}
	}
}