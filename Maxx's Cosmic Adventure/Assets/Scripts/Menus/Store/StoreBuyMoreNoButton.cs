using UnityEngine;
using System.Collections;

public class StoreBuyMoreNoButton : MonoBehaviour {

	void OnClick()
	{
		Store.Instance.SetBuyMoreNeedDisabled(true);
		Store.Instance.popupBuyUnlikeliumsConfirmation.SetActive(false);		
	}
}
