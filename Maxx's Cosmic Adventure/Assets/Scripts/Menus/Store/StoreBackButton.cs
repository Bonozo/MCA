using UnityEngine;
using System.Collections;

public class StoreBackButton : MonoBehaviour {

	void OnClick()
	{
		Store.Instance._currentPowerup = null;
		if(Store.Instance.IsMainMenu)
		{
			MainMenu.Instance.State = MainMenu.MenuState.Title;
			Store.Instance.ShowStore = false;
		}
		if(Store.Instance.IsPlayGame)
		{
			Store.Instance.ShowStore = false;
			Store.Instance._currentPowerup = null;
			LevelInfo.State.state = GameState.Paused;
		}
	}
	
	private UIButton button;
	void Awake()
	{
		button = this.GetComponent<UIButton>();
	}
	void Update()
	{	
		button.isEnabled = !Store.Instance.PopupActive;
	}
}
