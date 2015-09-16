//-----------------------------------------------------------
//カメラを動かすクラス
//-----------------------------------------------------------

//名前空間//-------------------------------------------------
using UnityEngine;
using System.Collections;

//クラス//---------------------------------------------------
public class CameraMove : MonoBehaviour {

	//変数//-------------------------------------------------
	//カメラの座標
	private	Vector3	f_look;
	public	Vector3	look{
		get{return	f_look;}
		set{f_look	= value;}
	}
	//ターゲットの座標
	private	Vector3	f_at;
	public	Vector3	at{
		get{return	f_at;}
		set{f_at	= value;}
	}
	public	GameObject	target{
		set{f_at	= value.transform.position;}
	}
	//頭上
	private	Vector3	f_up;
	public	Vector3	up{
		get{return	f_up;}
		set{f_up	= value;}
	}

	private	Vector3	lookBuf;
	private	Vector3	atBuf;
	private	Vector3	upBuf;

	public	GameObject	FirstPos;
	public	float	lookPower	= 0.5f;
	public	float	atPower		= 0.5f;
	public	float	upPower		= 0.5f;

	private	bool	f_touchPermit	= true;
	public	bool	touchPermit{
		set{f_touchPermit	= value;}
	}

	//初期化//-----------------------------------------------
	void Start () {
		f_look	= transform.position;
		if(FirstPos == null)	f_at	= transform.position + transform.forward;
		else 					f_at	= FirstPos.transform.position;
		f_up	= Vector3.up;
		lookBuf	= look;
		atBuf	= at;
		upBuf	= up;
	}
	
	//更新//-------------------------------------------------
	void Update () {
		UpdateLookAt();
		UpdateTouch();
	}
	//カメラの基本動作
	void	UpdateLookAt(){
		lookBuf	= lookBuf * (1.0f - lookPower) + look * lookPower;
		atBuf	= atBuf   * (1.0f - atPower  ) + at   * atPower;
		upBuf	= upBuf   * (1.0f - upPower  ) + up   * upPower;
		transform.position	= lookBuf;
		transform.LookAt(atBuf,upBuf);
	}

	//タッチ操作
	void	UpdateTouch(){
		if(!f_touchPermit)	return;
		int	touchCount	= Input.touchCount;
		if(touchCount == 1)	UpdateTouchFingerSingle();
		if(touchCount > 1)	UpdateTouchFingerMulti(touchCount);
	}
	void	UpdateTouchFingerSingle(){//指一本
		Touch	touch	= Input.touches[0];
		f_look.y	+= touch.deltaPosition.y * touch.deltaTime;
		if(f_look.y < 0.0f)	f_look.y	= 0.0f;
		float	rad		= touch.deltaPosition.x * touch.deltaTime;
		float	newX,newZ;
		newX	= f_look.x * Mathf.Cos(rad) - f_look.z * Mathf.Sin(rad);
		newZ	= f_look.x * Mathf.Sin(rad) + f_look.z * Mathf.Cos(rad);
		f_look.x	= newX;
		f_look.z	= newZ;
	}
	void	UpdateTouchFingerMulti(int touchCount){//指複数
		int		flg		= 0;
		float	speed	= 0.0f;
		for(int i = 0;i < touchCount - 1;i ++){
			for(int j = i + 1;j < touchCount;j ++){
				Touch	iTouch	= Input.touches[i];
				Touch	jTouch	= Input.touches[j];
				int		ix		= (iTouch.deltaPosition.x < 0.0f)?-1:((iTouch.deltaPosition.x > 0.0f)?1:0);
				int		jx		= (jTouch.deltaPosition.x < 0.0f)?-1:((jTouch.deltaPosition.x > 0.0f)?1:0);
				int		iy		= (iTouch.deltaPosition.y < 0.0f)?-1:((iTouch.deltaPosition.y > 0.0f)?1:0);
				int		jy		= (jTouch.deltaPosition.y < 0.0f)?-1:((jTouch.deltaPosition.y > 0.0f)?1:0);
				speed	= Mathf.Abs(iTouch.deltaPosition.magnitude / iTouch.deltaTime) + Mathf.Abs(jTouch.deltaPosition.magnitude / jTouch.deltaTime);
				speed	/= Screen.width;
				if(ix != 0 && jx != 0 && ix == jx && iy != 0 && jy != 0 && iy == jy)	continue;
				if(ix > jx){
					if(iTouch.position.x < jTouch.position.x)	flg	=  1;
					else										flg	= -1;
				}else if(ix < jx){
					if(iTouch.position.x < jTouch.position.x)	flg	= -1;
					else										flg	=  1;
				}
				if(iy > jy){
					if(iTouch.position.y < jTouch.position.y)	flg	=  1;
					else										flg	= -1;
				}else if(iy < jy){
					if(iTouch.position.y < jTouch.position.y)	flg	= -1;
					else										flg	=  1;
				}
				break;
			}
			if(flg != 0)	break;
		}
		float	fieldOfView		= Camera.main.fieldOfView + speed * flg;
		if(fieldOfView < 30.0f)		fieldOfView	= 30.0f;
		if(fieldOfView > 120.0f)	fieldOfView	= 120.0f;
		Camera.main.fieldOfView	= fieldOfView;
	}
}
