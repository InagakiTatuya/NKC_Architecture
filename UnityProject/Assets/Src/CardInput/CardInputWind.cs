//#############################################################################
//  社員証にデータ入力とそのウィンドウを管理
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class CardInputWind : MonoBehaviour {
    
    //ステート定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public const int STATE_NOTACTIVE      = 0;  //非アクティブ
    public const int STATE_OPENWIND       = 1;  //ウィンドウを開く
    public const int STATE_CLAUSEWIND     = 2;  //ウィンドウを閉じる
    public const int STATE_INPUTDATA      = 3;  //データを入力
    public const int STATE_PAGECHANGE     = 4;  //パーツのタブを変更する
    public const int STATE_OPENMESSWIND   = 5;  //メッセージウィンドウを出す
    public const int STATE_CLAUSEMESSWIND = 6;  //メッセージウィンドウを閉じる

    public const int STATE_MAX_CNT        = 7;   //ステートの種類数
    
    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_StateNo;      //ステート番号
    private int m_NextStateNo;  //次のフレームでのステート番号

    private float m_StateTime; //ステート内で使用する

    private float OPENWIND_TIME   = 1.0f; //開く際の演出時間
    private float CLAUSEWIND_TIME = 1.0f; //閉じる際の演出時間

    //各ステートの更新関数のポインタ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private delegate void StateFunc();
    private StateFunc[] m_fnIniteArr;   //初期化用
    private StateFunc[] m_fnUpdateArr;  //更新用
    
    //統括しているシステム^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputSystem ciSystem;  //このシーンを統括するSystem
    private CardManager     cardMgr;   //社員証を管理するMaanager
    
    //制御する子の参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private RectTransform   m_Back;     //黒背景
    private RectTransform   m_Wind;     //ウィンドウ
    private InputField      m_Input;     //名前入力
    private Image           m_ImageHair; //髪型
    private Image           m_ImageFace; //顔
    private Image           m_ImageBody; //体
    
    //編集するカード^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int                 m_IndexBff; //管理番号
    private StractPlayerData    m_DataBff;  //データ


    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //this.gameObject.SetActive(false);
    }
    
    void Start() {
        //ステートの関数ポインタを初期化---------------------------------------
        m_fnIniteArr = new StateFunc[STATE_MAX_CNT] {
            null,
            InitForOpenWind,
            InitForClauseWind,
            InitForInputData,
            null,
            null,
            null,
        };
        m_fnUpdateArr = new StateFunc[STATE_MAX_CNT] {
            null,
            UpdateForOpenWind,
            UpdateForClauseWind,
            UpdateForInputData,
            null,
            null,
            null,
        };

        //システムの参照-------------------------------------------------------
        ciSystem = GameObject.Find("System").GetComponent<CardInputSystem>();
        cardMgr  = GameObject.Find("Canvas/CardArea/CardAnc-CardMgr")
            .GetComponent<CardManager>();

        //制御する子の参照-----------------------------------------------------
        m_Back      = transform.FindChild("InputWindBack") as RectTransform;
        m_Wind      = m_Back.FindChild("Wind") as RectTransform;
        m_ImageHair = m_Wind.FindChild("Card/PhotoBack/PhotoHair")
            .GetComponent<Image>();
        m_ImageFace = m_Wind.FindChild("Card/PhotoBack/PhotoFace")
            .GetComponent<Image>();
        m_ImageBody = m_Wind.FindChild("Card/PhotoBack/PhotoBody")
            .GetComponent<Image>();

        //非表示にする---------------------------------------------------------
        m_Back.gameObject.SetActive(false);

        //参照以外の初期化-----------------------------------------------------
        Init();
    }
	
    //参照以外の初期化=========================================================
    private void Init() {
        m_NextStateNo = STATE_NOTACTIVE;  
        m_StateTime = 0.0f;
    }

    //更新=====================================================================
	void Update () {
        //ステートの初期化-----------------------------------------------------
        if(0 <= m_NextStateNo && m_NextStateNo < STATE_MAX_CNT) {
            //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
            #if UNITY_EDITOR 
            Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
                this.GetType().Name + " - StateChange : old=" +
                m_StateNo + " => new="+m_NextStateNo);
            #endif
            //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
            m_StateNo = m_NextStateNo;
            m_NextStateNo = -1; //Initを一回だけ呼ぶために-1を入れてる
            if(m_fnIniteArr[m_StateNo] != null) m_fnIniteArr[m_StateNo]();
            m_StateTime = 0.0f;
        }
        
        //ステート別のアップデート---------------------------------------------
        if(m_fnUpdateArr[m_StateNo] != null) { 
            m_fnUpdateArr[m_StateNo]();
            m_StateTime += Time.deltaTime;
        } 
	}

    //ステート更新関数=========================================================
    
    //========================================================= OpenWind ======
    //  初期化  OpenWind
    //　　アクティブにし、アニメーションを行い
    //　　アニメーションが終了後ステート移行する
    private void InitForOpenWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //アクティブ
        m_Back.gameObject.SetActive(true);
    }
    //  更新  OpenWind
    private void UpdateForOpenWind() {

        //一定時間になったら次のステートへ移行
        if(m_StateTime >= OPENWIND_TIME) {
            m_NextStateNo = STATE_INPUTDATA;
        }
    }
    
    //========================================================= ClauseWind ====
    //  初期化  ClauseWind
    private void InitForClauseWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }
    //  更新  ClauseWind
    private void UpdateForClauseWind() {

        //一定時間になったら自分を非表示にし
        //処理が終えたことをシステムに伝える
        if(m_StateTime >= CLAUSEWIND_TIME) {
            this.gameObject.SetActive(false);
            ciSystem.EndCardInputEvent();
        }
    }
    
    //========================================================= InputData =====
    //  初期化  InputData
    private void InitForInputData() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }
    //  更新  InputData
    private void UpdateForInputData() {
    
    }
    
    //公開関数/////////////////////////////////////////////////////////////////
    
    //ウィンドウを表示する=====================================================
    //　データを入力するためのウィンドウを表示します。
    //  編集するカードの管理番号が必須です。
    //  第一引数：カードの管理番号
    //  第二引数：カードに入っているデータ
    public void OpenCradInputWind(int _CardIndex, StractPlayerData _data) {
        Init();
        m_NextStateNo = STATE_OPENWIND; //ウィンドウを開く
        //データを一時保存
        m_IndexBff = _CardIndex;
        m_DataBff  = _data;

    }
    //オーバーロード===========================================================
    //  第一引数：カードの管理番号
    public void OpenCradInputWind(int _CardIndex) {
        StractPlayerData data = new StractPlayerData();
        data.Init();
        this.OpenCradInputWind(_CardIndex, data);
    }

    //イベント=================================================================

    //社員追加ボタン===========================================================
    //  タイミング：社員追加ボタンがタップされた瞬間。
    //    ウィンドウをアクティブにし、初期化する
    //=========================================================================
    public void OnPlayerAddButtonEnter() {
        //ステートが通常時以外は、処理しない
        if(ciSystem.getState != CardInputSystem.STATE_USUALLY) return;
        //プレイヤー数が最大値に達していたら処理しない
        if(cardMgr.getPlayerCount >= Database.PLAYER_MAX_COUNT) return;

        Init(); //初期化

    }
    
    //完了ボタン===============================================================
    //  タイミング：ＯＫボタンがタップされた瞬間。
    //    データが正しければ、データをCardManagerに渡す
    //    そうでない場合は、メッセージウィンドウをだす
    //=========================================================================
    public void OnAppButtonEnter() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        if(m_StateNo != STATE_INPUTDATA) return;

        //仮処理
        m_NextStateNo = STATE_CLAUSEWIND;
    }
    

}
