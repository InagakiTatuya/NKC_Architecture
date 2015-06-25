//#############################################################################
//  ゲームシーンのプレイヤー情報を管理する
//  作者：稲垣達也
//#############################################################################


//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//クラス///////////////////////////////////////////////////////////////////////
public class CardWind : MonoBehaviour {
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Image m_Image;    //顔写真
    private Text  m_Name;     //名前
    private Text  m_Job;      //職種名
    private Text  m_Other;    //自由に使えるText CardWindだと説明文

    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //写真^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    public Image image {
        get{ return m_Image;  }
        set{ m_Image = value; }
    }
    //名前^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    public Text nameText {
        get{ return m_Name;   }
        set{ m_Name = value;  }
    }
    //職種名^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    public Text jobText  {
        get{ return m_Job;    }
        set{ m_Job = value;   }
    }
    //その他テキスト^ ^ ^ ^ ^ ^ ^ ^ ^ ^
    public Text otherText {
        get{ return m_Other;  }
        set{ m_Other = value; }
    }


    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        m_Image = transform.FindChild("Fhoto"     ).GetComponent<Image>();
        m_Name  = transform.FindChild("Text_Name" ).GetComponent<Text >();
        m_Job   = transform.FindChild("Text_Job"  ).GetComponent<Text >();
        m_Other = transform.FindChild("Text_Other").GetComponent<Text >();

    }
    void Start() {

    }

    //更新=====================================================================
    void Update() {

    }

    //公開関数/////////////////////////////////////////////////////////////////

    //写真とテキストを変更する=================================================
    //  第一引数：プレイヤー番号
    //  第二引数：職種番号
    //  第三引数：その他のテキスト（"変更しない"を渡すと変更しない）
    //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    public void SetImageAndTexts( int _PlayerNo, int _JpbNo,
                                  string _OtherText = "変更しない"
                                 ) {
        StractPlayerData data = Database.obj.getPlayerData[_PlayerNo];
        //m_Image.sprite = Database.obj.FHOT_SPRITE[data.imageNo]; //現在未実装であるためコメントアウト
        m_Name .text   = data.pleyerName;
        m_Job  .text   = Database.obj.JOB_NAME[_JpbNo];
        //OtherTextは、"変更しない"にすると文字通り変更しないのだ！！
        if(_OtherText != "変更しない") m_Other.text = _OtherText;
    }
    //-------------------------------------------------------------------------

}
