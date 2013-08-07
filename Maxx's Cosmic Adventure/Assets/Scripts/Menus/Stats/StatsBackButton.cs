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
	
	private UIButton button;
	void Awake()
	{
		button = this.GetComponent<UIButton>();
	}
	
	void Update()
	{
		button.isEnabled = !Stats.Instance.PopupActive;
		if(button.isEnabled && Input.GetKeyUp(KeyCode.Escape) )
			Action();
		
	}
}
