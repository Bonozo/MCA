using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private Store store;
	
	void Start()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	/*void OnMouseUp()
	{
		Time.timeScale = 1-Time.timeScale;
	}*/
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.Escape) )
			Time.timeScale = 1-Time.timeScale;			
		
		store.Active = Time.timeScale == 0.0f;
	}
}
