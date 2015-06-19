//#####################################
//  プレイヤー情報を記憶する構造体
//  作者：稲垣達也
//#####################################


public struct StractPlayerData {
    public string pleyerName;
    public int    imageHairNo;
    public int    imageHeadNo;
    public int    imageBodyNo;

    public void Init() {
        pleyerName  = "";
        imageHairNo = 0;
        imageHeadNo = 0;
        imageBodyNo = 0;
    }
}

