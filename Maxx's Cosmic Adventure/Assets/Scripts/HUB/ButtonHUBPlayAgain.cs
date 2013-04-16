using UnityEngine;
using System.Collections;

public class ButtonHUBPlayAgain : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
