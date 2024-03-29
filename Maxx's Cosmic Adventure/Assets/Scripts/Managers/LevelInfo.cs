using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {
	
	public GameManager _gameManager;
	public static GameManager Environments { get { return Instance._gameManager; }}
	
	public AudioManager _audioManager;
	public static AudioManager Audio { get { return Instance._audioManager; }}
	
	public StateManager _stateManager;
	public static StateManager State { get { return Instance._stateManager; }}
	
	public LevelManager _levelManager;
	public static LevelManager Settings { get { return Instance._levelManager; }}
	
	// Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile LevelInfo _staticInstance;	
    public static LevelInfo Instance 
	{
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(LevelInfo)) as LevelInfo;
                    if (_staticInstance == null) {
                       Debug.LogError("The LevelInfo instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
}
