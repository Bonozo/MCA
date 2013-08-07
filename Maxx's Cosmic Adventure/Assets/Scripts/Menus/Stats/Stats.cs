using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	
	public GameObject gui;
	public GameObject missionCompleteGUI;
	public UILabel missionCompleteMessage;
	public UILabel labelLevel;
	public GameObject missionsRoot;
	
	#region Missions Implementation
	
	public Mission[] missions;
	private int[] _current = new int[3] {-1,-1,-1};
	public int CurrentMissionIndex(int index)
	{
		if(index<0||index>2)
		{
			Debug.Log("CurrentMission: index is out of range");
			return -1;
		}
		if(_current[index]==-1)
			_current[index] = PlayerPrefs.GetInt("missions_current_" + index,index);
		return _current[index];
	}
	
	public Mission CurrentMission(int index)
	{
		return missions[CurrentMissionIndex(index)];
	}
	
	public void ActivateCurrentMissions()
	{
		if(CurrentMissionIndex(0)<missions.Length) CurrentMission(0).isActive = true;
		if(CurrentMissionIndex(1)<missions.Length) CurrentMission(1).isActive = true;
		if(CurrentMissionIndex(2)<missions.Length) CurrentMission(2).isActive = true;		
	}
	
	public void MissionComplete(int index)
	{
		if(index<=0) PlayerPrefs.SetInt("missions_current_0",CurrentMissionIndex(1));
		if(index<=1) PlayerPrefs.SetInt("missions_current_1",CurrentMissionIndex(2));
		PlayerPrefs.SetInt("missions_current_2",CurrentMissionIndex(2)+1);
		_current[0] = _current[1] = _current[2] = -1;
		
		// Active new mission
		if(CurrentMissionIndex(2)<missions.Length) CurrentMission(2).isActive = true;
	}
	
	private void ShowMission(int index,float y)
	{
		if(index<missions.Length)
		{
			missions[index].transform.localPosition = new Vector3(0f,y,0f);
			missions[index].gameObject.SetActive(true);
		}
	}
	
	public void ShowCurrentMissions()
	{
		foreach(var mission in missions) mission.gameObject.SetActive(false);
		ShowMission(CurrentMissionIndex(0),100f);
		ShowMission(CurrentMissionIndex(1),0f);
		ShowMission(CurrentMissionIndex(2),-100f);
	}
	
	public bool Completed(int index)
	{
		if(CurrentMissionIndex(index)>=missions.Length) return false;
		return CurrentMission(index).IsComplete;
	}
	
	#endregion
	
	/*private int _level = -1;
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
	}*/
	
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
				ShowCurrentMissions();
			}
		}
	}
	
	void Awake()
	{
		ActivateCurrentMissions();
		ShowStats = false;
	}
	
	public void ClearInSingleRunMissions()
	{
		foreach(Transform m in missionsRoot.transform)
			if(m.GetComponent<Mission>() != null)
				m.GetComponent<Mission>().ClearIfNotCompleteForSingleRun();
	}
	
	#region Missions
	
	public Mission missionCollect100UnlikeliumsTotal;
	public Mission missionDestroy25AsteroidsTotal;
	public Mission missionKill25JeebiesTotal;
	public Mission missionPickUp10PowerupsTotal;
	public Mission missionPurchaseSomethingInTheStore;
	public Mission missionUsePow3Times;
	public Mission missionKill5BlueFightersInASingleRun;
	public Mission missionDestroy10AsteroidsInASingleRun;
	public Mission missionKillAJeebieDuringHoldItNowHitIt;
	public Mission missionEnterIntergalacticHyperspace3Times;
	public Mission missionKill10YellowLeaderJeebies;
	public Mission missionCollect50UnlikeliumsInASingleRun;
	public Mission missionHaveANearMeetWith5AsteroidsInASingleRun;
	public Mission missionKill10JeebiesWithALightenUpFireball;
	
	#endregion
	
	#region Reports
	
	public void ReportCollectedUnlikelium(int count)
	{
		missionCollect100UnlikeliumsTotal.Add(count);
		missionCollect50UnlikeliumsInASingleRun.Add(count);
	}
	
	public void ReportDestroyedAsteroid(Asteroids asteroidType)
	{
		missionDestroy25AsteroidsTotal.Add(1);
		missionDestroy10AsteroidsInASingleRun.Add(1);
		
		switch(asteroidType)
		{
		case Asteroids.Blue:
			break;
		case Asteroids.White:
			break;
		case Asteroids.Green:
			break;
		case Asteroids.Red:
			break;
		case Asteroids.Yellow:
			break;
		case Asteroids.Purple:
			break;	
		default:
			Debug.LogError("ReportDestroyedAsteroid: the asteroid type is not implemented");
			break;
		}
	}
	
	public void ReportKilledJeebie(Jeebie jeebie)
	{
		missionKill25JeebiesTotal.Add(1);
		
		switch(jeebie)
		{
		case Jeebie.BlueFighterPilot:
			missionKill5BlueFightersInASingleRun.Add(1);
			break;
		case Jeebie.BlueLeader:
			missionKill10YellowLeaderJeebies.Add(1);
			break;
		case Jeebie.RedKamikaze:
			break;
		case Jeebie.PurpleFigher:
			break;	
		case Jeebie.Reactive:
			break;
		default:
			Debug.LogError("ReportKilledJeebie: the jeebie type is not implemented");
			break;
		}
	}
	
	public void ReportKilledJeebieWithAWeapon(Gems gem)
	{
		switch(gem)
		{
		case Gems.FireBall:
			missionKill10JeebiesWithALightenUpFireball.Add(1);
			break;
		}
	}
	
	public void ReportCollectedPowerup(Gems gem)
	{
		missionPickUp10PowerupsTotal.Add(1);
		
		switch(gem)
		{
		case Gems.Intergalactic:
			missionEnterIntergalacticHyperspace3Times.Add(1);
			break;
		}
	}
	
	public void ReportPurchasedStoreItem(UpdateablePowerup item)
	{
		missionPurchaseSomethingInTheStore.Add(1);
	}
	
	public void ReportUsePowerup(Gems gem)
	{
		switch(gem)
		{
		case Gems.Pow:
			missionUsePow3Times.Add(1);
			break;
		}
	}
	
	public void ReportNearMeetWithAsteroid(Asteroids asteroid)
	{
		missionHaveANearMeetWith5AsteroidsInASingleRun.Add(1);
	}
	
	public void ReportKilledJeebieInHoldItNowHitIt(Jeebie jeebie)
	{
		missionKillAJeebieDuringHoldItNowHitIt.Add(1);
	}
	
	#endregion
	
	#region Mission Complete
	
	private bool showingmissioncomplete = false;
	
	public void ShowMissionComplete(string message)
	{
		StartCoroutine(ShowMissionCompleteThread(message));
	}
	
	private IEnumerator ShowMissionCompleteThread(string message)
	{
		while(showingmissioncomplete) yield return new WaitForEndOfFrame();
		showingmissioncomplete = true;
		
		missionCompleteGUI.transform.localPosition = new Vector3(0f,-80f,0f);
		missionCompleteMessage.text = message;
		
		float time = 1.0f;
		while(time>0f)
		{
			time -= 0.067f;
			missionCompleteGUI.transform.Translate(0f,0.067f*0.2f,0f);
			yield return new WaitForEndOfFrame();
		}
		
		time = 10f;
		while(time>0)
		{
			time -= 0.067f;
			yield return new WaitForEndOfFrame();
		}
		
		time = 1.0f;
		while(time>0f)
		{
			time -= 0.067f;
			missionCompleteGUI.transform.Translate(0f,-0.067f*0.2f,0f);
			yield return new WaitForEndOfFrame();
		}		
		
		showingmissioncomplete = false;
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
