//#############################################################################
//  社員のパーツを表示し、選択させる
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public class PlayerPartsSelection : MonoBehaviour {
    
    //参照^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private CardInputWind ciWind;
    private Transform     traTabHair, traTabFace, traTabBody;

    void Awake() {

    }
	
	void Start () {
        //タブの初期化---------------------------------------------------------
        //  タブのOnClickイベントにOnTabButtonEnterを設定し
        //  引数にプレイヤーパーツの種類番号を渡す。
        Button.ButtonClickedEvent eve = null;

        traTabHair = transform.FindChild("PartsTabBottonHair");
        eve = traTabHair.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabHair); });

        traTabFace = transform.FindChild("PartsTabBottonFace");
        eve = traTabFace.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabFace); });

        traTabBody = transform.FindChild("PartsTabBottonBody");
        eve = traTabBody.GetComponent<Button>().onClick;
        eve.RemoveAllListeners();
        eve.AddListener(delegate { OnTabButtonEnter(traTabBody); });

        //パーツアイコンの初期化-----------------------------------------------
        Transform traParts = transform.FindChild("PartsTabBottonHair/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_HAIR, i, OnPartsButtonEnter);
        }

        traParts = transform.FindChild("PartsTabBottonFace/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_FACE, i, OnPartsButtonEnter);
        }
        
        traParts = transform.FindChild("PartsTabBottonBody/PageBack");
        for(int i = 0; i < traParts.childCount; i++) {
            traParts.GetChild(i).GetComponent<PlayerPartsIcon>().
                Init(Database.PLAYER_PARTS_BODY, i, OnPartsButtonEnter);
        }
	}
	
	
	void Update () {
	
	}

    //イベント/////////////////////////////////////////////////////////////////
    
    //タブをタッチ=============================================================
    //  タッチされたタブを先頭に持ってくる
    //
    //=========================================================================
    public void OnTabButtonEnter(Transform _tra) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + " \n" +
            "name = " + _tra.name );
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=

        _tra.SetAsLastSibling();
    }

    //パーツを選択=============================================================
    //
    //=========================================================================
    public void OnPartsButtonEnter(int _ImageType, int _ImageID) {
        //デバック用=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
        #if UNITY_EDITOR
        Debug.Log(" Time:" + Time.time.ToString("0.00") + " - " +
            this.GetType().Name + " - " +
            System.Reflection.MethodBase.GetCurrentMethod().Name + " \n" +
            "ImageType = " + _ImageType + "  ImageID = " + _ImageID);
        #endif
        //=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=\=
    }

}
