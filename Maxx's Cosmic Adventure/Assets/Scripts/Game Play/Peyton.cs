using UnityEngine;
using System.Collections;

public class Peyton : MonoBehaviour {
	
	public GameObject guiRoot;
	public UIWidget[] gui;
	public UILabel label;
	public float framerate=0.03f;
	public UISprite spriteScreen;
	public float screenAnimFramerate=0.05f;
	
	private string defmessage = @"Maxx, grab all the Unlikelium you can. Found the unlock codes for lots of new weapons, but you'll need Unlikelium to activate them!";
	private int index=0;
	private float screenAnimFrames=3;
	private float screenAnimTime=0f;
	
	void Awake()
	{
		Active = false;
		foreach(var g in gui) g.color = new Color(1f,1f,1f,0f);
	}
	
	IEnumerator Start ()
	{
		yield return new WaitForSeconds(10f);
		StartCoroutine(ShowMessageThread(defmessage));
	}

	void Update ()
	{	
		// Update screen
		if(Active)
		{
			screenAnimTime += Time.deltaTime;
			screenAnimTime %= (screenAnimFrames*screenAnimFramerate);
			int frame = (int)(screenAnimTime/screenAnimFramerate)+1;
			spriteScreen.spriteName = "Peyton_Icon_Static_0" + frame;
		}
		
		guiRoot.SetActive(Active&&LevelInfo.Environments.playerShip.Ready&&LevelInfo.State.state == GameState.Play);
	}
	
	public bool Active{get;private set;}
	
	public void ShowMessage(string str)
	{
		if(!Active)
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
			
			float alpha = 0f;
			while(alpha<1f)
			{
				alpha = Mathf.Clamp01(alpha+Time.deltaTime);
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
				alpha = Mathf.Clamp01(alpha-2*Time.deltaTime);
				foreach(var g in gui) g.color = new Color(1f,1f,1f,alpha);
				yield return null;
			}
			Active = false;
		}
	}
}