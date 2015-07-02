﻿//#############################################################################
//  社員証にデータ入力とそのウィンドウを管理
//  Eventを定義する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public partial class CardInputWind : MonoBehaviour {

    //社員追加ボタン===========================================================
    //  タイミング：社員追加ボタンがタップされた瞬間。
    //    ウィンドウをアクティブにし、初期化する
    //=========================================================================
    public void OnPlayerAddButtonEnter() {
        //ステートが通常時以外は、処理しない
        if(ciSystem.getState != CardInputSystem.STATE_USUALLY) return;
        //プレイヤー数が最大値に達していたら処理しない
        if(cardMgr.getPlayerCount >= Database.PLAYER_MAX_COUNT) return;

        OpenCradInputWind(cardMgr.getPlayerCount - 1);

    }
    
    //完了ボタン===============================================================
    //  タイミング：ＯＫボタンがタップされた瞬間。
    //    データが正しければ、データをCardManagerに渡す
    //    そうでない場合は、メッセージウィンドウをだす
    //=========================================================================
    public void OnAppButtonEnter() {
        if(m_StateNo != STATE_INPUTDATA) return;
        
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        //=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#
        //　ここに以下の処理を書く
        //
        //    データが正しければ、データをCardManagerに渡す
        //    そうでない場合は、メッセージウィンドウをだす
        //=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#=#

        //仮処理
        m_NextStateNo = STATE_CLAUSEWIND;
    }
    
    //パーツを選択=============================================================
    //
    //=========================================================================
    public void OnPartsButtonEnter(int _ImageType, int _ImageID) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + " - " +
            "ImageType = " + _ImageType + "  ImageID = " + _ImageID);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }

}