using UnityEngine;
using System.Collections;

public class StatsBackButton : MonoBehaviour {

	void Action()
	{
		Stats.Instance.ShowStats = false;
		if(Store.Instance.IsMainMenu)
			MainMenu.Instance.State = MainMenu.MenuState.Title;
		if(Store.Instance.IsPlayGame)
			LevelInfo.State.state = GameState.Paused;
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
