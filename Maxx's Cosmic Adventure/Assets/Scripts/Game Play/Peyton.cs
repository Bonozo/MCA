using UnityEngine;
using System.Collections;

public class Peyton : MonoBehaviour {
	
	private string[] messagesFirst = new string[]
	{
		@"Maxx, To control the ship tilt the device left and right to turn. Use up/down tilting to change ship's height. Use calibrate button left to pause to reset ship's height. ",
		@"Press left or right 'F' button to fire. You can increase firing overheat from the store. Go store from the pause menu or from the main menu.",
		@"Use 'B' button to the right below to boost. You can increase firing overheat from the store.",
		@"Maxx, grab all the Unlikelium you can. Found the unlock codes for lots of new weapons, but you'll need Unlikelium to activate them!",
		@"Destroy as much asteroids as you can. They spawn powerups or high value unlikeliums.",
		@"Get Jeebies when they spawn while you have not hurt by them.",
		@"Upgrade powerups or buy new from the unlikelium store. With powerups you can go farther.",
		@"Swipe left, right or up to animate the ship. Swipe down to decrease the speed of the ship.",
		@"There are both timed and stored powerups. Timed powerup timer is shown the bottom of the screen. If you have a stored powerup it is shown below 'M' button and you can use it by pressing 'M' button."
	};
	private string[] messagesTip = new string[]
	{
		@"Maxx, head to Mars! That’s where the Jeebies have Commander Yauch!",
		@"When you grab a power up, use it quickly before you lose it!",
		@"Everything’s better In 3’s!  Hit the 3’s button when you activate a new weapon.",
		@"Lighten Up gives you Fireballs!",
		@"POW gets everything in one shot!",
		@"Open up P.J.’s weapons console to check out what I’ve unlocked.",
		@"Swipe left, right or up to animate the ship. In this way you can avoid enemies and have a fun if you are a good player."
	};
	
	public GameObject guiRoot;
	public UIWidget[] gui;
	public UILabel label;
	public float framerate=0.03f;
	public UISprite spriteScreen;
	public float screenAnimFramerate=0.05f;
	
	private float screenAnimFrames=3;
	private float screenAnimTime=0f;
	
	void Awake()
	{
		Active = false;
		foreach(var g in gui) g.color = new Color(1f,1f,1f,0f);
	}
	
	IEnumerator Start ()
	{
		var attemp = LevelInfo.Environments.playerShip.allAttempt;
		yield return new WaitForSeconds(Random.Range(5f,8f));
		if(attemp==1)
		{
			// do nothing
		}
		else if(attemp < messagesFirst.Length+2)
		{
			if( Random.Range(0,1+attemp/30)==0)
				ShowMessage(messagesFirst[attemp-2]);
		}
		else
		{
			ShowMessage(messagesTip[Random.Range(0,messagesTip.Length)]);
		}
	}

	void Update ()
	{	
		// Update screen
		if(Active)
		{
			screenAnimTime += 0.016f;
			screenAnimTime %= (screenAnimFrames*screenAnimFramerate);
			int frame = (int)(screenAnimTime/screenAnimFramerate)+1;
			spriteScreen.spriteName = "Peyton_Icon_Static_0" + frame;
		}
		
		guiRoot.SetActive(Active&&LevelInfo.Environments.playerShip.Ready&&LevelInfo.State.state == GameState.Play);
	}
	
	public bool Active{get;private set;}
	
	public void ShowMessage(string str)
	{
		if(!Active && Options.Instance.peyton)
		{
			StartCoroutine(ShowMessageThread(str));
		}
	}
	
	private IEnumerator ShowMessageThread(string message)
	{
		if(!Active)
		{
			Active = true;
			yield return new WaitForSeconds(1f);
			
			label.text = "";
			string[] s = new string[4] {"","","",""};
			
			int index=0;
			float alpha = 0f;
			while(alpha<1f)
			{
				alpha = Mathf.Clamp01(alpha+0.016f);
				foreach(var g in gui) g.color = new Color(1f,1f,1f,alpha);
				yield return null;
			}
			
			while(index<message.Length)
			{
				if(s[0].Length>20&&index>0&&(message[index-1]==' '||message[index-1]=='\n'))
				{
					s[3] = s[2];
					s[2] = s[1];
					s[1] = s[0];
					s[0] = "\n";
					label.text = s[3] + s[2] + s[1] + s[0];
				}
				
				s[0] += message[index];
				label.text += message[index];
				
				if(message[index]==',') yield return new WaitForSeconds(0.1f);
				if(message[index]=='.'||message[index]=='!') yield return new WaitForSeconds(0.2f);
				index++;
				yield return new WaitForSeconds(framerate);
				
			}
			yield return new WaitForSeconds(2f);
			
			while(alpha>0f)
			{
				alpha = Mathf.Clamp01(alpha-2*0.016f);
				foreach(var g in gui) g.color = new Color(1f,1f,1f,alpha);
				yield return null;
			}
			Active = false;
		}
	}
}