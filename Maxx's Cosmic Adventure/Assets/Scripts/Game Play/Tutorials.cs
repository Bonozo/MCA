using UnityEngine;
using System.Collections;

public class Tutorials : MonoBehaviour {
	
	#region Messages
	/*string messageFirstControls =
		"Use device tilting to turn and up/down moves!\n" +
		"Use 'F' button to the left or right to fire!\n" +
		"Use 'B' button to boost!";
	string messageSecondControls = 
		"Swipe Up for Loop!\n" +
		"Swipe Left or Right for Barrel Roll!\n" +
		"Swipe and Hold Down for Brakes!";
	string messageUnlikelium = 
		"Pick up unlikeliums to buy or upgrade\n" +
		"powerups in the store.";*/
	string messageSureShot = 
		"Sure Shot\n" +
		"Automatically targets & destroys the\n" +
		"nearest Jeebie.";
	string messageMagnet = 
		"Lay It On Me\n" +
		"Sucks in all nearby Unlikelium.";
	string messageIn3s = 
		"Triple Trouble\n" +
		"Three self-targeting missiles protect by\n" +
		"destroying the next 3 targets.\n" +
		"Push 'M' button in the bottom left to activate it.";
	string messageFreezeWorld = 
		"Hold It Now, Hit It\n" +
		"Super slow motion.";
	string messagePow = 
		"Pow\n" +
		"Destroys all obstacles in view with one shot.\n" +
		"Push 'M' button in the bottom left to activate it.";
	string messageFireBall = 
		"Lighten Up\n" +
		"Enables the fireball cannon.\n" +
		"Use 'M' button in the bottom left to fire.";
	string messageLoveUnlikelium = 
		"Shazam!\n" +
		"Automatic boost and a bonus Unlikelium trail.";
	string messageIntergalactic = 
		"Intergalactic\n" +
		"Sends you into hyperspace.";
	string messageToughGuy = 
		"Tough Guy\n" +
		"Adds one more shield.";
	string messageBeastieBoost = 
		"Beastie Boost!\n" +
		"Double the player's speed.";
	string messageBoostOverheated = 
		"The boost motor of the ship has been overheated.\n" +
		"It is shown in the bottom right of the screen.";
	string messageFireOverheated = 
		"Firing overheated.\n" +
		"It is shown in the bottom left of the screen.";
	
	#endregion
	
	#region Trainings
	
	public void StartTrainings()
	{
		StartCoroutine(TrainingThread());
	}
	
	private IEnumerator TrainingThread()
	{	
		yield return new WaitForSeconds(5f);
		
		ShowTraining("Use device left and right tilting\n to turn.",5f);
		yield return new WaitForSeconds(10f);
		
		ShowTraining("Use device up and down tilting\n to rise and down.",5f);
		yield return new WaitForSeconds(10f);
		
		ShowTraining("Use 'F' button to the left or right to fire.",5f);
		yield return new WaitForSeconds(10f);
		
		ShowTraining("Use 'B' button in the bottom right to boost.",5f);
		yield return new WaitForSeconds(10f);
		
		ShowTraining("Swipe and Hold Down to slow the speed of the ship down.",5f);
		yield return new WaitForSeconds(10f);
		
		LevelInfo.Environments.generator.SpawnTutorialUnlikeliumList();
		yield return new WaitForSeconds(1f);
		ShowTraining("An unlikelium list appeared.\n Pick them up and you can update\nor buy new items in the store.",6f);
		yield return new WaitForSeconds(12f);		
		
		LevelInfo.Environments.generator.SpawnTutorialAsteroid();
		yield return new WaitForSeconds(1f);
		ShowTraining("An asteroid appeard.\nDestroy this and you can find a powerup there.",6f);
		yield return new WaitForSeconds(12f);
		
		LevelInfo.Environments.generator.SpawnTutorialJeebie();
		yield return new WaitForSeconds(1f);
		ShowTraining("Warning! A Blue Fighter Jeebie appeared.\nDestroy this and you can find a high value unlikelium.",6f);
		yield return new WaitForSeconds(12f);
		
		ShowTraining("Avoid to collide with asteroids and jeebies.\nJeebies can attack to you.",5f);
		yield return new WaitForSeconds(10f);		
	
		ShowTraining("There are two types of powerups.\nbutton powerup: you can use this by pressing\n'M' button in the bottom left...",6f);
		yield return new WaitForSeconds(6.1f);	
		ShowTraining("or timed powerup that takes for a certain time.",5f);
		yield return new WaitForSeconds(10f);	
		
		ShowTraining("You can run tutorials again\nby going to options and pressing 'Reset Tutorial' button\nHave a good flight and enjoy it!",7f);
		yield return new WaitForSeconds(10f);	
		
		yield return new WaitForSeconds(10f);
		ShowTraining("Tutorials Complete.\nThe game will start after few seconds...",6f);
		PlayerPrefs.SetInt("tutorials",-1);
		yield return new WaitForSeconds(10f);
		Application.LoadLevel(Application.loadedLevel);
	}
	
	#endregion
	
	public static void ResetTutorials()
	{
		PlayerPrefs.SetInt("tutorials",0);
		
		PlayerPrefs.SetInt("tutorials_calibrate",0);// implemented in player.cs
		
		PlayerPrefs.SetInt("tutorials_firstcontrols",0);
		PlayerPrefs.SetInt("tutorials_secondcontrols",0);
		
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
		PlayerPrefs.SetInt("tutorials_beastieboost",0);
		
		PlayerPrefs.SetInt("tutorials_jeebie",0);
		PlayerPrefs.SetInt("tutorials_asteroid",0);

		PlayerPrefs.SetInt("tutorials_boostoverheated",0);
		PlayerPrefs.SetInt("tutorials_fireoverheated",0);
	}
	
	void Start()
	{
		//ShowTutorialPopup("tutorials_firstcontrols",messageFirstControls,7f);
		//ShowTutorialPopup("tutorials_secondcontrols",messageSecondControls,7.5f);
	}
	
	public void ShowTraining(string message,float wait)
	{
		StartCoroutine(ShowTutorialPopupThread("",message,0f,wait,false));
	}
	
	public void ShowTutorialPopup(string id,string message,float delay)
	{
		if(LevelInfo.Settings.tutorialMode) return;
		StartCoroutine(ShowTutorialPopupThread(id,message,delay,6f,true));
	}
	
	bool popupactive = false;
	private IEnumerator ShowTutorialPopupThread(string id,string message,float delay,float wait,bool savedata)
	{	
		yield return new WaitForSeconds(delay);
		if(!popupactive)
		{
			popupactive = true;
			
			if(!savedata || PlayerPrefs.GetInt(id,0)==0 )
			{
				LevelInfo.Environments.labelTutorial.text = message;
			
				var sc = LevelInfo.Environments.backgroundTutorial.transform.localScale;
				int maxlinelenght=0;
				int lines=0;
				lines = PopupSizeParamsByMessage(ref message,ref maxlinelenght);
				
				sc.x = 17*(maxlinelenght+5);
				sc.y = 60f*(lines+1);
				
				LevelInfo.Environments.backgroundTutorial.transform.localScale = sc;
				
				LevelInfo.Environments.popupTutorial.SetActive(true);
				yield return new WaitForSeconds(wait);
				LevelInfo.Environments.popupTutorial.SetActive(false);
				
				if(savedata)
					PlayerPrefs.SetInt(id,1);
				yield return new WaitForSeconds(1f);
			}
			
			popupactive = false;
		}
	}
	
	private int PopupSizeParamsByMessage(ref string s,ref int maxlinelenght) //:)
	{
		maxlinelenght=0;
		int n=0,len=0;
		for(int i=0;i<s.Length;i++)
		{
			len++;
			if(s[i]=='\n')
			{
				n++;
				maxlinelenght = Mathf.Max(maxlinelenght,len);
				len=0;
			}
		}
		maxlinelenght = Mathf.Max(maxlinelenght,len);
		return n;
	}
	
	public void SpawnedSimpleUnlikelium()
	{
		//ShowTutorialPopup("tutorials_unlikeliums",messageUnlikelium,3f);
	}
	
	public void SpawnedJeebie()
	{
		//ShowTutorialPopup("tutorials_jeebie",messageJeebie,2f);
	}
	
	public void SpawnedAsteroid()
	{
		//ShowTutorialPopup("tutorials_asteroid",messageAsteroid,2f);
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
		case Gems.BeastieBoost:
			ShowTutorialPopup("tutorials_beastieboost",messageBeastieBoost,delay);
			break;			
		}		
	}
	
	bool boostoverheated = false;
	public void BoostOverheated()
	{
		if(!boostoverheated&&!popupactive)
		{
			boostoverheated = true;
			ShowTutorialPopup("tutorials_boostoverheated",messageBoostOverheated,0.05f);
		}
	}
	
	bool fireoverheated = false;
	public void FireOverheated()
	{
		if(!fireoverheated&&!popupactive)
		{
			fireoverheated = true;
			ShowTutorialPopup("tutorials_fireoverheated",messageFireOverheated,0.05f);
		}
	}
	
	public bool Active{ get { return popupactive; }}
}
