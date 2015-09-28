using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class MobileInputField : MonoBehaviour {
    //Inspecterで編集^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    [SerializeField]private Text m_Text;
    [SerializeField]private Text m_Placeholder;

    //非公開変数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private TouchScreenKeyboard m_Keyboard;

    //公開プロパティ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public Text     textComponent {
        get { return m_Text; }
    }

    public string   text { 
        get { return m_Text.text;  }
        set { m_Text.text = value; }
    }

    //イベント関数^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public UnityAction<string> onValueChange;
    public UnityAction<string> endEdit;

    void Awake() {
        //ボタンにイベント設定
        transform.GetComponent<Button>().onClick.
            AddListener(this.OnButtonEnter);
    }
    
    void Start () {
	
	}
	
    //更新=====================================================================
	void Update () {
        
        // Placeholder の表示切替
        if(m_Text.text.Length <= 0) {
            m_Placeholder.enabled = true;
        }else{
            m_Placeholder.enabled = false;
        }

        //ここより先、Keyboardが無効のとき処理をしない
        if(m_Keyboard == null) return;

        //入力完了
        if(m_Keyboard.done) {
            if(endEdit != null) endEdit(m_Keyboard.text);
        }

        if(m_Keyboard.text.Length > 0 ) {
            m_Text.text = m_Keyboard.text;
            if(onValueChange != null) onValueChange(m_Text.text);
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
        }else{
            m_Text.text = "建築太郎";
        }
    
    }

    void OnGUI() {
        if(m_Keyboard != null)
            GUI.TextField(new Rect(0,0,300,400),"area = "+ TouchScreenKeyboard.area +
                "\nhideInput = " + TouchScreenKeyboard.hideInput + "\nisSupported = " + TouchScreenKeyboard.isSupported + 
                "\nvisible = " + TouchScreenKeyboard.visible +
                "\n---\nact = " + m_Keyboard.active + "\ndone = " + m_Keyboard.done + 
                "\ntext = " + m_Keyboard.text + "\nwasCanceled = " + m_Keyboard.wasCanceled);
    }
}
