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
		Gura,
		Gasshan,
		Lenght
	}

	//変数//---------------------------------------
	public	EffectID	id;
	public	GameObject	prefab	= null;

	//初期化//-------------------------------------
	void Start () {
		if(prefab == null)	return;
		UnityAction[]	startFunc	= new UnityAction[]{
			StartBon,
			StartGura,
			StartGasshan,
		};
		startFunc[(int)id]();
	}
	private	void	StartBon(){//ボン//------------
		TextEffect.ID[]	id	= new TextEffect.ID[]{
			TextEffect.ID.Bon_bo,
			TextEffect.ID.Bon_n
		};
		for(int i = 0;i < 2;i ++){
			GameObject	obj	= Instantiate(prefab);
			obj.transform.parent	= this.transform;
			TextEffect	te	= obj.GetComponent<TextEffect>();
			te.id			= id[i];
			te.pos			= this.transform.position;
			te.targetObject	= Camera.main.gameObject;
		}
	}
	private	void	StartGura(){//グラ//-----------

	}
	private	void	StartGasshan(){//ガッシャーン//

	}

}
