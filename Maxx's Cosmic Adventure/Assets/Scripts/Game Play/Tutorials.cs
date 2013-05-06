using UnityEngine;
using System.Collections;

public class Tutorials : MonoBehaviour {
	
	string messageUnlikelium = "Pick up unlikeliums";
	
	
	public void ResetTutorials()
	{
		PlayerPrefs.SetInt("tutorials_unlikeliums",0);
	}
	
	private void ShowTutorialPopup(string id,string message,float delay)
	{
		StartCoroutine(ShowTutorialPopupThread(id,message,delay));
	}
	
	private IEnumerator ShowTutorialPopupThread(string id,string message,float delay)
	{
		yield return new WaitForSeconds(delay);
		if( PlayerPrefs.GetInt(id,0)==0 )
		{
			Time.timeScale = 0f;
			LevelInfo.Environments.playerShip.Ready = false;
			LevelInfo.Environments.labelTutorial.text = message;
			LevelInfo.Environments.popupTutorial.SetActive(true);
			PlayerPrefs.SetInt(id,1);
		}
	}
	
	public void SpawnedGem(Gems gemtype)
	{
		switch(gemtype)
		{
		case Gems.Unlikelium:
			ShowTutorialPopup("tutorials_unlikeliums",messageUnlikelium,3f);
			break;
		}
	}
	
}
