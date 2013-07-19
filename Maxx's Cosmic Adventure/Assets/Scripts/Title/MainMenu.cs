using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Main Menu")]
public class MainMenu : MonoBehaviour {
	
	#region State Enum
	
	public enum MenuState
	{
		Title,
		Options,
		Store,
		Credits,
		Stats
	}
	
	public AudioClip clipMenuLoop;
	public GameObject objTitle,objCredits,objStats;
	
	private MenuState _state;
	public MenuState State{
		get
		{
			return _state;
		}
		set
		{
			if( (_state == MenuState.Store || _state == MenuState.Options) && value != MenuState.Store && value != MenuState.Options && !audio.isPlaying)
				audio.Play();
			
			_state = value;
			
			objTitle.SetActive(_state == MenuState.Title);
			objCredits.SetActive(_state == MenuState.Credits);
			objStats.SetActive(_state == MenuState.Stats);
			
			switch(_state)
			{
			case MenuState.Store:
				Store.Instance.ShowStore = true;
				audio.Stop();
				break;
			case MenuState.Options:
				Options.Instance.ShowOptions = true;
				audio.Stop();
				break;
			case MenuState.Stats:
				Stats.Instance.ShowStats = true;
				break;
			}
		}
	}
	
	#endregion
	
	// Use this for initialization
	void Awake ()
	{
		Time.timeScale = 1.0f;
		Store.Instance.ShowStore = false;
		State = MenuState.Title;
	}
	
	void Update()
	{
		if(_state == MenuState.Title && Input.GetKeyDown(KeyCode.Escape) )
			Application.Quit();
		
		audio.volume = Options.Instance.volumeMusic;
	}
	
	// Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile MainMenu _staticInstance;	
    public static MainMenu Instance 
	{
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(MainMenu)) as MainMenu;
                    if (_staticInstance == null) {
                       Debug.LogError("The MainMenu instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
}
