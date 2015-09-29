//#############################################################################
//  ゲームシーンのプレイヤー情報を表示するウィンドウ
//  作者：稲垣達也
//#############################################################################


//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;


//クラス///////////////////////////////////////////////////////////////////////
public class CardWind : MonoBehaviour {

    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Image m_Image;     //自身にアタッチしているImage
    private Image m_ImageHair; //髪型
    private Image m_ImageFace; //顔
    private Image m_ImageBody; //体
    private Text  m_Name;      //名前
    private Text  m_Job;       //職種名
    private Text  m_Other;     //自由に使えるText CardWindだと説明文
   
    //ステート^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private ClassStateManager m_State;

    public const int STATE_NOTACTIVE        = 0;   //非アクティブ
    public const int STATE_ACTIVE           = 1;   //アクティブ
    public const int STATE_OPEN_CYBER       = 2;   //サイバーチックに開く
    public const int STATE_CLOSE_CYBER      = 3;   //サイバーチックに閉じる
    public const int STATE_OPEN             = 4;
    public const int STATE_CLOSE            = 5;
    public const int STATE_MAX = 6;
    
    //アニメーション用^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Vector3 m_FIRST_POS;
    private Vector3 m_FIRST_SIZEDELTA;
    private Vector3 m_LScale;
    
    //公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public int  getState{ get{ return m_State.getState; } }

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //参照-----------------------------------------------------------------
        m_Image     = transform.GetComponent<Image>();
        m_ImageHair = transform.FindChild("PhotoBack/PhotoHair").GetComponent<Image>();
        m_ImageFace = transform.FindChild("PhotoBack/PhotoFace").GetComponent<Image>();
        m_ImageBody = transform.FindChild("PhotoBack/PhotoBody").GetComponent<Image>();
        m_Name      = transform.FindChild("Text_Name" ).GetComponent<Text>();
        m_Job       = transform.FindChild("Text_Job"  ).GetComponent<Text>();
        m_Other     = transform.FindChild("Text_Other").GetComponent<Text>();

        //アニメーション用-----------------------------------------------------
        m_FIRST_POS       = m_Image.rectTransform.position;
        m_FIRST_SIZEDELTA = m_Image.rectTransform.sizeDelta;
        m_LScale          = m_Image.rectTransform.localScale;

    }
    void Start() {
        //ステート初期化-------------------------------------------------------
        UnityAction[] InitFunc = new UnityAction[STATE_MAX] { 
            InitForNotActive,    //非アクティブ
            null,                //アクティブ
            InitForOpenInCyber,  //サイバーチックに開く
            InitForCloseInCyber, //サイバーチックに閉じる
            InitForOpen,         //開く
            InitForClose,        //閉じる
        };
        UnityAction[] UpdateFunc = new UnityAction[STATE_MAX] { 
            null,                    //非アクティブ
            null,                    //アクティブ
            UpdateForOpenInCyber,    //サイバーチックに開く
            UpdateForCloseInCyber,   //サイバーチックに閉じる
            null,                    //開く
            null,                    //閉じる
        };
        
        m_State = new ClassStateManager(STATE_MAX, InitFunc, UpdateFunc);
    }

    //更新=====================================================================
    void Update() {
        m_State.Update();
    }

    //公開関数/////////////////////////////////////////////////////////////////

    //ステート変更=============================================================
    public void ChangeState(int aStateNo) {
        if(m_State.getState == aStateNo) return;
        m_State.SetNextState(aStateNo);
    }

    //写真とテキストを変更する=================================================
    //  第一引数：プレイヤー番号
    //  第二引数：職種番号
    //  第三引数：その他のテキスト（"変更しない"を渡すと変更しない）
    //=========================================================================
    public void SetImageAndTexts(int aPlayerNo, int aJpbNo, string aOtherText = "変更しない") {
        
        //データ読み込み--------------------------------------------------
        StractPlayerData data = Database.obj.getPlayerData[aPlayerNo];
        //---------------------------------------------------------------- Text
        m_Name.text   = data.pleyerName;
        m_Job .text   = Database.obj.JOB_NAME[aJpbNo];
        
        //OtherTextは、"変更しない"にすると文字通り変更しないのだ！！
        if(aOtherText != "変更しない") m_Other.text = aOtherText;
    
        //---------------------------------------------------------------- Sprite
        m_ImageBody.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_BODY, data.imageBodyNo];
        m_ImageFace.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_FACE, data.imageFaceNo];
        m_ImageHair.sprite = Database.obj.
            PLAYER_SPRITE[Database.PLAYER_PARTS_HAIR, data.imageHairNo];
    }


    //ステート用関数///////////////////////////////////////////////////////////
    //非アクティブ=============================================================
    public void InitForNotActive() {
        m_Image.enabled  = false;
        foreach(Transform child in m_Image.transform) {
            child.gameObject.SetActive(false);
        }
    }

    //アクティブ===============================================================
    //サイバーチックに開く=====================================================
    public void InitForOpenInCyber() {
        //表示する
        m_Image.enabled  = true;
        foreach(Transform child in m_Image.transform) {
            child.gameObject.SetActive(true);
        }

        //アニメーション変数
        m_LScale = Vector3.one;
        m_LScale.y = 1f / m_FIRST_SIZEDELTA.y;
        m_Image.rectTransform.localScale = m_LScale;
    }

    public void UpdateForOpenInCyber() {
        //アニメーション
        m_LScale.y = m_LScale.y * 1.2f;
        if(m_LScale.y >= 1.0f) {
            m_LScale.y = 1f;
        }
        m_Image.rectTransform.localScale = m_LScale;
        
        //ステート変更
        if(m_LScale.y >= 1.0f) {
            m_State.SetNextState(STATE_ACTIVE);
        }
    }
    //サイバーチックに閉じる===================================================
    public void InitForCloseInCyber() {
        m_LScale = m_Image.rectTransform.localScale;
    }

    public void UpdateForCloseInCyber() {
        //アニメーション
        m_LScale.y = m_LScale.y * 0.8f;
        if(m_FIRST_SIZEDELTA.y * m_LScale.y < 1.0f) {
            m_LScale.y = 1f / m_FIRST_SIZEDELTA.y;
            m_State.SetNextState(STATE_NOTACTIVE); //ステート変更
        }
        m_Image.rectTransform.localScale = m_LScale;
        
    }
    //瞬間的に開く=============================================================
    public void InitForOpen() {
        //表示する
        m_Image.enabled  = true;
        foreach(Transform child in m_Image.transform) {
            child.gameObject.SetActive(true);
        }
        
        //大きさ
        m_Image.rectTransform.localScale = Vector3.one;

    }
    
    //瞬間的に閉じる===========================================================
    public void InitForClose() {
        //非表示にする
        m_Image.enabled = false;
        foreach(Transform child in m_Image.transform) {
            child.gameObject.SetActive(false);
        }
        
    }

}
