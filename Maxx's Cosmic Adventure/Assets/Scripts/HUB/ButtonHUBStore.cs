using UnityEngine;
using System.Collections;

public class ButtonHUBStore : MonoBehaviour {

	void OnClick()
	{
		LevelInfo.State.state = GameState.Store;
	}
	
}
