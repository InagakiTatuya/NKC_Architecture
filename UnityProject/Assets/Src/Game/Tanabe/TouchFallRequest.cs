using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{
	//設置物(上から)
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


	[SerializeField]
	private float fallSpeed = 50.0f;
	private int buildNo;
	private int partsID;

	private Vector3 pos;
	private GameSceneSystem system;

	//生成する建造物
	private GameObject[,] moveObj;
	private GameObject downObj;
	private Transform childObj;

	private Renderer render;
	private Color color;

	//初期化
	public void Awake(){
		system = transform.root.GetComponent<GameSceneSystem>();
		pos = Vector3.zero;

		moveObj = new GameObject[buildName.GetLength(0),buildName.GetLength(1)];
		for(int i=0;i<moveObj.GetLength(0);i++){
			for(int j=0;j<moveObj.GetLength(1);j++){
				//プレビュー用
				moveObj[i,j] = Instantiate(Resources.Load<GameObject>(buildName[i,j]));
				moveObj[i,j].transform.parent = transform.root.GetChild(8);
				if(moveObj[i,j].transform.tag != "Ice"){//既に透過されているので必要なし
					for(int k=0;k<moveObj[i,j].transform.childCount;k++){
						childObj = moveObj[i,j].transform.GetChild(k);
						render = childObj.GetComponent<Renderer>();
						if(render != null){
							color = render.material.color;
							color.a = 0.5f;
							render.material.color = color;
						}
					}
				}

				moveObj[i,j].SetActive(false);
			}
		}
	}

	//UIオブジェクトがタッチされたら
	public void OnPointerDown(PointerEventData e){
		buildNo = system.GetJob;
		partsID = system.PartsID;

		SetPos(e);
		//選択されたオブジェクトのプレビューを許可
		moveObj[partsID,buildNo].SetActive(true);
	}

	//UIオブジェクトがドラッグされたら
	public void OnDrag(PointerEventData e){
		SetPos(e);
		/*
		if (gameObject == e.pointerEnter)	color[buildNo] = Color.white;
		else								color[buildNo] = Color.red;
		color[buildNo].a = 0.5f;
		render[buildNo].material.color = color[buildNo];
		*/
	}

	//UIオブジェクトが放されたら
	public void OnPointerUp(PointerEventData e){
		//Debug.Log(targetObj +":"+ e.pointerEnter);
		//オブジェクト設置範囲内で離された
		if (gameObject == e.pointerEnter){
			system.seManager.Play(0);
			SetPos(e);

			//設置用オブジェクト生成
			downObj = (GameObject)Instantiate(Resources.Load<GameObject>(buildName[partsID,buildNo]), pos, Quaternion.identity);
			//設置数を記録
			FallObject.ChildCount = downObj.transform.childCount;
			while(downObj.transform.childCount>0){
				childObj = downObj.transform.GetChild(0);
				if(childObj.tag == "Shadow"){//影は子として数えない
					FallObject.ChildCount=0;
					break;
				}
				childObj.GetComponent<Collider>().enabled = true;
				childObj.GetComponent<FallObject>().enabled = true;
				childObj.GetComponent<Rigidbody>().AddForce(-transform.up * fallSpeed, ForceMode.Impulse);
				childObj.parent = transform.root.GetChild(7);
				//分解が完了したら削除
				if(downObj.transform.childCount == 0) DestroyObject(downObj);
			}
			gameObject.SetActive(false);
		}
		moveObj[partsID,buildNo].SetActive(false);
	}

	void SetPos(PointerEventData e){
		pos = e.position;
		pos.z = 85;
		pos = Camera.main.ScreenToWorldPoint(pos);
		moveObj[partsID,buildNo].transform.position = pos;
	}
}