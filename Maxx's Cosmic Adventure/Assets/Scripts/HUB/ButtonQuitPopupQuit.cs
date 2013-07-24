using UnityEngine;
using System.Collections;

public class ButtonQuitPopupQuit : MonoBehaviour {

	void OnClick()
	{
		Stats.Instance.ClearInSingleRunMissions();
		bool canRateApp = PlayerPrefs.GetInt("rateapppopupcompleted",0)==0;
		
		if(canRateApp && LevelInfo.Environments.playerShip.allAttempt%20==0)
		{
			ButtonRateApp.pushedActionIsRestart = false;
			LevelInfo.Environments.popupRateApp.SetActive(true);
			LevelInfo.Environments.popupQuit.SetActive(false);
		}
		else
			Application.LoadLevel("mainmenu");
	}
}
