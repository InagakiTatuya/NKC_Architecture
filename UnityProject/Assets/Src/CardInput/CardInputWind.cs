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
    public const int STATE_OPENMESSWIND   = 4;  //メッセージウィンドウを出す
    public const int STATE_CLAUSEMESSWIND = 5;  //メッセージウィンドウを閉じる
    public const int STATE_CLAUSEWIND     = 6;  //ウィンドウを閉じる

    public const int STATE_NO_MAX        = 7;   //ステートの種類数
    
    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private ClassStateManager m_State;

    private float OPENWIND_TIME   = 0.1f; //開く際の演出時間
    private float CLAUSEWIND_TIME = 0.1f; //閉じる際の演出時間
    
    //編集するカード^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int               m_IndexBff; //管理番号
    private StractPlayerData  m_DataBff;  //データ
    
    //制御する子の参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private RectTransform     m_Back;      //黒背景
    private RectTransform     m_Wind;      //ウィンドウ
    private InputField        m_Input;     //名前入力
    private Image             m_ImageHair; //髪型
    private Image             m_ImageFace; //顔
    private Image             m_ImageBody; //体

    //統括しているシステム^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputSystem ciSystem;  //このシーンを統括するSystem
    
    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //制御する子の参照-----------------------------------------------------
        m_Back      = transform.FindChild("InputWindBack") as RectTransform;
        m_Wind      = transform.FindChild("Wind"         ) as RectTransform;
        
        m_Input     = m_Wind.FindChild("Card/InputField")
                                                .GetComponent<InputField>();

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
            null,
            null,
            InitForClauseWind,
        };
        UnityAction[] fnUpdateArr = new UnityAction[STATE_NO_MAX] {
            null,
            UpdateForOpenWind,
            UpdateForInputData,
            null,
            null,
            null,
            UpdateForClauseWind,
        };

        m_State = new ClassStateManager(STATE_NO_MAX, fnInitArr, fnUpdateArr);

        //システムの参照-------------------------------------------------------
        ciSystem = GameObject.Find(CardInputSystem.GAMEOBJCT_NAME)
                                        .GetComponent<CardInputSystem>();
        //ステート-------------------------------------------------------------
        m_State.SetNextState(STATE_NOTACTIVE);
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
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //アクティブ
        m_Back.gameObject.SetActive(false);
        m_Wind.gameObject.SetActive(false);
    }
    
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
        m_Wind.gameObject.SetActive(true);

        //データを適用
        DataApp();
    }
    //  更新  OpenWind
    private void UpdateForOpenWind() {

        //一定時間になったら次のステートへ移行
        if(m_State.getStateTime >= OPENWIND_TIME) {
            m_State.SetNextState(STATE_INPUTDATA);
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

        //一定時間になったら
        //自分を非表示にして、
        //処理が終えたことをシステムに伝える
        if(m_State.getStateTime >= CLAUSEWIND_TIME) {
            //ステート変更＝＞非アクティブ
            m_State.SetNextState(STATE_NOTACTIVE);
            //システムにウィンドウが閉じたことを伝える
            ciSystem.EndCardInputEvent(); 
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
    public void OpenCradInputWind(int _CardIndex, StractPlayerData _data) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
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
            this.GetType().Name + " - " +
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

}
