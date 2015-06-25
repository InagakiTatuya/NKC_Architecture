using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchFallRequest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public GameObject moveObj;
	//プレファブの名前
	private string fileName = "Prefab/Game/Hashira00";

	private GameObject targetObj;
	private GameObject downObj;
	private Renderer render;
	private Color color;

	public void Start()
	{
		render = moveObj.GetComponent<Renderer>();
		color = render.material.color;
		color.a = 0.5f;
		render.material.color = color;

		moveObj.SetActive(false);
	}

	public void OnPointerDown(PointerEventData e)
	{
		targetObj = e.pointerEnter;

		Vector3 pos = e.position;
		pos.z = 95;
		pos = Camera.main.ScreenToWorldPoint(pos);
		moveObj.transform.position = pos;

		moveObj.SetActive(true);
	}

	public void OnDrag(PointerEventData e)
	{
		Debug.Log(targetObj + ":" + e.pointerEnter);
		if (targetObj == e.pointerEnter)
		{
			Vector3 pos = e.position;
			pos.z = 95;
			pos = Camera.main.ScreenToWorldPoint(pos);
			moveObj.transform.position = pos;
		}
	}

	public void OnPointerUp(PointerEventData e)
	{
		if (targetObj == e.pointerEnter)
		{
			Vector3 pos = e.position;
			pos.z = 95;
			pos = Camera.main.ScreenToWorldPoint(pos);

			downObj = (GameObject)Instantiate(Resources.Load<GameObject>(fileName), pos, Quaternion.identity);
			downObj.GetComponent<Rigidbody>().useGravity = true;
			downObj.GetComponent<Collider>().enabled = true;
		}
		moveObj.SetActive(false);
	}
}