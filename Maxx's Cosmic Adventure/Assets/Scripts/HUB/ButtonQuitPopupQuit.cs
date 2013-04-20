using UnityEngine;
using System.Collections;

public class ButtonQuitPopupQuit : MonoBehaviour {

	void OnClick()
	{
		Application.LoadLevel("mainmenu");
	}
}
