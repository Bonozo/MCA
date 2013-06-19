using UnityEngine;
using System.Collections;

public class ButtonStartTutorialYes : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.Environments.popupStartTrainings.SetActive(false);	
		
		LevelInfo.Environments.generator.GenerateAlienShip = false;
		LevelInfo.Environments.generator.GenerateAsteroid = false;
		LevelInfo.Environments.generator.GenerateUnlikeliums = false;
		
		LevelInfo.Settings.tutorialMode = true;
		LevelInfo.Environments.guiDistanceTravelled.gameObject.transform.localScale = new Vector3(35f,50f,1f);
		LevelInfo.Environments.guiDistanceTravelled.text = "TUTORIALS";
		
		LevelInfo.Environments.popupCalibrate.SetActive(true);
	}
}
