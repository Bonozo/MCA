using UnityEngine;
using System.Collections;

public class StoreBuyButton : MonoBehaviour {

	void OnClick()
	{
		if(Store.Instance._currentPowerup.FullyUpdated) return;
		
		var cost = Store.Instance._currentPowerup.stored?Store.Instance.costStored:
			Store.Instance.costs[Store.Instance._currentPowerup.level];
		
		if(Store.Instance.Unlikeliums>=cost)
		{
			Stats.Instance.ReportPurchasedStoreItem(Store.Instance._currentPowerup);
			Store.Instance.Unlikeliums -= cost;
			Store.Instance._currentPowerup.level++;
			Store.Instance.Activate(Store.Instance._currentPowerup);		
		}
		else
		{
			Store.Instance.SetBuyMoreNeedDisabled(false);
			Store.Instance.popupBuyUnlikeliumsConfirmation.SetActive(true);
		}
	}
}
