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
    private Image m_ImageHair;    //髪イメージ
    private Image m_ImageFace;    //顔イメージ
    private Text  m_Text;         //名前表示

    //データ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int              INDEXNO;   //CardManagerの管理番号
    private StractPlayerData m_Data;    //Dtabaseに渡すデータ

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public Image            imageHair {
        get{ return m_ImageHair;  }
        set{ m_ImageHair = value; }
    }
    public Image            imageFace {
        get{ return m_ImageFace;  }
        set{ m_ImageFace = value; }
    }
    public Text             Text {
        get{ return m_Text;  }
        set{ m_Text = value; }
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
        m_ImageHair = transform.FindChild("PhotoBack/PhotoHair").GetComponent<Image>();
        m_ImageFace = transform.FindChild("PhotoBack/PhotoFace").GetComponent<Image>();
        m_Text      = transform.FindChild("Name").GetComponent<Text >();
    }

    void Start() {
        //データ---------------------------------------------------------------
        this.DataReset();
    }

    //公開関数/////////////////////////////////////////////////////////////////
    //外部からもらう初期値=====================================================
    //  第一引数：CardManagerで管理されている番号
    //  第二引数：全体をタッチされたとき呼ばれる関数
    //  第三引数：削除ボタンをタッチされたとき呼ばれる関数
    public void Init(int              _IndexNo,
                     UnityAction<int> _onButtonEnter,
                     UnityAction<int> _onRemoveButtonEnter
                    ) {
        //CardManagerの管理番号------------------------------------------------
        this.INDEXNO = _IndexNo;

        //イベント設定---------------------------------------------------------
        //onButton-------------------------------------------------------------
        Button.ButtonClickedEvent eve = GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { _onButtonEnter(INDEXNO); });
        
        //onButton(Remove)-----------------------------------------------------
        eve = transform.FindChild("Remove").GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { _onRemoveButtonEnter(INDEXNO); });

    }

    //データを初期化===========================================================
    public void DataReset() {
        this.m_Data.Init();
        this.DataApp(this.m_Data);
    }

    //必要な値をコピー=========================================================
    public void DataCopyTo(ref Card _Card) {
        _Card.data             = this.data;
        _Card.imageHair.sprite = this.imageHair.sprite;
        _Card.imageFace.sprite = this.imageFace.sprite;
        _Card.Text.text        = this.Text.text;
    }

    //適応=====================================================================
    //  渡されたデータから適応するスプライトを込みこみ適応する
    //  テキストにも名前を入れる
    public void DataApp(StractPlayerData _data) {
        this.m_Text.text        = _data.pleyerName;
        this.m_ImageHair.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_HAIR, _data.imageHairNo];
        this.m_ImageFace.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_FACE, _data.imageFaceNo];
    }

}
