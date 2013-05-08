using UnityEngine;
using System.Collections;

public class StorePopupSetActive : MonoBehaviour {
	
	public GameObject popup;
	public bool toActiveState=false;
	void OnPress(bool isDown)
	{
		popup.SetActive(toActiveState);
	}
}
