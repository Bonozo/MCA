using UnityEngine;
using System.Collections;

public class ButtonStartTutorialYes : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.Environments.popupStartTrainings.SetActive(false);	
		
		LevelInfo.Environments.generator.GenerateAlienShip = false;
		LevelInfo.Environments.generator.GenerateAsteroid = false;
		LevelInfo.Environments.generator.GenerateUnlikeliums = false;
		
		LevelInfo.Environments.popupCalibrate.SetActive(true);
	}
}
