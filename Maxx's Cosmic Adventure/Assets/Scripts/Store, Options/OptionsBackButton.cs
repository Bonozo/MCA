using UnityEngine;
using System.Collections;

public class OptionsBackButton : MonoBehaviour {
	
	void Action()
	{
		Options.Instance.ShowOptions = false;
		if(Store.Instance.IsMainMenu)
			MainMenu.Instance.State = MainMenu.MenuState.Title;
		if(Store.Instance.IsPlayGame)
		{
			LevelInfo.Settings.UpdateOptions();
			LevelInfo.State.state = GameState.Paused;
		}
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
