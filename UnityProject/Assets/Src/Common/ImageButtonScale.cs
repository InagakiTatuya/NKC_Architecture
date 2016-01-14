//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	UnityEngine.EventSystems;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ボタンのシステム_Begin//----------------------------------
public class ImageButtonScale : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler {

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////

	private	Vector2		buttonPos;
	private	Vector2		buttonSize;
	private	Vector2		buttonTempSize;
	private Image		button;

	private	Vector2		imagePos;
	private	Vector2		imageSize;
	private	Vector2		imageTempSize;
	private Image		image;


	//ステートチェックプロパティ
	private	bool		f_press;
	public	bool		press{get{return	f_press;}}

	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化
		StartButton();
		StartButtonImage();
	}
	private	void	StartButton(){//ボタンを初期化
		button	= GetComponent<Image>();
		buttonPos = button.rectTransform.localPosition;
		buttonSize = button.rectTransform.localScale;
		buttonTempSize = buttonSize;
	}
	private	void	StartButtonImage(){//ボタンを初期化
		image	= transform.GetChild(0).GetComponent<Image>();
		imagePos = image.rectTransform.localPosition;
		imageSize = image.rectTransform.localScale;
		imageTempSize = imageSize;
	}

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		image.rectTransform.localScale		= imageTempSize;
		button.rectTransform.localScale		= buttonTempSize;
		if(f_press)	{
			imageTempSize	= imageTempSize * 0.5f + imageSize * 0.4f;
			buttonTempSize	= buttonTempSize * 0.5f + buttonSize * 0.4f;
		}else{
			imageTempSize	= imageTempSize * 0.5f + imageSize * 0.5f;
			buttonTempSize	= buttonTempSize * 0.5f + buttonSize * 0.5f;
		}
	}//更新_End//-------------------------------------------

	//その他関数///////////////////////////////////////////
	//コールバック関数を設定_Beign//------------------------
	public	void	OnPointerDown(PointerEventData eventData){
		f_press	= true;
	}
	public	void	OnDrag(PointerEventData eventData){
	}
	public	void	OnPointerUp(PointerEventData eventData){
		f_press	= false;
	}
	//コールバック関数を設定_Beign//------------------------

}//ボタンのシステム_End//-----------------------------------
