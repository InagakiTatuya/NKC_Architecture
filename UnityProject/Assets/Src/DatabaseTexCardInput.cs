//#############################################################################
//  すべてのシーンで使う共通のデータを保管する
//  tex_CardInputのスプライト配列のインデックス番号を定義する
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//クラス///////////////////////////////////////////////////////////////////////
public partial class Database : SingletonCustom<Database> {
	private const int TEXCARDINPUT_OFFSET_HAIR =  0; //社員パーツ髪型
	private const int TEXCARDINPUT_OFFSET_FACE =  9; //社員パーツ顔
	private const int TEXCARDINPUT_OFFSET_BODY = 17; //社員パーツ体

    //プレイヤーパーツの種類
    public  const int   PLAYER_PARTS_HAIR = 0;
    public  const int   PLAYER_PARTS_FACE = 1;
    public  const int   PLAYER_PARTS_BODY = 2;
    
    //パーツの数
    public  const int   FHOT_NO_MAX = 9;
    
    private Sprite[,]   M_PLAYER_SPRITE;
    public  Sprite[,]   PLAYER_SPRITE { get { return M_PLAYER_SPRITE; } }


    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    private void CardInputAwake() {
        //スプライト読み込み
        Sprite[] bff = 
            Resources.LoadAll<Sprite>("Texture/CardInput/tex_CardInput");

        //パーツだけを取り出す
        M_PLAYER_SPRITE = new Sprite[3, FHOT_NO_MAX];

        //髪型-----------------------------------------------------------------
        for(int i = 0; i < FHOT_NO_MAX; i++) {
            M_PLAYER_SPRITE[PLAYER_PARTS_HAIR, i] =
                                bff[TEXCARDINPUT_OFFSET_HAIR + i];
        }
        //顔-------------------------------------------------------------------
        for(int i = 0; i < FHOT_NO_MAX; i++) {
            M_PLAYER_SPRITE[PLAYER_PARTS_FACE, i] =
                                bff[TEXCARDINPUT_OFFSET_FACE + i];
        }
        //体-------------------------------------------------------------------
        for(int i = 0; i < FHOT_NO_MAX; i++) {
            M_PLAYER_SPRITE[PLAYER_PARTS_BODY, i] =
                                bff[TEXCARDINPUT_OFFSET_BODY + i];
        }
    }

}
