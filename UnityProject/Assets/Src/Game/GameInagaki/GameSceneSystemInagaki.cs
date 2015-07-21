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
    private int playerNo = 0; //操作しているのプレイヤー
    private int job      = 0; //職業

    //ステートCardView内のみ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private const int CARDVIEW_STATE_OPEN      = 0; //開く
    private const int CARDVIEW_STATE_ACTIVE    = 1; //表示中
    private const int CARDVIEW_STATE_CLAUSE    = 2; //閉じる
    
    private const int CARDVIEW_STATE_MAX_NO = 3;
    
    private ClassStateManager m_CardViewState;

    //ゲームシーン初期化=======================================================
	void AwakeInagaki() {
        CardAwake(); //社員証初期化

        //CardViewステート-----------------------------------------------------
        UnityAction[] InitArr = new UnityAction[CARDVIEW_STATE_MAX_NO] {
            CardViewStateInitForOpen,   //Open
            null,                       //Active
            CardViewStateInitForClause, //Clause
            };
        UnityAction[] UpdateArr = new UnityAction[CARDVIEW_STATE_MAX_NO] {
            null,
            CardViewStateUpdateForActive,
            null,
            };

        m_CardViewState = 
            new ClassStateManager(CARDVIEW_STATE_MAX_NO, InitArr, UpdateArr);
    }

	void StartInagaki () {
	
	}
	
    //ゲームシーン更新=========================================================
	void UpdateInagaki () {
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        if(Input.GetKeyDown(KeyCode.Q)) { OpenCardWind(playerNo, job);       }
	    if(Input.GetKeyDown(KeyCode.W)) { CloseCardWind();                   }
	    if(Input.GetKeyDown(KeyCode.E)) { OpenNextPleyarWind(playerNo, job); }
	    if(Input.GetKeyDown(KeyCode.R)) { CloseNextPleyarWind();             }
	    if(Input.GetKeyDown(KeyCode.T)) { OpenCardMiniWind(playerNo, job);   }
	    if(Input.GetKeyDown(KeyCode.Y)) { ChangeCardMiniWind(playerNo, job); }
	    if(Input.GetKeyDown(KeyCode.U)) { CloseCardMiniWind();               }
	
	    if(Input.GetKeyDown(KeyCode.UpArrow)) { 
            playerNo = (playerNo + 1) % Database.obj.getPlayerCount;
            job = (job + 1) % Database.JOB_NO_MAX;
            print("ply = " + playerNo + "   job = " + job);
        }

        if(Input.GetKeyDown(KeyCode.A)) { this.stateNo = (int)StateNo.CardView; }
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

    
    }

    //ステートの処理///////////////////////////////////////////////////////////

	//更新関数=================================================================
	private	void	UpdateCardView(){
        //ステート更新
        m_CardViewState.Update();
	
	}

    //CardView内のステート処理/////////////////////////////////////////////////
    //  CardView内のステート（m_CardViewState）で使用される関数
    //  関数名が長すぎて申し訳ない。自分しか使わないけど
    //========================================================== Open =========
    public void CardViewStateInitForOpen() {
        //プレイヤー変更
        playerNo = (playerNo + 1) % Database.obj.getPlayerCount;
        
        //NextWindを開く
        this.OpenNextPleyarWind(playerNo, job);
        
        //MiniWindを変更する
        if(!m_MiniWind.isActiveAndEnabled) {
            OpenCardMiniWind(playerNo, job);
        }else {
            ChangeCardMiniWind(playerNo, job);
        }

        //ステート変更＝＞Active
        m_CardViewState.SetNextState(CARDVIEW_STATE_ACTIVE);
    }

    //========================================================== Active =======
    public void CardViewStateUpdateForActive() {

        //タッチされたらステート変更＝＞Clause
        //　＊GetMouseButtonDownを使っているが
        //    たしか、タップにも反応したはず。
        if(Input.GetMouseButtonDown(0)) {
            m_CardViewState.SetNextState(CARDVIEW_STATE_CLAUSE);
        }
    }

    //========================================================== Clause =======
    public void CardViewStateInitForClause() {
        CloseNextPleyarWind(); //NextWindを閉じる
        m_CardViewState.SetNextState(CARDVIEW_STATE_OPEN);//ステート変更＝＞Open
        stateNo = (int)StateNo.PartsSelect; //GameSceneStateをPartsSelectにする
    }

}
