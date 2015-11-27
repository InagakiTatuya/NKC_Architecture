//#############################################################################
//  文字列入力フィールドクラス
//    モバイル・ＰＣに対応した文字列入力フィールドのオブジェクトを管理する
//    今後拡張し、他のデバイスにも対応する予定なのでMultiという名前を使っている
//    
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class MultiInputField : MonoBehaviour {

    private InputField       m_InputOther;  //その他用
    private MobileInputField m_InputMobile; //モバイル用

    //公開プロパティ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //テキスト
    public string text {
        get { 
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return m_InputMobile.text;
            #else
            return m_InputOther.text;
            #endif
        }
    
        set {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            m_InputMobile.text = value;
            #else
            m_InputOther.text = value;
            #endif
        }
    }

    //未入力時の文字


    //文字数制限
    public int characterLimit {
        get { 
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return m_InputMobile.characterLimit;
            #else
            return m_InputOther.characterLimit;
            #endif
        }
        set {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            m_InputMobile.characterLimit = value;
            #else
            m_InputOther.characterLimit = value;
            #endif
        }
    }

    //イベント関数
    public UnityAction<string> onValueChange {
        get {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return m_InputMobile.onValueChange;
            #else
            return null;
            #endif
        }
        set {
            
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            m_InputMobile.onValueChange = value;
            #else
            m_InputOther.onValueChange.AddListener(value);
            #endif
        }
    }
    public UnityAction<string> onEndEdit {
        get {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return m_InputMobile.onEndEdit;
            #else
            return null;
            #endif
        }
        set {
            
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            m_InputMobile.onEndEdit = value;
            #else
            m_InputOther.onEndEdit.AddListener(value);
            #endif
        }
    }   


    //初期化===================================================================
    void Awake() {
        m_InputOther  = transform.FindChild("InputField_Other")
                                            .GetComponent<InputField>();
        m_InputMobile = transform.FindChild("InputField_Mobile")
                                            .GetComponent<MobileInputField>();
    }
    
    void Start() {
        //必要の無いプラットフォーム用のフィールドを無効化
        #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if(m_InputOther  != null) m_InputOther.gameObject.SetActive(false);
        #else
        if(m_InputMobile != null) m_InputMobile.gameObject.SetActive(false);
        #endif
    }

    //更新=====================================================================
    void Update() {

    }

}
