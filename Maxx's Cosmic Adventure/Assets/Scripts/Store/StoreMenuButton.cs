using UnityEngine;
using System.Collections;

public class StoreMenuButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			Time.timeScale = 1f;
			Store.Instance.ShowStore = false;
			Store.Instance._currentPowerup = null;
			if(Store.Instance.IsMainMenu)
				MainMenu.Instance.State = MainMenu.MenuState.Title;
			if(Store.Instance.IsPlayGame)
				Application.LoadLevel("mainmenu");
		}
	}
}