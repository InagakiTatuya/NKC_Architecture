//#############################################################################
//  社員証入力ウィンドウで表示される見た目パーツのアイコン
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Events; //関数ポインタに使ってる
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class PlayerPartsIcon : MonoBehaviour {
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private Image m_Image;    //イメージ

    //データ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private int IMAGE_TYPE; //パーツの種類番号
    private int IMAGE_ID;   //データベースで管理されているID

    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        //参照-----------------------------------------------------------------
        m_Image = transform.FindChild("PhotoParts").GetComponent<Image>();
    }

    void Start() { }

    //公開関数/////////////////////////////////////////////////////////////////
    //外部からもらう初期値=====================================================
    //  CardInputWind(予定)から呼び出され必要な値を渡される
    //    第一引数：データベースで管理されている種類番号
    //    第二引数：データベースで管理されているID
    //    第三引数：タッチされたとき呼ばれる関数
    public void Init(int _ImageType, int _ImageNo, UnityAction<int,int> _onButtonEnter) {
        
        //データ---------------------------------------------------------------
        this.IMAGE_TYPE = _ImageType;
        this.IMAGE_ID   = _ImageNo;

        //onButton-------------------------------------------------------------
        Button.ButtonClickedEvent eve = GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { _onButtonEnter(IMAGE_TYPE, IMAGE_ID); });
        
        //Imageの適応----------------------------------------------------------
        m_Image.sprite = Database.obj.PLAYER_SPRITE[IMAGE_TYPE, IMAGE_ID];

    }


}
