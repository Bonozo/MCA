using UnityEngine;
using System.Collections;

[AddComponentMenu("Menu/Stats")]
public class Stats : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyUp(KeyCode.Escape) )
			MainMenu.Instance.State = MainMenu.MenuState.Title;
	}
}
