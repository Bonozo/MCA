using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
		
	public int HitCountForDestroy = 10;
	public float DestroyTime = 2.0f;
	
	private Camera mainCamera;
	private Player player;
	
	// Use this for initialization
	void Start () {
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
		player = (Player)GameObject.FindObjectOfType(typeof(Player));
		gameObject.tag = "Asteroid";
	}
	
	bool f = true;
	
	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<Detonator>().enabled) 
		{
			DestroyTime -= Time.deltaTime;
			if( DestroyTime <= 0f )
			{
				LevelInfo.Environments.generator.GenerateNewGem(transform.position);
				Destroy(this.gameObject);
			}
			return;
		}
		
		f = !f;
		transform.rigidbody.velocity = Vector3.zero;
		transform.rigidbody.angularVelocity = Vector3.zero;
		transform.Translate(new Vector3(0,0,0.001f*(f?-1:1)));
		

		//if( mainCamera.WorldToScreenPoint(transform.position).z < -10f )
		//	Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(gameObject.GetComponent<Detonator>().enabled) return;
		if( GameEnvironment.HitWithName(col.gameObject.name,"Bullet") )
		{
			if(--HitCountForDestroy == 0 )
			{
				gameObject.transform.localScale *= 0.0f;
				gameObject.GetComponent<Detonator>().enabled = true;
			}
			Destroy(col.gameObject);
		}
	}
	
	public void DestroyObject()
	{
		if( GetComponent<Detonator>().enabled ) return;
		gameObject.transform.localScale *= 0.0f;
		gameObject.GetComponent<Detonator>().enabled = true;
	}
}
