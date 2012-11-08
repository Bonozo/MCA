using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
		
	public int HitCountForDestroy = 10;
	public float DestroyTime = 2.0f;
	public GameObject asteroidPrefab;
	
	private bool exploded = false;
	
	// Use this for initialization
	void Start () {
		gameObject.tag = "Asteroid";
	}
	
	bool f = true;
	
	// Update is called once per frame
	void Update () {
		if(exploded) 
		{
			DestroyTime -= Time.deltaTime;
			if( DestroyTime <= 0f )
			{
				Instantiate(asteroidPrefab,transform.position,Quaternion.identity);
				//LevelInfo.Environments.generator.GenerateNewGem(transform.position);
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
		if(exploded) return;
		if( GameEnvironment.HitWithName(col.gameObject.name,"Bullet") )
		{
			if(--HitCountForDestroy == 0 )
			{
				exploded = true;
				gameObject.transform.localScale *= 0.0f;
				Instantiate(LevelInfo.Environments.AsteroidExplosionPrefab,transform.position,Quaternion.identity);
			}
			Destroy(col.gameObject);
		}
	}
	
	public void DestroyObject()
	{
		if( exploded ) return;
		exploded = true;
		gameObject.transform.localScale *= 0.0f;
	}
}
