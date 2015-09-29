//#############################################################################
//  ゲームシーン　ステートCardView時の処理を定義
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public partial class GameSceneSystem : MonoBehaviour {

    //GameSystem全体で使う^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int ply = 0; //操作しているのプレイヤー
    private int job = 0; //職業

    //ステートCardView内のみ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private const int CARDVIEW_STATE_START   = 0; //はじめ
    private const int CARDVIEW_STATE_ACTIVE  = 1; //表示中
    private const int CARDVIEW_STATE_END     = 2; //おわり

    private const int CARDVIEW_STATE_MAX_NO  = 3;

    private ClassStateManager m_CardViewState;

    //公開関数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public int GetPly { get { return ply; } }
    public int GetJob { get { return job; } }

    //ゲームシーン初期化=======================================================
    void AwakeInagaki() {
        CardAwake(); //社員証初期化
    }

    void StartInagaki() {
        //CardViewステート-----------------------------------------------------
        UnityAction[] InitArr = new UnityAction[CARDVIEW_STATE_MAX_NO] {
            CardViewStateInitForStart,  //Start
            CardViewStateInitForActive, //Active
            CardViewStateInitForEnd,    //End
            };
        UnityAction[] UpdateArr = new UnityAction[CARDVIEW_STATE_MAX_NO] {
            null,
            CardViewStateUpdateForActive,
            null,
            };

        m_CardViewState =
            new ClassStateManager(CARDVIEW_STATE_MAX_NO, InitArr, UpdateArr);
        //---------------------------------------------------------------------
        ply = -1; //最初のプレイヤーを０番目にするために-1を入れておく
    }

    //ゲームシーン更新=========================================================
    void UpdateInagaki() {
        if(stateNo == (int)StateNo.Check &&
            m_MiniWind.getState != CardWind.STATE_NOTACTIVE) { 

            m_MiniWind.ChangeState(CardWind.STATE_CLOSE_CYBER);
        }
    }

    //ステートの処理///////////////////////////////////////////////////////////
    //更新関数=================================================================
    private void UpdateCardView() {
        //ステート更新
        m_CardViewState.Update();

    }

    //CardView内のステート処理/////////////////////////////////////////////////
    //  CardView内のステート（m_CardViewState）で使用される関数
    //========================================================== Start ========
    //プレイヤー番号を加算し、NextWindを開き、MiniWindを更新する。
    //=========================================================================
    public void CardViewStateInitForStart() {
        //プレイヤー変更
        ply = (ply + 1) % Database.obj.getPlayerCount;

        //カードウィンドウを開く
        OpenNextPleyarWind(ply, job);
        OpenCardMiniWind(ply, job);

        //ステート変更＝＞Active
        m_CardViewState.SetNextState(CARDVIEW_STATE_ACTIVE);

    }

    //========================================================== Active =======
    public void CardViewStateInitForActive() {
    
    }
    public void CardViewStateUpdateForActive() {
        //タッチされたらステート変更＝＞Clause
        if(Input.GetMouseButtonDown(0)) {
            m_CardViewState.SetNextState(CARDVIEW_STATE_END);
        }
    }

    //========================================================== Ebd ==========
    public void CardViewStateInitForEnd() {
        CloseNextPleyarWind(); //NextWindを閉じる
        m_CardViewState.SetNextState(CARDVIEW_STATE_START);//ステート変更＝＞Open
        stateNo = (int)StateNo.PartsSelect; //GameSceneStateをPartsSelectにする
    }

}
