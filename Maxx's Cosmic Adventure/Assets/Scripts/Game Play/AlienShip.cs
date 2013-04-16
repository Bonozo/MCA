using UnityEngine;
using System.Collections;

public class AlienShip : MonoBehaviour {
	
	#region References
	
	public static int GlobalCount=0;
	
	// Spawn
	public bool initBeginTransform = true;
	public float spawnDistanceMin=230f, spawnDistanceMax=250f;
	public float maxFrontAngle=30f, autoKillDistance = 350f;
	
	// Power, Speed
	public int Power = 10;
	public float Speed = 2f;
	
	// Fire
	public bool canFire = true;
	public bool targetedFire = false;
	public int fireFrequency = 80;
	public float fireRelax = 0.5f;
	
	// Move
	public bool targetedMove = false;
	public bool appearAtPlayerFront = false;
	
	public bool randomHeight;
	
	public GameObject alienBullet;
	public Transform Centr,Up;
	public Transform projectilePosition;
	
	public AudioClip ExplosionSoundEffect;
	
	#endregion
	
	#region Private Variables
	private bool ready = false;
	private float fireDeltaTime = 0.0f;
	#endregion
	
	#region Start, Update
	
	void Awake()
	{
		GlobalCount++;
	}
	
	void OnDestroy()
	{
		GlobalCount--;
	}
	
	void Start () 
	{
		if( initBeginTransform )
		{
			var player = LevelInfo.Environments.playerShip.transform;
			transform.position = new Vector3(player.position.x,0,player.position.z+Random.Range(spawnDistanceMin,spawnDistanceMax));
		
			var dlt = Random.Range(-maxFrontAngle,maxFrontAngle);
			dlt *= 1f/Random.Range(1,3);
			transform.RotateAround (LevelInfo.Environments.playerShip.transform.position, Vector3.up, 
			LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y + dlt );	
		}
		
		if(randomHeight)
		{
			float height = Random.Range(-LevelInfo.Settings.MaxSpaceY,LevelInfo.Settings.MaxSpaceY);
			transform.position = new Vector3(transform.position.x,height,transform.position.z);
		}
		
		if(targetedMove)
			transform.rotation = ToForwardPlayerRotation();
		else if(appearAtPlayerFront)
			transform.rotation = ToPlayerRotation();
		else
			transform.rotation = ToNearPlayerRotation();
		
		tag = "Enemy";
		
		StartCoroutine(GetReady());
	}
	
	IEnumerator GetReady()
	{
		var beginScale = transform.localScale;
		transform.localScale *= 0.0f;
		
		float appearTime = LevelInfo.Settings.AlienShipAppearTime;
		while( appearTime >= 0 )
		{
			transform.localScale = beginScale*(LevelInfo.Settings.AlienShipAppearTime-appearTime)/LevelInfo.Settings.AlienShipAppearTime;
			appearTime -= Time.deltaTime;
			yield return null;
		}
		ready = true;
	}
	
	void Update ()
	{
		if(LevelInfo.State.state != GameState.Play) return;
		if( !ready ) return;

		
		if( DestroyNeed() )
		{
			Destroy(this.gameObject);
			return;
		}
		
		if(LevelInfo.Environments.playerShip.FreezeWorld) return;
		
		// Move
		if( appearAtPlayerFront && IsFrontOfCamera() && PlayerDistance()>50f)
		{
			float playery = LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y+360f;
			float y = transform.rotation.eulerAngles.y+360f+180f;
			if(y>playery+180f) y-=360f;
			
			float ignore = 2f;
			float speed = 10f;
		
			transform.rotation = ToPlayerRotation();
			
			if( y < playery-ignore )
				transform.RotateAround(LevelInfo.Environments.playerShip.transform.position, Vector3.up,Time.deltaTime*speed);
			if( y > playery+ignore)
				transform.RotateAround(LevelInfo.Environments.playerShip.transform.position, Vector3.up,-Time.deltaTime*speed);
			
		}
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);
		
		// Fire
		if( canFire )
		{
			if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
			if( Random.Range(1,fireFrequency) == 1 && fireDeltaTime<=0f && IsFrontOfCamera() && PlayerDistance()>50f )
			{
				var c = ((GameObject)Instantiate(alienBullet,projectilePosition.position,transform.rotation)).GetComponent<AlienBullet>();
				if(targetedFire)
				{
					c.Speed *= 2;
					c.GoTo(PlayerForwardPosition());
				}
				
				fireDeltaTime = fireRelax;
			}
		}
	}
	
	#endregion
	
	#region Methods
	
	private bool DestroyNeed()
	{
		/*float y = transform.rotation.eulerAngles.y; if( y>=180f) y-=360f;
		float playery = LevelInfo.Environments.playerShip.transform.rotation.eulerAngles.y; if(playery>=180f) playery-=360f;
		if( Vector3.Distance(transform.position,LevelInfo.Environments.playerShip.transform.position) >= 50f && Mathf.Abs(y-playery) <= 45f )
			return true;*/
		return PlayerDistance() >= autoKillDistance;
	}
	
	private Quaternion ToPlayerRotation()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player.y = transform.position.y;
		Quaternion rot = Quaternion.LookRotation(-(transform.position-player));
		rot.x = 0.0f;
		return rot;
	}	
	
	private Quaternion ToNearPlayerRotation()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player.y = transform.position.y;
		player += Random.Range(0f,40f)*LevelInfo.Environments.playerShip.transform.forward;
		Quaternion rot = Quaternion.LookRotation(-(transform.position-player));
		rot.x = 0.0f;
		return rot;
	}
	
	Quaternion ToForwardPlayerRotation()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player.y = transform.position.y;
		player += Random.Range(50f,150f)*LevelInfo.Environments.playerShip.transform.forward;
		Quaternion rot = Quaternion.LookRotation(-(transform.position-player));
		rot.x = 0.0f;
		return rot;
	}	
	
	private Vector3 PlayerForwardPosition()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player += Random.Range(30f,80f)*LevelInfo.Environments.playerShip.transform.forward;
		return player;
	}
	
	private float PlayerDistance()
	{
		var p = LevelInfo.Environments.playerShip.transform.position; p.y = transform.position.y;
		return Vector3.Distance(transform.position,p);
	}
	
	private bool IsFrontOfCamera()
	{
		var sc = LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position);
		sc.x /= Screen.width; sc.y /= Screen.height;
		return sc.z > 20f && Mathf.Clamp(sc.x,0.1f,0.9f)==sc.x && Mathf.Clamp(sc.y,0.1f,0.9f) == sc.y;
	}
	
	#endregion
	
	#region Collision, Explosion
	
	void OnTriggerEnter(Collider col)
	{	
		if( col.gameObject.CompareTag("Asteroid") || col.gameObject.CompareTag("Enemy") )
		{
			col.gameObject.SendMessage("Explode",false);
			Explode(false);
		}
	}
	
	public void GetHit(int power)
	{
		Power -= power;
		if(Power <= 0 )
			Explode();
	}
	
	private bool exploded = false;
	void Explode()
	{
		Explode(true);
	}
	
	void Explode(bool byMaxx)
	{
		if(exploded) return;
		exploded = true;
		Instantiate(LevelInfo.Environments.particleExplosionJeeb,Centr.transform.position,Quaternion.identity);
		
		LevelInfo.Audio.audioSourceJeebles.PlayOneShot(ExplosionSoundEffect);
		LevelInfo.Audio.audioSourceJeebles.time = 0.5f;
		
		if(byMaxx && GameEnvironment.Probability(2))
					LevelInfo.Audio.PlayAudioGotEm();
		
		if(byMaxx)
			LevelInfo.Environments.score.score += LevelInfo.Settings.scoreJeebie;
		
		Destroy(this.gameObject);		
	}
	
	#endregion
	
	#region Targeting Box
	
	private float targetingboxappeartime = 0f;
	private GameObject targetingBox = null;
	
	public void EnableTargetingBox()
	{
		StartCoroutine(EnableTargetingBoxThread());
	}
	
	private IEnumerator EnableTargetingBoxThread()
	{
		targetingboxappeartime = 2f;
		if(targetingBox==null&&!exploded)
		{
			targetingBox = (GameObject)Instantiate(LevelInfo.Environments.targetingBoxPrefab);
			targetingBox.transform.parent = transform;
			while(targetingboxappeartime>0f)
			{
				targetingboxappeartime -= Time.deltaTime;
				
				Vector3 centr = LevelInfo.Environments.mainCamera.WorldToScreenPoint(Centr.position);
				Vector3 up = LevelInfo.Environments.mainCamera.WorldToScreenPoint(Up.position);
			
				centr.x /= Screen.width; centr.y /= Screen.height;
				up.x /= Screen.width; up.y /= Screen.height;
				float sc = Mathf.Abs(centr.y-up.y)*4;
			
				targetingBox.guiTexture.enabled = centr.z > 0;
			
				targetingBox.transform.position = new Vector3(centr.x,centr.y,targetingBox.transform.position.z);
				targetingBox.transform.localScale = new Vector3(sc,sc,targetingBox.transform.localScale.z);	
				
				yield return null;
			}
			Destroy(targetingBox);
			targetingBox = null;
		}
	}
	
	#endregion
}
