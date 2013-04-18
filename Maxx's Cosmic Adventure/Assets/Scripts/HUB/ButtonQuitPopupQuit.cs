using UnityEngine;
using System.Collections;

public class ButtonQuitPopupQuit : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			Application.LoadLevel("mainmenu");
		}
	}
}
