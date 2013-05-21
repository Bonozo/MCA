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
	}
	
	void OnDestroy()
	{
		GlobalCount--;
	}
	
	IEnumerator Start () 
	{
		gameObject.tag = "Asteroid";
		LevelInfo.Environments.tutorials.SpawnedAsteroid();
		
		Color color = gameObject.renderer.material.color;
		color.a = 0f;
		gameObject.renderer.material.color = color;
		while(color.a<1f)
		{
			color.a = Mathf.Clamp01(color.a+Time.deltaTime);
			gameObject.renderer.material.color = color;
			yield return null;
		}
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
			LevelInfo.Environments.score.asteroidsDestoyed++;
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
