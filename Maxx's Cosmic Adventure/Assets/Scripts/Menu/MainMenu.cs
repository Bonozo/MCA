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
	
	public GameObject objTitle,objOptions,objCredits,objStats,objStore;
	
	private MenuState _state;
	public MenuState State{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			
			objTitle.SetActive(_state == MenuState.Title);
			objOptions.SetActive(_state == MenuState.Options);
			objCredits.SetActive(_state == MenuState.Credits);
			objStats.SetActive(_state == MenuState.Stats);
			objStore.SetActive(_state == MenuState.Store);
			
		}
	}
	
	#endregion
	
	// Use this for initialization
	void Awake ()
	{
		State = MenuState.Title;
	}
	
	float exittime=0.5f;
	void Update()
	{
		if(_state == MenuState.Title && Input.GetKey(KeyCode.Escape) )
		{
			exittime-=Time.deltaTime;
			if(exittime<=0f) Application.Quit();
		}
		else
			exittime = 0.5f;
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
