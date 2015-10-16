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
	private	Material	material		= null;
	private	Vector2		tiling			= Vector2.one;
	private	Vector2		offset			= Vector2.zero;
	private	Color		CharaColor		= Color.black;
	private	Color		OutlineColor	= Color.white;
	
	public	int			id;
	public	Texture2D	texture;
	public	GameObject	targetObject;
	public	Vector3		pos;
	private	Vector3		scale;
	private	Quaternion	rotate;
	
	//初期化//----------------------------------------
	void Start () {
		StartGetMaterial();
	}
	private	void	StartGetMaterial(){//マテリアルを読み込む
		Renderer	renderer	= GetComponent<Renderer>();
		material	= renderer.material;
	}
	
	//更新//------------------------------------------
	void Update () {
		if(material == null)	return;
		UpdateMaterial();
		UpdateTranceform();
	}
	private	void	UpdateMaterial(){//マテリアル関連を更新
		material.SetTexture			("_MainTex",texture);
		material.SetTextureScale	("_MainTex",tiling);
		material.SetTextureOffset	("_MainTex",offset);
	}
	private	void	UpdateTranceform(){//オブジェクトステータスを更新
		Vector3		dist	= targetObject.transform.position - pos;
		Quaternion	lookAt	= Quaternion.LookRotation(dist,Vector3.up);
		transform.position		= pos;
		transform.localScale	= scale;
		transform.rotation		= lookAt * rotate;
	}
}
