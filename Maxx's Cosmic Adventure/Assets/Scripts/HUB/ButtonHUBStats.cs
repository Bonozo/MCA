using UnityEngine;
using System.Collections;

public class ButtonHUBStats : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.State.state = GameState.Stats;
	}
}
