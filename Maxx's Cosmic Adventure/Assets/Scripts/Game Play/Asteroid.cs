using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	
	public static int GlobalCount = 0;
	
	public int Power = 10;
	
	private bool exploded=false;
	private Vector3 beginposition;
	private bool f=false;
	
	void Awake()
	{
		GlobalCount++;
		if(GlobalCount > 5 )
			LevelInfo.Audio.MaxxAsteroidBelt();
	}
	
	void OnDestroy()
	{
		GlobalCount--;
	}
	
	void Start () 
	{
		gameObject.tag = "Asteroid";
		LevelInfo.Environments.tutorials.SpawnedAsteroid();
	}
	
	
	void Update()
	{
		if(LevelInfo.State.state != GameState.Play) return;
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
		Explode(true);
	}
	
	public void Explode(bool byMaxx)
	{
		if(exploded) return; exploded=true;
		LevelInfo.Audio.PlayAudioAsteroidExplode();
		
		var gempos = transform.position; gempos.y=0;
		LevelInfo.Environments.generator.GenerateNewGem(gempos);
			
		if(byMaxx)
		{
			LevelInfo.Environments.score.asteroidsDestoyed++;
			LevelInfo.Audio.MaxxExplodedAsteroid();
		}
		
		Instantiate(LevelInfo.Environments.particleExplosionAsteroid,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
}
