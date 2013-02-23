using UnityEngine;
using System.Collections;

public class StoreBuyButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			if(!Store.Instance._currentPowerup.FullyUpdated && Store.Instance.Unlikeliums>=Store.Instance.costs[Store.Instance._currentPowerup.level])
			{
				Store.Instance.Unlikeliums -= Store.Instance.costs[Store.Instance._currentPowerup.level];
				Store.Instance._currentPowerup.level++;
				Store.Instance.Activate(Store.Instance._currentPowerup);		
			}
		}
	}
}
