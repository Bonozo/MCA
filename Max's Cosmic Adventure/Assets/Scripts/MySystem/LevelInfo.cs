using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {
	
	public GameManager gameManager;
	public static GameManager Environments { get { return Instance.gameManager; }}
	
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
