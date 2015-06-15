using UnityEngine;
using System.Collections;

public class PauseSceneSystem : BaseObject, TouchManagerPartial
{
	private const int SCREEN_WIDTH = 361;
	private const int SCREEN_HEIGHT = 637;
	private const float SCREEN_PULLDOWN = 26.0f / 637.0f;
	public Vector2 scale;
	public float pos;

	/*
	361 673	611
	364 582 589
	*/

	protected override void AwakeEx()
	{
	}

	protected override void StartEx()
	{
	}

	protected override void UpdateEx()
	{
		scale.x = (float)Screen.width / SCREEN_WIDTH;
		scale.y = (float)Screen.height / SCREEN_HEIGHT;
		pos = Screen.height - Screen.height * SCREEN_PULLDOWN;

		traPos = new Vector3(traPos.x, pos, traPos.z);
	}

	protected override void FixedUpdateEx()
	{
	}

	protected override void LateUpdateEx()
	{
	}

	public void TouchDownOnce()
	{
	}

	public void TouchDown()
	{
	}

	public void TouchMove()
	{
	}

	public void TouchUp(bool b)
	{
	}
}
