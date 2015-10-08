//----------------------------------------------------
//チュートリアルオブジェクト
//----------------------------------------------------

//名前空間//------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス//--------------------------------------------
public class TutorialParent : MonoBehaviour {

	//列挙//------------------------------------------
	public	enum StateNo{
		Open,
		Wait,
		Close,
		Destroy,
		Length
	}

	//変数//------------------------------------------
	private	UnityAction[]	updateFunc;
	private	int				stateNo;
	private	float			stateTime;
	private	Image[]			image;
	private	Text[]			text;
	private	float			alpha;

	public	TutorialChild[]	child		= null;
	public	TutorialText[]	childText	= null;
	public	int				tutorialID	= -1;

	//初期化//----------------------------------------
	void Start () {
		if(tutorialID < 0 || tutorialID >= Database.tableTutorialText.Length){
			Destroy(this.gameObject);
			return;
		}
		updateFunc	= new UnityAction[]{
			UpdateOpen,
			UpdateWait,
			UpdateClose,
			UpdateDestroy,
			null
		};
		ChangeState(StateNo.Open,true);
		image		= new Image[1 + child.Length];
		image[0]	= GetComponent<Image>();
		for(int i = 1;i <= child.Length;i ++){
			image[i]	= child[i - 1].GetComponent<Image>();
		}
		text		= new Text[childText.Length];
		for(int i = 0;i < childText.Length;i ++){
			text[i]	= childText[i].GetComponent<Text>();
			text[i].text	= Database.tableTutorialText[tutorialID];
		}
		alpha		= 0.0f;
	}
	
	//更新//------------------------------------------
	void Update () {
		if(updateFunc[stateNo] != null)	updateFunc[stateNo]();
		stateTime	+= Time.deltaTime;
		//色を調整
		for(int i = 0;i < image.Length;i ++){
			Color	color	= image[i].color;
			color.a			= alpha * ((i == 0)?0.5f:1.0f);
			image[i].color	= color;
		}
		for(int i = 0;i < text.Length;i ++){
			Color	color	= text[i].color;
			color.a			= alpha;
			text[i].color	= color;
		}
	}
	private	void	UpdateOpen(){//ウィンドウを開く
		float	n	= Mathf.Min(stateTime / 0.25f,1.0f);
		alpha	= n;
		if(stateTime >= 0.25f)	ChangeState(StateNo.Wait);
	}
	private	void	UpdateWait(){//待機
		alpha	= 1.0f;
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))					ChangeState(StateNo.Close);
#endif
		if(Input.touchCount <= 0)	return;
		if(Input.touches[0].phase == TouchPhase.Ended)	ChangeState(StateNo.Close);
	}
	private	void	UpdateClose(){//閉じる
		float	n	= 1.0f - Mathf.Min(stateTime / 0.25f,1.0f);
		alpha	= n;
		if(stateTime >= 0.25f)	ChangeState(StateNo.Destroy);
	}
	private	void	UpdateDestroy(){//破棄
		Destroy(this.gameObject);
	}

	//シーン遷移//------------------------------------
	public	void	ChangeState(StateNo stateNo,bool overlapFlg = false){
		if(this.stateNo == (int)stateNo && !overlapFlg)	return;
		this.stateNo	= (int)stateNo;
		this.stateTime	= 0.0f;
	}
}
