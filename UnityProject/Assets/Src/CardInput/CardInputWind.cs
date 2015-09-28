//#############################################################################
//  社員証にデータ入力とそのウィンドウを管理
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public partial class CardInputWind : MonoBehaviour {
    
    //ステート定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public const int STATE_NOTACTIVE      = 0;  //非アクティブ
    public const int STATE_OPENWIND       = 1;  //ウィンドウを開く
    public const int STATE_INPUTDATA      = 2;  //データを入力
    public const int STATE_PABCHANGE      = 3;  //パーツのタブを変更する
    public const int STATE_OPENMESWIND    = 4;  //メッセージウィンドウを出す
    public const int STATE_CLAUSEMESWIND  = 5;  //メッセージウィンドウを閉じる
    public const int STATE_CLAUSEWIND     = 6;  //ウィンドウを閉じる

    public const int STATE_NO_MAX         = 7;   //ステートの種類数
    
    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private ClassStateManager m_State;

    private float OPEN_WIND_TIME      = 0.4f; //開く際の演出時間
    private float CLAUSE_WIND_TIME    = 0.1f; //閉じる際の演出時間
    private float OPEN_MESWIND_TIME   = 0.1f; //メッセージウィンドウを開く演出時間
    private float CLAUSE_MESWIND_TIME = 1.0f; //メッセージウィンドウを閉じる演出時間
    
    //編集するカード^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int               m_IndexBff; //管理番号
    private StractPlayerData  m_DataBff;  //データ
    
    //制御する子の参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private RectTransform     m_Back;      //黒背景
    private RectTransform     m_Wind;      //ウィンドウ
    private RectTransform     m_Card;      //社員証

    //ウィンドウ
    private MessageWind       m_MesWind;  //メッセージウィンドウ
    
    //見た目
    private MobileInputField  m_Input;     //名前入力
    private Image             m_ImageHair; //髪型
    private Image             m_ImageFace; //顔
    private Image             m_ImageBody; //体

    //アニメーション用変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Vector3           m_WindNotActPos;
    private Vector3           m_WindActPos;
    private Vector3           m_WindVec;

    //統括しているシステム^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputSystem ciSystem;  //このシーンを統括するSystem
    
    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //制御する子の参照-----------------------------------------------------
        m_Back      = transform.FindChild("InputWindBack") as RectTransform;
        m_Wind      = transform.FindChild("Wind"         ) as RectTransform;
        m_Card      = transform.FindChild("Wind/Card"    ) as RectTransform;
        
        //メッセージウィンドウ
        m_MesWind   = transform.FindChild("MessageWind"  )
                                                .GetComponent<MessageWind>();
        
        //名前入力
        m_Input     = m_Wind.FindChild("Card/InputField")
                                        .GetComponent<MobileInputField>();
        
        //見た目
        m_ImageHair = m_Wind.FindChild("Card/PhotoBack/PhotoHair")
                                                .GetComponent<Image>();
        m_ImageFace = m_Wind.FindChild("Card/PhotoBack/PhotoFace")
                                                .GetComponent<Image>();
        m_ImageBody = m_Wind.FindChild("Card/PhotoBack/PhotoBody")
                                                .GetComponent<Image>();

    }
    
    void Start() {
        //ステートの関数ポインタを初期化---------------------------------------
        UnityAction[] fnInitArr = new UnityAction[STATE_NO_MAX] {
            InitForNotActive,
            InitForOpenWind,
            InitForInputData,
            null,
            InitForOpenMesWind,
            InitForClauseMesWind,
            InitForClauseWind,
        };
        UnityAction[] fnUpdateArr = new UnityAction[STATE_NO_MAX] {
            null,
            UpdateForOpenWind,
            UpdateForInputData,
            null,
            UpdateForOpenMesWind,
            UpdateForClauseMesWind,
            UpdateForClauseWind,
        };

        m_State = new ClassStateManager(STATE_NO_MAX, fnInitArr, fnUpdateArr);

        //システムの参照-------------------------------------------------------
        ciSystem = GameObject.Find(CardInputSystem.GAMEOBJCT_NAME)
                                        .GetComponent<CardInputSystem>();
        //初期ステート---------------------------------------------------------
        m_State.SetNextState(STATE_NOTACTIVE);
        
        //イベント初期化-------------------------------------------------------
        EventsInit();

        //アニメーション初期化-------------------------------------------------
        m_WindActPos    = m_Wind.localPosition;
        m_WindNotActPos = new Vector3(m_WindActPos.x + 544, m_WindActPos.y, m_WindActPos.z);
        m_Wind.localPosition = m_WindNotActPos;
    }
	
    //更新=====================================================================
	void Update () {
        //ステートの更新-------------------------------------------------------
        m_State.Update();
        
	}

    //ステート更新関数/////////////////////////////////////////////////////////
    //========================================================= NotActive =====
    //  初期化  NotActive
    //    非表示にする
    private void InitForNotActive() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //アクティブ
        ChildSetActive(false);
    }
    
    //========================================================= OpenWind ======
    //  初期化  OpenWind
    //　　アクティブにし、アニメーションを行い
    //　　アニメーションが終了後ステート移行する
    private void InitForOpenWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //アクティブ
        m_Back   .gameObject.SetActive(true);
        m_Wind   .gameObject.SetActive(true);
        m_MesWind.gameObject.SetActive(true);

        //アニメーション用
        m_Wind.localPosition = m_WindNotActPos;
        m_WindVec = m_WindActPos - m_WindNotActPos;
        //データを適用
        DataApp();
    }
    //  更新  OpenWind
    private void UpdateForOpenWind() {
        
        //アニメーション-----------------------------------
        const float MIN = -45f;
        const float MAX = 90f;
        const float A = (MAX - MIN) / 180f * Mathf.PI;
        const float B = MIN / 180f * Mathf.PI;

        Vector3 lpos = m_Wind.localPosition;
        float t = m_State.getStateTime / OPEN_WIND_TIME;
        float n = (Mathf.Sin(t * A + B) + 1) * 0.5f;
        
        lpos = m_WindNotActPos + m_WindVec * n;
        if(m_State.getStateTime > OPEN_WIND_TIME) {
            lpos.x = 0;
            m_State.SetNextState(STATE_INPUTDATA);
        }
        m_Wind.localPosition = lpos;

    }
    
    //========================================================= InputData =====
    //  初期化  InputData
    private void InitForInputData() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }
    //  更新  InputData
    private void UpdateForInputData() {
    
    }

    //========================================================= OpenMesWind ===
    //  初期化  OpenMesWind
    private void InitForOpenMesWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
    }
    //  更新  OpenMesWind
    private void UpdateForOpenMesWind() {

        //一定時間になったら次のステートへ移行
        if(m_State.getStateTime >= OPEN_MESWIND_TIME) {
            m_State.SetNextState(STATE_CLAUSEMESWIND);
        }
    }
    //========================================================= ClauseMesWind =
    //  初期化  ClauseMesWind
    private void InitForClauseMesWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }
    //  更新  ClauseMesWind
    private void UpdateForClauseMesWind() {
        //一定時間になったら次のステートへ移行
        //  メッセージウィンドウを非表示にする
        if(m_State.getStateTime >= CLAUSE_MESWIND_TIME) {
            m_State.SetNextState(STATE_INPUTDATA);
        }
    }
    //========================================================= ClauseWind ====
    //  初期化  ClauseWind
    private void InitForClauseWind() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }
    //  更新  ClauseWind
    private void UpdateForClauseWind() {

        //一定時間になったら
        //自分を非表示にして、
        //処理が終えたことをシステムに伝える
        if(m_State.getStateTime >= CLAUSE_WIND_TIME) {
            //ステート変更＝＞非アクティブ
            m_State.SetNextState(STATE_NOTACTIVE);
            //システムにウィンドウが閉じたことを伝える
            ciSystem.EndCardInputEvent(); 
        }
    }

    //全ての子のアクティブ状態を操作===========================================
    //  全ての子に対しSetActive関数を呼び出す
    private void ChildSetActive(bool _value) {
        foreach(Transform ct in transform) {
            ct.gameObject.SetActive(_value);
        }
    }

    //データを画像とテキストに適用=============================================
    private void DataApp() {
        //----------------------------------------------------------- Name
        m_Input.text = m_DataBff.pleyerName;

        //----------------------------------------------------------- sprite
        m_ImageBody.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_BODY, m_DataBff.imageBodyNo];
        m_ImageFace.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_FACE, m_DataBff.imageFaceNo];
        m_ImageHair.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_HAIR, m_DataBff.imageHairNo];
    }

    //公開関数/////////////////////////////////////////////////////////////////
    
    //ウィンドウを表示する=====================================================
    //　データを入力するためのウィンドウを表示します。
    //  編集するカードの管理番号が必須です。
    //  第一引数：カードの管理番号
    //  第二引数：カードに入っているデータ
    //=========================================================================
    public void OpenCradInputWind(int _CardIndex, StractPlayerData _data) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            "Index=" + _CardIndex + " data=" + _data.ToString());
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        //データを一時保存-----------------------------------------------------
        m_IndexBff = _CardIndex;
        m_DataBff  = _data;

        //ステート移行---------------------------------------------------------
        m_State.SetNextState(STATE_OPENWIND); //＝＞ウィンドウを開く

    }
    //オーバーロード = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
    //  第一引数：カードの管理番号
    public void OpenCradInputWind(int _CardIndex) {
        StractPlayerData data = new StractPlayerData();
        data.Init();
        this.OpenCradInputWind(_CardIndex, data);
    }

    //パーツを選択=============================================================
    //  PlayerPartsSelectionから呼び出される
    public void SetPartsData(int _ImageType, int _ImageNo) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + " \n" +
            "ImageType = " + _ImageType + "  ImageID = " + _ImageNo);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        //一時保存
        switch(_ImageType) {
            //------------------------------------------------------- Body
            case Database.PLAYER_PARTS_BODY:
                m_DataBff.imageBodyNo = _ImageNo;
                break;
            //------------------------------------------------------- Face
            case Database.PLAYER_PARTS_FACE:
                m_DataBff.imageFaceNo = _ImageNo;
                break;
            //------------------------------------------------------- Hair
            case Database.PLAYER_PARTS_HAIR:
                m_DataBff.imageHairNo = _ImageNo;
                break;
        }

        //イメージ適用
        DataApp();
    }

    //イベント/////////////////////////////////////////////////////////////////
    //  CradInptuWindEvent.cs に定義


    //void OnGUI() {
    //    GUI.TextField(new Rect(0, Screen.height - 100, Screen.width / 3, 100),
    //        "name = " + m_DataBff.pleyerName + "\nHair = " + m_DataBff.imageHairNo +
    //        "\nFace = " + m_DataBff.imageFaceNo + "\nBody = " + m_DataBff.imageBodyNo 
    //        );
    //}
}
