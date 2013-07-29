using UnityEngine;
using System.Collections;

public class StoreTabButton : MonoBehaviour {
	
	public bool tabVal;
	
	void OnClick()
	{
		Store.Instance.tabWeapons = tabVal;
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
