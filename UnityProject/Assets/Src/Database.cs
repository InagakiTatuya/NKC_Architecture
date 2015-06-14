//#############################################################################
//  すべてのシーンで使う共通のデータを保管する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//クラス///////////////////////////////////////////////////////////////////////
public class Database : SingletonCustom<Database> {

    //ステージ情報^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_StageId;
    public  int StageId { 
        get{ return m_StageId;  } 
        set{ m_StageId = value; }
    }

    //プレイヤー情報^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  const int          PLAYER_MAX_COUNT = 15; //参加できる数
    private StractPlayerData[] m_PlayerDatas;         //プレイヤーデータ
    
    public  int                PlayerCount{ get{ return m_PlayerDatas.Length; } }
    public  StractPlayerData[] PlayerData { get{ return m_PlayerDatas;        } }
    
    //初期化===================================================================
    void Awake() {
        BaseAwake(this); //シングルトンの設定をする

        Debug.Log("Database.awake()");
    }

	void Start () {
	
	}
	
    //更新=====================================================================
	void Update () {
	
	}

    //公開関数=================================================================
    
    //プレイヤーデータを入れる=================================================
    //  CardInputシーンで作られたデータを保存
    //-------------------------------------------------------------------------
    public void SetPlyaerDatas(ref StractPlayerData[] _datas) {
        m_PlayerDatas = new StractPlayerData[_datas.Length];
        _datas.CopyTo(this.m_PlayerDatas, 0);
    }

}
