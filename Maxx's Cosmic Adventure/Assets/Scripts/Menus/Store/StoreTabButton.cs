using UnityEngine;
using System.Collections;

public class StoreTabButton : MonoBehaviour {

	void OnClick()
	{
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
