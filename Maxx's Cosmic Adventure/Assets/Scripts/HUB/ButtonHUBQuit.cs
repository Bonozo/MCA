using UnityEngine;
using System.Collections;

public class ButtonHUBQuit : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.Environments.pause.WantToExitGame = true;
	}
}
