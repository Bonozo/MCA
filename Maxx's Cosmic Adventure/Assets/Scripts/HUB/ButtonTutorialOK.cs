using UnityEngine;
using System.Collections;

public class ButtonTutorialOK : MonoBehaviour {

	void OnClick()
	{
		Time.timeScale = 1f;
		LevelInfo.Environments.playerShip.Ready = true;
		LevelInfo.Environments.popupTutorial.SetActive(false);
	}
}
