//#############################################################################
//  社員のパーツを表示し、選択させる
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class PlayerPartsSelection : MonoBehaviour {
    
    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private const int         STATE_INPUT      = 0; //入力可能
    private const int         STATE_CHANGE_TAB = 1; //タブ変更

    private const int         STATE_NO_MAX     = 2; //ステートの数

    private ClassStateManager m_State;

    //ステート　タブ変更
    private Transform  m_toListTab; //一番上に持ってくるタブの参照

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputWind ciWind;
    private Transform     traTabHair, traTabFace, traTabBody;

    
    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {

    }
	
	void Start () {
        //ステート-------------------------------------------------------------
        UnityAction[] initFanc   = new UnityAction[] { null, InitForChangeTab   };
        UnityAction[] updateFanc = new UnityAction[] { null, UptateForChangeTab };

        m_State = new ClassStateManager(STATE_NO_MAX, initFanc, updateFanc);

        //参照の取得-----------------------------------------------------------
        ciWind = GameObject.Find(CardInputSystem.GAMEOBJCT_NAME).
                            GetComponent<CardInputSystem>().getCardInputWind;

        //タブの初期化---------------------------------------------------------
        //  タブのOnClickイベントにOnTabButtonEnterを設定し
        //  引数にプレイヤーパーツの種類番号を渡す。
        Button.ButtonClickedEvent eve = null;

        traTabHair = transform.FindChild("PartsTabBottonHair");
        eve = traTabHair.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabHair); });

        traTabFace = transform.FindChild("PartsTabBottonFace");
        eve = traTabFace.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabFace); });

        traTabBody = transform.FindChild("PartsTabBottonBody");
        eve = traTabBody.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabBody); });

        //パーツアイコンの初期化-----------------------------------------------
        Transform traParts = transform.FindChild("PartsTabBottonHair/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_HAIR, i, OnPartsButtonEnter);
        }

        traParts = transform.FindChild("PartsTabBottonFace/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_FACE, i, OnPartsButtonEnter);
        }
        
        traParts = transform.FindChild("PartsTabBottonBody/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_BODY, i, OnPartsButtonEnter);
        }
        //髪型のタブを一番上に持ってくる---------------------------------------

	}
	
	
	void Update () {
	    m_State.Update();
	}

    //ステート関数/////////////////////////////////////////////////////////////

    //========================================================= ChangeTab =====
    //初期化
    //  演出に必要な変数の初期化をする
    public void InitForChangeTab() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }

    //更新 
    //  演出を行った後に
    //  タッチされたタブを一番上に持ってくる
    public void UptateForChangeTab() {
        //演出を入れるなら、ここに書く
        //タッチされたタブを一番上に持ってくる
        m_toListTab.SetAsLastSibling();
        //ステート変更
        m_State.SetNextState(STATE_INPUT);
    }

    //イベント/////////////////////////////////////////////////////////////////
    
    //タブをタッチ=============================================================
    //  タッチされたらタブを変更するステートに移行
    //=========================================================================
    public void OnTabButtonEnter(Transform _tra) {
        //ステートがINPUT以外は、処理しない
        if(m_State.getState != STATE_INPUT) return;

        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + " \n" +
            "name = " + _tra.name );
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        m_State.SetNextState(STATE_CHANGE_TAB);
        m_toListTab = _tra;
    }

    //パーツを選択=============================================================
    //
    //=========================================================================
    public void OnPartsButtonEnter(int _ImageType, int _ImageNo) {
        //ステートがINPUT以外は、処理しない
        if(m_State.getState != STATE_INPUT) return;

        //ウィンドウにデータを渡す
        ciWind.SetPartsData(_ImageType, _ImageNo);

    }

}
