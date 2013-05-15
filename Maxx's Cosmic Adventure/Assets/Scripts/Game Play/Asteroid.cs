using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	
	public static int GlobalCount = 0;
	
	public int Power = 10;
	
	private bool exploded=false;
	private Vector3 beginposition;
	private bool f=false;
	
	private static int numberInRun = 0;
	[System.NonSerializedAttribute]
	public int numberInRunLocal = 0;
	
	void Awake()
	{
		GlobalCount++;
		numberInRun++;
		numberInRunLocal = numberInRun;
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
	
	void OnTriggerEnter(Collider col)
	{
		if( col.gameObject.CompareTag("Asteroid") && numberInRunLocal>col.gameObject.GetComponent<Asteroid>().numberInRunLocal)
			Destroy(this.gameObject);
	}
	
	public void Explode()
	{
		Explode(true);
	}
	
	public void Explode(bool byMaxx)
	{
		if(exploded) return; exploded=true;
		LevelInfo.Audio.PlayAudioAsteroidExplode();
		
		var gempos = transform.parent.position; gempos.y=0;
		LevelInfo.Environments.generator.GenerateNewGem(gempos);
			
		if(byMaxx)
		{
			LevelInfo.Environments.score.asteroidsDestoyed++;
			LevelInfo.Audio.MaxxExplodedAsteroid();
		}
		
		Instantiate(LevelInfo.Environments.particleExplosionAsteroid,transform.parent.position,Random.rotation);
		Destroy(this.gameObject);
	}
	
	public bool IsFrontOfCamera{ 
		get{
		var sc = LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position);
		sc.x /= Screen.width; sc.y /= Screen.height;
		return sc.z > 20f && Mathf.Clamp(sc.x,0.1f,0.9f)==sc.x && Mathf.Clamp(sc.y,0.1f,0.9f) == sc.y;
		}
	}
}
