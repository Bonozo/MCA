using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Option")]
public class Option : MonoBehaviour {
	
	public static float hSlideVolume = 1f;
	public static float hSlideBackgroundVolume = 0.5f;
	public static float hSlideEffectsVolume = 1f;
	public static bool Vibration = true;
	
	
	
	private bool debug;
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		AudioListener.volume = hSlideVolume;
	}
	
	private Vector2 textSize = new Vector2(Screen.width*0.2f,30);
	private Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);

	private Rect textRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*50f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.0f+textSize.x,index*50f,buttonSize.x,buttonSize.y);
	}
	
	void OnGUI()
	{
		if(debug)
		{
			if( GUI.Button( new Rect(Screen.width-200,Screen.height-60,80,40),"Options"))
				debug = false;
		}
		else
		{
			GUI.Label(textRect(1),"Volume");
			hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
			
			GUI.Label(textRect(2),"Vibration");
			if( GUI.Button(buttonRect(2),Vibration?"ON":"OFF" ) )
				Vibration = !Vibration;
			
			GUI.Label(textRect(3),"Backsound Volume");
			hSlideBackgroundVolume = GUI.HorizontalSlider(buttonRect(3),hSlideBackgroundVolume,0f,1f);
		
			GUI.Label(textRect(4),"Effects Volume");
			hSlideEffectsVolume = GUI.HorizontalSlider(buttonRect(4),hSlideEffectsVolume,0f,1f);			

			if( GUI.Button( new Rect(Screen.width-200,Screen.height-60,80,40),"Debug"))
				debug = true;
		}
	}
}
