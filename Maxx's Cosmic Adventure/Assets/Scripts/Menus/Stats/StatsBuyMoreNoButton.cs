using UnityEngine;
using System.Collections;

public class StatsBuyMoreNoButton : MonoBehaviour {

	void OnClick()
	{
		Stats.Instance.popupBuyMoreUnlikeliums.SetActive(false);
	}
}
