using UnityEngine;
using System.Collections;

public class ButtonHUBResume : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.Environments.pause.UnPauseGame();
	}
}
