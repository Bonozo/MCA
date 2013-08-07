using UnityEngine;
using System.Collections;

public class MissionButton : MonoBehaviour {

	public Mission mission;
	
	private UIButton button;
	private UILabel text;
	
	void OnClick()
	{
		Stats.Instance.currentSkippingMission = mission;
		if(Store.Instance.Unlikeliums >= 5000)
			Stats.Instance.popupSkipMission.SetActive(true);
		else
			Stats.Instance.popupBuyMoreUnlikeliums.SetActive(true);
	}
	
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
			mission.labelDescription.color = Color.green;
		}
		else
		{
			button.isEnabled = true;
			text.text = "SKIP";
			text.color = Color.white;
			mission.labelDescription.color = Color.white;
		}
	}
	
	void Update()
	{
		button.isEnabled = !Stats.Instance.PopupActive;
	}
}
