using UnityEngine;
using System.Collections;

public class ButtonHUBPlayAgain : MonoBehaviour {

	void OnClick()
	{
		bool canRateApp = PlayerPrefs.GetInt("rateapppopupcompleted",0)==0;
		
		if(canRateApp && LevelInfo.Environments.playerShip.allAttempt%20==0)
		{
			ButtonRateApp.pushedActionIsRestart = true;
			LevelInfo.Environments.popupRateApp.SetActive(true);
			LevelInfo.Environments.popupLose.SetActive(false);
		}
		else
			Application.LoadLevel(Application.loadedLevel);
	}
}
