using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{
	private string[,] buildName = new string[,]{
		{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Ice/Floor",
			"Prefab/Game/Build/Ice/Pillar",
			"Prefab/Game/Build/Ice/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},{
			"Prefab/Game/Build/Brick/Floor",
			"Prefab/Game/Build/Brick/Pillar",
			"Prefab/Game/Build/Brick/Wall",
		},
	};

	//private string optionName = "Prefab/Game/TouchDisAbleArea";

	[SerializeField]
	private float fallSpeed = 50.0f;
	private int buildNo;
	private int partsID;

	private Vector3 pos;

	//システム
	private GameSceneSystem system;

	//生成する建造物
	private GameObject[,] moveObj;
	private GameObject targetObj;
	private GameObject downObj;
	private Transform childObj;

	//自身のマテリアル
	private Renderer render;
	private Color color;

	//初期化
	public void Awake(){
		system = transform.root.GetComponent<GameSceneSystem>();
		pos = Vector3.zero;

		moveObj = new GameObject[buildName.GetLength(0),buildName.GetLength(1)];
		for(int i=0;i<moveObj.GetLength(0);i++){
			for(int j=0;j<moveObj.GetLength(1);j++){
				moveObj[i,j] = Instantiate(Resources.Load<GameObject>(buildName[i,j]));
				moveObj[i,j].transform.parent = transform.root;

				for(int k=0;k<moveObj[i,j].transform.childCount;k++){
					childObj = moveObj[i,j].transform.GetChild(k);
					render = childObj.GetComponent<Renderer>();
					if(render != null){
						color = render.material.color;
						color.a = 0.5f;
						render.material.color = color;
					}
					for(int l=0;l<childObj.childCount;l++){
						if(childObj.GetChild(l).tag == "Shadow") continue;
						render = childObj.GetChild(l).GetComponent<Renderer>();
						if(render != null){
							color = render.material.color;
							color.a = 0.5f;
							render.material.color = color;
						}
					}
					if(childObj.GetComponent<Collider>()!=null){
						childObj.GetComponent<Collider>().enabled = false;
					}
				}

				//GameObject optionObj = Instantiate(Resources.Load<GameObject>(optionName));
				//optionObj.transform.parent = moveObj[i].transform;

				moveObj[i,j].SetActive(false);
			}
		}
	}

	//UIオブジェクトがタッチされたら
	public void OnPointerDown(PointerEventData e){
		buildNo = system.GetJob;
		partsID = system.PartsID;

		SetPos(e);
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
		if (gameObject == e.pointerEnter){
			SetPos(e);

			downObj = (GameObject)Instantiate(Resources.Load<GameObject>(buildName[partsID,buildNo]), pos, Quaternion.identity);
			FallObject.ChildCount = downObj.transform.childCount;
			while(downObj.transform.childCount>0){
				childObj = downObj.transform.GetChild(0);
				if(childObj.tag == "Shadow"){
					FallObject.ChildCount=0;
					break;
				}
				childObj.GetComponent<Collider>().enabled = true;
				childObj.GetComponent<FallObject>().enabled = true;
				childObj.GetComponent<Rigidbody>().AddForce(-transform.up * fallSpeed, ForceMode.Impulse);
				childObj.parent = transform.root;
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