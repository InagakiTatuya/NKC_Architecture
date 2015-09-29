//#############################################################################
//  ゲームシーンのプレイヤー情報を管理する
//  作者：稲垣達也
//#############################################################################


//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//クラス///////////////////////////////////////////////////////////////////////
public partial class GameSceneSystem : MonoBehaviour {

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardWind m_CardWind; //社員証
    private CardWind m_NextWind; //次のプレイヤー
    private CardWind m_MiniWind; //右下の社員証

    //非公開関数///////////////////////////////////////////////////////////////

    //初期化===================================================================
    private void CardAwake() {
        //参照-----------------------------------------------------------------
        m_CardWind = GameObject.Find("Card_Wind").GetComponent<CardWind>();
        m_NextWind = GameObject.Find("Card_Next").GetComponent<CardWind>();
        m_MiniWind = GameObject.Find("Card_Mini").GetComponent<CardWind>();
        #if UNITY_EDITOR
            if(m_CardWind == null) Debug.LogError("Card_Windが取得できなかったよ！おこだよっ！");
            if(m_NextWind == null) Debug.LogError("Card_Nextが取得できなかったよ！おこだよっ！");
            if(m_MiniWind == null) Debug.LogError("Card_Miniが取得できなかったよ！おこだよっ！");
        #endif
       
    }




    //社員証=================================================================■
    //社員証を表示=============================================================
    //  社員証を表示する。
    //  閉じるときは、CardWindClose()を呼ぶ。
    //    現在、瞬間的に表示されるが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void OpenCardWind(int aPlayerNo, int aJobNo,
                               float aAnimeTime = 0.0f
                             ) {
        //取り出したデータを適用
        m_CardWind.SetImageAndTexts(
            aPlayerNo, aJobNo, Database.obj.JOB_TEXT[aJobNo]);
        //アクティブの設定
        m_CardWind.ChangeState(CardWind.STATE_OPEN);
    }

    //社員証を非表示===========================================================
    //  社員証を閉じる。
    //    現在、非アクティブにしているだけだが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void CloseCardWind(float aAnimeTime = 0.0f) {
        m_CardWind.ChangeState(CardWind.STATE_CLOSE);
    }

    //次のプレイヤーです=====================================================■
    //”丸々さんの番です”を表示===============================================
    //  次のプレイヤーの番を伝えるウィンドウを開く
    //    現在、瞬間的に表示されるが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void OpenNextPleyarWind(int aPlayerNo, int aJobNo,
                                     float aAnimeTime = 0.0f
                                    ) {
        //取り出したデータを適用
        m_NextWind.SetImageAndTexts(aPlayerNo, aJobNo);
        //開く
        m_NextWind.ChangeState(CardWind.STATE_OPEN);
    }

    //”丸々さんの番です”を非表示=============================================
    //  次のプレイヤーの番を伝えるウィンドウを開く
    //    現在、非アクティブにしているだけだが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void CloseNextPleyarWind(float aAnimeTime = 0.0f) {
        m_NextWind.ChangeState(CardWind.STATE_CLOSE);

    }

    //右下社員証=============================================================■
    //社員証を開く=============================================================
    //  右下の社員証を表示する。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void OpenCardMiniWind(int aPlayerNo, int aJobNo,
                                   float aAnimeTime = 0.0f) {
        //取り出したデータを適用
        m_MiniWind.SetImageAndTexts(aPlayerNo, aJobNo);
        m_MiniWind.ChangeState(CardWind.STATE_OPEN_CYBER);
    }

    //社員証を変更==============================================================
    //  右下の社員証を変更する。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void ChangeCardMiniWind(int aPlayerNo, int aJobNo,
                                     float aAnimeTime = 0.0f) {
        //取り出したデータを適用
        m_MiniWind.SetImageAndTexts(aPlayerNo, aJobNo);
    }

    //社員証を閉じる============================================================
    //  右下の社員証を非表示にする。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void CloseCardMiniWind() {
        m_MiniWind.ChangeState(CardWind.STATE_CLOSE_CYBER);
    }
}
