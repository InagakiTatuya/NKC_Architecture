//#############################################################################
//  社員証に必要な参照を持つ
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events; //関数ポインタに使ってる
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class Card : MonoBehaviour {
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Image           m_Image;    //顔写真
    private InputField      m_Input;    //名前入力

    //データ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int              INDEXNO;   //CardManagerの管理番号
    private StractPlayerData m_Data;    //Dtabaseに渡すデータ

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public Image            image {
        get{ return m_Image;  }
        set{ m_Image = value; }
    }
    public InputField       input {
        get{ return m_Input;  }
        set{ m_Input = value; }
    }
    //データ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public int              indexNo {
        get{ return INDEXNO;  }
        set{ INDEXNO = value; }
    }
    public StractPlayerData data {
        get{ return m_Data;  }
        set{ m_Data = value; }
    }

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //参照-----------------------------------------------------------------
        m_Image = transform.FindChild("Photo"     ).GetComponent<Image     >();
        m_Input = transform.FindChild("InputField").GetComponent<InputField>();

        //データ---------------------------------------------------------------
        m_Data.Init();
    }

    void Start() {

    }

    //公開関数/////////////////////////////////////////////////////////////////
    //外部からもらう初期値=====================================================
    //  第一引数：CardManagerで管理されている番号
    //  第二引数：入力終了時に呼ばれる関数
    //  第三引数：入力するたびに呼ばれる関数
    public void Init(int                _IndexNo,
                     UnityAction<int> _onButtonEnter,
                     UnityAction<int> _onEndEdit,
                     UnityAction<int> _callValidateInput = null
                    ) {
        //CardManagerの管理番号------------------------------------------------
        this.INDEXNO = _IndexNo;

        //イベント設定---------------------------------------------------------
        //onButton
        Button.ButtonClickedEvent eve = GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { _onButtonEnter(INDEXNO); });
        //onEndEdit------------------------------------------------------------
        //  リスナーをすべて削除してから、引数の関数を登録
        m_Input.onEndEdit.RemoveAllListeners();
        m_Input.onEndEdit.AddListener(delegate { _onEndEdit(INDEXNO); });
       //onValueChange--------------------------------------------------------
        //  リスナーをすべて削除してから、引数の関数を登録
        m_Input.onValueChange.RemoveAllListeners();
        if(_callValidateInput != null) {
            m_Input.onValueChange.
                AddListener(delegate { _callValidateInput(INDEXNO); });
        }
    }

    //入力された名前をDataに格納===============================================
    public void DataSetName() {
        m_Data.name = m_Input.text;
    }
    //選択された写真の番号をDataに格納=========================================
    public void DataSetPhotoNo(int _PhotoNo) {

    }


}
