using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashingUI {
	private Image	image;
	private Color	color;
	private float	speed;
	private float	max,min;
	private bool	flg;

	public FlashingUI(Image image, float speed = 1.0f, float max = 1.0f, float min = 0.0f){
		this.image	=	image;
		this.speed	=	speed;

		if(min > max){
			float temp;
			temp	=	max;
			max		=	min;
			min		=	temp;
		}
			
		this.max	=	Mathf.Clamp01(max);
		this.min	=	Mathf.Clamp01(min);
		
		this.color	=	image.color;
		if(color.a == 0)	flg	=	true;
		else				flg	=	false;
	}

	public void Flash(){
		if(flg){
			color.a	+=	Time.deltaTime * speed;
			if(color.a >= max){
				color.a	=	max;
				flg		=	false;
			}
		}else{
			color.a	-=	Time.deltaTime * speed;
			if(color.a <= min){
				color.a	=	min;
				flg		=	true;
			}
		}
		image.color	=	color;
	}

	public void SetColorAlpha(float a){
		Color color = new Color();
		color		= image.color;
		color.a		= a;
		image.color	= color;
	}

}
