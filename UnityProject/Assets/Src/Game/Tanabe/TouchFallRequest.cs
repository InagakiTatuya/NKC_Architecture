using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	private string[] buildName = new string[]{
		"Prefab/Game/Build/Brick/Floor",
		"Prefab/Game/Build/Brick/Pillar",
		"Prefab/Game/Build/Brick/Wall",
	};

	private string optionName = "Prefab/Game/TouchDisAbleArea";

	[SerializeField]
	private float fallSpeed = 50.0f;
	private int buildNo;

	private Vector3 pos;

	//システム
	private GameSceneSystem system;

	//生成する建造物
	private GameObject[] moveObj;
	private GameObject targetObj;
	private GameObject downObj;
	private Transform childObj;

	//自身のマテリアル
	private Renderer[] render;
	private Color[] color;

	//初期化
	public void Awake(){
		system = transform.root.GetComponent<GameSceneSystem>();

		pos = Vector3.zero;

		moveObj = new GameObject[buildName.Length];
		render = new Renderer[buildName.Length];
		color = new Color[buildName.Length];

		for(int i=0;i<moveObj.Length;i++){
			moveObj[i] = Instantiate(Resources.Load<GameObject>(buildName[i]));
			moveObj[i].transform.parent = transform.root;

			for(int j=0;j<moveObj[i].transform.childCount;j++){
				render[i] = moveObj[i].transform.GetChild(j).GetComponent<Renderer>();
				color[i] = render[i].material.color;
				color[i].a = 0.5f;
				render[i].material.color = color[i];
				moveObj[i].transform.GetChild(j).GetComponent<Collider>().enabled = false;
			}

			//GameObject optionObj = Instantiate(Resources.Load<GameObject>(optionName));
			//optionObj.transform.parent = moveObj[i].transform;

			moveObj[i].SetActive(false);
		}
	}

	//UIオブジェクトがタッチされたら
	public void OnPointerDown(PointerEventData e){
		buildNo = system.GetJob;

		SetPos(e);
		moveObj[buildNo].SetActive(true);
	}

	//UIオブジェクトがドラッグされたら
	public void OnDrag(PointerEventData e){
		SetPos(e);
		if (gameObject == e.pointerEnter)	color[buildNo] = Color.white;
		else								color[buildNo] = Color.red;
		color[buildNo].a = 0.5f;
		render[buildNo].material.color = color[buildNo];
	}

	//UIオブジェクトが放されたら
	public void OnPointerUp(PointerEventData e){
		//Debug.Log(targetObj +":"+ e.pointerEnter);
		if (gameObject == e.pointerEnter){
			SetPos(e);

			downObj = (GameObject)Instantiate(Resources.Load<GameObject>(buildName[buildNo]), pos, Quaternion.identity);
			FallObject.ChildCount = downObj.transform.childCount;
			while(downObj.transform.childCount>0){
				childObj = downObj.transform.GetChild(0);
				childObj.GetComponent<Collider>().enabled = true;
				childObj.GetComponent<FallObject>().enabled = true;
				childObj.GetComponent<Rigidbody>().AddForce(-transform.up * fallSpeed, ForceMode.Impulse);
				childObj.parent = transform.root;
				if(downObj.transform.childCount == 0) DestroyObject(downObj);
			}
			gameObject.SetActive(false);
		}
		moveObj[buildNo].SetActive(false);
	}

	void SetPos(PointerEventData e){
		pos = e.position;
		pos.z = 85;
		pos = Camera.main.ScreenToWorldPoint(pos);
		moveObj[buildNo].transform.position = pos;
	}
}