using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public float Wait = 3.0f;
	
	// Use this for initialization
	void Start () {
		guiText.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		Wait -= Time.deltaTime;
		if (Wait <= 0 )
			Application.LoadLevel(Application.loadedLevel);
	}
}
