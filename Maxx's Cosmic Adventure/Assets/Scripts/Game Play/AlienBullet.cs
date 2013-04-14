using UnityEngine;
using System.Collections;

public class AlienBullet : MonoBehaviour {
	
	public Vector3 BeginPosition { set { transform.position = value; } }
	public Vector3 BeginRotation { set { transform.position = value; } }
	public float Speed = 10f;
	public float DeadTime = 10f;
	
	
	void Awake()
	{
		tag = "AlienBullet";
		Destroy(this.gameObject,DeadTime);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(LevelInfo.State.state != GameState.Play) return;
		if(LevelInfo.Environments.playerShip.FreezeWorld) return;
		
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);
	}
	
	void OnTriggerEnter(Collider col)
	{	
		if( col.gameObject.CompareTag("Asteroid") || col.gameObject.CompareTag("Enemy") )
		{
			col.gameObject.SendMessage("Explode");
			Destroy(this.gameObject);
		}
	}
	
	public void GoTo(Vector3 position)
	{
		transform.LookAt(position);
	}
}
