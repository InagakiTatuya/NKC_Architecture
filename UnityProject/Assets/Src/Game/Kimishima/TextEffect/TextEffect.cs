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

	private	UnityAction[]	updateFunc;
	
	public	ID			id;
	public	Texture2D	texture;
	public	GameObject	targetObject;
	public	Vector3		pos;
	private	Vector3		scale	= Vector3.one * 8;
	private	Quaternion	rotate;
	private	float		timer;
	
	//初期化//----------------------------------------
	void Start () {
		StartGetMaterial();
		StartInitUpdateFunc();
		rotate	= Quaternion.Euler(1.0f,1.0f,1.0f);
		timer	= 0.0f;
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

	private	void	UpdateBon_bo(){//ボンのボ//-------
		pos		+= new Vector3(2.0f,8.0f,0.0f) * Time.deltaTime;
		scale	= Vector3.one * 4.0f * timer;
		rotate	= Quaternion.Euler(0.0f,0.0f,30.0f);
		CharaColor.a	= Mathf.Max(1.0f - timer,0.0f);
		OutlineColor.a	= Mathf.Max(1.0f - timer,0.0f);
		if(timer > 1.0f)	Destroy(this.gameObject);
	}
	private	void	UpdateBon_n(){//ボンのン//--------
		pos		+= new Vector3(8.0f,2.0f,0.0f) * Time.deltaTime;
		scale	= Vector3.one * 4.0f * timer;
		rotate	= Quaternion.Euler(0.0f,0.0f,60.0f);
		CharaColor.a	= Mathf.Max(1.0f - timer,0.0f);
		OutlineColor.a	= Mathf.Max(1.0f - timer,0.0f);
		if(timer > 1.0f)	Destroy(this.gameObject);
	}
	private	void	UpdateGura_gu(){//グラのグ//------

	}
	private	void	UpdateGura_ra(){//グラのラ//------

	}
	private	void	UpdateGassharn_ga(){//ガッシャーン!!のガ

	}
	private	void	UpdateGassharn_xtu(){//ガッシャーン!!のッ
		
	}
	private	void	UpdateGassharn_shi(){//ガッシャーン!!のシ
		
	}
	private	void	UpdateGassharn_xya(){//ガッシャーン!!のャ
		
	}
	private	void	UpdateGassharn_haihun(){//ガッシャーン!!のー
		
	}
	private	void	UpdateGassharn_n(){//ガッシャーン!!のン
		
	}
	private	void	UpdateGassharn_ex(){//ガッシャーン!!の!!
		
	}
}
