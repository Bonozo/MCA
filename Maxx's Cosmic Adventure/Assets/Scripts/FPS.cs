using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	float time = 0.0f;
	int frame = 0;
	
	// Update is called once per frame
	void Update () {
		frame++;
		if( Time.time-time > 1 )
		{
			guiText.text = "FPS : " + frame;
			frame=0;
			time+=1f;
		}
	}
}
