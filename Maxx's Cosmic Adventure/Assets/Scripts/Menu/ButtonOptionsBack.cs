using UnityEngine;
using System.Collections;

public class ButtonOptionsBack : MonoBehaviour {
	
	void Action()
	{
		Debug.Log("Options Back Action");
		Options.Instance.ShowOptions = false;
		if(Store.Instance.IsMainMenu)
			MainMenu.Instance.State = MainMenu.MenuState.Title;
	}
	
	void OnClick()
	{
		Action();
	}
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.Escape) )
			Action();
	}
}
