using UnityEngine;
using System.Collections;

public class StatsBuyMoreYesButton : MonoBehaviour {

	void OnClick()
	{
		Stats.Instance.ShowStats = false;
		
		if(Store.Instance.IsMainMenu)
			MainMenu.Instance.State = MainMenu.MenuState.Store;
		else
			Store.Instance.ShowStore = true;
		
		Store.Instance.SetBuyMoreNeedDisabled(true);
		Store.Instance.popupBuyUnlikeliumsConfirmation.SetActive(false);	
		Store.Instance.popupUpgradePowerups.SetActive(false);
		Store.Instance.popupBuyUnlikeliums.SetActive(true);
	}
}
