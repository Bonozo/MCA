using UnityEngine;
using System.Collections;

public class ButtonHUBOptions : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.State.state = GameState.Options;
		}
	}
}
