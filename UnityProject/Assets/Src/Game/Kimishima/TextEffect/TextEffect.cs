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
	void Update () {
		UpdateFuncInvoke();
		UpdateMaterial();
		UpdateTranceform();
		timer	+= Time.deltaTime;
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
		vel				= new Vector3(0.0f,128.0f,0.0f);
		float	r		= Random.Range(-30.0f,0.0f);
		float	newX	= Mathf.Cos(r * Mathf.Deg2Rad) * vel.x - Mathf.Sin(r * Mathf.Deg2Rad) * vel.y;
		float	newY	= Mathf.Sin(r * Mathf.Deg2Rad) * vel.x + Mathf.Cos(r * Mathf.Deg2Rad) * vel.y;
		vel.x			= newX;
		vel.y			= newY;
		rotate			= Quaternion.Euler(0.0f,0.0f,-r);
	}
	private	void	UpdateBon_bo(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos		+= vel * Time.deltaTime;
		vel		*= 0.9f;
		scale	= Vector3.one * 8.0f * Mathf.Pow(n,0.25f);
		CharaColor		= Color.black * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		OutlineColor	= Color.white * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//ボンのン//--------------------------------------
	private	void	StartBon_n(){//初期化
		vel				= new Vector3(0.0f,128.0f,0.0f);
		float	r		= Random.Range(-90.0f,-60.0f);
		float	newX	= Mathf.Cos(r * Mathf.Deg2Rad) * vel.x - Mathf.Sin(r * Mathf.Deg2Rad) * vel.y;
		float	newY	= Mathf.Sin(r * Mathf.Deg2Rad) * vel.x + Mathf.Cos(r * Mathf.Deg2Rad) * vel.y;
		vel.x			= newX;
		vel.y			= newY;
		rotate			= Quaternion.Euler(0.0f,0.0f,-r - 30);
	}
	private	void	UpdateBon_n(){//更新
		float	n	= Mathf.Min(timer,1.0f);
		pos		+= vel * Time.deltaTime;
		vel		*= 0.9f;
		scale	= Vector3.one * 8.0f * Mathf.Pow(n,0.25f);
		CharaColor		= Color.black * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		OutlineColor	= Color.white * (1.0f - n) + new Color(0.5f,0.5f,0.5f,0.0f) * n;
		if(timer > 1.0f)	Destroy(this.gameObject);
	}

	//グラのグ//--------------------------------------
	private	void	StartGura_gu(){//初期化

	}
	private	void	UpdateGura_gu(){//更新

	}

	//グラのラ//--------------------------------------
	private	void	StartGura_ra(){//初期化

	}
	private	void	UpdateGura_ra(){//更新

	}

	//ガッシャーン!!のガ//----------------------------
	private	void	StartGassharn_ga(){//初期化

	}
	private	void	UpdateGassharn_ga(){//更新

	}

	//ガッシャーン!!のッ//----------------------------
	private	void	StartGassharn_xtu(){

	}
	private	void	UpdateGassharn_xtu(){
		
	}

	//ガッシャーン!!のシ//----------------------------
	private	void	StartGassharn_shi(){

	}
	private	void	UpdateGassharn_shi(){
		
	}

	//ガッシャーン!!のャ//----------------------------
	private	void	StartGassharn_xya(){
		
	}
	private	void	UpdateGassharn_xya(){
		
	}

	//ガッシャーン!!のー//----------------------------
	private	void	StartGassharn_haihun(){
		
	}
	private	void	UpdateGassharn_haihun(){
		
	}

	//ガッシャーン!!のン//----------------------------
	private	void	StartGassharn_n(){
		
	}
	private	void	UpdateGassharn_n(){
		
	}

	//ガッシャーン!!の!!//----------------------------
	private	void	StartGassharn_ex(){
		
	}
	private	void	UpdateGassharn_ex(){
		
	}
}
