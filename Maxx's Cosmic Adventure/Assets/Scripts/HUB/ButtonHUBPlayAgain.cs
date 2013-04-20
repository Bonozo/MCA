using UnityEngine;
using System.Collections;

public class ButtonHUBPlayAgain : MonoBehaviour {

	void OnClick()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
