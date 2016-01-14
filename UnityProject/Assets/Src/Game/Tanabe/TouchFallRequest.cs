using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	#region //field
	#region //設置物(上から)
	//床
	//柱
	//壁
	//屋根
	private string[,] buildName = new string[,]{
		{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
			"Prefab/Game/Build/Brick/Roof",
		},{
			"Prefab/Game/Build/Ice/Floor",
			"Prefab/Game/Build/Ice/Pillar",
			"Prefab/Game/Build/Ice/Wall",
			"Prefab/Game/Build/Ice/Roof",
		},{
			"Prefab/Game/Build/Dannbo/Floor",
			"Prefab/Game/Build/Dannbo/Pillar",
			"Prefab/Game/Build/Dannbo/Wall",
			"Prefab/Game/Build/Dannbo/Roof",
		},{
			"Prefab/Game/Build/Marble/Floor",
			"Prefab/Game/Build/Marble/Pillar",
			"Prefab/Game/Build/Marble/Wall",
			"Prefab/Game/Build/Marble/Roof",
		},{
			"Prefab/Game/Build/Lego/floor",
			"Prefab/Game/Build/Lego/pillar",
			"Prefab/Game/Build/Lego/wall",
			"Prefab/Game/Build/Lego/roof",
		},{
			"Prefab/Game/Build/Tohu/floor",
			"Prefab/Game/Build/Tohu/pillar",
			"Prefab/Game/Build/Tohu/wall",
			"Prefab/Game/Build/Tohu/roof",
		},{
			"Prefab/Game/Build/sand/floor",
			"Prefab/Game/Build/sand/pillar",
			"Prefab/Game/Build/sand/wall",
			"Prefab/Game/Build/sand/roof",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
			"Prefab/Game/Build/Brick/Roof",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
			"Prefab/Game/Build/Brick/Roof",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
			"Prefab/Game/Build/Brick/Roof",
		},
	};
	#endregion
	private GameSceneSystem system;
	
	private const float	depth	=	85.0f;

	[SerializeField]
	private float	fallSpeed	=	50.0f;

	private int		buildNo;
	private int		partsId;
	private bool	firstOutBuildingFlag;

	//生成する建造物
	private GameObject[,]	moveObj;
	private GameObject		downObj;
	private Transform		childObj;
	private Color			buildColor;
	private Vector3			pos;
	
	//タッチフィールド用
	private FlashingUI		touchFieldFlashUI;

	#endregion

	#region //method
	void Awake(){
		moveObj					=	new GameObject[buildName.GetLength(0),buildName.GetLength(1)];

		system					=	transform.root.GetComponent<GameSceneSystem>();
		touchFieldFlashUI		=	new FlashingUI(GetComponent<Image>(),1.5f);

		pos						=	Vector3.zero;
		firstOutBuildingFlag	=	false;
	}
	void Update(){
		if(touchFieldFlashUI!=null) touchFieldFlashUI.Flash();
	}

	//UIオブジェクトがタッチされたら
	public void OnPointerDown(PointerEventData e){
		buildNo = system.GetJob;
		partsId = system.PartsID;

		InstanceData();
		SetObjPos(e);
		//選択されたオブジェクトのプレビューを許可
		if(moveObj[partsId,buildNo] == null) return;
		moveObj[partsId,buildNo].SetActive(true);
	}
	//UIオブジェクトがドラッグされたら
	public void OnDrag(PointerEventData e){
		SetObjPos(e);
	}
	//UIオブジェクトが放されたら
	public void OnPointerUp(PointerEventData e){
		if(moveObj[partsId,buildNo] == null) return;
		if(gameObject == e.pointerEnter){//オブジェクト設置範囲内で離された
			system.seManager.Play(0);
			SetObjPos(e);
			//設置するオブジェクトを生成
			downObj					=	Resources.Load<GameObject>(buildName[partsId,buildNo]);
			downObj					=	(GameObject)Instantiate(downObj, pos, Quaternion.identity);
			//設置数を記録
			FallObject.ChildCount	=	downObj.transform.childCount;

			//設置オブジェクトを分解する
			DisassemblyObj();
			DestroyObject(downObj);
			downObj = null;
			
			DestroyObject(moveObj[partsId,buildNo]);
			moveObj[partsId,buildNo] = null;
			
			touchFieldFlashUI.SetColorAlpha(0.0f);
			gameObject.SetActive(false);

			return;
		}
		moveObj[partsId,buildNo].SetActive(false);
	}

	//設置オブジェクトを分解する
	private void DisassemblyObj()
	{
		Transform childTransCache = downObj.transform;
		int childNum = downObj.transform.childCount;
		while(childNum>0){//接続された子を独立させる
			childObj = childTransCache.GetChild(childNum-1);
			if(childObj.tag == "Shadow"){//影は子として数えない
				childNum--;
				continue;
			}
			childObj.GetComponent<Collider	>().enabled	= true;
			childObj.GetComponent<FallObject>().enabled	= true;
			childObj.GetComponent<Rigidbody	>().AddForce(-childTransCache.up * fallSpeed, ForceMode.VelocityChange);
			if(!firstOutBuildingFlag){
				childObj.parent			=	transform.root;
				firstOutBuildingFlag	=	true;
			}else{
				childObj.parent			=	transform.root.GetChild(7);
			}
			childNum--;
		}
	}
	//オブジェクトの設置位置を設定
	void SetObjPos(PointerEventData e){
		if(moveObj[partsId,buildNo] == null) return;
		pos		=	e.position;
		pos.z	=	depth;
		pos		=	Camera.main.ScreenToWorldPoint(pos);
		moveObj[partsId,buildNo].transform.position = pos;
	}
	//選択されたオブジェクトのプレビュー用データ生成
	void InstanceData(){
		if(moveObj[partsId,buildNo] != null)			return;
		if(partsId<0 || partsId>=moveObj.GetLength(0))	return;
		if(buildNo<0 || buildNo>=moveObj.GetLength(1))	return;
		
		GameObject temp,loadTemp;
		loadTemp				=	Resources.Load<GameObject>(buildName[partsId,buildNo]);
		temp					=	Instantiate(loadTemp);
		temp.transform.parent	=	transform.root.GetChild(8);
		temp.SetActive(false);
		moveObj[partsId,buildNo] = temp;
	}
	#endregion
}