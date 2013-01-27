using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Button Back")]
public class ButtonMenuBack : MonoBehaviour {
	
	void OnClick()
	{
		MainMenu.Instance.State = MainMenu.MenuState.Title;
	}
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.Escape) )
			MainMenu.Instance.State = MainMenu.MenuState.Title;
	}
}
