using UnityEngine;
using System.Collections;

public class StoreMenuButton : MonoBehaviour {

	void OnClick()
	{
		Time.timeScale = 1f;
		Store.Instance.ShowStore = false;
		Store.Instance._currentPowerup = null;
		if(Store.Instance.IsMainMenu)
			MainMenu.Instance.State = MainMenu.MenuState.Title;
		if(Store.Instance.IsPlayGame)
			Application.LoadLevel("mainmenu");
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
