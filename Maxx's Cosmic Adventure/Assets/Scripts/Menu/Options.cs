using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Option")]
public class Options : MonoBehaviour {
	
	#region Options
	public float volumeMusic = 1f;
	public float volumeSFX = 1f;
	public bool Vibration = true;
	#endregion
	
	#region Debug Options
	public static bool ShowFPS = false;
	#endregion
	
	#region GUI
	
	public GameObject gui;
	
	private bool _showOptions = false;
	public bool ShowOptions{
		get{
			return _showOptions;
		}
		set{
			_showOptions = value;
			gui.SetActive(value);
		}
	}
	
	private bool debug;
	
	private Rect textRect(float index)
	{
		Vector2 textSize = new Vector2(Screen.width*0.2f,30);
		
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*50f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		Vector2 textSize = new Vector2(Screen.width*0.2f,30);
		Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);
		
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.0f+textSize.x,index*50f,buttonSize.x,buttonSize.y);
	}
	
	void Awake()
	{
		ShowOptions = false;
	}
	
	void OnGUI()
	{
		if(!ShowOptions) return;
		if(debug)
		{
			GUI.Label(textRect(1),"Display Framerate");
			if( GUI.Button(buttonRect(1),ShowFPS?"ON":"OFF" ) )
				ShowFPS = !ShowFPS;
			
			
			if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Options"))
				debug = false;
		}
		else
		{
			GUI.Label(textRect(1),"Music");
			volumeMusic = GUI.HorizontalSlider(buttonRect(1),volumeMusic,0f,1f);
	
			GUI.Label(textRect(2),"Sfx");
			volumeSFX = GUI.HorizontalSlider(buttonRect(2),volumeSFX,0f,1f);
			NGUITools.soundVolume = volumeSFX;
			
			GUI.Label(textRect(3),"Vibration");
			if( GUI.Button(buttonRect(3),Vibration?"ON":"OFF" ) )
				Vibration = !Vibration;			

			if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Debug"))
				debug = true;
		}
	}
	
	#endregion
	
	#region  Static Instance
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Options _staticInstance;	
    public static Options Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Options)) as Options;
                    if (_staticInstance == null) {
                       Debug.LogError("The Options instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
