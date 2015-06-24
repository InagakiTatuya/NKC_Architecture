//#############################################################################
//  社員証入力シーンを統括するクラス
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class CardInputSystem : MonoBehaviour {

    //ステート定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public const int STATE_INSCENE   = 0;  //シーンに入ってきた
    public const int STATE_USUALLY   = 1;  //通常
    public const int STATE_OUTSCENE  = 2;  //シーンを出る
    public const int STATE_CARDINPUT = 3;  //カードのデータを入力


    public const int STATE_MAX_CNT   = 4;   //ステートの種類数

    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_StateNo;      //ステート番号
    private int m_NextStateNo;  //次のフレームでのステート番号

    private float m_StateTime; //ステート内で使用する
    private const float INSCENE_TIME  = 1.0f; //INSTAET
    private const float OUTSCENE_TIME = 1.0f; //OUTSTATE

    //各ステートの更新関数のポインタ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private delegate void StateFunc();
    private StateFunc[] m_fnIniteArr;   //初期化用
    private StateFunc[] m_fnUpdateArr;  //更新用

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardManager     cardMgr;
    private CardInputWind   ciWind;

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  int getState        { get{return m_StateNo;       } }
    public  int setNextState    { set{ m_NextStateNo = value; } }

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {

    }
    
    void Start() {
        //ステートの関数ポインタを初期化---------------------------------------
        m_fnIniteArr = new StateFunc[STATE_MAX_CNT] {
            InitForInScene,
            InitForUsually,
            InitForOutScene,
            InitForCardInput,
        };
        m_fnUpdateArr = new StateFunc[STATE_MAX_CNT] {
            UpdateForInScene,
            UpdateForUsually,
            UpdateForOutScene,
            null,
        };
        //参照-----------------------------------------------------------------
        cardMgr = GameObject.Find("Canvas/CardArea/CardAnc-CardMgr")
            .GetComponent<CardManager>();
        ciWind = GameObject.Find("Canvas/CardInputWind")
            .GetComponent<CardInputWind>();
    
    }

    //更新=====================================================================
    void Update() {

        //ステートの初期化-----------------------------------------------------
        if(0 <= m_NextStateNo && m_NextStateNo < STATE_MAX_CNT) {
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
    
    //========================================================= InScene =======
    //  初期化  InScene
    private void InitForInScene() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }

    //  更新  InScene
    private void UpdateForInScene() {

        //一定時間になったら次のステートへ移行
        if(m_StateTime >= INSCENE_TIME) {
            m_NextStateNo = STATE_USUALLY;
        }
    }

    //========================================================= Usually =======
    //  初期化  Usually
    private void InitForUsually() { 
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //最初の一人目のデータを入力するため社員証入力ウィンドウを開く
        ciWind.OpenCradInputWind( 0 );
    }

    //  更新  Usually
    private void UpdateForUsually() {

    }


    //========================================================= OutScene ======
    //  初期化  OutScene
    private void InitForOutScene() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }
    //  更新  OutScene
    private void UpdateForOutScene() {

        //一定時間になったら次のシーンへ移行
        if(m_StateTime >= INSCENE_TIME) {
            Application.LoadLevel("Game");
        }
    }
    //========================================================= CardInput ======
    //  初期化  OutScene
    private void InitForCardInput() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }
    //  更新  OutScene
    //  何も処理しないためコメントアウト
    //private void UpdateForCardInput() { }
    
    //CardInput終了
    //  CardInptuState終了イベント CardInptuWindからよび出される
    public void EndCardInputEvent() {
        m_NextStateNo = STATE_USUALLY;
    }

    //OnButtonイベント=========================================================
    
    //完了ボタン===============================================================
    //  シーンから出るステートに移行する
    public void OnAppButtonEnter() {
        if(m_StateNo != STATE_USUALLY) return;
        m_NextStateNo = STATE_OUTSCENE;
    }

}
