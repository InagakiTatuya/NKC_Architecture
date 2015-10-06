//-----------------------------------------------------------
//カメラを動かすクラス
//更新者 :	君島一刀
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

	public	float	maxY;
	private	Vector3	lookBuf;
	private	Vector3	atBuf;
	private	Vector3	upBuf;
	
	public	float	lookPower	= 0.5f;
	public	float	atPower		= 0.5f;
	public	float	upPower		= 0.5f;

	private	bool	f_touchPermit	= false;
	public	bool	touchPermit{
		set{f_touchPermit	= value;}
	}

	//初期化//-----------------------------------------------
	void Start () {
		f_look	= transform.position;
		f_at	= new Vector3(60.0f,-50.0f,60.0f);
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
		if(touchCount <= 0)	return;
		if(touchCount == 1)	UpdateTouchFingerSingle();
		if(touchCount > 1)	UpdateTouchFingerMulti(touchCount);
	}
	void	UpdateTouchFingerSingle(){//指一本
		Touch	touch	= Input.touches[0];
		float	touchDeltaTime	= touch.deltaTime;
		if(touchDeltaTime == 0.0f)	return;
		float	n	= Time.deltaTime / touchDeltaTime;
		f_look.y	+= touch.deltaPosition.y * n;
		if(f_look.y > maxY)	f_look.y	= maxY;
		if(f_look.y < -25.0f)	f_look.y	= -25.0f;
		float	rad		= touch.deltaPosition.x * n * -0.005f;
		float	newX,newZ;
		f_look.x	-= 60.0f;
		f_look.z	-= 60.0f;
		newX		= f_look.x * Mathf.Cos(rad) - f_look.z * Mathf.Sin(rad);
		newZ		= f_look.x * Mathf.Sin(rad) + f_look.z * Mathf.Cos(rad);
		f_look.x	= newX + 60.0f;
		f_look.z	= newZ + 60.0f;
	}
	void	UpdateTouchFingerMulti(int touchCount){//指複数
		int		flg		= 0;
		float	speed	= 0.0f;
		for(int i = 0;i < touchCount - 1;i ++){
			//タッチ情報を取得
			Touch	iTouch	= Input.touches[i];
			if(iTouch.deltaTime == 0.0f)	continue;
			for(int j = i + 1;j < touchCount;j ++){
				//タッチ情報を取得
				Touch	jTouch	= Input.touches[j];
				if(jTouch.deltaTime == 0.0f)	continue;
				//座標の差を取得
				int		ix		= (iTouch.deltaPosition.x < 0.0f)?-1:((iTouch.deltaPosition.x > 0.0f)?1:0);
				int		jx		= (jTouch.deltaPosition.x < 0.0f)?-1:((jTouch.deltaPosition.x > 0.0f)?1:0);
				int		iy		= (iTouch.deltaPosition.y < 0.0f)?-1:((iTouch.deltaPosition.y > 0.0f)?1:0);
				int		jy		= (jTouch.deltaPosition.y < 0.0f)?-1:((jTouch.deltaPosition.y > 0.0f)?1:0);
				//スピードを計算
				speed	= Mathf.Abs(iTouch.deltaPosition.magnitude / iTouch.deltaTime)
						+ Mathf.Abs(jTouch.deltaPosition.magnitude / jTouch.deltaTime);
				speed	/= Screen.width;
				//移動量を確認
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
		Camera.main.fieldOfView	= Mathf.Max(Mathf.Min(Camera.main.fieldOfView + speed * flg,120.0f),30.0f);
	}
}
