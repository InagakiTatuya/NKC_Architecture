﻿//#############################################################################
//  作者：稲垣達也
//  Stateの更新や初期化を行う
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
class ClassStateManager {

    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_StateNo;          //ステート番号
    private int m_NextStateNo;      //次のフレームでのステート番号

    private int   m_STATE_NO_MAX;   //ステートの最大数
    private float m_StateTime;      //ステート内で使用する

    //各ステートの更新関数のポインタ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private UnityAction[] m_fnIniteArr;   //初期化用
    private UnityAction[] m_fnUpdateArr;  //更新用

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public int   getState       { get{ return m_StateNo;      } }
    public float getStateTime   { get{ return m_StateTime;    } }
    
    //コンストラクタ///////////////////////////////////////////////////////////
    public ClassStateManager(int aStateMax,
        UnityAction[] aInitFuncArr, UnityAction[] aUdataFuncArr) {
        
        m_STATE_NO_MAX = aStateMax;
        //ステートの関数ポインタを初期化---------------------------------------
        m_fnIniteArr  = aInitFuncArr;
        m_fnUpdateArr = aUdataFuncArr;
    }
    //公開関数/////////////////////////////////////////////////////////////////
    //更新=====================================================================
    public void Update() {

        //ステートの初期化-----------------------------------------------------
        if(0 <= m_NextStateNo && m_NextStateNo < m_STATE_NO_MAX) {
            //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
            //#if UNITY_EDITOR
            //Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            //    this.GetType().Name +" == 0x" + this.GetHashCode().ToString("X") + " \n" +
            //    "StateChange: oldNo = " + m_StateNo + ", newNo = " + m_NextStateNo +
            //    ", OldStateTime = " + m_StateTime );
            //#endif
            //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
            
            m_StateNo     = m_NextStateNo;
            m_NextStateNo = -1; //Initを一回だけ呼ぶために-1を入れてる
            if(m_fnIniteArr[m_StateNo] != null) m_fnIniteArr[m_StateNo]();
            m_StateTime = 0.0f; 
        }
        
        //ステート別のアップデート---------------------------------------------
        if(m_fnUpdateArr[m_StateNo] != null) { 
            m_fnUpdateArr[m_StateNo]();
            m_StateTime += Time.deltaTime;
        }   
    }

    //ステート移行=============================================================
    //  ステートを移行する予約をする。
    //  実際のステート移行は、Updateで行われる。
    public void SetNextState(int aState) {
        m_NextStateNo = aState;
    }
    
}
