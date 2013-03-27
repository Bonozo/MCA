using UnityEngine;
using System.Collections;

public class UpdateablePowerup : MonoBehaviour {
	
	public string powerupName;
	
	private int _level;
	public int level{
		get{
			return _level;
		}
		set{
			_level=value;
			PlayerPrefs.SetInt("powerup_"+powerupName,_level);
		}
	}
	
	void Awake()
	{
		level = PlayerPrefs.GetInt("powerup_"+powerupName,0);
	}
	
	void OnPress(bool isDown)
	{
		//if(!isDown)
		//	Store.Instance.Activate(this);
	}
	
	public bool FullyUpdated { get { return level==4; }}
	public float LevelTime { get { return 10+5*level; }}
}
