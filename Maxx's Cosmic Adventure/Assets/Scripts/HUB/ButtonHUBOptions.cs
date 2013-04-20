using UnityEngine;
using System.Collections;

public class ButtonHUBOptions : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.State.state = GameState.Options;
	}
}
