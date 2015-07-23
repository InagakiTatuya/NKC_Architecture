//#############################################################################
//  ゲームシーンのプレイヤー情報を表示するウィンドウ
//  作者：稲垣達也
//#############################################################################


//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//クラス///////////////////////////////////////////////////////////////////////
public class CardWind : MonoBehaviour {

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Image m_ImageHair; //髪型
    private Image m_ImageFace; //顔
    private Image m_ImageBody; //体
    private Text  m_Name;      //名前
    private Text  m_Job;       //職種名
    private Text  m_Other;     //自由に使えるText CardWindだと説明文

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        m_ImageHair = transform.FindChild("PhotoBack/PhotoHair" ).GetComponent<Image>();
        m_ImageFace = transform.FindChild("PhotoBack/PhotoFace" ).GetComponent<Image>();
        m_ImageBody = transform.FindChild("PhotoBack/PhotoBody" ).GetComponent<Image>();
        m_Name      = transform.FindChild("Text_Name" ).GetComponent<Text >();
        m_Job       = transform.FindChild("Text_Job"  ).GetComponent<Text >();
        m_Other     = transform.FindChild("Text_Other").GetComponent<Text >();
    }
    void Start() {
        //初期状態で非アクティブにする
        gameObject.SetActive(false);
    }

    //更新=====================================================================
    void Update() {

    }
    //非公開関数///////////////////////////////////////////////////////////////

    //データを画像とテキストに適用=============================================
    private void DataApp(ref StractPlayerData _data) {

    }

    //公開関数/////////////////////////////////////////////////////////////////

    //写真とテキストを変更する=================================================
    //  第一引数：プレイヤー番号
    //  第二引数：職種番号
    //  第三引数：その他のテキスト（"変更しない"を渡すと変更しない）
    //=========================================================================
    public void SetImageAndTexts(int _PlayerNo, int _JpbNo, string _OtherText = "変更しない") {
        
        //データ読み込み--------------------------------------------------
        StractPlayerData data = Database.obj.getPlayerData[_PlayerNo];
        //---------------------------------------------------------------- Text
        m_Name.text   = data.pleyerName;
        m_Job .text   = Database.obj.JOB_NAME[_JpbNo];
        
        //OtherTextは、"変更しない"にすると文字通り変更しないのだ！！
        if(_OtherText != "変更しない") m_Other.text = _OtherText;
    
        //---------------------------------------------------------------- Sprite
        m_ImageBody.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_BODY, data.imageBodyNo];
        m_ImageFace.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_FACE, data.imageFaceNo];
        m_ImageHair.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_HAIR, data.imageHairNo];
    }

}
