using UnityEngine;
using System.Collections;

public class StatsSkipMissionYesButton : MonoBehaviour {

	void OnClick()
	{
		if(Store.Instance.Unlikeliums>=5000)
		{
			for(int i=0;i<3;i++)
			{
				if(i<Stats.Instance.missions.Length&&Stats.Instance.CurrentMission(i)==Stats.Instance.currentSkippingMission)
				{
					Store.Instance.Unlikeliums -= 5000;
					Stats.Instance.currentSkippingMission.CompleteDirectly();
					Stats.Instance.MissionComplete(i);
					Stats.Instance.popupSkipMission.SetActive(false);
					Stats.Instance.ShowStats = true;
					break;
				}
			}
		}
	}
}
