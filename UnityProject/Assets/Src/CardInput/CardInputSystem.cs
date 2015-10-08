//#############################################################################
//  社員証入力シーンを統括するクラス
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class CardInputSystem : MonoBehaviour {

    //公開定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    
    //アタッチされてるGameObjectの名前
    public const string GAMEOBJCT_NAME = "System"; 

    //ステート定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public const int STATE_INSCENE   = 0;  //シーンに入ってきた
    public const int STATE_USUALLY   = 1;  //通常
    public const int STATE_OUTSCENE  = 2;  //シーンを出る
    public const int STATE_CARDINPUT = 3;  //カードのデータを入力

    public const int STATE_NO_MAX    = 4;   //ステートの種類数

    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private ClassStateManager m_State;

    private const float INSCENE_TIME  = 0.25f; //INSTAET
    private const float OUTSCENE_TIME = 0.25f; //OUTSTATE

    private string m_NextSceneName;

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private MessageWind     m_MesWind;  //メッセージウィンドウ

    private CardManager     m_cardMgr;
    private CardInputWind   m_ciWind;
    private CanvasFade      m_Fade;
    private SeManager       m_seMgr;

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  int getState        { get{return m_State.getState;       } }

    public CardManager   getCardMgr       { get{ return m_cardMgr;  } }
    public CardInputWind getCardInputWind { get{ return m_ciWind;   } }
    public SeManager     getSeMgr         { get{ return m_seMgr;    } }

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        gameObject.name = GAMEOBJCT_NAME;
    }
    
    void Start() {
        //ステートの関数ポインタを初期化---------------------------------------
        UnityAction[] fnInitArr = new UnityAction[STATE_NO_MAX] {
            InitForInScene,
            InitForUsually,
            InitForOutScene,
            InitForCardInput,
        };
        UnityAction[] fnUpdateArr = new UnityAction[STATE_NO_MAX] {
            UpdateForInScene,
            UpdateForUsually,
            UpdateForOutScene,
            null,
        };

        m_State = new ClassStateManager(STATE_NO_MAX, fnInitArr, fnUpdateArr);

        //ネクストシーン-------------------------------------------------------
        m_NextSceneName = "";

        //参照-----------------------------------------------------------------
        m_MesWind = GameObject.Find("Canvas/MessageWind")
                                            .GetComponent<MessageWind>();
        m_cardMgr = GameObject.Find("Canvas/CardArea/CardAnc-CardMgr")
                                            .GetComponent<CardManager>();
        m_ciWind  = GameObject.Find("Canvas/CardInputWind")
                                            .GetComponent<CardInputWind>();
        m_Fade    = GameObject.Find("Canvas/CanvasFade")
                                            .GetComponent<CanvasFade>();
        m_seMgr   = GameObject.Find("SEObject").GetComponent<SeManager>();
    }

    //更新=====================================================================
	void Update () {
        //ステートの更新
        m_State.Update(); 
    }

    //ステート関数/////////////////////////////////////////////////////////////
    
    //========================================================= InScene =======
    private void InitForInScene() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //カードの初期化
        m_cardMgr.SendLordData();
        
        //フェード
        m_Fade.In(INSCENE_TIME);
    }

    private void UpdateForInScene() {

        //一定時間になったら次のステートへ移行
        if(m_State.getStateTime >= INSCENE_TIME) {
            m_State.SetNextState(STATE_USUALLY);
        }
    }

    //========================================================= Usually =======
    private void InitForUsually() { 
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }

    private void UpdateForUsually() {

    }


    //========================================================= OutScene ======
    private void InitForOutScene() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        
        //次のシーンが指定されているか
        if(m_NextSceneName == "") {
            Debug.LogError("次のシーン名が指定されていません");
            return;
        }
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        
        m_Fade.Out(OUTSCENE_TIME);
    }
    private void UpdateForOutScene() {

        //一定時間になったら次のシーンへ移行
        if(m_State.getStateTime >= INSCENE_TIME) {
            Application.LoadLevel(m_NextSceneName);
        }
    }
    //========================================================= CardInput ======
    private void InitForCardInput() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
       //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }
    //  何も処理しないためコメントアウト
    //private void UpdateForCardInput() { }
    

    //イベント/////////////////////////////////////////////////////////////////
    //CardInput終了============================================================
    //  CardInptuState終了イベント CardInptuWindからよび出される
    //=========================================================================
    public void EndCardInputEvent() {
        m_State.SetNextState(STATE_USUALLY);
    }
    
    //完了ボタン===============================================================
    //  タイミング：完了ボタンが押されたとき
    //    カードマネージャーにデータをデータベースに送るよう指示をだす
    //    カードマネージャーが失敗したらメッセージをだす
    //    成功したらシーン移行する。
    //=========================================================================
    public void OnAppButtonEnter() {
        if(m_State.getState != STATE_USUALLY) return;

        bool error = m_cardMgr.SendDataToDatabase();

        if(error) {
            m_MesWind.OpenWind();
            return;
        }

        m_NextSceneName = "Game";
        m_State.SetNextState(STATE_OUTSCENE);
    }

    //戻るボタン===============================================================
    //  タイミング：戻るボタンが押されたとき
    //    セレクトシーン移行する。
    //=========================================================================
    public void OnReturnButtonEnter() {
        if(m_State.getState != STATE_USUALLY) return;

        m_NextSceneName = "Select";
        m_State.SetNextState(STATE_OUTSCENE);
    }
}
