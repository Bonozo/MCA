using UnityEngine;
using System.Collections;

public class ButtonHUBStore : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.State.state = GameState.Store;
		}
	}
	
}
