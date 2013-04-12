using UnityEngine;
using System.Collections;

public class UpdateablePowerup : MonoBehaviour {
	
	public bool stored = false;
	public string powerupName;
	
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
	
	void OnPress(bool isDown)
	{
		if(!isDown)
			Store.Instance.Activate(this);
	}
	
	void Update()
	{
		button.isEnabled = !Store.Instance.PopupActive;
	}
	
	public bool FullyUpdated { get { return level==4; }}
	public float LevelTime { get { return 10+5*level; }}
}
