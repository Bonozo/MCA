using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Option")]
public class Options : MonoBehaviour {
	
	public OptionsSlider _volumeMusic;
	public float volumeMusic{ get{ return _volumeMusic.value; }}
	
	public OptionsSlider _volumeSFX;
	public float volumeSFX{ get{ return _volumeSFX.value; }}
	
	
	public OptionsItem _vibration;
	public bool vibration { get{ return _vibration.isEnabled; }}
	
	public OptionsItem _yInvert;
	public bool yInvert { get{ return _yInvert.isEnabled; }}
	
	public OptionsItem _flightControls3D;
	public bool flightControls3D { get{ return _flightControls3D.isEnabled; }}
	
	public OptionsItem _cameraRotation;
	public bool cameraRotation { get{ return _cameraRotation.isEnabled; }}
	
	public OptionsItem _peyton;
	public bool peyton { get{ return _peyton.isEnabled; }}
	
	public OptionsItem _showFPS;
	public bool showFPS { get{ return _showFPS.isEnabled; }}
	
	#region GUI
	
	public GameObject gui;
	public GameObject standardGUI;
	public GameObject debugGUI;
	public AudioSource audioSource;
	
	private bool _showOptions = false;
	public bool ShowOptions{
		get{
			return _showOptions;
		}
		set{
			_showOptions = value;
			gui.SetActive(value);
			debug = false;
			if(_showOptions)
			{
				audioSource.volume = Options.Instance.volumeMusic;
				audioSource.Play();
			}
			else
			{
				audioSource.Stop();
			}
		}
	}
	
	private bool _debug = false;
	public bool debug{
		get{
			return _debug;
		}
		set{
			_debug = value;
			standardGUI.SetActive(!value);
			debugGUI.SetActive(value);
		}
	}
	
	void Awake()
	{
		ShowOptions = false;
		
		_volumeMusic.Init();
		_volumeSFX.Init();
		_vibration.Init();
		_yInvert.Init();
		_flightControls3D.Init();
		_cameraRotation.Init();
		_peyton.Init();
		_showFPS.Init();

	}
	
	void Update()
	{
		NGUITools.soundVolume = volumeSFX;
		audioSource.volume = volumeMusic;
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
