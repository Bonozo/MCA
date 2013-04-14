using UnityEngine;
using System.Collections;

public class AlienShip : MonoBehaviour {
	
	#region References
	
	public static int GlobalCount=0;
	
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
		if(randomHeight)
		{
			float height = Random.Range(-LevelInfo.Settings.MaxSpaceY,LevelInfo.Settings.MaxSpaceY);
			transform.position = new Vector3(transform.position.x,height,transform.position.z);
		}
		
		transform.rotation = targetedMove?ToForwardPlayerRotation():ToPlayerRotation();
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
		
		/*// Move
		if(targetedMove && PlayerDistance()>60f )
			transform.rotation = ToForwardPlayerRotation();
		else 
			targetedMove = false;*/
		
		
		transform.Translate(Speed*Time.deltaTime*Vector3.forward);
		
		// Fire
		if( canFire )
		{
			if( fireDeltaTime > 0f ) fireDeltaTime -= Time.deltaTime;
			if( Random.Range(1,fireFrequency) == 1 && fireDeltaTime <= 0f && IsFrontOfCamera() )
			{
				var c = ((GameObject)Instantiate(alienBullet,projectilePosition.position,transform.rotation)).GetComponent<AlienBullet>();
				if(targetedFire)
				{
					c.Speed *= 2;
					c.GoTo(PlayerForwardPosition());
					Debug.Log("targetted fire to player");
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
		return PlayerDistance() >= 350f;
	}
	
	private Quaternion ToPlayerRotation()
	{
		Quaternion rot = Quaternion.LookRotation(-(transform.position-LevelInfo.Environments.playerShip.transform.position));
		rot.x = 0.0f;
		return rot;
	}
	
	Quaternion ToForwardPlayerRotation()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player += Random.Range(50f,150f)*Vector3.forward;
		Quaternion rot = Quaternion.LookRotation(-(transform.position-player));
		rot.x = 0.0f;
		return rot;
	}	
	
	private Vector3 PlayerForwardPosition()
	{
		var player = LevelInfo.Environments.playerShip.transform.position;
		player += 30f*Vector3.forward;
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
			col.gameObject.SendMessage("Explode");
			Explode();
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
		if(exploded) return;
		exploded = true;
		Instantiate(LevelInfo.Environments.particleExplosionJeeb,Centr.transform.position,Quaternion.identity);
		
		LevelInfo.Audio.audioSourceJeebles.PlayOneShot(ExplosionSoundEffect);
		LevelInfo.Audio.audioSourceJeebles.time = 0.5f;
		
		if( GameEnvironment.Probability(2))
					LevelInfo.Audio.PlayAudioGotEm();
		
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
