using UnityEngine;
using System.Collections;

public partial class GameSceneSystem : MonoBehaviour
{
	private bool execute;

	void StartTanabe()
	{

	}

	//更新関数
	void UpdateTanabe()
	{

	}

	//初期化
	private void UpdateIntro()
	{
		if (execute)
		{
			ChangeState(StateNo.PartsSet, false);
			execute = false;
		}
		//ChangeState(StateNo.CardView, false);
	}

	//パーツ配置
	private void UpdatePartsSet()
	{
		if (execute)
		{
			ChangeState(StateNo.Check, false);
			execute = false;
		}
	}

	//建物倒壊チェック
	private void UpdateCheck()
	{
		if (execute)
		{
			ChangeState(StateNo.CardView, false);
			execute = false;
		}
	}

	//建物倒壊
	private void UpdateGameOver()
	{

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
