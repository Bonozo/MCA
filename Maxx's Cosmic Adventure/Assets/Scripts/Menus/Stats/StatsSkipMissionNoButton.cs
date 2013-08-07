using UnityEngine;
using System.Collections;

public class StatsSkipMissionNoButton : MonoBehaviour {

	void OnClick()
	{
		Stats.Instance.popupSkipMission.SetActive(false);
	}
}
