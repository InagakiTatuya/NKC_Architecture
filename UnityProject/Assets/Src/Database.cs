﻿//#############################################################################
//  すべてのシーンで使う共通のデータを保管する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;


//クラス///////////////////////////////////////////////////////////////////////
public class Database : SingletonCustom<Database> {

    private int m_StageId;
    public  int StageId { 
        get{ return m_StageId;  } 
        set{ m_StageId = value; }
    }


    //初期化===================================================================
    void Awak() {
        BaseAwake(this); //シングルトンの設定をする
    }

	void Start () {
	
	}
	
    //更新=====================================================================
	void Update () {
	
	}

}