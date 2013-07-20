using UnityEngine;
using System.Collections;

public class Mission : MonoBehaviour
{
	public string missionName;
	public string missionDescriptionPrefix;
	public string missionDescriptionPostfix;
	public bool single;
	public int[] levelCount;
	
	public UILabel labelDescription;
	
	private int _complete = -1;
	private int complete{
		get{
			if(_complete == -1)
				_complete = PlayerPrefs.GetInt("stats_" + missionName,0);
			return _complete;
		}
		set{
			_complete = value;
			PlayerPrefs.SetInt("stats_" + missionName,_complete);
		}
	}
	
	public bool IsComplete{
		get{
			if(single) return complete==1;
			return levelCount[Stats.Instance.level-1]==complete;
		}
	}
	
	public void Add(int count)
	{
		if(IsComplete) return;
		int cmpt = complete + count;
		if(single && cmpt>1) cmpt=1;
		else if(cmpt>levelCount[Stats.Instance.level-1]) cmpt=levelCount[Stats.Instance.level-1];
	}
	
	void OnEnable()
	{
		if(single)
			labelDescription.text = missionDescriptionPrefix;
		else
		{
			labelDescription.text = missionDescriptionPrefix + " " + levelCount[Stats.Instance.level-1] + " " + missionDescriptionPostfix;
			if(!IsComplete) labelDescription.text += " (" + (levelCount[Stats.Instance.level-1]-complete) + " left)";
		}
	}
}