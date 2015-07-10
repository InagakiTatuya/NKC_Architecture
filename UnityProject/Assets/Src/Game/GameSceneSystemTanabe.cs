using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameSceneSystem : MonoBehaviour
{
	private List<FallObject> building;
	public List<FallObject> Building
	{
		get { return building; }
		set { building = value; }
	}

	private bool execute;
	public bool Execute
	{
		get { return execute; }
		set { execute = value; }
	}

	void StartTanabe()
	{
		building = new List<FallObject>();
		execute = false;
	}

	//更新関数
	void UpdateTanabe()
	{

	}

	//初期化
	private void UpdateIntro()
	{
		if (execute || true)
		{
			//ChangeState(StateNo.CardView, false);
			ChangeState(StateNo.PartsSet);
			execute = false;
		}
	}

	//パーツ配置
	private void UpdatePartsSet()
	{
		if (execute)
		{
			ChangeState(StateNo.Check);
			execute = false;
		}
	}

	//建物倒壊チェック
	private void UpdateCheck()
	{
		UpdateCheckKimishima();
		if (execute)
		{
			//ChangeState(StateNo.CardView, false);
			ChangeState(StateNo.PartsSet);
			execute = false;
		}
	}

	//建物倒壊
	private void UpdateGameOver()
	{
		UpdateGameOverKimishima();
	}

	//ポーズ初期化
	private void UpdatePauseBegin()
	{

	}

	//ポーズ実行中
	private void UpdatePause()
	{

	}

	//ポーズ終わり
	private void UpdatePauseEnd()
	{

	}
}
