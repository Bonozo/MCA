using UnityEngine;
using System.Collections;

public class ButtonExit : MonoBehaviour {

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}
}
