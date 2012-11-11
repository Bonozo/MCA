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
			Instantiate(prefab,new Vector3(Random.Range(0,0),Random.Range(1,1),Random.Range(0,10)),Quaternion.identity);
			n++;
		}
		
		if( Input.GetKey(KeyCode.W) ) 
			transform.Translate(0,0,10*Time.deltaTime);
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(0,100,500,500),"count : " + n );
	}
}
