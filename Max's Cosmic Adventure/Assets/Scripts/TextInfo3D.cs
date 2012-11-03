using UnityEngine;
using System.Collections;

public class TextInfo3D : MonoBehaviour {
	
	public float ComeTime = 2f;
	public float WaitTime = 2f;
	public float GoTime = 2f;
	public float FromPositionZ = 1000f;
	public float WaitPositionZ = 0f;
	public float GoPositionZ = -100f;
	public float PositionX = 0f,PositionY = 3.5f;
	
	private float comeTime;
	private float goTime;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(PositionX,PositionY,FromPositionZ);
		comeTime = ComeTime;
		goTime = GoTime;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( comeTime > 0 )
		{
			transform.Translate(0,0, Time.deltaTime*(WaitPositionZ-FromPositionZ)/ComeTime );
			comeTime -= Time.deltaTime;
		}
		else if( WaitTime > 0 )
			WaitTime -= Time.deltaTime;
		else if(goTime > 0 )
		{
			transform.Translate(0,0, Time.deltaTime*(GoPositionZ-WaitPositionZ)/GoTime );
			goTime -= Time.deltaTime;	
		}
		else
			Destroy(this.gameObject);
	}
}
