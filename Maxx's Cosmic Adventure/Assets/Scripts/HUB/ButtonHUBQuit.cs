using UnityEngine;
using System.Collections;

public class ButtonHUBQuit : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.Environments.pause.WantToExitGame = true;
		}
	}
}
