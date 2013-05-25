using UnityEngine;
using System.Collections;

public class Options5000Distance : MonoBehaviour {

	void OnClick()
	{
		if(Store.Instance.IsPlayGame)
		{
			LevelInfo.Environments.playerShip.travelled += 5000;
			LevelInfo.Environments.generator.ResetAfterOptions5000Distance();
		}
	}
}
