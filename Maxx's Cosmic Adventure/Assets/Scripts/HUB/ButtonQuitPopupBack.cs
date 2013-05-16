using UnityEngine;
using System.Collections;

public class ButtonQuitPopupBack : MonoBehaviour {

	void OnClick()
	{
		if(LevelInfo.State.state == GameState.Lose)
			LevelInfo.State.goMenuWhenLose = false;
		else
			LevelInfo.Environments.pause.WantToExitGame = false;
	}
}
