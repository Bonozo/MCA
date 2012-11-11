using UnityEngine;
using System.Collections;

	public enum Gems
	{
		Unlikelium,
		SureShot,
		Shield,
		Magnet
	}

public class Gem : MonoBehaviour {
	
	public float RotateSpeed = 10f;
	public GameObject particle;
	public Gems gemType;
	
	private float MoveSpeed = 70f;
	private bool magned = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		this.rigidbody.velocity = this.rigidbody.angularVelocity = Vector3.zero;
		
		if( magned )
		{
			Vector3 dir = (LevelInfo.Environments.playerShip.transform.position - transform.position).normalized;
			transform.Translate(dir*Time.deltaTime*MoveSpeed,Space.World);
		}
		
		particle.transform.Rotate(0,-1.25f*RotateSpeed*Time.deltaTime,0);
		transform.Rotate(0,RotateSpeed*Time.deltaTime,0);
		
	}
	
	public void ActivateMagnet()
	{
		if( gemType == Gems.Unlikelium )
			magned = true;
	}
}
