//#############################################################################
//  社員証入力シーンを統括するクラス
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class CardInputSystem : MonoBehaviour {

    //ステート定数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public const int STATE_INSCENE     = 0;   //シーンに入ってきた
    public const int STATE_USUALLY     = 1;   //通常
    public const int STATE_OUTSCENE    = 2;   //シーンを出る

    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int m_State;
    public  int State { 
        get{ return m_State; } 
        set { m_State = value; }
    }

    //各ステートの更新関数のポインタ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private delegate void UpdateFunc();
    private UpdateFunc[] m_fnUpdateArr;

    //初期化===================================================================
    void Awak() {
        //ステートの関数ポインタを初期化---------------------------------------
        m_fnUpdateArr = new UpdateFunc[] {
            StateFuncInScene,
            StateFuncUsually,
            StateFuncOutScene,
        };

    }
    
    void Start() {

    }

    //更新=====================================================================
    void Update() {

    }

    //ステート更新関数=========================================================
    private void StateFuncInScene() {

    }

    private void StateFuncUsually() {

    }

    private void StateFuncOutScene() {

    }

}
