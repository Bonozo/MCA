using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Option")]
public class Options : MonoBehaviour {
	
	#region Options
	public float _volumeMusic = 1f;
	public float volumeMusic{
		get{
			return _volumeMusic;
		}
		set{
			_volumeMusic = value;
			PlayerPrefs.SetFloat("options_volume_music",_volumeMusic);
		}
	}
	
	
	public float _volumeSFX = 1f;
	public float volumeSFX{
		get{
			return _volumeSFX;
		}
		set{
			_volumeSFX = value;
			NGUITools.soundVolume = value;
			PlayerPrefs.SetFloat("options_volume_sfx",_volumeSFX);
		}
	}
	
	
	public bool _vibration = true;
	public bool vibration{
		get{
			return _vibration;
		}
		set{
			_vibration = value;
			PlayerPrefs.SetInt("options_vibration",_vibration?1:0);
		}
	}
	
	public void RestoreOptions()
	{
		volumeMusic = PlayerPrefs.GetFloat("options_volume_music");
		volumeSFX = PlayerPrefs.GetFloat("options_volume_sfx");
		vibration = PlayerPrefs.GetInt("options_vibration")==1;
	}
	
	#endregion
	
	#region Debug Options
	public bool showFPS = false;
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
			if( GUI.Button(buttonRect(1),showFPS?"ON":"OFF" ) )
				showFPS = !showFPS;
			
			
			if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Options"))
				debug = false;
		}
		else
		{
			GUI.Label(textRect(1),"Music");
			volumeMusic = GUI.HorizontalSlider(buttonRect(1),volumeMusic,0f,1f);
	
			GUI.Label(textRect(2),"Sfx");
			volumeSFX = GUI.HorizontalSlider(buttonRect(2),volumeSFX,0f,1f);
			
			GUI.Label(textRect(3),"Vibration");
			if( GUI.Button(buttonRect(3),vibration?"ON":"OFF" ) )
				vibration = !vibration;			

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
