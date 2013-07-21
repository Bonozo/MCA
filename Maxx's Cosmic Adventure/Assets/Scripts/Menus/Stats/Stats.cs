using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	
	public GameObject gui;
	public UILabel labelLevel;
	
	private int _level = -1;
	public int level{
		get{
			if(_level==-1)
				_level = PlayerPrefs.GetInt("stats_level",1);
			return _level;
		}
		private set
		{
			_level = value;
			PlayerPrefs.SetInt("stats_level",_level);
		}
	}
	
	private bool _showStats = false;
	public bool ShowStats{
		get{
			return _showStats;
		}
		set{
			_showStats = value;
			gui.SetActive(value);
			
			if(_showStats)
			{
				labelLevel.text = "LEVEL " + level;
			}
		}
	}
	
	void Awake()
	{
		ShowStats = false;
	}
	
	#region Missions
	
	public Mission missionCollectUnlikeliums;
	public Mission missionKillBlueFighterPilots;
	public Mission missionKillYellowLeaders;
	public Mission missionKillRedKamikazes;
	public Mission missionKillPurpleFighters;
	public Mission missionKillReactiveFighter;
	
	public void ReportCollectedUnlikelium(int count)
	{
		missionCollectUnlikeliums.Add(count);
	}
	
	public void ReportKilledJeebie(Jeebie jeebie)
	{
		switch(jeebie)
		{
		case Jeebie.BlueFighterPilot:
			missionKillBlueFighterPilots.Add(1);
			break;
		case Jeebie.BlueLeader:
			missionKillYellowLeaders.Add(1);
			break;
		case Jeebie.RedKamikaze:
			missionKillRedKamikazes.Add(1);
			break;
		case Jeebie.PurpleFigher:
			missionKillPurpleFighters.Add(1);
			break;	
		case Jeebie.Reactive:
			missionKillReactiveFighter.Add(1);
			break;
		default:
			Debug.LogError("ReportKilledJeebie: the jeebie type is not implemented");
			break;
		}
	}
	
	#endregion
	
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
