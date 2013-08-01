using UnityEngine;
using System.Collections;

public class ButtonHUBFighting : MonoBehaviour {

	void OnClick()
	{
		if(Store.Instance.powerupKeepFighting.level>0)
		{
			Store.Instance.powerupKeepFighting.level--;
			LevelInfo.State.KeepFighting();
		}
	}
	
	void OnEnable()
	{
		GetComponentInChildren<UILabel>().text = "FIGHTING (" + Store.Instance.powerupKeepFighting.level + ")";
	}
}
