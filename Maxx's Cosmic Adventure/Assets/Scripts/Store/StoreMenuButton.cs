using UnityEngine;
using System.Collections;

public class StoreMenuButton : MonoBehaviour {
	
	public bool disableWhenPopupActive = true;
	public GameObject vertifyPopup;
	
	void OnClick()
	{
		if(Store.Instance.IsPlayGame && vertifyPopup != null)
		{
			vertifyPopup.SetActive(true);
		}
		else
		{
			Time.timeScale = 1f;
			Store.Instance._currentPowerup = null;
			if(Store.Instance.IsMainMenu)
			{
				MainMenu.Instance.State = MainMenu.MenuState.Title;
				Store.Instance.ShowStore = false;
			}
			if(Store.Instance.IsPlayGame)
			{
				Store.Instance.audio.Stop();
				Store.Instance.SetQuitPopupButtonEnabled(false);
				Application.LoadLevel("mainmenu");
			}
		}
	}
	
	
	private UIButton button;
	
	void Awake()
	{
		button = this.GetComponent<UIButton>();
	}
	void Update()
	{	
		if(disableWhenPopupActive)
			button.isEnabled = !Store.Instance.PopupActive;
	}
}
