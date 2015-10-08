//---------------------------------------------------
//数字を描画
//---------------------------------------------------

//名前空間//-----------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス//-------------------------------------------
public class NumberDisp : MonoBehaviour {

	//変数//-----------------------------------------
	private	Image[]		image;
	private	Sprite[]	sprite;

	public	int			cnt	= 3;
	public	int			value;
	public	Vector3		pos;
	public	Vector2		size;
	public	float		offset;
	public	Color		color;

	//初期化//---------------------------------------
	void Start () {
		image	= new Image[cnt];
		sprite	= Resources.LoadAll<Sprite>("Texture/Select/number");
		for(int i = 0;i < image.Length;i ++){
			GameObject	obj			= Instantiate(new GameObject());
			obj.transform.parent	= transform.parent;
			image[i]	= obj.AddComponent<Image>();
		}
	}
	
	//更新//-----------------------------------------
	void Update () {
		int[]	table	= new int[]{//数値を計算して出しておく
			 value % 10,
			(value <  10)?-1:(value / 10)  % 10,
			(value < 100)?-1:(value / 100) % 10
		};
		for(int i = 0;i < image.Length;i ++){//桁の分だけループ
			if(table[i] < 0){
				image[i].sprite	= null;
				image[i].color	= new Color(0.0f,0.0f,0.0f,0.0f);
			}else{
				image[i].sprite	= sprite[table[i]];
				image[i].color	= color;
			}
			image[i].rectTransform.localPosition= pos + new Vector3(offset * i,0.0f,0.0f);
			image[i].rectTransform.sizeDelta	= size;
		}
	}
}
