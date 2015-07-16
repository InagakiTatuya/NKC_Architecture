﻿//#############################################################################
//  社員証にデータ入力とそのウィンドウを管理
//  Eventを定義する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

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
        if(ciSystem.getCardMgr.getPlayerCount >= Database.PLAYER_MAX_COUNT) return;

        OpenCradInputWind(ciSystem.getCardMgr.getPlayerCount - 1);

    }
    
    //名前入力中===============================================================
    //  タイミング：新たに入力がされたとき
    //    演出用
    //=========================================================================
    public void OnChangeValueName() {
        //ステートがデータ入力状態以外は、処理しない
        if(m_State.getState != STATE_INPUTDATA) return;
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            "Text = " + m_Input.text);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }

    //名前入力完了=============================================================
    //  タイミング：入力が完了したとき
    //    スペースのみの場合、空にする
    //    一定以上長さの場合、それ以降を消す
    //    入力されたデータを一時保存する
    //=========================================================================
    public void OnEndNameEidt() {
        //ステートがデータ入力状態以外は、処理しない
        if(m_State.getState != STATE_INPUTDATA) return;
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
            "Text = " + m_Input.text);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
     
        //スペースのみの場合、空にする
        bool b = false;
        foreach(char c in m_Input.text) {
            b = (c == ' ' || c == '　');
            if(b) break;
        }
        if(b) {
            m_Input.text = "";
            return;
        }
        //一定以上長さの場合、それ以降を消す
        
        //ここに一定以上長さの場合、それ以降を消す処理

        //一時保存
        m_DataBff.pleyerName = m_Input.text;
    }

    
    //完了ボタン===============================================================
    //  タイミング：ＯＫボタンがタップされた瞬間。
    //    データが正しければ、データをCardManagerに渡す
    //    そうでない場合は、メッセージウィンドウをだす
    //=========================================================================
    public void OnAppButtonEnter() {
        //ステートがデータ入力状態以外は、処理しない
        if(m_State.getState != STATE_INPUTDATA) return;
        
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        if(m_DataBff.pleyerName != "") {
            //CardManagerにデータを渡す
            ciSystem.getCardMgr.SetCardData(ref m_IndexBff, ref m_DataBff);
            m_State.SetNextState(STATE_CLAUSEWIND);
        }else {
            Debug.Log("名前を入力してください");
            //ステート変更　＝＞　メッセージ表示
            m_State.SetNextState(STATE_OPENMESWIND);
        }

    }
}