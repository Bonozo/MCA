using UnityEngine;
using System.Collections;

public class ButtonQuitPopupBack : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.Environments.pause.WantToExitGame = false;
		}
	}
}
