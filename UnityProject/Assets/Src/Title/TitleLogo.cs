//------------------------------------------------------
//タイトルエフェクト
//------------------------------------------------------

//名前空間//--------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

//クラス//----------------------------------------------
public class TitleLogo : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler{
	//変数//--------------------------------------------
	#region //変数宣言
	private readonly	Vector2	BOUND_SIZE_MUL	=	new	Vector2(1.1f,0.9f);
	private const		float	INIT_POS_Y		=	1024.0f;
	private const		float	GRAVITY			=	1024.0f;
	private const		float	BOUND_VEL_Y		=	-128.0f;
	private const		float	ELASTIC_FORCE	=	-0.5f;
	private const		float	TOUCH_FORCE		=	512.0f;

	public	enum StateNo{
		Idol,			//待機中
		Fall,			//落下中
		Bound,			//バウンド中
	}
	public	enum IdolStateNo{
		Wait,			//待機中
		DateSet,		//データセット
		Bound,			//バウンド中
	}
	private StateMachineManager	updateSMM;
	private StateMachineManager	idolSMM;

	private	Image[]		image;
	private Sprite[]	sprite;

	private	Vector3[]	pos;
	private	Vector3[]	posBuf;
	private	Vector2[]	size;
	private	Vector2[]	sizeBuf;
	private	float		vel;

	private	bool		onMouseFlg;
	private	bool		touchFlg;
	#endregion

	//初期化//------------------------------------------
	void Start () {
		sprite	= Resources.LoadAll<Sprite>("Texture/Title/logo1");
		image	= new Image[sprite.Length];
		pos		= new Vector3[sprite.Length];
		size	= new Vector2[sprite.Length];
		posBuf	= new Vector3[sprite.Length];
		sizeBuf	= new Vector2[sprite.Length];

		Debug.Log(sprite);
		for(int i=0;i<sprite.Length;i++){
			GameObject g	= Instantiate(Resources.Load<GameObject>("Prefab/Title/ImageTemplate"));
			g.transform.SetParent(transform);

			image[i]							= g.GetComponent<Image>();
			image[i].sprite						= sprite[i];
			pos[i]								= GetComponent<RectTransform>().position;
			size[i]								= image[i].rectTransform.sizeDelta;
			sizeBuf[i]							= size[i];
			posBuf[i]							= pos[i];
			posBuf[i].y							= INIT_POS_Y;
			image[i].rectTransform.position		= posBuf[i];
			image[i].rectTransform.sizeDelta	= sizeBuf[i];
		}

		onMouseFlg	= false;

		updateSMM	= new StateMachineManager(
			new UnityAction[]{Idol,Fall,Bound,},
			(int)StateNo.Fall
		);
		idolSMM		= new StateMachineManager(
			new UnityAction[]{IdolWait,IdolDataSet,IdolBound},
			(int)IdolStateNo.Wait
		);
	}
	
	//更新//--------------------------------------------
	void Update () {
		GetTouch();
		updateSMM.UpdateFunc();
	}

	#region //Updatefunc
	void Idol(){
		//if(onMouseFlg && touchFlg){
		//    vel	= TOUCH_FORCE;
		//    updateSMM.ChangeState((int)StateNo.Fall);
		//}else{
		//    idolSMM.UpdateFunc();
		//}
	}
	void Fall(){

		//sizeBuf	=	size	* 0.1f + sizeBuf * 0.9f;
		//posBuf.y	+=	vel		* Time.deltaTime;
		//vel		-=	GRAVITY	* Time.deltaTime;

		//if(posBuf.y <= pos.y){
		//    posBuf.y	=	pos.y;
		//    updateSMM.ChangeState((int)StateNo.Bound);
		//}
		//image.rectTransform.position	= posBuf;
		//image.rectTransform.sizeDelta	= sizeBuf;
		//if(onMouseFlg && touchFlg) vel= TOUCH_FORCE;
	}
	void Bound(){
		//if(vel < BOUND_VEL_Y){
		//    vel			*=	ELASTIC_FORCE;
		//    sizeBuf.x	=	sizeBuf.x*BOUND_SIZE_MUL.x;
		//    sizeBuf.y	=	sizeBuf.y*BOUND_SIZE_MUL.y;
		//    updateSMM.ChangeState((int)StateNo.Fall);
		//}else{
		//    vel			=	0.0f;
		//    sizeBuf		=	size;
		//    updateSMM.ChangeState((int)StateNo.Idol);
		//}
		//image.rectTransform.sizeDelta	= sizeBuf;
	}
	#endregion

	#region //IdolFunc
	void IdolWait(){
		//image.rectTransform.sizeDelta	= size;
		//if(idolSMM.StateTimer > 3.0f || Random.Range(0,999) > 950){
		//    idolSMM.ChangeState((int)IdolStateNo.DateSet);
		//}
	}
	void IdolDataSet(){
		//sizeBuf.x	= sizeBuf.x*BOUND_SIZE_MUL.x;
		//sizeBuf.y	= sizeBuf.y*BOUND_SIZE_MUL.y;
		//idolSMM.ChangeState((int)IdolStateNo.Bound);
	}
	void IdolBound(){
		//sizeBuf	= size * 0.1f + sizeBuf * 0.9f;
		//if(idolSMM.StateTimer >= 1.0f){
		//    sizeBuf	= size;
		//    idolSMM.ChangeState((int)IdolStateNo.Wait);
		//}
		//image.rectTransform.sizeDelta	= sizeBuf;
	}
	#endregion

	void GetTouch(){
		touchFlg	= false;
		if(Input.GetMouseButtonDown(0))		touchFlg	= true;
		if(Input.touchCount <= 0)	return;
		Touch	touch = Input.touches[0];
		if(touch.phase == TouchPhase.Began)	touchFlg	= true;
	}
	public	void OnPointerEnter(PointerEventData eventData){
		onMouseFlg	= true;
	}
	public	void OnPointerExit(PointerEventData eventData){
		onMouseFlg	= false;
	}

}
