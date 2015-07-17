using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	
	public static int GlobalCount = 0;
	
	public Asteroids asteroidType;
	public int Power = 10;
	
	private bool exploded=false;
	private Vector3 beginposition;
	private bool f=false;
	
	private static int numberInRun = 0;
	[System.NonSerializedAttribute]
	public int numberInRunLocal = 0;
	
	private float health;
	void Awake()
	{
		// health
		health = (float)Power;
		
		GlobalCount++;
		numberInRun++;
		numberInRunLocal = numberInRun;
	}
	
	void OnDestroy()
	{
		GlobalCount--;
	}
	
	IEnumerator Start () 
	{
		gameObject.tag = "Asteroid";
		LevelInfo.Environments.tutorials.SpawnedAsteroid();
		
		Color color = gameObject.GetComponent<Renderer>().material.color;
		color.a = 0f;
		gameObject.GetComponent<Renderer>().material.color = color;
		while(color.a<1f)
		{
			color.a = Mathf.Clamp01(color.a+Time.deltaTime);
			gameObject.GetComponent<Renderer>().material.color = color;
			yield return null;
		}
		
		// check and say lot's of asteroids
		LevelInfo.Audio.PlayVoiceOverLotsOfAsteroids();
	}
	
	void Update()
	{
		if(LevelInfo.State.state != GameState.Play) return;
		f=!f;
		transform.Rotate(0f,0f,0.001f*(f?-1:1));
		
		if( !nearmisshappened)
		{
			minnearmiss = Mathf.Min(minnearmiss,PlayerDistance);
			if(minnearmiss<=20f && PlayerDistance>=50f)
				NearMissThread();
		}
	}
	
	public void GetHit(int power)
	{
		float factor = (1f+0.2f*Store.Instance.powerupShipBullet.level);
		health -= factor*power;
		if(health <= 0 )
			Explode();
	}
	
	public float PlayerDistance{
		get{
		var p = LevelInfo.Environments.playerShip.transform.position; p.y = transform.position.y;
		return Vector3.Distance(transform.position,p);
		}
	}
	
	private float minnearmiss = 10000f;
	private bool nearmisshappened = false;
	void NearMissThread()
	{
		nearmisshappened = true;
		// reportings
		Stats.Instance.ReportNearMeetWithAsteroid(asteroidType);
		LevelInfo.Audio.PlayVoiceOverNearMissWithObstacle();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if( col.gameObject.CompareTag("Asteroid") && numberInRunLocal>col.gameObject.GetComponent<Asteroid>().numberInRunLocal)
		{
			Debug.LogWarning("MCA Warning: Asteroids overlapping");
			Destroy(this.gameObject);
		}
	}
	
	public void Explode()
	{
		Explode(true);
	}
	
	public void Explode(bool byMaxx)
	{
		if(exploded) return; exploded=true;
		
		var gempos = transform.parent.position; if(!Options.Instance.flightControls3D) gempos.y=0f;
		LevelInfo.Environments.generator.GenerateNewGem(gempos);
			
		if(byMaxx)
		{
			// reportings
			LevelInfo.Environments.score.asteroidsDestoyed++;
			Stats.Instance.ReportDestroyedAsteroid(asteroidType);
			
			LevelInfo.Audio.PlayVoiceOverAsteroidDestroyed();
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
