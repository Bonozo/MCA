using UnityEngine;
using System.Collections;

public class ButtonHUBResume : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.Environments.pause.UnPauseGame();
		}
	}
}
