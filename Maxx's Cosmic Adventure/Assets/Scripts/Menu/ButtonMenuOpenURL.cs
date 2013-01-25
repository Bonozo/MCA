using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Button Open URL")]
public class ButtonMenuOpenURL : MonoBehaviour {
	
	public string url;
	
	void OnClick()
	{
		Application.OpenURL(url);
	}
}
