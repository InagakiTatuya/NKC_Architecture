//#####################################
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

    public void SetImageData(int _ImageType, int _no) {
        
        switch(_ImageType) {
            case Database.PLAYER_PARTS_BODY:
                imageBodyNo = _no; break;
            case Database.PLAYER_PARTS_FACE:
                imageFaceNo = _no; break;
            case Database.PLAYER_PARTS_HAIR:
                imageHairNo = _no; break;
        }

    }

    public override string ToString() {
        return "[ Name = " + pleyerName + ", HairNo = " + imageHairNo +
             ", FaceNo = " + imageFaceNo + ", BodyNo = " + imageBodyNo + " ]";
    }
}

