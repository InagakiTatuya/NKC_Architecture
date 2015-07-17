﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
	private GameSceneSystem system;
	private Text text;

	private int viewTimer;
	private float time;

	void Start () {
		system = transform.root.GetComponent<GameSceneSystem>();
		text = GetComponent<Text>();
		text.text = "";
		time = 0;
		viewTimer = 3;
	}

	void Update()
	{
		if (true)
		{
			if (system.stateNo == (int)GameSceneSystem.StateNo.Check)
			{
				text.text = "" + viewTimer;
				text.fontSize = (int)(280 * time);
				if (time >= 1.0f || Input.GetMouseButtonDown(0))
				{
					viewTimer--;
					time = 0;
					if (viewTimer <= 0 || Input.GetMouseButtonDown(0))
					{
						text.text = "";
						time = 0;
						viewTimer = 3;
						system.completeFlg = true;
					}
				}
				time += Time.deltaTime;
			}
			else
			{
				text.text = "";
				time = 0;
				viewTimer = 3;
			}
		}
		}
	}
}