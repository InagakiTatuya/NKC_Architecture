//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	UnityEngine.EventSystems;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ボタンのシステム_Begin//----------------------------------
public class ButtonScale : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler {

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////

	private	Vector2		buttonSize;
	private Image		image;

	private	Vector2		size;

	//ステートチェックプロパティ
	private	bool		f_press;
	public	bool		press{get{return	f_press;}}

	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化
		StartButton();
		size			= buttonSize;
	}
	private	void	StartButton(){//ボタンを初期化
		image	= GetComponent<Image>();
		buttonSize = image.rectTransform.localScale;
	}

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		image.rectTransform.localScale		= size;
		if(f_press)	size	= size * 0.5f + buttonSize * 0.4f;
		else 		size	= size * 0.5f + buttonSize * 0.5f;
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
