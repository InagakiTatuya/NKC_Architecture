//----------------------------------------------------------
//ボタンのシステム
//更新日 :	06 / 13 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
	#define	DEBUG_BUTTONSYSTEM
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.UI;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ボタンのシステム_Begin//----------------------------------
public class ButtonSystem : MonoBehaviour {

//パブリックフィールド//------------------------------------

	//変数//////////////////////////////////////////////////
	public	string		text		= "debug";
	public	int			fontSize	= 32;
	public	Color		color		= Color.white;

	//初期化////////////////////////////////////////////////
	public	void	Start () {//初期化_Begin//--------------
		buttonImage	= GetComponent<Image>();
		StartCreateText();
	}//初期化_End//-----------------------------------------

	//更新//////////////////////////////////////////////////
	public	void	Update(){//更新_Beign//-----------------
		textObject.rectTransform.sizeDelta		= buttonImage.rectTransform.sizeDelta;
	}//更新_End//-------------------------------------------

//プライベートフィールド//----------------------------------

	//変数//////////////////////////////////////////////////
	private	Text	textObject	= null;
	private	Image	buttonImage	= null;

	//初期化////////////////////////////////////////////////
	//ボタンの文字を生成_Begin//----------------------------
	private	void	StartCreateText(){
		GameObject	obj			= Instantiate(Resources.Load<GameObject>("Prefab/Select/Text"));
					textObject	= obj.GetComponent<Text>();
		obj.transform.SetParent(this.gameObject.transform);
		textObject.text							= text;
		textObject.fontSize						= fontSize;
		textObject.color						= color;
		textObject.rectTransform.localPosition	= Vector3.zero;
		textObject.rectTransform.localScale		= Vector3.one;
	}//ボタンの文字を生成_End//-----------------------------

}//ボタンのシステム_End//-----------------------------------
