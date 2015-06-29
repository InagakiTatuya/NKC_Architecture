//----------------------------------------------------------
//データベース
//更新日 :	06 / 29 / 2015
//更新者 :	君島一刀
//----------------------------------------------------------

//名前空間//////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//データベース_Begin//--------------------------------------
public partial class Database : SingletonCustom<Database> {

	//パーツの名前_Begin//----------------------------------
	public	static	readonly	string[,]	tablePartsName	= new string[,]{
		{
			"Yuka 00",		"Yuka 01",		"Yuka 02",		"Yuka 03",
			"Yuka 04",		"Yuka 05",		"Yuka 06",		"Yuka 07",
			"Yuka 08",		"Yuka 09",
		},
		{
			"Hashira 00",	"Hashira 01",	"Hashira 02",	"Hashira 03",
			"Hashira 04",	"Hashira 05",	"Hashira 06",	"Hashira 07",
			"Hashira 08",	"Hashira 09",
		},
		{
			"Kabe 00",		"Kabe 01",		"Kabe 02",		"Kabe 03",
			"Kabe 04",		"Kabe 05",		"Kabe 06",		"Kabe 07",
			"Kabe 08",		"Kabe 09",
		},
		{
			"Yane 00",		"Yane 01",		"Yane 02",		"Yane 03",
			"Yane 04",		"Yane 05",		"Yane 06",		"Yane 07",
			"Yane 08",		"Yane 09",
		}
	};//パーツの名前_End//----------------------------------

	//職業の説明文_Begin//----------------------------------
	public	static	readonly	string[]	tableJobDesc	= new string[]{
		"床を作るスーパー職人。\nその腕前は超一流で\n見るものを魅了する。",
		"柱を作るスーパー職人。\nその腕前は超一流で\n見るものを魅了する。",
		"壁を作るスーパー職人。\nその腕前は超一流で\n見るものを魅了する。",
		"屋根を作るスーパー職人。\nその腕前は超一流で\n見るものを魅了する。",
	};//職業の説明文_End//----------------------------------

}//データベース_End//---------------------------------------
