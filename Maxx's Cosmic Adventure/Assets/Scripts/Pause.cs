using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	/*void OnMouseUp()
	{
		Time.timeScale = 1-Time.timeScale;
	}*/
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.Escape) )
			Time.timeScale = 1-Time.timeScale;			
		
		Store.Instance.ShowStore = Time.timeScale == 0.0f;
	}
}
