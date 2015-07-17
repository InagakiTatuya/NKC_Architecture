//#############################################################################
//  社員証を管理するクラス
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//クラス///////////////////////////////////////////////////////////////////////
public class CardManager : MonoBehaviour {
    //定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //CardPackの生成数
    private const int CARDPACK_MAX = (Database.PLAYER_MAX_COUNT - 1) / 3 + 1;
    //Cardの数／Cardの最大要素数
    private const int CARD_MAX     = CARDPACK_MAX * 3;
    
    //リソースの参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private GameObject  CARDPACK_PREFAB;    //社員証パック(CardPack)のプレハブ

    //統括しているシステム^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputSystem ciSystem;  //このシーンを統括するSystem
    
    //管理データ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Card[] m_Card;             //社員証
    private int    m_PlayerCount;      //参加するプレイヤー数
    
    //公開プロパティ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public  int getPlayerCount { get{ return m_PlayerCount; } }

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //プレハブ読み込み-----------------------------------------------------
        CARDPACK_PREFAB =
            Resources.Load<GameObject>("Prefab/CardInput/CardPack");
    }

    void Start() {
        //システムの参照-------------------------------------------------------
        ciSystem = GameObject.Find(CardInputSystem.GAMEOBJCT_NAME)
                                        .GetComponent<CardInputSystem>();

        //CardPackを生成と初期化-----------------------------------------------
        for(int i=0; i < CARDPACK_MAX; i++) {
            //生成・親設定
            Transform tra = Instantiate<GameObject>(CARDPACK_PREFAB).transform;
            tra.SetParent(this.transform, false);
        }

        //Card初期化-----------------------------------------------------------
        m_Card = new Card[CARD_MAX];
        int index = 0;
        foreach(Transform cpTra in transform) {
            //Cardコンポーネット取得- - - - - - - - - - - - - - - - - - - - - -
            m_Card[index + 0] = cpTra.GetChild(0).GetComponent<Card>();
            m_Card[index + 1] = cpTra.GetChild(1).GetComponent<Card>();
            m_Card[index + 2] = cpTra.GetChild(2).GetComponent<Card>();
            //IndexNoを設定 - - - - - - - - - - - - - - - - - - - - - - - - - -
            m_Card[index + 0].Init( index + 0,
                OnFhotoEnter, OnPlayerRemoveButtonEnter);

            m_Card[index + 1].Init( index + 1,
                OnFhotoEnter, OnPlayerRemoveButtonEnter);

            m_Card[index + 2].Init( index + 2,
                OnFhotoEnter, OnPlayerRemoveButtonEnter);

            //要素数加算- - - - - - - - - - - - - - - - - - - - - - - - - - - -
            index += 3;
        }

        //アクティブ設定-------------------------------------------------------
        CardsSetActive(1, true);//一つ目だけtrue

        //プレイヤー数---------------------------------------------------------
        m_PlayerCount = 1;
    }

    //更新=====================================================================
    void Update() {

    }

    //社員証のアクティブ設定===================================================
    //  Card と CardPack のアクティブの設定を行う
    //  非アクティブにする際、データをリセットする
    //=========================================================================
    private void CardsSetActive(int _Length, bool _Active = true) {
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        //  配列の先頭からすべてアクティブにする。
        //  重いようであれば変更する。
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        //配列先頭から_Lengthまでは、_Activeを渡す
        int index = 0;
        for(/**/; index < _Length; index++) {
            if(index % 3 == 0) {
                transform.GetChild(index / 3).gameObject.SetActive(_Active);
            }
            m_Card[index].gameObject.SetActive(_Active);
            if(_Active == false) m_Card[index].DataReset();
        }
        //_Leingth 以下のものは !_Active を渡す
        for(/**/; index < CARD_MAX; index++) {
            if(index % 3 == 0) {
                transform.GetChild(index / 3).gameObject.SetActive(!_Active);
            }
            m_Card[index].gameObject.SetActive(!_Active);
            if(!_Active == false) m_Card[index].DataReset();
        }
        
    }

    //公開関数/////////////////////////////////////////////////////////////////

    //カードにデータを入れる===================================================
    //  CardInputWindでOKボタンが押されたときに呼ばれる。
    //  受け取ったデータを格納する
    //=========================================================================
    public void SetCardData(ref int _index, ref StractPlayerData _data) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            "Index=" + _index + " data=" + _data.ToString());
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        //データを適用---------------------------------------------------------
        m_Card[_index].data = _data;
        m_Card[_index].DataApp();
    }

    //データベースに渡す=======================================================
    //  カードに格納されているデータを Database のフォーマットにあわせデータを
    //  作り、Databaseに入れる。
    //  データが不適切な場合、失敗する。
    //  戻り値：データが不適切な場合 Ture を返す。
    //=========================================================================
    public bool SendDataToDatabese() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR 
        Debug.Log(" Time:"+Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            " PlyCount="+(m_PlayerCount < Database.PLAYER_MAX_COUNT) );
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //プレイヤーがいない場合、メッセージ処理をする
        if(m_PlayerCount >= Database.PLAYER_MAX_COUNT) {
            Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
                this.GetType().Name + " - " +
                System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
                "データに不備がありました");
            return true;
        }

        //社員証全てに名前があるかチェックする
        bool error = false;
        for(int i=0; i < m_PlayerCount; i++) {
            error = (m_Card[i].data.pleyerName == "");
            if(error) {
                Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
                            this.GetType().Name + " - " +
                            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
                            "データに不備がありました");
                return true;
            }
        }


        //Databaseに渡すデータを作る-------------------------------------------
        StractPlayerData[] data = new StractPlayerData[m_PlayerCount];
        for(int i = 0; i < m_PlayerCount; i++) {
            data[i] = m_Card[i].data;
        }
        //Databaseに渡す-------------------------------------------------------
        Database.obj.SetPlyaerDatas(ref data);
        
        return false;
    }
    
    //イベント/////////////////////////////////////////////////////////////////

    //社員追加ボタン===========================================================
    //  タイミング：社員追加ボタンがタップされた瞬間。
    //    プレイヤーデータを追加し、参加者数を増やす。
    //=========================================================================
    public void OnPlayerAddButtonEnter() {
        //ステートが通常時以外は、処理しない
        if(ciSystem.getState != CardInputSystem.STATE_USUALLY) return;
        //プレイヤー数が最大値に達していたら処理しない
        if(m_PlayerCount >= Database.PLAYER_MAX_COUNT) return;

        m_PlayerCount++;                //プレイヤー数加算
        CardsSetActive(m_PlayerCount);  //アクティブの操作
    }

    //写真を変更===============================================================
    //  タイミング：写真がタップされた瞬間。
    //    社員証入力ウィンドウを呼び出し、タッチされたカードのデータを渡す
    //=========================================================================
    public void OnFhotoEnter(int _cardIndex) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name +
            "\nCardIndex = " + _cardIndex);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //写真選択ウィンドウを出す---------------------------------------------
        ciSystem.getCardInputWind.
            OpenCradInputWind(_cardIndex, m_Card[_cardIndex].data);
    }

    //社員証の削除ボタン=======================================================
    //  タイミング：社員証削除ボタンがタップされた瞬間。
    //    プレイヤーデータを削除し、参加者数を減らす。
    //=========================================================================
    public void OnPlayerRemoveButtonEnter(int _cardIndex) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name +
            "\nCardIndex = " + _cardIndex);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        
        //ステートが通常時以外は、処理しない
        if(ciSystem.getState != CardInputSystem.STATE_USUALLY) return;
        
        //プレイヤー数が１以下のときは、別処理
        if(m_PlayerCount <= 1) {
            m_Card[0].DataReset();
            return;
        }
        
        //データをコピー
        for(int i = _cardIndex + 1; i < m_PlayerCount; i++) {
            m_Card[i].DataCopyTo(ref m_Card[i - 1]);
        }

        m_PlayerCount--;
        CardsSetActive(m_PlayerCount);  //アクティブの操作

    }


}
