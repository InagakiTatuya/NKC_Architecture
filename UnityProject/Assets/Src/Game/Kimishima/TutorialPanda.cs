//----------------------------------------------------
//チュートリアルオブジェクト
//----------------------------------------------------

//名前空間//------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス//--------------------------------------------
public class TutorialPanda : MonoBehaviour {

	//変数//------------------------------------------
	private	Image		image;
	private	Sprite[]	sprite;
	private	float[]		tableIDChangeTime;
	private	int			count;
	private	float		animTimer;
	private	int			id;

	//ぽよんぽよんさせるんじゃーい
	private	float		jumpTimer;
	private	Vector3		defaultPos,pos;
	private	Vector2		defaultSize,size;
	private	float		vel;

	//初期化//----------------------------------------
	void Start () {
		tableIDChangeTime	= new float[]{
			1.0f,0.1f,0.1f,0.1f
		};
		count		= 0;
		animTimer	= 0.0f;
		id			= 0;
		image		= GetComponent<Image>();
		sprite		= Resources.LoadAll<Sprite>("Texture/Game/panda");
		jumpTimer	= 0.0f;
		pos			= defaultPos	= image.rectTransform.localPosition;
		size		= defaultSize	= image.rectTransform.sizeDelta;
		vel			= 0.0f;
	}
	
	//更新//------------------------------------------
	void FixedUpdate () {
		animTimer	+= Time.fixedDeltaTime;
		if(tableIDChangeTime[count] <= animTimer){
			animTimer	= 0.0f;
			count	= (count + 1) % tableIDChangeTime.Length;
			id		= (id + 1) % 2;
			image.sprite	= sprite[id];
		}
		//ぽよんぽよんさせるんじゃーい
		size		= (size + defaultSize) * 0.5f;
		jumpTimer	+= Time.fixedDeltaTime;
		if(jumpTimer > 3.0f){
			vel			= 512.0f;
			jumpTimer	= 0.0f;
			size.x		*= 1.25f;
			size.y		*= 0.8f;
		}
		pos.y		+= vel * Time.fixedDeltaTime;
		if(pos.y < defaultPos.y){
			pos.y		= defaultPos.y;
			vel			= 0.0f;
			size.x		*= 1.25f;
			size.y		*= 0.8f;
		}else if(pos.y > defaultPos.y){
			vel	-= 32.0f;
		}
		image.rectTransform.localPosition	= pos;
		image.rectTransform.sizeDelta		= size;
	}
}
