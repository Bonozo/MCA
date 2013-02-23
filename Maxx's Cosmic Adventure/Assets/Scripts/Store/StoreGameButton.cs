using UnityEngine;
using System.Collections;

public class StoreGameButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			Store.Instance.ShowStore = false;
			Store.Instance._currentPowerup = null;
			LevelInfo.State.state = GameState.Paused;
		}
	}
}
