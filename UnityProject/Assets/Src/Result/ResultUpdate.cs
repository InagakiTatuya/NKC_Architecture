//----------------------------------------------------------
//リザルトのシステム
//更新日 :	06 / 13 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_RESULT
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//リザルトのシステム_Begin//--------------------------------
public	partial	class ResultSystem : MonoBehaviour {

	//更新//////////////////////////////////////////////////
	private	delegate void	UpdateFunc();
	private	UpdateFunc[]	updateFunc;

	//ステート処理//////////////////////////////////////////
	private	void	UpdateGideIn(){//ガイド挿入_Beign//-----
		float	timeBuf	= stateTime / 0.25f;
		for(int i = 0;i < gidePos.Length;i ++){
			float	n	= Mathf.Max(Mathf.Min(timeBuf - i * 0.25f,1.0f),0.0f);
			gidePos[i].x	= 64.0f * n + 544.0f * (1.0f - n);
		}
		if(stateTime >= 0.5f)	ChangeState(StateNo.Gide);
	}//ガイド挿入_End//-------------------------------------
	
	private	void	UpdateGide(){//ガイド待機_Beign//-------
#if DEBUG_RESULT
		if(Input.GetMouseButtonDown(0))	ChangeState(StateNo.GideOut);
#endif
		if(Input.touchCount <= 0)						return;
		if(Input.GetTouch(0).phase != TouchPhase.Began)	return;
		ChangeState(StateNo.GideOut);
	}//ガイド待機_End//-------------------------------------
	
	private	void	UpdateGideOut(){//ガイド退出_Beign//----
		float	timeBuf	= stateTime / 0.25f;
		for(int i = 0;i < gidePos.Length;i ++){
			float	n	= Mathf.Max(Mathf.Min(timeBuf - i * 0.25f,1.0f),0.0f);
			gidePos[i].x	= 64.0f * (1.0f - n) + 544.0f * n;
		}
		if(stateTime >= 0.5f)	ChangeState(StateNo.RuncIn);
	}//ガイド退出_End//-------------------------------------
	
	private	void	UpdateRuncIn(){//ランク挿入_Beign//-----
		float	timeBuf	= stateTime / 0.25f;
		for(int i = 0;i < runcGidePos.Length;i ++){
			float	n	= Mathf.Max(Mathf.Min(timeBuf - i * 0.25f,1.0f),0.0f);
			runcGidePos[i].x	= 64.0f * n + 544.0f * (1.0f - n);
		}
		if(stateTime >= 0.5f)	ChangeState(StateNo.Runc);
	}//ランク挿入_End//-------------------------------------
	
	private	void	UpdateRunc(){//ランク待機_Begin//-------
#if DEBUG_RESULT
		if(Input.GetMouseButtonDown(0))	ChangeState(StateNo.RuncOut);
#endif
		if(Input.touchCount <= 0)						return;
		if(Input.GetTouch(0).phase != TouchPhase.Began)	return;
		ChangeState(StateNo.RuncOut);
	}//ランク待機_End//-------------------------------------
	
	private	void	UpdateRuncOut(){//ランク退出_Begin//----
		float	timeBuf	= stateTime / 0.25f;
		for(int i = 0;i < runcGidePos.Length;i ++){
			float	n	= Mathf.Max(Mathf.Min(timeBuf - i * 0.25f,1.0f),0.0f);
			runcGidePos[i].x	= 64.0f * (1.0f - n) + 544.0f * n;
		}
		if(stateTime >= 0.5f){
			CreateButton();
			ChangeState(StateNo.ButtonIn);
		}
	}//ランク退出_End//-------------------------------------
	
	private	void	UpdateButtonIn(){//ボタン挿入_Begin//---
		float	n	= Mathf.Min(stateTime / 0.25f,1.0f);
		for(int i = 0;i < buttonSize.Length;i ++){
			buttonSize[i].x	= 256.0f *         n ;
			buttonSize[i].y	=  64.0f * (2.0f - n);
		}
		if(stateTime > 0.5f)	ChangeState(StateNo.Button);
	}//ボタン挿入_End//-------------------------------------
	
	private	void	UpdateButton(){//ボタン待機_Begin//-----
	}//ボタン待機_End//-------------------------------------
	
	private	void	UpdateGoNext(){//次のシーンへ_Begin//---
		string[]	tableNextSceneName	= new string[]{
			"Select",	//デバッグ用にリトライの代わりにセレクト
			"Select",
			"CardInput",
		};
		float	n	= Mathf.Min(stateTime * 4.0f,1.0f);
		buttonSize[selectNo].x	*= 1.25f;
		buttonSize[selectNo].y	*= 0.8f;
		fadeColor.a	= n;
		if(stateTime > 0.5f)Application.LoadLevel(tableNextSceneName[selectNo]);
	}//次のシーンへ_End//-----------------------------------

	//常時呼び出し//////////////////////////////////////////
	//ガイドとテキストを更新_Beign//------------------------
	private	void	UpdateGideAndText(){
		for(int i = 0;i < gide.Length;i ++){
			gide[i].rectTransform.localPosition	= gidePos[i];
			text[i].rectTransform.localPosition	= gidePos[i] + new Vector3(0.0f,64.0f,0.0f);
		}
	}//ガイドとテキストを更新_End//-------------------------
	
	//ランクを更新_Begin//----------------------------------
	private	void	UpdateRuncGideAndRuncText(){
		for(int i = 0;i < runcGidePos.Length;i ++){
			runcGide[i].rectTransform.localPosition	= runcGidePos[i];
			runcText[i].rectTransform.localPosition	= runcGidePos[i] + new Vector3(0.0f,64.0f,0.0f);
		}
	}//ランクを更新_End//-----------------------------------

	//ボタンを更新_Begin//----------------------------------
	private	void	UpdateButtonImage(){
		if(button == null)		return;
		if(buttonImage == null)	return;
		for(int i = 0;i < buttonImage.Length;i ++)
			buttonImage[i].rectTransform.sizeDelta	= buttonSize[i];
	}//ボタンを更新_End//-----------------------------------

	private	void	UpdateFade(){//フェードを更新_Begin//---
		if(fade == null)	return;
		fade.color		= fadeColor;
	}//フェードを更新_End//---------------------------------
	
}//リザルトのシステム_End//---------------------------------
