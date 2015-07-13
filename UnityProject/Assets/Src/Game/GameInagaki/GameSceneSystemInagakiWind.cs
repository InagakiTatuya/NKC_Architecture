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
    }


    //社員証=================================================================■
    //社員証を表示=============================================================
    //  社員証を表示する。
    //  閉じるときは、CardWindClose()を呼ぶ。
    //    現在、瞬間的に表示されるが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void OpenCardWind( int _PlayerNo, int _JobNo,
                               float _animeTime = 0.0f
                             ) {
        //取り出したデータを適用
        m_CardWind.SetImageAndTexts(_PlayerNo, _JobNo, 
            Database.obj.JOB_TEXT[_JobNo]);
        //アクティブの設定
        m_CardWind.gameObject.SetActive(true);
    }

    //社員証を非表示===========================================================
    //  社員証を閉じる。
    //    現在、非アクティブにしているだけだが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void CloseCardWind(float _animeTime = 0.0f) {
        //アクティブの設定
        m_CardWind.gameObject.SetActive(false);
    }

    //次のプレイヤーです=====================================================■
    //”丸々さんの番です”を表示===============================================
    //  次のプレイヤーの番を伝えるウィンドウを開く
    //    現在、瞬間的に表示されるが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void OpenNextPleyarWind( int _PlayerNo, int _JobNo,
                                     float _animeTime = 0.0f
                                    ) {
        //取り出したデータを適用
        m_NextWind.SetImageAndTexts(_PlayerNo, _JobNo);
        //アクティブの設定
        m_NextWind.gameObject.SetActive(true);
    }

    //”丸々さんの番です”を非表示=============================================
    //  次のプレイヤーの番を伝えるウィンドウを開く
    //    現在、非アクティブにしているだけだが、
    //    演出の強化のため変更の可能性あり。
    //-------------------------------------------------------------------------
    private void CloseNextPleyarWind(float _animeTime = 0.0f) {
        //アクティブの設定
        m_NextWind.gameObject.SetActive(false);

    }

    //右下社員証=============================================================■
    //社員証を開く=============================================================
    //  右下の社員証を表示する。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void OpenCardMiniWind( int _PlayerNo, int _JobNo,
                                   float _animeTime = 0.0f) {
        //取り出したデータを適用
        m_MiniWind.SetImageAndTexts(_PlayerNo, _JobNo);
        //アクティブの設定
        m_MiniWind.gameObject.SetActive(true);
    }

    //社員証を変更==============================================================
    //  右下の社員証を変更する。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void ChangeCardMiniWind( int _PlayerNo, int _JobNo,
                                     float _animeTime = 0.0f) {
        //取り出したデータを適用
        m_MiniWind.SetImageAndTexts(_PlayerNo, _PlayerNo);
    }

    //社員証を閉じる============================================================
    //  右下の社員証を非表示にする。
    //    演出強化のため大幅に変更する可能性あり。
    //-------------------------------------------------------------------------
    private void CloseCardMiniWind() {
        //アクティブの設定
        m_MiniWind.gameObject.SetActive(false);
    }
}
