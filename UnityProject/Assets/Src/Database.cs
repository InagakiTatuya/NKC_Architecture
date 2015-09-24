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
public partial class Database : SingletonCustom<Database> {
    //リソースデータ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //写真^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    // DatabaseTexCardInput.cs に定義

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
    public  const int          PLAYER_MAX_COUNT     = 15; //参加できる数
    public  const int          PLAYER_NAME_MAX_BYTE =  8; //名前の長さ制限
    private StractPlayerData[] m_PlayerDatas;             //プレイヤーデータ
    
    public  int                getPlayerCount{ get{ return m_PlayerDatas.Length; } }
    public  StractPlayerData[] getPlayerData { get{ return m_PlayerDatas;        } }
    

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        BaseAwake(this); //シングルトンの設定をする
        DontDestroyOnLoad(gameObject); //シーンを切り替えても破棄しない

        //写真データ-----------------------------------------------------------
        this.CardInputAwake(); //DatabaseTexCardInput.cs に定義

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

        Debug.Log("Database.Awake End");
    }

	void Start () {

	}
	
    //更新=====================================================================
	void Update () {
	
	}

    //公開関数/////////////////////////////////////////////////////////////////
    
    //プレイヤーデータを入れる=================================================
    //  CardInputシーンで作られたデータを保存
    //=========================================================================
    public void SetPlyaerDatas(ref StractPlayerData[] aDatas) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        m_PlayerDatas = new StractPlayerData[aDatas.Length];
        aDatas.CopyTo(m_PlayerDatas, 0);
    }


    //プレイヤーデータを読み込む===============================================
    //  保存されているデータを第一引数にコピーする
    //  戻り値：保存されていなかったら Ture を返す
    //=========================================================================
    public bool LordPlyaerDatas(ref StractPlayerData[] aDatas) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            "PlayerDatas = " + m_PlayerDatas +
            (m_PlayerDatas != null ? (" Length = " + m_PlayerDatas.Length) : ("null")));
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        if(m_PlayerDatas == null || m_PlayerDatas.Length == 0) return true;

        aDatas = null;
        aDatas = new StractPlayerData[m_PlayerDatas.Length];
        m_PlayerDatas.CopyTo(aDatas, 0);
        return false;
    }

}
