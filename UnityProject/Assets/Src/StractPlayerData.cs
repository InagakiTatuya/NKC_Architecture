﻿//#####################################
//  プレイヤー情報を記憶する構造体
//  作者：稲垣達也
//#####################################


public struct StractPlayerData {
    public string pleyerName;
    public int    imageHairNo;
    public int    imageFaceNo;
    public int    imageBodyNo;

    public void Init() {
        pleyerName  = "";
        imageHairNo = 0;
        imageFaceNo = 0;
        imageBodyNo = 0;
    }
}

