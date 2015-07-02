using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[SerializeField]
	private float fallSpeed = 50.0f;

	//リードオンリー
	private GameObject moveObj;
	private string fileName = "Prefab/Game/Hashira00";

	//生成する建造物
	private GameObject targetObj;
	private GameObject downObj;

	//自身のマテリアル
	private Renderer render;
	private Color color;

	//初期化
	public void Awake()
	{
		moveObj = GameObject.Find("Hashira00");

		render = moveObj.GetComponent<Renderer>();
		color = render.material.color;
		color.a = 0.5f;
		render.material.color = color;

		moveObj.SetActive(false);
	}

	//UIオブジェクトがタッチされたら
	public void OnPointerDown(PointerEventData e)
	{
		Vector3 pos = e.position;
		pos.z = 95;
		pos = Camera.main.ScreenToWorldPoint(pos);
		moveObj.transform.position = pos;

		moveObj.SetActive(true);
	}

	//UIオブジェクトがドラッグされたら
	public void OnDrag(PointerEventData e)
	{
		Vector3 pos = e.position;
		pos.z = 95;
		pos = Camera.main.ScreenToWorldPoint(pos);
		moveObj.transform.position = pos;

		if (gameObject == e.pointerEnter) color = Color.white;
		else color = Color.red;
		color.a = 0.5f;
		render.material.color = color;
	}

	//UIオブジェクトが放されたら
	public void OnPointerUp(PointerEventData e)
	{
		//Debug.Log(targetObj +":"+ e.pointerEnter);
		if (gameObject == e.pointerEnter)
		{
			Vector3 pos = e.position;
			pos.z = 95;
			pos = Camera.main.ScreenToWorldPoint(pos);

			downObj = (GameObject)Instantiate(Resources.Load<GameObject>(fileName), pos, Quaternion.identity);
			downObj.GetComponent<Rigidbody>().useGravity = true;
			downObj.GetComponent<Collider>().enabled = true;
			downObj.GetComponent<Rigidbody>().AddForce(-transform.up * fallSpeed, ForceMode.Impulse);
		}
		moveObj.SetActive(false);
	}
}