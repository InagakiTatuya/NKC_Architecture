//#############################################################################
//  社員証入力シーンにてメッセージを表示するウィンドウの開閉を行う
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class MessageWind : MonoBehaviour {

    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private const int STATE_NOTACTIVE    = 0; //非表示状態
    private const int STATE_OPEN         = 1; //開く
    private const int STATE_ACTIVE       = 2; //表示状態
    private const int STATE_CLAUSE       = 3; //閉じる

    private const int STATE_NO_MAX       = 4;

    private ClassStateManager m_State;

    private const float OPEN_TIME   = 1.5f; //開く時間
    private const float ACTEVE_TIME = 1.0f; //開いている時間
    private const float CLAUSE_TIME = 0.1f; //閉じる時間


    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private RectTransform m_Image;

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //参照の取得-----------------------------------------------------------
        m_Image = transform.FindChild("Image") as RectTransform;

        //ステート初期化-------------------------------------------------------
        UnityAction[] InitFancs = new UnityAction[STATE_NO_MAX] {
            InitForNotActive, InitForOpen  , InitForActive  , InitForClause  ,
        };
        UnityAction[] UpdateFancs = new UnityAction[STATE_NO_MAX] {
            null            , UpdateForOpen, UpdateForActive, UpdateForClause,
        };
        m_State = new ClassStateManager(STATE_NO_MAX, InitFancs, UpdateFancs);

        //初期ステート
        m_State.SetNextState(STATE_NOTACTIVE);
    }

    void Start() {


    }

    //更新=====================================================================
    void Update() {
        m_State.Update();
    }

    //ステート用関数///////////////////////////////////////////////////////////
    //===================================================== NotActive =========
    public void InitForNotActive() {
        m_Image.gameObject.SetActive(false);
    }
    //===================================================== Open ==============
    public void InitForOpen() {
        m_Image.gameObject.SetActive(true);
    }

    public void UpdateForOpen() {
        
        //一定時間たったらステート移行　＝＞　表示状態
        if(m_State.getStateTime >= OPEN_TIME) {
            m_State.SetNextState(STATE_ACTIVE);
        }
    }
    //===================================================== Active ============
    public void InitForActive() {
        m_Image.gameObject.SetActive(true);
    }

    public void UpdateForActive() {
        

    }
    //===================================================== Clause ============
    public void InitForClause() {
    
    }

    public void UpdateForClause() {

        //一定時間たったらステート移行　＝＞　非表示
        if(m_State.getStateTime >= CLAUSE_TIME) {
            m_State.SetNextState(STATE_NOTACTIVE);
        }
    }

    //公開関数/////////////////////////////////////////////////////////////////
    //ウィンドウを開く=========================================================
    //  ウィンドウを開く指示を受けます
    public void OpenWind() {
        m_State.SetNextState(STATE_OPEN);
    }


    //イベント/////////////////////////////////////////////////////////////////
    //OnButtonEnter============================================================
    public void OnTouchWindEnter() {
        if(m_State.getState != STATE_ACTIVE) return;
        m_State.SetNextState(STATE_CLAUSE);
    }



}
