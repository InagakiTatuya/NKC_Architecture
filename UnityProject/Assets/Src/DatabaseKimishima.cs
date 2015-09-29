//----------------------------------------------------------
//データベース
//更新者 :	君島一刀
//----------------------------------------------------------

//名前空間//////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//データベース_Begin//--------------------------------------
public partial class Database : SingletonCustom<Database> {
	
	public	enum JobID{//ジョブ番号//-----------------------
		Yuka,
		Hashira,
		Kabe,
		Yane,
		Length
	}

	public	static	string	TutorialDataName	= "MaxFloorTutorial";
	public	static	string	MainDataName		= "MaxFloorMain";

	/// <summary>最大フロアを取得できるよやったー（棒）</summary>
	/// <value>The max floor.</value>
	public	static	int		MaxFloor{
		get{
			if(SelectSystem.TutorialFlg)	return	PlayerPrefs.GetInt(TutorialDataName,0);
			else							return	PlayerPrefs.GetInt(MainDataName,0);
		}
		set{
			if(SelectSystem.TutorialFlg)	PlayerPrefs.SetInt(TutorialDataName,value);
			else							PlayerPrefs.SetInt(MainDataName,value);
		}
	}

	/// <summary>最大のフロアを取得できる</summary>
	/// <returns>The max floor.</returns>
	/// <param name="name">Name.</param>
	public	static	int		GetMaxFloor(string name){
		return	PlayerPrefs.GetInt(name,0);
	}

	//パーツの名前_Begin//----------------------------------
	public	static	readonly	string[,]	tablePartsName	= new string[,]{
		{
			"Yuka 00",		"Yuka 01",//		"Yuka 02",		"Yuka 03",
		//	"Yuka 04",		"Yuka 05",		"Yuka 06",		"Yuka 07",
		//	"Yuka 08",		"ダンボール",
		},
		{
			"Hashira 00",	"Hashira 01",//	"Hashira 02",	"Hashira 03",
		//	"Hashira 04",	"Hashira 05",	"Hashira 06",	"Hashira 07",
		//	"Hashira 08",	"ダンボール",
		},
		{
			"Kabe 00",		"Kabe 01",//		"Kabe 02",		"Kabe 03",
		//	"Kabe 04",		"Kabe 05",		"Kabe 06",		"Kabe 07",
		//	"Kabe 08",		"ダンボール",
		},
		{
			"Yane 00",		"Yane 01",//		"Yane 02",		"Yane 03",
		//	"Yane 04",		"Yane 05",		"Yane 06",		"Yane 07",
		//	"Yane 08",		"ダンボール",
		}
	};//パーツの名前_End//----------------------------------

	//職業の説明文_Begin//----------------------------------
	public	static	readonly	string[]	tableJobDesc	= new string[]{
		"床を作るSuper職人。\nその腕前は超!一流で\n見るものを魅了する。",
		"柱を作るSuper職人。\nその腕前は超!一流で\n見るものを魅了する。",
		"壁を作るSuper職人。\nその腕前は超!一流で\n見るものを魅了する。",
		"屋根を作るSuper職人。\nその腕前は超!一流で\n見るものを魅了する。",
	};//職業の説明文_End//----------------------------------

	public	enum 	ColorBlockID{//色のID_Beign//-----------
		White,
		Black,
		Red,
		Yellow,
		Green,
		Cyan,
		Blue,
		Length,
	}//色のID_End//-----------------------------------------
	private	static	bool			colorBlocksInitedFlg	= false;
	public	static	ColorBlock[]	colorBlocks;
	//色を初期化_Begin//------------------------------------
	public	static	void	InitColorBlock(){
		if(colorBlocksInitedFlg)	return;
		Color[]	nomalColor		= new Color[]{
			Color.white,Color.black,Color.red,Color.yellow,
			Color.green,Color.cyan,Color.blue,
		};
		Color[]	highlightColor	= new Color[]{
			Color.yellow + Color.gray,
			Color.yellow + Color.gray,
			Color.yellow + Color.gray,
			Color.white,
			Color.yellow + Color.gray,
			Color.yellow + Color.gray,
			Color.yellow + Color.gray,
		};
		Color[]	pressedColor	= new Color[]{
			Color.yellow,Color.yellow,Color.yellow,Color.white,
			Color.yellow,Color.yellow,Color.yellow,
		};
		colorBlocks	= new ColorBlock[(int)ColorBlockID.Length];
		for(int i = 0;i < colorBlocks.Length;i ++){
			colorBlocks[i].normalColor		= nomalColor[i];
			colorBlocks[i].highlightedColor	= highlightColor[i];
			colorBlocks[i].pressedColor		= pressedColor[i];
			colorBlocks[i].disabledColor	= nomalColor[i] - new Color(0.75f,0.75f,0.75f,0.0f);
			colorBlocks[i].colorMultiplier	= 1;
			colorBlocks[i].fadeDuration		= 0.1f;
		}
		colorBlocksInitedFlg	= true;
	}//色を初期化_End//-------------------------------------

}//データベース_End//---------------------------------------
