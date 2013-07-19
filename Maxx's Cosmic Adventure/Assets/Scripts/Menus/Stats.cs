using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	
	public GameObject gui;
	
	private bool _showStats = false;
	public bool ShowStats{
		get{
			return _showStats;
		}
		set{
			_showStats = value;
			gui.SetActive(value);
		}
	}
	
	void Awake()
	{
		ShowStats = false;
	}
	
	#region  Static Instance
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Stats _staticInstance;	
    public static Stats Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Stats)) as Stats;
                    if (_staticInstance == null) {
                       Debug.LogError("The Stats instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
