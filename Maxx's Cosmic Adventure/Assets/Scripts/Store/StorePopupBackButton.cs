using UnityEngine;
using System.Collections;

public class StorePopupBackButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		Store.Instance.popup.SetActive(false);
	}
}
