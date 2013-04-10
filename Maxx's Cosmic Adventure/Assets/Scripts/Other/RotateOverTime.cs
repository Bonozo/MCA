using UnityEngine;
using System.Collections;

public class RotateOverTime : MonoBehaviour {
	
	public float Speed = 10f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0.016f*Speed,0);
	}
}
