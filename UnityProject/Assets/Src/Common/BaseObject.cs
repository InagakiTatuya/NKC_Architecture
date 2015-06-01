using UnityEngine;
using System.Collections;


//###############################################
//　作者：稲垣達也
//###############################################
/// <summary>
/// MonoBehaviourの代わりに継承する
/// </summary>
public abstract class BaseObject : MonoBehaviour {
	
	
    [SerializeField] private bool m_isUpdate = true;
	
    public bool isUpdate{ set{ m_isUpdate = value; } get{ return m_isUpdate; } }
	
    //トランスフォーム^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

    //座標-------------------------------------------------
    public Vector3 traPos { 
        get{ return this.transform.position; }
        set{ this.transform.position = value; }
    }
    public Vector3 traLPos { 
        get{ return this.transform.localPosition; }
        set{ this.transform.localPosition = value; }
    }
    
    //回転値-----------------------------------------------
    public Quaternion traRot {
        get{ return this.transform.rotation; }
        set{ this.transform.rotation = value; }
    }
    public Quaternion traLRot {
        get{ return this.transform.localRotation; }
        set{ this.transform.localRotation = value; }
    }
    
    //拡縮-------------------------------------------------
    public Vector3 traSca {
        get{ return this.transform.lossyScale; }
    }
    public Vector3 traLSca {
        get{ return this.transform.localScale; }
        set{ this.transform.localScale = value; }
    }
	
    //abstract関数=============================================================
   

    /// <summary>
    /// Awakeの代わり　
    /// gameObjectが有効になった時、
    /// 一度だけ呼ばれる。コンストラクタの代わりに使う事を推奨
    /// </summary>
    protected abstract void AwakeEx();
	
    /// <summary>
    /// Startの代わり　
    /// このスクリプトが有効になった時、一度だけ呼ばれる
    /// </summary>
    protected abstract void StartEx();

    /// <summary>
    /// Updateの代わり　
    /// isUpdateが有効の場合に、毎フレーム呼び出される
    /// </summary>
    protected abstract void UpdateEx();

    /// <summary>
    /// FixedUpdateの代わり　
    /// isUpdateが有効の場合、FixedUpdateのタイミングで呼び出される
    /// </summary>
    protected abstract void FixedUpdateEx();
    
    /// <summary>
    /// LateUpdateの代わり　
    /// isUpdateが有効の場合、LateUpdateのタイミングで呼び出される
    /// </summary>
    protected abstract void LateUpdateEx();
    
    //Initialize ==============================================================
	//gameObjectが有効になった時、一度だけ呼ばれる
	//コンストラクタの代わりに使うよう推奨されている
    void Awake() {
	    AwakeEx();
    }

	//このスクリプトが有効になった時、呼ばれる
    void OnEnable() {
	
    }

	//このスクリプトが有効になった時、一度だけ呼ばれる
	void Start() {
	    StartEx();
    }

    //Updates =================================================================
    //MonoBehaviourが有効の場合に、毎フレーム呼び出される
    void Update() {
        if(!isUpdate) return;
        UpdateEx();
    }

    //MonoBehaviourが有効の場合、固定フレームレートで呼び出される
    void FixedUpdate() {
        if(!isUpdate) return;
	    FixedUpdateEx();
    }

    //Behaviourが有効の場合、は毎フレーム呼びだされる
    void LateUpdate() {
        if(!isUpdate) return;
	    LateUpdateEx();
    }


}
