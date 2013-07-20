using UnityEngine;
using System.Collections;

public class MissionButton : MonoBehaviour {

	public Mission mission;
	
	private UIButton button;
	private UILabel text;
	
	void Awake()
	{
		button = this.GetComponent<UIButton>();
		text = this.GetComponentInChildren<UILabel>();
	}
	
	void OnEnable()
	{
		if( mission.IsComplete )
		{
			button.isEnabled = false;
			text.text = "COMPLETE";
			text.color = Color.green;
		}
		else
		{
			button.isEnabled = true;
			text.text = "SKIP";
			text.color = Color.white;
		}
	}
}
