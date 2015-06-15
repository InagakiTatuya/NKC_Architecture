//#############################################################################
//  ゲームシーン
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
public partial class GameSystem : MonoBehaviour {

    void Awake() {
        CardAwake(); //社員証初期化
    }

	// Use this for initialization
	void Start () {
	
	}
	
    int ply = 0;
    int job    = 0;

	void Update () {
        if(Input.GetKeyDown(KeyCode.Q)) { OpenCardWind(ply, job);       }
	    if(Input.GetKeyDown(KeyCode.W)) { CloseCardWind();              }
	    if(Input.GetKeyDown(KeyCode.E)) { OpenNextPleyarWind(ply, job); }
	    if(Input.GetKeyDown(KeyCode.R)) { CloseNextPleyarWind();        }
	    if(Input.GetKeyDown(KeyCode.T)) { OpenCardMiniWind(ply, job);   }
	    if(Input.GetKeyDown(KeyCode.Y)) { ChangeCardMiniWind(ply, job); }
	    if(Input.GetKeyDown(KeyCode.U)) { CloseCardMiniWind();          }
	
	    if(Input.GetKeyDown(KeyCode.UpArrow)) { 
            ply = (ply + 1) % Database.obj.getPlayerCount;
            job = (job + 1) % Database.JOB_NO_MAX;
            print("ply = " + ply + "   job = " + job);
        }
	
    }
}
