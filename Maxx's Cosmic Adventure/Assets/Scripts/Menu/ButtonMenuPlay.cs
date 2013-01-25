using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Button Play")]
public class ButtonMenuPlay : MonoBehaviour {
	
	void OnClick()
	{
		Application.LoadLevel("story");
	}
}
