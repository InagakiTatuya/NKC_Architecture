using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System.Collections;

//ニューレコードクラス//---------------------------------------
public class ResultNewRecode : MonoBehaviour {

	public	enum StateNo {//ステート番号の列挙
		Neutral,
		FadeIn,
		Length,
	}

	//変数//---------------------------------------------------
	public	Vector2	size;
	private	Image	image;

	private	UnityAction[]	updateFunc;
	private	int				stateNo;
	private	float			stateTime;
	private	Vector2			sizeBuf;

	//初期化//-------------------------------------------------
	void Start () {
		image	= GetComponent<Image>();
		image.rectTransform.sizeDelta	= Vector2.zero;
		updateFunc	= new UnityAction[]{
			UpdateNeutral,
			UpdateFadeIn,
			null
		};
		stateNo		= (int)StateNo.Neutral;
		stateTime	= 0.0f;
		sizeBuf		= image.rectTransform.sizeDelta;
	}

	//更新//----------------------------------------------------
	void Update () {
		if(updateFunc[stateNo] != null)	updateFunc[stateNo]();
		stateTime	+= Time.deltaTime;
	}
#region
	private	void	UpdateNeutral(){
		float	n	= Mathf.Min(stateTime / 0.25f,1.0f);
		sizeBuf		= Vector2.zero * n + size * (1.0f - n);
	}
	private	void	UpdateFadeIn(){
		float	n	= Mathf.Min(stateTime / 0.25f,1.0f);
		sizeBuf		= Vector2.zero * (1.0f - n) + size * n;
	}
#endregion
}
