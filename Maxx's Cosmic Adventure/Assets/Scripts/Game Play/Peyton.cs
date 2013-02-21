using UnityEngine;
using System.Collections;

public class Peyton : MonoBehaviour {
	
	public UIWidget[] gui;
	
	public UILabel label;
	public float framerate=0.03f;
	public string message = "Maxx, grab all the Unlikelium you can.\n"+
							"Found the unlock codes for lots of new \n"+
							"weapons, but you'll need Unlikelium to activate them!";
	
	private int index=0;
	
	void Awake()
	{
		foreach(var g in gui) g.color = new Color(1f,1f,1f,0f);
	}
	
	void Start ()
	{
		StartCoroutine(ShowMessage());
	}
	
	private IEnumerator ShowMessage()
	{
		yield return new WaitForSeconds(10f);
		
		label.text = "";
		
		float alpha = 0f;
		while(alpha<1f)
		{
			alpha = Mathf.Clamp01(alpha+Time.deltaTime);
			foreach(var g in gui) g.color = new Color(1f,1f,1f,alpha);
			yield return null;
		}
		
		while(index<message.Length)
		{
			label.text += message[index++];
			yield return new WaitForSeconds(framerate);
		}
		yield return new WaitForSeconds(3f);
		
		while(alpha>0f)
		{
			alpha = Mathf.Clamp01(alpha-2*Time.deltaTime);
			foreach(var g in gui) g.color = new Color(1f,1f,1f,alpha);
			yield return null;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateScreen();
	}
	
	public UISprite spriteScreen;
	public float screenAnimFramerate=0.05f;
	
	private float screenAnimFrames=3;
	private float screenAnimTime=0f;
	
	void UpdateScreen()
	{
		screenAnimTime += Time.deltaTime;
		screenAnimTime %= (screenAnimFrames*screenAnimFramerate);
		int frame = (int)(screenAnimTime/screenAnimFramerate)+1;
		spriteScreen.spriteName = "Peyton_Icon_Static_0" + frame;
	}
}
