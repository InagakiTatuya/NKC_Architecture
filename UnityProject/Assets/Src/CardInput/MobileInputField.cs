using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class MobileInputField : MonoBehaviour {
    //Inspecterで編集^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    [SerializeField]private Text m_Text;            //入力データ
    [SerializeField]private Text m_Placeholder;     //未入力のとき表示する文字
    [SerializeField]private int  m_CharacterLimit;  //文字数制限（０で無限）

    //非公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private TouchScreenKeyboard m_Keyboard;

    //公開プロパティ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public Text     textComponent {
        get { return m_Text; }
    }

    public string   text { 
        get { return m_Text.text;  }
        set {
            m_Text.text = value;
            if(m_Placeholder != null){
                m_Placeholder.enabled = (value.Length <= 0);
            }
        }//End set
    }
    public int      characterLimit{
        get { return m_CharacterLimit;  } 
        set { m_CharacterLimit = value; }
    }

    //イベント関数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public UnityAction<string> onValueChange;
    public UnityAction<string> endEdit;

    //初期化===================================================================
    void Awake() {
        //ボタンにイベント設定
        transform.GetComponent<Button>().onClick.
            AddListener(this.OnButtonEnter);
    }
    
    void Start () {
        m_Text.enabled        = true;
        m_Placeholder.enabled = (m_Text.text.Length <= 0);
	}
	
    //更新=====================================================================
	void Update () {
        GameObject curruntObj = EventSystem.current.currentSelectedGameObject;
        bool cur = (curruntObj != null && curruntObj.Equals(this.gameObject));

        // Placeholder の表示切替
        if(cur) {
            m_Placeholder.enabled = (m_Text.text.Length <= 0);
        }

        //ここより先、Keyboardが無効のとき処理をしない
        if(m_Keyboard == null || !curruntObj) return;

        //キーボードの入力を適用
        if(m_Keyboard.text.Length > 0) {
            //テキストに適用
            m_Text.text = m_Keyboard.text;
            if(onValueChange != null) onValueChange(m_Text.text);
        }
	    
        //入力完了
        if(!m_Keyboard.active) {
            //文字数制限
            if(m_CharacterLimit > 0 && m_Text.text.Length > m_CharacterLimit) {
                m_Text.text = m_Text.text.Remove(m_CharacterLimit);
            }

            if(endEdit != null) endEdit(m_Text.text);
            m_Keyboard.text = ""; //キーボードのテキストを破棄
        }

    }

    //ボタンイベント///////////////////////////////////////////////////////////
    //入力エリアをタッチ=======================================================
    public void OnButtonEnter() {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " :: " +
                System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        if(TouchScreenKeyboard.isSupported) {
            //キーボードを呼び出す
            m_Keyboard = TouchScreenKeyboard.Open(m_Text.text);
            m_Keyboard.text = m_Text.text;

        }else{
            m_Text.text = "テストマン";
            if(endEdit != null) endEdit(m_Text.text);
        }
    
    }

    //void OnGUI() {
    //    if(m_Keyboard != null)
    //        GUI.TextField(new Rect(0,0,300,280),"area = "+ TouchScreenKeyboard.area +
    //            "\nhideInput = " + TouchScreenKeyboard.hideInput + "\nisSupported = " + TouchScreenKeyboard.isSupported + 
    //            "\nvisible = " + TouchScreenKeyboard.visible +
    //            "\n---\ncurrent = " + (EventSystem.current.currentSelectedGameObject.Equals(this.gameObject)) +
    //            "\n---\nact = " + m_Keyboard.active + "\ndone = " + m_Keyboard.done + 
    //            "\ntext = " + m_Keyboard.text + "\nwasCanceled = " + m_Keyboard.wasCanceled + 
    //            "\nendEdit" + (m_Keyboard.done || !m_Keyboard.active) + "\nonValueChange = " + (m_Keyboard.text.Length > 0) +
    //            "\n---\nText = " + this.text
    //            );
    //}
}
