using UnityEngine;
using System.Collections;

public class UpdateablePowerup : MonoBehaviour {
	
	public bool stored = false;
	public string powerupName;
	public string description;
	public UILabel labelName;
	
	private int _level = -1;
	public int level{
		get{
			if(_level==-1)
				level = PlayerPrefs.GetInt("powerup_"+powerupName,0);
			return _level;
		}
		set{
			_level=value;
			PlayerPrefs.SetInt("powerup_"+powerupName,_level);
		}
	}
	
	private UIButton button;
	
	void Awake()
	{
		button = this.GetComponent<UIButton>();
	}
	
	void OnClick()
	{
		Store.Instance.Activate(this);
	}
	
	void Update()
	{	
		button.isEnabled = !Store.Instance.PopupActive;
	}
	
	public bool FullyUpdated { get { return stored?false:level==5; }}
	
	// for timed powerups
	public float TimedLevelTime{
		get{
		switch(level)
		{
		case 0: return 0f; 
		case 1: return 5f;
		case 2: return 10f;
		case 3: return 15f;
		case 4: return 30f;
		case 5: return 60f;
		}
		return 0;
		}
	}
	// for stored powerups
	public int 	StoredPowerupCount{
		get{
		switch(level)
		{
		case 0: return 0; 
		case 1: return 1;
		case 2: return 2;
		case 3: return 3;
		case 4: return 5;
		case 5: return 10;
		}
		return 0;
		}
	}
}
