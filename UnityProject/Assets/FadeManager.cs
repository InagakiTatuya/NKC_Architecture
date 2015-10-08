//----------------------------------------------------------
//画面全体のフェードインとフェードアウトを実行できるよ
//更新日 :	10 / 08 / 2015
//更新者 :	田辺　純也
//----------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour {
	
	public	enum	STATE{
		Hide,
		FadeIn,
		FadeOut,
		Black,
	}
	private STATE			state;
	public	STATE			State	{get{return state;}}

	private	UnityAction[]	tableFade;
	private UnityAction		callBack;
	private	Image			image;
	private	Color			color;
	
	private float			fadeTimer;
	private float			fadeCompleteTimer;

	void Start () {
		image			=	GetComponent<Image>();
		image.color		=	Color.black;
		image.enabled	=	false;
		state			=	STATE.Hide;
		tableFade		=	new UnityAction[]{//stateに応じた関数を呼び出す
			this.Hide,
			this.FadeIn,
			this.FadeOut,
			this.Black,
		};
	}

	void Update(){
		tableFade[(int)state]();
		fadeTimer += Time.deltaTime;
	}

	public	void setFadeIn(UnityAction c = null, float timer = 0.25f){
		if(	state == STATE.Hide || state == STATE.Black){
			state				=	STATE.FadeIn;
			image.enabled		=	true;
			//フェード終了時に呼び出す関数
			callBack			=	c;
			fadeTimer			=	0.0f;
			fadeCompleteTimer	=	timer;
			if(fadeCompleteTimer <= 0.0f) fadeCompleteTimer = 0.25f;
		}
	}
	public	void setFadeOut(UnityAction c = null, float timer = 0.25f){
		if(	state == STATE.Hide || state == STATE.Black){
			state				=	STATE.FadeOut;
			image.enabled		=	true;
			//フェード終了時に呼び出す関数
			callBack			=	c;
			fadeTimer			=	0.0f;
			fadeCompleteTimer	=	timer;
			if(fadeCompleteTimer <= 0.0f) fadeCompleteTimer = 0.25f;
		}
	}

	void FadeIn(){
		color.a = 1.0f - fadeTimer / fadeCompleteTimer;
		if(color.a < 0.0f){
			color.a = 0.0f;
			state	=	STATE.Hide;
			if(callBack != null)	callBack();
		}
		image.color = color;
	}
	void FadeOut(){
		color.a = fadeTimer / fadeCompleteTimer;
		if(color.a > 1.0f){
			color.a =	1.0f;
			state	=	STATE.Black;
			if(callBack != null)	callBack();
		}
		image.color = color;
	}
	void Hide(){
		color.a			=	0.0f;
		image.color		=	color;
		image.enabled	=	false;
	}
	void Black(){
		color.a			=	1.0f;
		image.color		=	color;
		image.enabled	=	true;
	}
}
