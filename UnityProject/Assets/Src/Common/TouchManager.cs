using UnityEngine;
using System.Collections;

public partial class TouchManager : MonoBehaviour
{
	//現在タッチされている場所
	private Vector2 touchPos;
	public Vector2 TouchPos
	{
		get { return touchPos; }
		set { touchPos = value; }
	}
	//タッチした瞬間の場所
	private Vector2 startPos;
	public Vector2 StartPos
	{
		get { return startPos; }
		set { startPos = value; }
	}
	//タッチ
	private Vector2 endPos;
	public Vector2 EndPos
	{
		get { return endPos; }
		set { endPos = value; }
	}
	private Vector2 direction;

	private TouchManagerPartial downTarget;
	private TouchManagerPartial upTarget;
	public Touch touch;
	private Ray ray;
	private RaycastHit hit;

	private float touchTime;
	private int layerMask;
	private bool isOnceTouch;

	public LayerMask[] layerMaskName;

	void Awake()
	{
		hit = new RaycastHit();
		isOnceTouch = false;
		touchTime = 0;
		layerMask = 0;
		foreach (int maskName in layerMaskName)
		{
			//タッチに反応させたいオブジェクトのタグ名を追加すること
			layerMask += (1 << maskName);
		}
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);
			touchPos = touch.position;
			touchTime += Time.deltaTime; 
			switch (touch.phase)
			{
				// タッチされたとき
				case TouchPhase.Began:
					startPos = touchPos;
					TouchDownOnce();
					break;
				// タッチされているが移動していないとき
				case TouchPhase.Stationary:
					TouchDown();
					break;
				// スライドされたとき
				case TouchPhase.Moved:
					direction = touchPos - startPos;
					TouchMove();
					break;

				//離されたとき
				case TouchPhase.Canceled:
				case TouchPhase.Ended:
					endPos = touch.position;
					TouchUp();
					break;
			}
		}
	}

	void TouchDownOnce()
	{
		// メインカメラからクリックしたポジションに向かってRayを撃つ。
		ray = Camera.main.ScreenPointToRay(touchPos);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			downTarget = hit.collider.gameObject.GetComponent(typeof(TouchManagerPartial)) as TouchManagerPartial;
			if (downTarget != null)
			{
				downTarget.TouchDownOnce();
			}
		}
	}

	void TouchDown()
	{
		if (downTarget != null) downTarget.TouchDown();
	}

	void TouchMove()
	{
		if (downTarget != null) downTarget.TouchMove();
	}

	void TouchUp()
	{
		// メインカメラからクリックしたポジションに向かってRayを撃つ。
		ray = Camera.main.ScreenPointToRay(touchPos);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			upTarget = hit.collider.gameObject.GetComponent(typeof(TouchManagerPartial)) as TouchManagerPartial;
			if (upTarget != null && downTarget == upTarget) downTarget.TouchUp(true);
			else											downTarget.TouchUp(false);
		}

		touchTime = 0;
		upTarget = null;
		downTarget = null;
	}
}