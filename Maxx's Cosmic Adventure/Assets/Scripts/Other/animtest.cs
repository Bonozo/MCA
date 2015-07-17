using UnityEngine;
using System.Collections;

public class animtest : MonoBehaviour {
	
	public string str;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Animation>().Play(str);
		Debug.Log(GetComponent<Animation>()[str].time);
	}
}
