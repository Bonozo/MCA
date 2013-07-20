using UnityEngine;
using System.Collections;

public class StoreGameButton : MonoBehaviour {

	void OnClick()
	{
		Store.Instance.ShowStore = false;
		Store.Instance._currentPowerup = null;
		LevelInfo.State.state = GameState.Paused;
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
