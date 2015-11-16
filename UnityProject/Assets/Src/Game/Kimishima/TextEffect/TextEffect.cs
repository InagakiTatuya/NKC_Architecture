//-------------------------------------------------
//文字のエフェクト
//-------------------------------------------------

//名前空間//---------------------------------------
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス//-----------------------------------------
public class TextEffect : MonoBehaviour {

	//列挙//------------------------------------------
	public	enum ID{
		Bon_bo,			//ボンのボ
		Bon_n,			//ボンのン
		Gura_gu,		//グラのグ
		Gura_ra,		//グラのラ
		Gassharn_ga,	//ガッシャーン!!のガ
		Gassharn_tsu,	//ガッシャーン!!のッ
		Gassharn_shi,	//ガッシャーン!!のシ
		Gassharn_ya,	//ガッシャーン!!のャ
		Gassharn_haihun,//ガッシャーン!!のー
		Gassharn_n,		//ガッシャーン!!のン
		Gassharn_ex,	//ガッシャーン!!の!!
	}
	
	//変数//------------------------------------------
	private	Material	material;
	private	Color		CharaColor		= Color.black;
	private	Color		OutlineColor	= Color.white;

	private	UnityAction[]	startFunc;
	private	UnityAction[]	updateFunc;
	
	public	ID			id;
	public	Texture2D	texture;
	public	GameObject	targetObject;
	public	Vector3		pos;
	private	Vector3		posBuf;
	private	Vector3		vel;
	private	Vector3		scale	= Vector3.one * 8;
	private	Quaternion	rotate;
	private	float		timer;
	
	//初期化//----------------------------------------
	void Start () {
		StartGetMaterial();
		StartInitUpdateFunc();
		rotate	= Quaternion.Euler(1.0f,1.0f,1.0f);
		timer	= 0.0f;
		StartFuncInvoke();
	}
	private	void	StartGetMaterial(){//マテリアルを読み込む
		Renderer	renderer	= GetComponent<Renderer>();
		material	= renderer.material;
		Vector2		offset;
		offset.x	= 0.75f - ((int)id % 4) * 0.25f;
		offset.y	= 0.75f - ((int)id / 4) * 0.25f;
		material.SetTexture			("_MainTex",texture);
		material.SetTextureScale	("_MainTex",Vector3.one * 0.25f);
		material.SetTextureOffset	("_MainTex",offset);
	}
	private	void	StartInitUpdateFunc(){//更新関数を初期化
		startFunc	= new UnityAction[]{
			StartBon_bo,
			StartBon_n,
			StartGura_gu,
			StartGura_ra,
			StartGassharn_ga,
			StartGassharn_xtu,
			StartGassharn_shi,
			StartGassharn_xya,
			StartGassharn_haihun,
			StartGassharn_n,
			StartGassharn_ex,
			null
		};
		updateFunc	= new UnityAction[]{
			UpdateBon_bo,
			UpdateBon_n,
			UpdateGura_gu,
			UpdateGura_ra,
			UpdateGassharn_ga,
			UpdateGassharn_xtu,
			UpdateGassharn_shi,
			UpdateGassharn_xya,
			UpdateGassharn_haihun,
			UpdateGassharn_n,
			UpdateGassharn_ex,
			null
		};
	}
	private	void	StartFuncInvoke(){
		if(startFunc == null)			return;
		if(startFunc[(int)id] == null)	return;
		startFunc[(int)id]();
	}
	
	//更新//------------------------------------------
	void FixedUpdate () {
		UpdateFuncInvoke();
		UpdateMaterial();
		UpdateTranceform();
		timer	+= Time.fixedDeltaTime;
	}
	private	void	UpdateTranceform(){//オブジェクトステータスを更新
		Vector3		dist	= targetObject.transform.position - pos;
		Quaternion	lookAt	= Quaternion.LookRotation(dist,Vector3.up);
		transform.position		= pos;
		transform.localScale	= scale;
		transform.rotation		= lookAt * rotate;
	}
	private	void	UpdateFuncInvoke(){//更新関数を実行
		if(updateFunc == null)			return;
		if(updateFunc[(int)id] == null)	return;
		updateFunc[(int)id]();
	}
	private	void	UpdateMaterial(){
		material.SetColor("_Color",CharaColor);
		material.SetColor("_OutlineColor",OutlineColor);
	}

	//ボンのボ//--------------------------------------
	private	void	StartBon_bo(){//初期化
		vel				= new Vector3(0.0f,256.0f,0.0f);
		//rotate			= Quaternion.Euler(0.0f,0.0f,-r);
	}
	private	void	UpdateBon_bo(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos		+= vel * Time.fixedDeltaTime;
		vel		*= 0.75f;
		scale	= Vector3.one * 12.0f * Mathf.Pow(n,0.25f);
		CharaColor		= Color.black * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		OutlineColor	= Color.white * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//ボンのン//--------------------------------------
	private	void	StartBon_n(){//初期化
		vel				= new Vector3(256.0f,0.0f,0.0f);
		rotate			= Quaternion.Euler(0.0f,0.0f,45.0f);
	}
	private	void	UpdateBon_n(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos		+= vel * Time.fixedDeltaTime;
		vel		*= 0.75f;
		scale	= Vector3.one * 12.0f * Mathf.Pow(n,0.25f);
		CharaColor		= Color.black * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		OutlineColor	= Color.white * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//グラのグ//--------------------------------------
	private	void	StartGura_gu(){//初期化
		pos	+= new Vector3(0.0f,4.0f,0.0f);
		CharaColor		= Color.black;
		OutlineColor	= Color.yellow;
	}
	private	void	UpdateGura_gu(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos	+= new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f));
		CharaColor.a	= Mathf.Pow(1.0f - n,0.5f);
		OutlineColor.a	= Mathf.Pow(1.0f - n,0.5f);
		rotate		= Quaternion.Euler(0.0f,0.0f,5.0f * Mathf.Sin(Mathf.PI * 8.0f * timer));
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//グラのラ//--------------------------------------
	private	void	StartGura_ra(){//初期化
		pos	+= new Vector3(0.0f,-4.0f,0.0f);
		CharaColor		= Color.black;
		OutlineColor	= Color.yellow;
	}
	private	void	UpdateGura_ra(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos	+= new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f));
		CharaColor.a	= Mathf.Pow(1.0f - n,0.5f);
		OutlineColor.a	= Mathf.Pow(1.0f - n,0.5f);
		rotate		= Quaternion.Euler(0.0f,0.0f,5.0f * Mathf.Sin(Mathf.PI * 8.0f * timer));
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!のガ//----------------------------
	private	const	float	gassharnSpeed	= 2.0f;
	private	void	StartGassharn_ga(){//初期化
		posBuf		= pos + new Vector3(-20.0f,-5.0f,0.0f);
		vel			= new Vector3(-12.0f,32.0f,0.0f) * gassharnSpeed;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_ga(){//更新
		if(timer < 0.3f){
			scale	= (scale + new Vector3(50.0f,50.0f,1.0f)) * 0.5f;
		}
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}
	private	void	GassharnCommon(){
		if(timer < 0.3f)	return;
		if(timer < 1.0f){
			pos		= pos * 0.8f + posBuf * 0.2f;
			scale	= scale * 0.8f + new Vector3(10.0f,10.0f,1.0f) * 0.2f;
			pos		+= new Vector3(Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f),Random.Range(-0.2f,0.2f));
			return;
		}
		pos		+= vel * Time.fixedDeltaTime;
		vel.y	-= 2.0f;
		rotate	= Quaternion.Euler(0.0f,0.0f,timer * 720.0f);
		float	alpha	= 1.0f - Mathf.Min((timer - 1.0f) / 2.0f,1.0f);
		CharaColor.a	= alpha;
		OutlineColor.a	= alpha;
	}

	//ガッシャーン!!のッ//----------------------------
	private	void	StartGassharn_xtu(){
		posBuf		= pos + new Vector3(-10.0f,-3.75f,0.0f);
		vel			= new Vector3(-8.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_xtu(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!のシ//----------------------------
	private	void	StartGassharn_shi(){
		posBuf		= pos + new Vector3(0.0f,-2.5f,0.0f);
		vel			= new Vector3(-4.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_shi(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!のャ//----------------------------
	private	void	StartGassharn_xya(){
		posBuf		= pos + new Vector3(10.0f,-1.25f,0.0f);
		vel			= new Vector3(0.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_xya(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!のー//----------------------------
	private	void	StartGassharn_haihun(){
		posBuf		= pos + new Vector3(20.0f,0.0f,0.0f);
		vel			= new Vector3(4.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_haihun(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!のン//----------------------------
	private	void	StartGassharn_n(){
		posBuf		= pos + new Vector3(30.0f,1.25f,0.0f);
		vel			= new Vector3(8.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_n(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}

	//ガッシャーン!!の!!//----------------------------
	private	void	StartGassharn_ex(){
		posBuf		= pos + new Vector3(40.0f,2.5f,0.0f);
		vel			= new Vector3(12.0f,32.0f,0.0f) * gassharnSpeed;
		scale		= Vector3.zero;
		rotate		= Quaternion.Euler(0.0f,0.0f,-30.0f);
		CharaColor		= Color.red;
		OutlineColor	= Color.black;
	}
	private	void	UpdateGassharn_ex(){
		GassharnCommon();
		if(timer > 3.0f)	Destroy(this.gameObject);
	}
}
