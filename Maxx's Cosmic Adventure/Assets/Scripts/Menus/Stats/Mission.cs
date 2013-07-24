using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour
{
	public string missionNumber;
	public string missionDescription;
	public int missionCount;
	public bool inSigleRun;
	
	public UILabel labelDescription;
	
	private int _complete = -1;
	private int complete{
		get{
			if(_complete == -1)
				_complete = PlayerPrefs.GetInt("stats_mission" + missionNumber,0);
			return _complete;
		}
		set{
			bool completed = IsComplete;
			_complete = value;
			if(!completed&&IsComplete) Stats.Instance.ShowMissionComplete(missionDescription);
			PlayerPrefs.SetInt("stats_mission" + missionNumber,_complete);
		}
	}
	
	public bool IsComplete{
		get{
			return complete == missionCount;
		}
	}
	
	public void CompleteDirectly()
	{
		complete = missionCount;
	}
	
	public void Add(int count)
	{
		complete = Mathf.Min(missionCount,complete+count);
	}
	
	public void ClearIfNotCompleteForSingleRun()
	{
		if(inSigleRun&&!IsComplete) complete=0;
	}
	
	void OnEnable()
	{
		labelDescription.text = missionDescription;
		if(missionCount>1&&!IsComplete)
			labelDescription.text += " (" + (missionCount-complete) + " left)";
	}
}