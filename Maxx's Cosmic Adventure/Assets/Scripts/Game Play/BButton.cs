using UnityEngine;
using System.Collections;

public class BButton : MonoBehaviour {
	
	public bool isDown {get;private set;}
	
	void OnPress(bool isDown)
	{
		this.isDown = isDown;
	}

}
