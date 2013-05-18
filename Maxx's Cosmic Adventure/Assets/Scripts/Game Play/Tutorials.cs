using UnityEngine;
using System.Collections;

public class Tutorials : MonoBehaviour {
	
	#region Messages
	string messageFirstControls =
		"First Controls\n" +
		"Use device tilting to turn and up/down moves!\n" +
		"Use 'F' button to the left or right to fire!\n" +
		"Use 'B' button to boost!";
	string messageUnlikelium = 
		"Pick up unlikeliums";
	string messageSureShot = 
		"Sure Shot\n" +
		"Automatically targets & destroys the nearest Jeebie.";
	string messageMagnet = 
		"Lay It On Me\n" +
		"Sucks in all nearby Unlikelium.";
	string messageIn3s = 
		"Triple Trouble\n" +
		"Three self-targeting missiles protect by\n" +
		"destroying the next 3 targets.";
	string messageFreezeWorld = 
		"Hold It Now, Hit It\n" +
		"Super slow motion.";
	string messagePow = 
		"Pow\n" +
		"Destroys all obstacles in view with one shot.";
	string messageFireBall = 
		"Lighten Up\n" +
		"Enables the fireball cannon.";
	string messageLoveUnlikelium = 
		"Shazam!\n" +
		"Automatic boost and a bonus Unlikelium trail.";
	string messageIntergalactic = 
		"Intergalactic\n" +
		"Sends you into hyperspace.";
	string messageToughGuy = 
		"Tough Guy\n" +
		"Adds one more shield.";
	string messageAsteroid =
		"Spawned Asteroid";
	string messageJeebie =
		"Spawned Jeebie";
	string messageOverheated = 
		"Button Overheated";
	
	#endregion
	
	public static void ResetTutorials()
	{
		PlayerPrefs.SetInt("tutorials_calibrate",0);// implemented in player.cs
		
		PlayerPrefs.SetInt("tutorials_firstcontrols",0);
		
		PlayerPrefs.SetInt("tutorials_unlikeliums",0);
		PlayerPrefs.SetInt("tutorials_sureshot",0);
		PlayerPrefs.SetInt("tutorials_magned",0);
		PlayerPrefs.SetInt("tutorials_in3s",0);
		PlayerPrefs.SetInt("tutorials_freeze",0);
		PlayerPrefs.SetInt("tutorials_pow",0);
		PlayerPrefs.SetInt("tutorials_fireball",0);
		PlayerPrefs.SetInt("tutorials_loveunlikelium",0);
		PlayerPrefs.SetInt("tutorials_intergalactic",0);
		PlayerPrefs.SetInt("tutorials_toughguy",0);
		
		PlayerPrefs.SetInt("tutorials_jeebie",0);
		PlayerPrefs.SetInt("tutorials_asteroid",0);

		PlayerPrefs.SetInt("tutorials_overheated",0);
	}
	
	void Start()
	{
		ShowTutorialPopup("tutorials_firstcontrols",messageFirstControls,7f);
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
			LevelInfo.Audio.StopEffects();
			LevelInfo.Environments.labelTutorial.text = message;
			LevelInfo.Environments.popupTutorial.SetActive(true);
			PlayerPrefs.SetInt(id,1);
		}
	}
	
	public void SpawnedSimpleUnlikelium()
	{
		ShowTutorialPopup("tutorials_unlikeliums",messageUnlikelium,3f);
	}
	
	public void SpawnedJeebie()
	{
		ShowTutorialPopup("tutorials_jeebie",messageJeebie,2f);
	}
	
	public void SpawnedAsteroid()
	{
		ShowTutorialPopup("tutorials_asteroid",messageAsteroid,2f);
	}
	
	public void TakenGem(Gems gemtype)
	{
		float delay = 1.5f;
		switch(gemtype)
		{
		case Gems.SureShot:
			ShowTutorialPopup("tutorials_sureshot",messageSureShot,delay);
			break;
		case Gems.Magnet:
			ShowTutorialPopup("tutorials_magned",messageMagnet,delay);
			break;
		case Gems.In3s:
			ShowTutorialPopup("tutorials_in3s",messageIn3s,delay);
			break;
		case Gems.FreezeWorld:
			ShowTutorialPopup("tutorials_freeze",messageFreezeWorld,delay);
			break;
		case Gems.Pow:
			ShowTutorialPopup("tutorials_pow",messagePow,delay);
			break;
		case Gems.FireBall:
			ShowTutorialPopup("tutorials_fireball",messageFireBall,delay);
			break;
		case Gems.LoveUnlikelium:
			ShowTutorialPopup("tutorials_loveunlikelium",messageLoveUnlikelium,delay);
			break;
		case Gems.Intergalactic:
			ShowTutorialPopup("tutorials_intergalactic",messageIntergalactic,delay);
			break;
		case Gems.ToughGuy:
			ShowTutorialPopup("tutorials_toughguy",messageToughGuy,delay);
			break;
		}		
	}
	
	bool overheated = false;
	public void ButtonOverheated()
	{
		if(!overheated)
		{
			overheated = true;
			ShowTutorialPopup("tutorials_overheated",messageOverheated,0.05f);
		}
	}
	
}
