//#############################################################################
//  すべてのシーンで使う共通のデータを保管する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//クラス///////////////////////////////////////////////////////////////////////
public class Database : SingletonCustom<Database> {
    //リソースデータ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //写真^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  const int   FHOT_NO_MAX = 1;
    private Sprite[]    M_FHOT_SPRITE;

    public  Sprite[]    FHOT_SPRITE { get { return M_FHOT_SPRITE; } }

    //職種^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  const int   JOB_NO_MAX = 4;
    private string[]    M_JOB_NAME;
    private string[]    M_JOB_TEXT;

    public  string[]    JOB_NAME { get { return M_JOB_NAME; } }
    public  string[]    JOB_TEXT { get { return M_JOB_TEXT; } }

    //ステージ情報^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_StageId;
    public  int StageId { 
        get{ return m_StageId;  } 
        set{ m_StageId = value; }
    }

    //プレイヤー情報^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  const int          PLAYER_MAX_COUNT = 15; //参加できる数
    private StractPlayerData[] m_PlayerDatas;         //プレイヤーデータ
    
    public  int                getPlayerCount{ get{ return m_PlayerDatas.Length; } }
    public  StractPlayerData[] getPlayerData { get{ return m_PlayerDatas;        } }

    //初期化===================================================================
    void Awake() {
        BaseAwake(this); //シングルトンの設定をする
        DontDestroyOnLoad(gameObject); //シーンを切り替えても破棄しない
        //写真データ-----------------------------------------------------------
        //=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#
        //  ここにSpriteを読み込む処理
        //=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#


        //職人データ-----------------------------------------------------------
	    M_JOB_NAME = new string[JOB_NO_MAX] {
            "床職人",
            "柱職人",
            "壁職人",
            "屋根職人",
        };
        M_JOB_TEXT = new string[JOB_NO_MAX] {
            "床を作る人",
            "柱を作る人",
            "壁を作る人",
            "屋根を作る人",
        };
        //プレイヤー情報^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        //  デバック用仮データ
        StractPlayerData[] kariData = new StractPlayerData[ 5 ];
        kariData[0].name = "仮名零";
        kariData[1].name = "仮名壱";
        kariData[2].name = "仮名弐";
        kariData[3].name = "仮名参";
        kariData[4].name = "仮名私";
        this.SetPlyaerDatas(ref kariData);


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
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        m_PlayerDatas = new StractPlayerData[_datas.Length];
        _datas.CopyTo(this.m_PlayerDatas, 0);
    }

}
