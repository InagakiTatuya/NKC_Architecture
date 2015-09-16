//------------------------------------------------------
//効果音のマネージャ
//------------------------------------------------------

//名前空間//--------------------------------------------
using UnityEngine;
using System.Collections;

//マネージャ//------------------------------------------
public class SeManager : MonoBehaviour {

	//変数//--------------------------------------------
	public	AudioClip[]		se;
	private	AudioSource[]	audioSource;
	private	float[]			audioTimer;

	//初期化//------------------------------------------
	void Start () {
		if(se == null)	return;
		audioSource	= new AudioSource[se.Length];
		audioTimer	= new float[se.Length];
		for(int i = 0;i < audioSource.Length;i ++){
			audioSource[i]		= gameObject.AddComponent<AudioSource>();
			audioSource[i].clip	= se[i];
			audioTimer[i]		= 1.0f;
		}
	}
	
	//更新//--------------------------------------------
	void Update () {
		if(audioTimer == null)	return;
		for(int i = 0;i < audioTimer.Length;i ++){
			audioTimer[i]	+= Time.deltaTime;
		}
	}

	//関数//--------------------------------------------
	public	void	Play(int id){
		if(se == null)					return;
		if(id < 0 || id >= se.Length)	return;
		if(audioSource == null)			return;
		if(audioSource[id] == null)		return;
		if(audioTimer == null)			return;
		if(audioTimer[id] < 0.05f)		return;
		audioSource[id].Play();
		audioTimer[id]	= 0.0f;
	}
}
