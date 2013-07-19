using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Button Change Menu State")]
public class ButtonMenuChangeState : MonoBehaviour {
	
	public MainMenu.MenuState state;
	
	void OnClick()
	{
		MainMenu.Instance.State = state;
	}
}
