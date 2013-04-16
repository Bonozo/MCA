using UnityEngine;
using System.Collections;

public class ButtonHUBMenu : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			Application.LoadLevel("mainmenu");
		}
	}
}
