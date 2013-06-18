using UnityEngine;
using System.Collections;

public class ButtonStartTutorialNo : MonoBehaviour {

	void OnClick()
	{
		int notcount = PlayerPrefs.GetInt("tutorials",0);
		if(++notcount==3) notcount = -1;
		PlayerPrefs.SetInt("tutorials",notcount);
		
		LevelInfo.Environments.popupStartTrainings.SetActive(false);
		LevelInfo.Environments.playerShip.GetReady();
	}
}
