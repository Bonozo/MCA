using UnityEngine;
using System.Collections;

public class StoreBuyMoreYesButton : MonoBehaviour {

	void OnClick()
	{
		Store.Instance.SetBuyMoreNeedDisabled(true);
		Store.Instance.popupBuyUnlikeliumsConfirmation.SetActive(false);	
		Store.Instance.popupUpgradePowerups.SetActive(false);
		Store.Instance.popupBuyUnlikeliums.SetActive(true);
	}
}
