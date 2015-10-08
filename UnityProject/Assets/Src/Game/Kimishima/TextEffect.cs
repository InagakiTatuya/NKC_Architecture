//-------------------------------------------------
//文字のエフェクト
//-------------------------------------------------

//名前空間//---------------------------------------
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス//-----------------------------------------
public class TextEffect : MonoBehaviour {

	//変数//---------------------------------------
	private	UnityAction[]	updateFunc;
	public	GameObject	cameraObject;
	public	Vector3		pos			= new Vector3(60.0f,-70.0f,60.0f);
	public	Vector3		scale		= new Vector3(1.0f,1.0f,1.0f);
	public	float		rotate		= 0.0f;
	public	Vector3		shaceSize	= new Vector3(0.0f,0.0f,0.0f);
	private	Vector3		posBuf;
	private	Vector3		scaleBuf;
	public	int			id;
	private	float		timer;

	//初期化//-------------------------------------
	void Start () {
		updateFunc	= new UnityAction[]{
			UpdateBon,
		};
		posBuf		= pos;
		scaleBuf	= scale;
		Renderer	renderer	= GetComponent<Renderer>();
		Material	material	= renderer.material;
		Vector2		offset		= new Vector2((3 - (id % 4)) * 0.25f,(3 - (id % 4)) * 0.25f);
		material.SetTextureOffset("_MainTex",offset);
		timer	= 0.0f;
	}
	
	//更新//---------------------------------------
	void Update () {
		/*
		if(timer > 0.1f){
			posBuf	= pos + new Vector3(Random.Range(-shaceSize.x,shaceSize.x),
			                           	Random.Range(-shaceSize.y,shaceSize.y),
			                           	Random.Range(-shaceSize.z,shaceSize.z));
			timer	= 0.0f;
		}
		scaleBuf	= (scaleBuf + scale) * 0.5f;
		transform.position	= (transform.position + posBuf) * 0.5f;
		transform.localScale= (transform.localScale + scaleBuf) * 0.5f;
		transform.LookAt(cameraObject.transform.position);
		Vector3	vec	= cameraObject.transform.position - transform.position;
		vec.Normalize();
		transform.rotation	= Quaternion.AngleAxis(rotate,vec) * transform.rotation;
		timer	+= Time.deltaTime;
		*/
	}
	void	UpdateBon(){
		pos	+= transform.up;
		transform.position	= pos;
	}
}
