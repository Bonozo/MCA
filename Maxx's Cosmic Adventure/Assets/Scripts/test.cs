using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	
	public GameObject prefab;
	
	// Use this for initialization
	void Start () {
	}
	
	int n=0;
	
	// Update is called once per frame
	void Update () {
		 if( (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0) )
		{
			Instantiate(prefab,new Vector3(-50+20*(n%5),-50,100+10*(n/5)),Quaternion.identity);
			n++;
		}
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(0,100,500,500),"count : " + n );
	}
}
