//-------------------------------------------------
//文字のエフェクト
//-------------------------------------------------

//名前空間//---------------------------------------
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス//-----------------------------------------
public class TextEffectManager : MonoBehaviour {

	//列挙//---------------------------------------
	public	enum 	EffectID{
		Bon,
	}

	private	int[]	effectCount	= new int[]{4,};

	//変数//---------------------------------------
	private	GameObject[]	effectObject;
	private	TextEffect[]	textEffect;
	private	UnityAction[]	updateFunc;

	public	GameObject	cameraObject;
	public	EffectID	effectID	= EffectID.Bon;
	public	Vector3		pos			= new Vector3(60.0f,-70.0f,60.0f);

	//初期化//-------------------------------------
	void Start () {
		GameObject	prefab	= Resources.Load<GameObject>("Prefab/Game/TextEffect");
		int		id			= (int)effectID;
		effectObject		= new GameObject[effectCount[id]];
		textEffect			= new TextEffect[effectCount[id]];
		for(int i = 0;i < effectObject.Length;i ++){
			effectObject[i]	= Instantiate(prefab);
			textEffect[i]	= effectObject[i].GetComponent<TextEffect>();
			textEffect[i].cameraObject	= cameraObject;
			textEffect[i].scale			= new Vector3(10.0f,10.0f,10.0f);
			textEffect[i].rotate		= i * 15.0f;
		}
		updateFunc	= new UnityAction[]{
			UpdateBon,
		};
	}

	//更新//---------------------------------------
	void Update () {
		int	id	= (int)effectID;
		updateFunc[id]();
	}
	private	void	UpdateBon(){
		for(int i = 0;i < effectObject.Length;i ++){
			textEffect[i].pos += new Vector3(Mathf.Sin(i * 15 * Mathf.Deg2Rad),
			                                 Mathf.Cos(i * 15 * Mathf.Deg2Rad),0.0f);
		}
	}
}
