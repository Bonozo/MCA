using UnityEngine;
using System.Collections;

public class ButtonQuitPopupBack : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.Environments.pause.WantToExitGame = false;
	}
}
