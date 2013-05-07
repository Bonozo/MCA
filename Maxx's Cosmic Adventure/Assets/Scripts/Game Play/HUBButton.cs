using UnityEngine;
using System.Collections;

public class HUBButton : MonoBehaviour {
	
	public bool isDown {get;private set;}
	
	void OnPress(bool isDown)
	{
		this.isDown = isDown;
	}

}
