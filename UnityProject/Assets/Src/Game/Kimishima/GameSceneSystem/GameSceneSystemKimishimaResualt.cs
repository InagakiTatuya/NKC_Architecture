//----------------------------------------------------------
//ゲームシーンのシステムのリザルト
//更新者 :	君島一刀
//----------------------------------------------------------

//プリプロセッサ////////////////////////////////////////////
#if UNITY_EDITOR
#define	DEBUG_GAMESCENE
#endif

//名前空間//////////////////////////////////////////////////
using	UnityEngine;
using	UnityEngine.Events;
using	UnityEngine.UI;
using	System;
using	System.IO;
using	System.Collections;

//クラス////////////////////////////////////////////////////
//ゲームシーンのシステム_Begin//----------------------------
public	partial class GameSceneSystem : MonoBehaviour{
	
	//列挙・テーブル//////////////////////////////////////////
	private	enum	ResultStateNo{//リザルトのステート番号
		Open,		//開く
		Liquidation,//清算
		Neutral,	//通常
		NextScene,	//次のシーンへ
		Hide,		//隠す
		SSBegin,SS,SSEnd,	//スクリーンショット
	}
	private	enum	ResultButtonNo{//リザルトボタンの番号
		Retry,		//リトライ
		Back,		//バック
		Length,
	}
	private	UnityAction[]	resultUpdateFunc= null;

	//変数//////////////////////////////////////////////////
	//リザルト関連
	private	Image		resultWindow		= null;
	private	Vector3		resultWindowSize;
	private	NumberDisp	resultText			= null;
	private	Image		resultTextImage		= null;
	private	Button[]	resultButton		= null;
	private	ButtonSystem[]	resultButtonSystem			= null;
	private	ButtonSystem[]	resultCameraButtonSystem	= null;
	private	int			resultStateNo;
	private	float		resultStateTime;
	private	int			resultDispFloor;
	private	bool		newRecodeFlg;
	private	int			resultSelectID;
	private bool		nextSceneFadeFlag;
	private	bool		isRunning;
	
	//初期化////////////////////////////////////////////////
	
	//更新//////////////////////////////////////////////////
	private	void	UpdateResult(){//リザルトの更新中枢
		if(resultUpdateFunc[resultStateNo] != null)	resultUpdateFunc[resultStateNo]();
		resultStateTime	+= Time.deltaTime;
	}
	private	void	UpdateResultOpen(){//リザルトを開く
		cameraMove.touchPermit	= true;
		resultDispFloor			= 0;
		resultSelectID			= -1;
		CreateResultWindow();
		CreateResultText();
		CreateResultButton();
		CreateResultCameraButton();
		ChangeResultState(ResultStateNo.Liquidation);
	}
	private	void	UpdateResultLiquidation(){//精算ステート
		resultDispFloor	= (int)(((gameOverFlg)?0:floor) * Mathf.Min(resultStateTime,1.0f));
		resultText.value= resultDispFloor;
		if(resultStateTime < 1.0f)	return;
		ChangeResultState(ResultStateNo.Neutral);
		if(!gameOverFlg && Database.MaxFloor < floor){
			Database.MaxFloor	= floor;
			newRecodeFlg		= true;
		}else{
			newRecodeFlg		= false;
		}
	}
	private	void	UpdateResultNeutral(){//リザルトシーンの通常時
		floorSize.y	= 0.0f;
		cameraMove.touchPermit	= true;
		resultWindow.rectTransform.sizeDelta	= resultWindow.rectTransform.sizeDelta * 0.5f + new Vector2(256.0f,256.0f);
		resultText.size							= resultText.size * 0.5f + new Vector2(32.0f,32.0f);
		resultTextImage.rectTransform.sizeDelta	= resultTextImage.rectTransform.sizeDelta * 0.5f + new Vector2(32.0f,16.0f);
		for(int i = 0;i < resultButton.Length;i ++){
			resultButtonSystem[i].buttonSize	= new Vector2(512.0f,128.0f);
		}
	}
	private	void	UpdateResultNextScene(){//次のシーンへ遷移
		if(!nextSceneFadeFlag)	{
			fadeObj.setFadeOut();
			nextSceneFadeFlag = true;
		}
		if(resultStateTime < 0.5f)	return;
		string[]	nextSceneName	= new string[]{"Game","Select"};
		Application.LoadLevel(nextSceneName[resultSelectID]);
	}
	private	void	UpdateResultHide(){//ウィンドウやボタンを隠す
		resultWindow.rectTransform.sizeDelta	= resultWindow.rectTransform.sizeDelta * 0.5f + new Vector2(1024.0f,0.0f);
		resultText.size							= resultText.size * 0.5f + new Vector2(64.0f,0.0f);
		resultTextImage.rectTransform.sizeDelta	= resultTextImage.rectTransform.sizeDelta * 0.5f + new Vector2(64.0f,0.0f);
		for(int i = 0;i < resultButton.Length;i ++){
			resultButtonSystem[i].buttonSize	= new Vector2(1024.0f,0.0f);
		}
	}
	private	void	UpdateResultSSBegin(){
		for(int i = 0;i < resultCameraButtonSystem.Length;i	++){
			resultCameraButtonSystem[i].buttonSize	= Vector2.zero;
		}
		if(resultStateTime >= 0.5f)	ChangeResultState(ResultStateNo.SS);
	}
	private	void	UpdateResultSS(){
		StartCoroutine("CaptureScreen");
	}
	IEnumerator CaptureScreen(){
		//コルーチンの多重起動を防ぐ
		if(isRunning)	yield break;
		isRunning = true;

		string	tempName;
		string	name	=	"建塔師_" + DateTime.Now.Ticks + ".png";

		if (Application.platform == RuntimePlatform.Android){
			string	bufName	= "../../../../DCIM/Kentoushi/" + name;
			Application.CaptureScreenshot(bufName);
		}else{
			Application.CaptureScreenshot(name);
		}

		yield return new WaitForEndOfFrame();//1F停止

		ScanMedia(name);//アルバム更新
		ChangeResultState(ResultStateNo.SSEnd);
		isRunning = false;
	}
	private	void	ScanMedia(string fileName){//アルバムを更新して画像を適用させる
        if (Application.platform != RuntimePlatform.Android)	return;
#if UNITY_ANDROID
        using (AndroidJavaClass		jcMediaScannerConnection	= new AndroidJavaClass ("android.media.MediaScannerConnection"))
        using (AndroidJavaClass		jcUnityPlayer				= new AndroidJavaClass ("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaClass		jcEnvironment				= new AndroidJavaClass ("android.os.Environment"))
        using (AndroidJavaObject	joActivity					= jcUnityPlayer.GetStatic	<AndroidJavaObject> ("currentActivity"))
        using (AndroidJavaObject	joContext					= joActivity.Call			<AndroidJavaObject> ("getApplicationContext"))
        using (AndroidJavaObject	joExDir						= jcEnvironment.CallStatic	<AndroidJavaObject> ("getExternalStorageDirectory")) {
            string path = joExDir.Call<string> ("toString") + "/DCIM/Kentoushi/" + fileName;
            jcMediaScannerConnection.CallStatic ("scanFile", joContext, new string[] { path }, new string[] { "image/png" }, null);
        }
#endif
    }
	private	void	UpdateResultSSEnd(){
		for(int i = 0;i < resultCameraButtonSystem.Length;i ++){
			resultCameraButtonSystem[i].buttonSize	= new Vector2(128,128);
		}
		if(resultStateTime >= 0.5f)	ChangeResultState(ResultStateNo.Neutral);
	}
	
	//ボタン関連////////////////////////////////////////////
	private	void	OnResultButtonEnter(ButtonSystem bs){
		resultSelectID	= bs.buttonID;
		seManager.Play(6);
		ChangeResultState(ResultStateNo.NextScene);
	}

	private	void	OnCameraButtonEnter(ButtonSystem bs){
		seManager.Play(6);
		if(bs.buttonID == 0)	GUIOFF();
		else 					Shot();
	}
	private	void	GUIOFF(){
		if(resultStateNo == (int)ResultStateNo.Hide)	ChangeResultState(ResultStateNo.Neutral);
		else 											ChangeResultState(ResultStateNo.Hide);
	}
	private	void	Shot(){
		ChangeResultState(ResultStateNo.SSBegin);
	}
	
	//その他関数////////////////////////////////////////////
	//リザルトのステートを遷移する_Begin//--------------------
	void	ChangeResultState(ResultStateNo value){
		resultStateNo		= (int)value;
		resultStateTime	= 0.0f;
	}//リザルトのステートを遷移する_End//---------------------

	//リザルトウィンドウを生成_Begin//------------------------
	private	void	CreateResultWindow(){
		GameObject	obj		= TitleSystem.CreateObjectInCanvas("Prefab/Game/FloorWindow",canvasObject);
		resultWindow		= obj.GetComponent<Image>();
		resultWindow.sprite	= Resources.Load<Sprite>("Texture/Result/window");
		resultWindow.rectTransform.localPosition	= new Vector3(0.0f,192.0f);
		resultWindow.rectTransform.sizeDelta		= new Vector2(512.0f,512.0f);
		resultWindow.color							= Color.white;
	}//リザルトウィンドウを生成_End//-------------------------

	//リザルトのテキストを生成_Begin//-----------------------
	private	void	CreateResultText(){
		GameObject	obj	= TitleSystem.CreateObjectInCanvas("Prefab/Select/number",resultWindow.gameObject);
		resultText		= obj.GetComponent<NumberDisp>();
		resultText.value	= 0;
		resultText.pos		= new Vector3(0.0f,-60.0f,0.0f);
		resultText.size		= new Vector2(64.0f,64.0f);
		resultText.offset	= -64.0f;
		resultText.color	= new Color(0.13f,0.21f,0.84f,1.0f);
		GameObject	buf			= Instantiate(new GameObject());
		buf.transform.parent	= resultWindow.transform;
		Sprite[]	spriteBuf	= Resources.LoadAll<Sprite>("Texture/Select/number");
		resultTextImage			= buf.AddComponent<Image>();
		resultTextImage.sprite	= spriteBuf[10];
		resultTextImage.color	= Color.white;
		resultTextImage.rectTransform.localPosition	= new Vector3(96.0f,-72.0f,0.0f);
		resultTextImage.rectTransform.localScale	= Vector3.one;
		resultTextImage.rectTransform.sizeDelta		= new Vector2(64.0f,32.0f);
	}//リザルトのテキストを生成_End//------------------------

	//リザルトのボタンを生成_Begin//-------------------------
	private	void	CreateResultButton(){
		GameObject	obj				= null;
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(0.0f,-64.0f),
			new Vector3(0.0f,-192.0f)
		};
		resultButton		= new Button[(int)ResultButtonNo.Length];
		resultButtonSystem	= new ButtonSystem[(int)ResultButtonNo.Length];
		for(int i = 0;i < resultButton.Length;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			resultButton[i]			= obj.GetComponent<Button>();
			resultButton[i].colors	= Database.colorBlocks[(int)Database.ColorBlockID.White];
			resultButtonSystem[i]				= obj.GetComponent<ButtonSystem>();
			Sprite[]	sprite		= Resources.LoadAll<Sprite>("Texture/Result/button" + i);
			resultButtonSystem[i].sprite.normalSprite	= sprite[0];
			resultButtonSystem[i].sprite.pushSprite		= sprite[1];
			resultButtonSystem[i].text.init();
			resultButtonSystem[i].buttonPos		= tableButtonPos[i];
			resultButtonSystem[i].buttonSize	= new Vector2(512.0f,128.0f);
			resultButtonSystem[i].buttonID		= i;
			resultButtonSystem[i].buttonEnter	= OnResultButtonEnter;
		}
	}

	private	void	CreateResultCameraButton(){
		GameObject	obj	= null;
		Vector3[]	tableButtonPos	= new Vector3[]{
			new Vector3(-96.0f,-388.0f),
			new Vector3( 96.0f,-388.0f)
		}; 
		Sprite[]	sprite	= Resources.LoadAll<Sprite>("Texture/Result/camerabutton");
		resultCameraButtonSystem	= new ButtonSystem[tableButtonPos.Length];
		for(int i = 0;i < 2;i ++){
			obj	= TitleSystem.CreateObjectInCanvas("Prefab/Title/Button",canvasObject);
			resultCameraButtonSystem[i]			= obj.GetComponent<ButtonSystem>();
			resultCameraButtonSystem[i].sprite.normalSprite	= sprite[i * 2 + 0];
			resultCameraButtonSystem[i].sprite.pushSprite	= sprite[i * 2 + 1];
			resultCameraButtonSystem[i].text.init();
			resultCameraButtonSystem[i].buttonPos	= tableButtonPos[i];
			resultCameraButtonSystem[i].buttonSize	= new Vector2(128.0f,128.0f);
			resultCameraButtonSystem[i].buttonID	= i;
			resultCameraButtonSystem[i].buttonEnter	= OnCameraButtonEnter;
		}
	}

	void	OnApplicationQuit(){
		if(isRunning){
			StopCoroutine("CaptureScreen");
			isRunning	=	false;
		}
	}
	
}//ゲームシーンのシステム_End//------------------------------