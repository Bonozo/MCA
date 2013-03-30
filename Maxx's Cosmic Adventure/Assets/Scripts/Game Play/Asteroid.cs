using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
		
	public int Power = 10;
	public GameObject asteroidPrefab;
	
	private bool exploded=false;
	private Vector3 beginposition;
	private bool f=false;
	
	void Start () 
	{
		gameObject.tag = "Asteroid";
	}
	
	
	void Update()
	{
		f=!f;
		transform.Rotate(0f,0f,0.001f*(f?-1:1));
	}
	
	public void GetHit(int power)
	{
		Power -= power;
		if(Power <= 0 )
			Explode();
	}
	
	public void Explode()
	{
		if(exploded) return; exploded=true;
		LevelInfo.Audio.PlayAudioAsteroidExplode();
		LevelInfo.Environments.generator.GenerateNewGem(transform.position);
		//Instantiate(asteroidPrefab,transform.position,Quaternion.identity);
		Instantiate(LevelInfo.Environments.particleExplosionAsteroid,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
}
